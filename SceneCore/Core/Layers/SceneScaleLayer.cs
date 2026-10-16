using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BazisGUI.Scene; // SceneScale (существующий переиспользуемый класс)
using BazisGUI.Scene.Core.Rendering;
using BazisGUI.Scene.Core.Text;
using Geometry;
using OpenTK.Graphics.OpenGL;
using PostProc;

namespace BazisGUI.Scene.Core.Layers
{
    /// <summary>
    /// Перенос BaseForm.DisplaySceneScale(title, info) поверх переиспользуемого SceneScale
    /// (цветные прямоугольники шкалы рисует он же, без изменений). Центрирование заголовка
    /// раньше считалось через Graphics.MeasureString (System.Drawing.Common) — здесь заменено
    /// приблизительной оценкой ширины по количеству символов, так как GDI+ в ядре запрещён
    /// (см. scene.avalonia.md, раздел 2).
    /// </summary>
    public class SceneScaleLayer : ISceneLayer
    {
        private const float ApproxCharWidth = 6f;

        public string Name => "SceneScale";
        public int Order { get; set; } = 60;
        public bool IsVisible { get; set; } = true;

        private readonly ISceneTextRenderer textRenderer;
        private SceneScale scale;
        private IEnumerable<ItemRange> items;

        public SceneScaleLayer(ISceneTextRenderer textRenderer)
        {
            this.textRenderer = textRenderer;
        }

        public void Show(SceneScale scale, IEnumerable<ItemRange> items)
        {
            this.scale = scale;
            this.items = items;
        }

        public void Clear()
        {
            scale = null;
            items = null;
        }

        public void Draw(IRenderContext context)
        {
            if (scale == null || items == null)
                return;

            var itemList = items.ToList();
            if (itemList.Count == 0)
                return;

            var viewport = context.Viewport;
            var length = viewport.Height - scale.Coord_Y - 100;
            var gapY = 2;
            var cellSizeY = (length - (itemList.Count - 1) * gapY) / itemList.Count;
            var stepY = cellSizeY + gapY;

            // Как BaseForm.Initialize_GUI_Plane: прямоугольники шкалы задаются в пикселях окна.
            var lighting = GL.IsEnabled(EnableCap.Lighting);
            var depthTest = GL.IsEnabled(EnableCap.DepthTest);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.DepthTest);
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(0, viewport.Width, 0, viewport.Height, 0.1, 200);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();

            scale.DisplayScale(scale.Coord_X, scale.Coord_Y, gapY, cellSizeY, stepY, itemList);

            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix();
            if (depthTest)
                GL.Enable(EnableCap.DepthTest);
            if (lighting)
                GL.Enable(EnableCap.Lighting);

            var posY = scale.Coord_Y;
            foreach (var item in itemList)
            {
                DrawLabel(item.Min.ToString(), new Point2D(scale.Coord_X + 20, posY), context);
                DrawLabel(item.Max.ToString(), new Point2D(scale.Coord_X + 20, posY + stepY), context);
                posY += stepY;
            }

            DrawLabel(scale.Title, new Point2D(scale.Coord_X - (scale.Title?.Length ?? 0) * ApproxCharWidth / 2, posY + 30), context);
            DrawLabel(scale.Info, new Point2D(scale.Coord_X - (scale.Info?.Length ?? 0) * ApproxCharWidth / 2, posY + 15), context);
        }

        private void DrawLabel(string text, Point2D position, IRenderContext context)
        {
            textRenderer.DrawText2D(new TextLabel { Text = text, Color = Color.Black, Position2D = position, IsScreenSpace = true }, context);
        }
    }
}
