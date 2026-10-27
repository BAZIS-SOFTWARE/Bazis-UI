using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace BazisGUI.SceneCore
{
    /// <summary>
    /// Класс - GLSL программа для хранения шейдеров
    /// </summary>
    public class ShaderProgramCreator : IDisposable
    {
        /// <summary>
        /// Идентификатор сгенерированного вершинного шейдера
        /// </summary>
        public int Vertex { get; set; }
        /// <summary>
        /// Идентификатор сгенерированного фрагментного шейдера
        /// </summary>
        public int Fragment { get; set; }
        /// <summary>
        /// Идентификатор сгенерированного геометрического шейдера
        /// </summary>
        public int Geometry { get; set; }
        /// <summary>
        /// Идентификатор сгенерированной программы
        /// </summary>
        public int Program { get; private set; }
        /// <summary>
        /// Конструктор GLSL программы
        /// </summary>
        public ShaderProgramCreator() { }
        /// <summary>
        /// Делает активной текущую GLSL программу
        /// </summary>
        public void Bind() => GL.UseProgram(Program);
        /// <summary>
        /// Делает активной программу по умолчанию
        /// </summary>
        public void Unbind() => GL.UseProgram(0);
        /// <summary>
        /// Активирует текстурный слот для переменной с именем name в шейдере
        /// </summary>
        /// <param name="name">Строковое имя переменной в шейдере</param>
        /// <param name="texId">Идентификатор сгенерированной текстуры</param>
        /// <param name="texUnit">Номер текстурного слота</param>
        public void BindTextureRect(string name, int texId, int texUnit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texUnit);
            GL.BindTexture(TextureTarget.TextureRectangleArb, texId);
            var id = GL.GetUniformLocation(Program, name);
            GL.Uniform1(id, texUnit);
        }
        /// <summary>
        /// Связывает пользовательские аттрибуты с программой
        /// </summary>
        /// <param name="buffer">Идентификатор буффера пользовательских данных</param>
        /// <param name="variable">Имя аттрибута в шейдере</param>
        /// <param name="attribsCount">Число аттрибутов</param>
        /// <param name="usedType">Используемый тип данных</param>
        public void SetCustomAttributes(int buffer, string variable, int attribsCount = 3, VertexAttribPointerType usedType = VertexAttribPointerType.Float)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            var location = GL.GetAttribLocation(Program, variable);
            GL.BindAttribLocation(Program, location, variable);
            GL.EnableVertexAttribArray(location);
            GL.VertexAttribPointer(location, attribsCount, usedType, false, 0, IntPtr.Zero);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        /// <summary>
        /// Отвязывает пользовательские аттрибуты от программы
        /// </summary>
        /// <param name="variable">Имя аттрибута в шейдере</param>
        public void UnsetCustomAttributes(string variable)
        {
            var location = GL.GetAttribLocation(Program, variable);
            GL.DisableVertexAttribArray(location);
        }

        /// <summary>
        /// Передать значение в переменную шейдера с именем name
        /// </summary>
        /// <param name="name">Имя переменной в шейдере</param>
        /// <param name="values">Массив значений переменной</param>
        public void SetUniform(string name, float[] values)
        {
            var id = GL.GetUniformLocation(Program, name);
            var err = GL.GetError();
            var count = values.Length;
            if (id == -1)
                return;
            if (count == 1)
                GL.Uniform1(id, values[0]);
            else if (count == 2)
                GL.Uniform2(id, values[0], values[1]);
            else if (count == 3)
                GL.Uniform3(id, values[0], values[1], values[2]);
            else if (count == 4)
                GL.Uniform4(id, values[0], values[1], values[2], values[3]);
            else
                GL.UniformMatrix4(id, 1, false, values);//Передача матрицы в шейдер
        }
        /// <summary>
        /// Подгружает из файла исходный код шейдера, указанного типа, компилирует и привязывает к идентификатору шейдера
        /// </summary>
        /// <param name="type">Тип шейдера (вершинный, фрагментный и т.д)</param>
        /// <param name="path">Путь до файла</param>
        public void CreateShaderFromFile(ShaderType type, string path)
        {
            var data = new List<string>();
            using (var reader = new StreamReader(path))
            {
                data.Add(reader.ReadToEnd());
            }
            CreateShaderFromString(type, data.ToArray());
        }
        /// <summary>
        /// Принимает массив строк(исходного кода шейдера) указанного типа шейдера и привязывает к идентификатору шейдера
        /// </summary>
        /// <param name="type">Тип шейдера (вершинный, фрагментный и т.д)</param>
        /// <param name="data">Массив строк исходного кода шейдера</param>
        public void CreateShaderFromString(ShaderType type, string[] data)
        {
            var length = 0;
            var shader = GL.CreateShader(type);
            GL.ShaderSource(shader, data.Length, data, null as int[]);
            try
            {
                GL.CompileShader(shader);
                int status;
                GL.GetShader(shader, ShaderParameter.CompileStatus, out status);
                if (status == 0)
                    throw new Exception();
                if (type == ShaderType.VertexShader)
                    Vertex = shader;
                else if (type == ShaderType.FragmentShader)
                    Fragment = shader;
                else if (type == ShaderType.GeometryShaderExt)
                    Geometry = shader;
            }
            catch
            {
                var sb = string.Empty;
                GL.GetShaderInfoLog(shader, 1024, out length, out sb);
                throw new InvalidOperationException($"Shader compile error ({type}): {sb}");
            }
        }
        /// <summary>
        /// Связывает все шейдеры с указанной программой
        /// </summary>
        /// <param name="tfVars">Название строк переменных для Transform Feedback, которые должны быть заданы до линковки программы</param>
        /// <exception cref="Exception">Исключение, если связывание не удалось</exception>
        public void Link(string[] tfVars = null)
        {
            Program = GL.CreateProgram();
            if (Vertex != 0)
                GL.AttachShader(Program, Vertex);
            if (Fragment != 0)
                GL.AttachShader(Program, Fragment);
            if (Geometry != 0)
                GL.AttachShader(Program, Geometry);
            try
            {
                if(tfVars != null)
                    GL.TransformFeedbackVaryings(Program, tfVars.Length, tfVars, TransformFeedbackMode.SeparateAttribs);
                GL.LinkProgram(Program);
                int status;
                GL.GetProgram(Program, GetProgramParameterName.LinkStatus, out status);
                if (status == 0)
                    throw new Exception();
            }
            catch
            {
                var length = 0;
                var sb = string.Empty;
                GL.GetProgramInfoLog(Program, 1024, out length, out sb);
                throw new InvalidOperationException($"Shader program link error: {sb}");
            }
        }
        /// <summary>
        /// Освобождает все ресурсы, выделенные программой (шейдер и сама программа)
        /// </summary>
        public void Dispose()
        {
            GL.DetachShader(Program, Vertex);
            GL.DeleteShader(Vertex);
            GL.DetachShader(Program, Fragment);
            GL.DeleteShader(Fragment);
            GL.DetachShader(Program, Geometry);
            GL.DeleteShader(Geometry);
            GL.DeleteProgram(Program);
        }
    }
}
