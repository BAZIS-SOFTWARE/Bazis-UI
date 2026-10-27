
using BazisGUI.SceneCore.VBO;
using Geometry;
using OpenTK.Graphics.OpenGL;
using System;

namespace BazisGUI.SceneCore
{
    /// <summary>
    /// Класс для визуализации отсекающей плоскости
    /// </summary>
    public class ClipPlaneRenderer
    {
        /// <summary>
        /// Возвращает программу для отрисовки
        /// </summary>
        public ShaderProgramCreator Program { get; set; }
        /// <summary>
        /// Конструктор класса-визуализатора отсекающей плоскости
        /// </summary>
        public ClipPlaneRenderer()
        {
            Program = new ShaderProgramCreator();
            Program.CreateShaderFromString(ShaderType.VertexShader, ShaderCollections.clipPlaneVertex);
            Program.CreateShaderFromString(ShaderType.GeometryShaderExt, ShaderCollections.clipPlaneGeometry);
            Program.CreateShaderFromString(ShaderType.FragmentShader, ShaderCollections.clipPlaneFragment);
            Program.Link();
        }
        /// <summary>
        /// Очищает все неуправляемые ресурсы
        /// </summary>
        public void Dispose() => Program.Dispose();
    }
}
