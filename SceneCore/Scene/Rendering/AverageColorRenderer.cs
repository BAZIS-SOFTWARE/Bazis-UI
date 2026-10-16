using System;
using System.Drawing;
using BazisGUI.Scene.Interfaces;
using BazisGUI.Scene.VBO;
using OpenTK.Graphics.OpenGL;


namespace BazisGUI.Scene
{
    /// <summary>
    /// Класс для корректной визуализации прозрачных и непрозрачных объектов на сцене
    /// </summary>
    public class AverageColorRenderer : IDisposable, IActiveDrawingObject
    {
        int nodesDepthTex = 0;//Буфер глубины для узлов
        int nodesColorTex = 0;//Текстура цвета для узлов
        int linesDepthTex = 0;//Буфер глубины для 1D-элементов
        int linesColorTex = 0;//Текстура цвета для линий
        int frameDepthTex = 0;//Буфер глубины для ребер
        int frameColorTex = 0;//Текстура цвета для ребер
        int opaqueDepthTex = 0;//Буфер глубины непрозрачных объектов
        int opaqueColorTex = 0;//Текстура цвета непрозрачных объектов
        int transpDepthTex = 0;//Общий буфер глубины для объектов типа Surface и LineSurface
        int transpColorTex = 0;//Текстура цвета для объектов типа Surface и LineSurface
        int transpCountTex = 0;//Текстура количества фрагменетов накопленных при смешивании

        int quadDisplayList = 0;//Список отображения для рендера на плоскость
        public uint[] Fbo { get; private set; } = new uint[5];//0 - узлы, 1 - линии, 2 - ребра поверхностей, 3 - непрозрач. поверхности и текст,
                                                              //4 - прозрач. поверхности

        //int surfacePassCount = 0;
        ///
        public ShaderProgramCreator SurfaceShader { get; private set; }//Программа смешивания объектов типа Surface и LineSurface
        ///
        public ShaderProgramCreator BlendShader { get; private set; }//Программа финального смешивания основных и вспомогательных объектов
        /// <summary>
        /// Включить расчет освещения
        /// </summary>
        public bool IsLighting { get; set; }
        /// <summary>
        /// Игнорировать задние ребра сурфейсов
        /// </summary>
        public bool ShowSurfaceBackEdges { get; set; }
        /// <summary>
        /// Установка заднего цвета - используется при смешивании цветов
        /// </summary>
        public Color BackgroundColor { get; set; }
        /// <summary>
        /// Включение\выключения усредненного рендера
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>Целевой буфер графического хоста после операций с внутренними FBO.</summary>
        public int TargetFramebuffer { get; set; }
        ///
        public AverageColorRenderer(int width, int height)
        {
            SurfaceShader = new ShaderProgramCreator();

            var bV_1 = ShaderCollections.baseVertex[1];
            ChangeCompilationCondition(1, ShaderCollections.baseVertex, "#define TRANSPARENT\n");

            SurfaceShader.CreateShaderFromString(ShaderType.VertexShader, ShaderCollections.baseVertex);
            SurfaceShader.CreateShaderFromString(ShaderType.FragmentShader, ShaderCollections.baseFragment);
            SurfaceShader.Link();

            ChangeCompilationCondition(1, ShaderCollections.baseVertex, bV_1);

            BlendShader = new ShaderProgramCreator();
            BlendShader.CreateShaderFromString(ShaderType.VertexShader, ShaderCollections.averageColorFinalBlendVertex);
            BlendShader.CreateShaderFromString(ShaderType.FragmentShader, ShaderCollections.averageColorFinalBlendFragment);
            BlendShader.Link();

            InitBuffers(width, height);
            MakeFullScreenQuad();
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, TargetFramebuffer);
        }


        /// <summary>
        /// Выполнить действия перед вызовом glDrawElements
        /// </summary>
        /// <param name="vbo">[In]Вбо-объект, который вызывает отрисовку</param>
        /// <param name="elements">[In]Элемент отрисовки</param>
        public void DoActionsBeforeDrawing(VBObject vbo, DrawElements elements)
        {
            var index = (int)elements;
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, Fbo[index]);
            if (elements == DrawElements.Surfaces)
            {
                GL.DrawBuffer(DrawBufferMode.Back);
                GL.ColorMask(false, false, false, false);//Проход только по буферу глубины
                vbo.Draw();
                GL.ColorMask(true, true, true, true);

                GL.DrawBuffers(2, [DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1]);
                GL.Disable(EnableCap.DepthTest);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
                GL.BlendEquation(BlendEquationMode.FuncAdd);
                SurfaceShader.Bind();
                SurfaceShader.SetUniform("isLighting", [Convert.ToInt32(IsLighting)]);
            }
            else if (elements == DrawElements.Wireframe)
            {
                GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
                if (!ShowSurfaceBackEdges)
                {
                    GL.Enable(EnableCap.CullFace);
                    GL.CullFace(TriangleFace.Back);
                }
            }
            else if (elements == DrawElements.GeometryObjects)
                GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
        }
        /// <summary>
        /// Выполнить действия после вызова glDrawElements
        /// </summary>
        /// <param name="vbo">[In]Вбо-объект, который заканчивает отрисовку</param>
        /// <param name="elements">[In]Элемент отрисовки</param>
        public void DoActionsAfterDrawing(VBObject vbo, DrawElements elements)
        {
            if (elements == DrawElements.Surfaces)
            {
                GL.Enable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Blend);
            }
            else if (elements == DrawElements.Wireframe)
            {
                if (!ShowSurfaceBackEdges)
                    GL.Disable(EnableCap.CullFace);
            }
            SurfaceShader.Unbind();
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, TargetFramebuffer);
            GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);//Необходимо вызывать чтобы избежать артефакты при рендеринге
        }
        /// <summary>
        /// Смешивает прозрачные и непрозрачные объекты отображая их на плоскость
        /// </summary>
        public void BlendFramebuffers()
        {
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, TargetFramebuffer);
            GL.DrawBuffer(TargetFramebuffer == 0 ? DrawBufferMode.Back : DrawBufferMode.ColorAttachment0);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);

            BlendShader.Bind();
            var back = new float[] { BackgroundColor.R / 255f, BackgroundColor.G / 255f, BackgroundColor.B / 255f };
            BlendShader.SetUniform("backColor", back);
            BlendShader.BindTextureRect("nodesDepth", nodesDepthTex, 0);
            BlendShader.BindTextureRect("nodesColor", nodesColorTex, 1);
            BlendShader.BindTextureRect("linesDepth", linesDepthTex, 2);
            BlendShader.BindTextureRect("linesColor", linesColorTex, 3);
            BlendShader.BindTextureRect("frameDepth", frameDepthTex, 4);
            BlendShader.BindTextureRect("frameColor", frameColorTex, 5);
            BlendShader.BindTextureRect("opaqueDepth", opaqueDepthTex, 6);
            BlendShader.BindTextureRect("opaqueColor", opaqueColorTex, 7);
            BlendShader.BindTextureRect("transpDepth", transpDepthTex, 8);
            BlendShader.BindTextureRect("transpColor", transpColorTex, 9);
            BlendShader.BindTextureRect("transpCount", transpCountTex, 10);
            GL.DepthMask(false);
            GL.CallList(quadDisplayList);
            GL.DepthMask(true);
            BlendShader.Unbind();
            // Текстуры смешивания привязаны к блокам 0–10; после смешивания рисуются подписи и компас,
            // которые работают с блоком 0 — иначе атлас шрифта привязывается к последнему активному блоку.
            GL.ActiveTexture(TextureUnit.Texture0);
        }
        /// <summary>
        /// Очистка всех подключенных буфферов
        /// </summary>
        public void ClearColors()
        {
            for (var i = 0; i < 4; ++i)
            {
                GL.BindFramebuffer(FramebufferTarget.FramebufferExt, Fbo[i]);
                GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
                GL.ClearDepth(1);
                GL.ClearColor(0, 0, 0, 0);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            }
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, Fbo[4]);
            GL.DrawBuffers(2, [DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1]);
            GL.ClearColor(0, 0, 0, 0);
            GL.ClearDepth(1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, TargetFramebuffer);
        }
        /// <summary>
        /// Изменяет размеры текстур при изменении окна
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Reshape(int width, int height)
        {
            DeleteBuffersAndTextures();
            InitBuffers(width, height);
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, TargetFramebuffer);
        }
        /// <summary>
        /// Освобождает все привязанные объекты OpenGL
        /// </summary>
        public void Dispose()
        {
            SurfaceShader.Dispose();
            BlendShader.Dispose();
            DeleteBuffersAndTextures();
            GL.DeleteLists(quadDisplayList, 15);
        }
        /// <summary>
        /// Задает плоскость, на которую будет отображаться результат смешивания 
        /// </summary>
        public void MakeFullScreenQuad()
        {
            //var id = Thread.CurrentThread.ManagedThreadId;
            quadDisplayList = GL.GenLists(1);
            GL.NewList(quadDisplayList, ListMode.Compile);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0.0, 1.0, 0.0, 1.0f, -1.0, 1.0f);
            GL.Begin(PrimitiveType.Quads);
            {
                GL.Vertex2(0.0f, 0.0f);
                GL.Vertex2(1.0f, 0.0f);
                GL.Vertex2(1.0f, 1.0f);
                GL.Vertex2(0.0f, 1.0f);
            }
            GL.End();
            GL.PopMatrix();
            GL.EndList();
        }
        /// <summary>
        /// Инициализация текстур с настройкой параметров
        /// </summary>
        /// <param name="texture">[In]Индеск текстуры</param>
        /// <param name="width">[In]Ширина текстуры</param>
        /// <param name="height">[In]Высота текстуры</param>
        /// <param name="intFormat">[In]Внутренний формат текстуры</param>
        /// <param name="format">[In]Формат пикселей в памяти</param>
        private static void SetTexture(ref int texture, int width, int height, PixelInternalFormat intFormat, PixelFormat format)
        {
            GL.BindTexture(TextureTarget.TextureRectangleArb, texture);
            GL.TexParameter(TextureTarget.TextureRectangleArb, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.TextureRectangleArb, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.TextureRectangleArb, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.TextureRectangleArb, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexImage2D(TextureTarget.TextureRectangleArb, 0, intFormat,
                            width, height, 0, format, PixelType.Float, IntPtr.Zero);
        }
        /// <summary>
        /// Инициализация фреймбуфферов, текстур
        /// </summary>
        private void InitBuffers(int width, int height)
        {
            GL.GenFramebuffers(5, Fbo);

            GL.GenTextures(1, out nodesDepthTex);//Буфер глубины для узлов
            GL.GenTextures(1, out nodesColorTex);//Текстура цвета для узлов

            GL.GenTextures(1, out linesDepthTex);//Буфер глубины для 1D-элементов
            GL.GenTextures(1, out linesColorTex);//Текстура цвета 1D-элементов

            GL.GenTextures(1, out frameDepthTex);//Буфер глубины для ребер
            GL.GenTextures(1, out frameColorTex);//Текстура цвета для ребер

            GL.GenTextures(1, out opaqueDepthTex);//Общий буфер глубины для вспомогательных объектов
            GL.GenTextures(1, out opaqueColorTex);//Общий буфер глубины для вспомогательных объектов

            GL.GenTextures(1, out transpDepthTex);//Общий буфер глубины для объектов типа Surface и LineSurface
            GL.GenTextures(1, out transpColorTex);//Текстура цвета для объектов типа Surface и LineSurface
            GL.GenTextures(1, out transpCountTex);//Текстура количества фрагменетов накопленных при смешивании

            SetTexture(ref nodesDepthTex, width, height, PixelInternalFormat.DepthComponent16, PixelFormat.DepthComponent);
            SetTexture(ref nodesColorTex, width, height, PixelInternalFormat.Rgba32f, PixelFormat.Rgba);

            SetTexture(ref linesDepthTex, width, height, PixelInternalFormat.DepthComponent16, PixelFormat.DepthComponent);
            SetTexture(ref linesColorTex, width, height, PixelInternalFormat.Rgba32f, PixelFormat.Rgba);

            SetTexture(ref frameDepthTex, width, height, PixelInternalFormat.DepthComponent16, PixelFormat.DepthComponent);
            SetTexture(ref frameColorTex, width, height, PixelInternalFormat.Rgba32f, PixelFormat.Rgba);

            SetTexture(ref opaqueDepthTex, width, height, PixelInternalFormat.DepthComponent16, PixelFormat.DepthComponent);
            SetTexture(ref opaqueColorTex, width, height, PixelInternalFormat.Rgba32f, PixelFormat.Rgba);

            SetTexture(ref transpDepthTex, width, height, PixelInternalFormat.DepthComponent16, PixelFormat.DepthComponent);
            SetTexture(ref transpColorTex, width, height, PixelInternalFormat.Rgba32f, PixelFormat.Rgba);
            SetTexture(ref transpCountTex, width, height, PixelInternalFormat.R32f, PixelFormat.Red);

            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, Fbo[0]);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, TextureTarget.TextureRectangleArb, (uint)nodesDepthTex, 0);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.TextureRectangleArb, (uint)nodesColorTex, 0);

            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, Fbo[1]);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, TextureTarget.TextureRectangleArb, (uint)linesDepthTex, 0);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.TextureRectangleArb, (uint)linesColorTex, 0);

            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, Fbo[2]);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, TextureTarget.TextureRectangleArb, (uint)frameDepthTex, 0);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.TextureRectangleArb, (uint)frameColorTex, 0);

            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, Fbo[3]);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, TextureTarget.TextureRectangleArb, (uint)opaqueDepthTex, 0);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.TextureRectangleArb, (uint)opaqueColorTex, 0);

            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, Fbo[4]);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, TextureTarget.TextureRectangleArb, (uint)transpDepthTex, 0);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.TextureRectangleArb, (uint)transpColorTex, 0);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment1Ext, TextureTarget.TextureRectangleArb, (uint)transpCountTex, 0);
        }
        /*
        /// <summary>
        /// Прикрепляет вершинный и фрагментный шейдер к указанной программе
        /// </summary>
        /// <param name="program">[In]Программа</param>
        /// <param name="vertexCode">[In]Массив строк кода вершинного шейдера</param>
        /// <param name="fragmentCode">[In]Массив строк кода фрагментного шейдера</param>
        private void AttachShadersToProgram(ShaderProgramCreator program, string[] vertexCode, string[] fragmentCode)
        {
            program.CreateShaderFromString(Gl.GL_VERTEX_SHADER, vertexCode);
            program.CreateShaderFromString(Gl.GL_FRAGMENT_SHADER, fragmentCode);
            program.Link();
        }*/
        /// <summary>
        /// Удаляет существующие фреймбуфферы и текстуры
        /// </summary>
        private void DeleteBuffersAndTextures()
        {
            GL.DeleteFramebuffers(5, Fbo);
            GL.DeleteTextures(11, [nodesDepthTex, nodesColorTex, linesDepthTex, linesColorTex,
                                               frameDepthTex, frameColorTex, opaqueDepthTex, opaqueColorTex,
                                               transpDepthTex, transpColorTex, transpCountTex]);
        }

        private static void ChangeCompilationCondition(int position, string[] source, string newCondition)
        {
            source[position] = newCondition;
        }
    }
}
