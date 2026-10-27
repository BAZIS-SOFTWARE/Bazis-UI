using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BazisGUI.SceneCore.Camera;
using BazisGUI.SceneCore.Capture;
using BazisGUI.SceneCore.Input;
using BazisGUI.SceneCore.Layers;
using BazisGUI.SceneCore.Picking;
using BazisGUI.SceneCore.Primitives;
using BazisGUI.SceneCore.Rendering;
using BazisGUI.SceneCore.Text;
using BazisGUI.SceneCore.EventsArgs;
using BazisGUI.SceneCore.Interfaces;
using BazisGUI.SceneCore.VBO;
using BazisGUI.SceneCore; // AverageColorRenderer, Advanced3DClipper, ClipPlaneRenderer, SceneScale, ScenePath
using Geometry;
using BazisGUI.SceneCore.Coordinates;
using MathNet.Numerics.LinearAlgebra;
using OpenTK.Graphics.OpenGL;

using RectangleBox = BazisGUI.SceneCore.Picking.RectangleBox;

namespace BazisGUI.SceneCore
{
    /// <summary>
    /// Заменяет ~40 partial-файлов GUI/Methods/Scene/*.cs и реализацию ISceneControl,
    /// которая раньше должна была жить в BaseForm (см. scene.avalonia.md, раздел 6).
    /// ISceneControl сегодня в проекте никем не реализован — это уже был неподключённый,
    /// местами не полностью реализованный контракт (например RotateObjs/CreateScaleObject
    /// не имели реализации нигде в коде); там, где аналога в BaseForm не нашлось,
    /// реализация даётся по смыслу метода, с пометкой в комментарии.
    /// </summary>
    public class SceneController : ISceneControl, IDisposable
    {
        private readonly SceneCamera camera = new SceneCamera();
        private readonly VBOController vboController = new VBOController();
        private readonly SceneRenderSettings settings = new SceneRenderSettings();
        private readonly ScenePicker picker;
        private readonly SceneInputController input;
        private readonly IQuadricMeshFactory meshFactory = new QuadricMeshFactory();
        private readonly ISceneTextRenderer textRenderer;
        private readonly Dictionary<ISceneScale, List<SceneScaleRange>> scaleItems = new Dictionary<ISceneScale, List<SceneScaleRange>>();

        private SceneLayerCollection layers;
        private SceneRenderer renderer;
        private AverageColorRenderer averageColorRenderer;
        private Advanced3DClipper advanced3DClipper;
        private ClipPlaneRenderer clipPlaneRenderer;

        private BasisLayer basisLayer;
        private RotationPointLayer rotationPointLayer;
        private GeometryObjectsLayer geometryObjectsLayer;
        private ClipPlaneLayer clipPlaneLayer;
        private ReflectionPlaneLayer reflectionPlaneLayer;
        private ModelObjectsLayer modelObjectsLayer;
        private CompassLayer compassLayer;
        private TextLayer textLayer;
        private SceneScaleLayer sceneScaleLayer;
        private SelectionRectangleLayer selectionRectangleLayer;

        public event Action RenderRequested;
        public event EventHandler<SelectObjectsEventArgs> SelectionChanged;
        public event EventHandler<InfoObjectsEventArgs> InfoRequested;

        public event Action SceneControlExpandEvent;
        public event Action SceneControlFoldEvent;

        public bool MouseMoveFlag { get; private set; }

        public SceneController(ISceneTextRenderer textRenderer = null)
        {
            this.textRenderer = textRenderer ?? new SkiaGlyphAtlasTextRenderer();
            picker = new ScenePicker(camera);
            input = new SceneInputController(camera);

            picker.ObjectsHit += (name, numbers) => InfoRequested?.Invoke(this, new InfoObjectsEventArgs(name, numbers));
            input.Invalidated += () => RenderRequested?.Invoke();
            input.SelectionRequested += (s, e) => SelectionChanged?.Invoke(this, e);
            input.RotationPointVisibilityChanged += visible =>
            {
                if (rotationPointLayer != null)
                    rotationPointLayer.IsVisible = visible;
                RenderRequested?.Invoke();
            };
            input.FitToScreenRequested += () => { FitObjectsToScreen(); RenderRequested?.Invoke(); };
        }

        public void Initialization()
        {
            var width = Math.Max(camera.Width, 1);
            var height = Math.Max(camera.Height, 1);

            averageColorRenderer = new AverageColorRenderer(width, height) { BackgroundColor = settings.BackGroundColor };
            advanced3DClipper = new Advanced3DClipper();
            clipPlaneRenderer = new ClipPlaneRenderer();

            basisLayer = new BasisLayer(meshFactory) { IsVisible = settings.DisplayBasis };
            rotationPointLayer = new RotationPointLayer(meshFactory);
            geometryObjectsLayer = new GeometryObjectsLayer();
            reflectionPlaneLayer = new ReflectionPlaneLayer();
            clipPlaneLayer = new ClipPlaneLayer(vboController, advanced3DClipper);
            modelObjectsLayer = new ModelObjectsLayer(vboController);
            compassLayer = new CompassLayer(meshFactory, textRenderer) { IsVisible = settings.DisplayCompass };
            textLayer = new TextLayer(textRenderer);
            sceneScaleLayer = new SceneScaleLayer(textRenderer);
            selectionRectangleLayer = new SelectionRectangleLayer();

            layers = new SceneLayerCollection();
            layers.Add(basisLayer);
            layers.Add(rotationPointLayer);
            layers.Add(geometryObjectsLayer);
            layers.Add(reflectionPlaneLayer);
            layers.Add(clipPlaneLayer);
            layers.Add(modelObjectsLayer);
            layers.Add(compassLayer);
            layers.Add(textLayer);
            layers.Add(sceneScaleLayer);
            layers.Add(selectionRectangleLayer);

            renderer = new SceneRenderer(layers, averageColorRenderer, advanced3DClipper);

            camera.Initialize(0, 0, -5);
            textRenderer.SetFont(null, 12f);

            GL.Enable(EnableCap.DepthTest); // разовая настройка контекста, как в старом CameraInitialization
            Resize(width, height);
        }

        public void Resize(int width, int height)
        {
            // Нулевой размер (свёрнутое окно, панель до раскладки) даёт деление на ноль в матрице проекции
            width = Math.Max(width, 1);
            height = Math.Max(height, 1);
            camera.Width = width;
            camera.Height = height;
            input.Viewport = new Viewport(width, height);

            GL.Viewport(0, 0, width, height);
            UpdateProjection();
            renderer?.Reshape(width, height);
        }

        /// <summary>
        /// Освобождает GL-ресурсы (шейдеры, FBO, текстуры атласа шрифта, ClipPlaneRenderer,
        /// меши вспомогательной геометрии и слоёв базиса/точки вращения/компаса).
        /// Буферы VboController освобождает хост (как и раньше в BaseForm/SceneSurface).
        /// </summary>
        public void Dispose()
        {
            geometryObjectsLayer?.Clear();
            if (layers != null)
                foreach (var layer in layers.GetOrdered())
                    (layer as IDisposable)?.Dispose();
            renderer?.Dispose();
            clipPlaneRenderer?.Dispose();
            (textRenderer as IDisposable)?.Dispose();
        }

        public void OnPointerPressed(SceneMouseEventArgs args)
        {
            MouseMoveFlag = false;
            input.OnPointerPressed(args);
        }

        public void OnPointerMoved(SceneMouseEventArgs args)
        {
            MouseMoveFlag = true;
            input.OnPointerMoved(args);
        }

        public void OnPointerReleased(SceneMouseEventArgs args) => input.OnPointerReleased(args);

        public void OnWheelChanged(SceneMouseEventArgs args) => input.OnWheelChanged(args);

        public void OnKeyDown(SceneKeyEventArgs args) => input.OnKeyDown(args);

        public ISceneCamera GetCamera() => camera;

        /// <summary>
        /// Доступ к внутренним объектам для постепенного переноса BaseForm на SceneController:
        /// пока не весь рендер-код перенесён (см. GUI/Documents/scene.avalonia.md, шаг 4),
        /// часть кода в BaseForm обращается к этим объектам напрямую (например,
        /// UtilityToolStrip.cs выставляет advanced3DClipper.LayerThickness/ClipMode).
        /// </summary>
        public VBOController VboController => vboController;
        public AverageColorRenderer AverageColorRenderer => averageColorRenderer;
        public Advanced3DClipper Advanced3DClipper => advanced3DClipper;
        public ScreenRectangle SelectionRectangle => selectionRectangleLayer.Rectangle;

        /// <summary>
        /// Открыт для хоста: например, если это WglBitmapFontTextRenderer, хосту нужно вызвать
        /// AttachToCurrentContext(hdc), когда GL-контекст станет текущим (см. BaseForm.SceneInitialization).
        /// </summary>
        public ISceneTextRenderer TextRenderer => textRenderer;

        public float ShadowAngle { get; set; }
        public bool IsSmoothShadow { get; set; }

        /// <summary>Преобразует вектор в плоскости камеры в вектор координат модели.</summary>
        public ModelVector3D GetSceneCoordOfScreenVector(float x, float y) => camera.GetSceneVectorOfScreenVector(x, y);

        public ViewProjection Projection
        {
            get => camera.Projection;
            set => camera.Projection = value;
        }

        public ViewAxis RotationAxis
        {
            get => input.RotationAxis;
            set => input.RotationAxis = value;
        }

        public float RotationAngle
        {
            get => input.RotationAngle;
            set => input.RotationAngle = value;
        }

        /// <summary>
        /// Угол обзора камеры в градусах (SettingsConfig.AngleOfProjection): по нему строятся и матрица
        /// проекции, и пересчёт экранных координат при выборе объектов.
        /// </summary>
        public float AngleOfProjection
        {
            get => camera.AngleOfProjection;
            set => camera.AngleOfProjection = value;
        }

        /// <summary>Прерывает начатое выделение рамкой (например, при потере захвата указателя).</summary>
        public void CancelSelection() => input.CancelSelection();

        public Color BackGroundColor
        {
            get => settings.BackGroundColor;
            set
            {
                settings.BackGroundColor = value;
                if (averageColorRenderer != null)
                    averageColorRenderer.BackgroundColor = value;
            }
        }

        public bool DisplayBasis
        {
            get => settings.DisplayBasis;
            set
            {
                settings.DisplayBasis = value;
                if (basisLayer != null)
                    basisLayer.IsVisible = value;
            }
        }

        public bool DisplayCompass
        {
            get => settings.DisplayCompass;
            set
            {
                settings.DisplayCompass = value;
                if (compassLayer != null)
                    compassLayer.IsVisible = value;
            }
        }

        public Color SelectionColor
        {
            get => settings.SelectionColor;
            set => settings.SelectionColor = value;
        }

        public bool IsCutting
        {
            get => settings.IsCutting;
            set => settings.IsCutting = value;
        }

        public bool IsLighting
        {
            get => settings.IsLighting;
            set
            {
                settings.IsLighting = value;
                if (averageColorRenderer != null)
                    averageColorRenderer.IsLighting = value; // синхронизация с рендером прозрачности, как в старом SetGeneralSettings
            }
        }

        public bool IsClipPlane
        {
            get => settings.IsClipPlane;
            set
            {
                settings.IsClipPlane = value;
                if (clipPlaneLayer != null)
                    clipPlaneLayer.IsVisible = value;
            }
        }

        public bool IsBlending
        {
            get => settings.IsBlending;
            set => settings.IsBlending = value;
        }

        public int SceneWidth => camera.Width;
        public int SceneHeight => camera.Height;
        public float ScaleFactor => camera.ScaleFactor;

        public float LightTranslateX
        {
            get => (float)settings.LighterPosition._x;
            set => settings.LighterPosition = new Point2D(value, settings.LighterPosition._y);
        }

        public float LightTranslateY
        {
            get => (float)settings.LighterPosition._y;
            set => settings.LighterPosition = new Point2D(settings.LighterPosition._x, value);
        }

        /// <summary>Не участвует в расчёте освещения — в исходном коде тоже не было реализации.</summary>
        public float LightTranslateZ { get; set; }
        public float LightAttenuation { get; set; }

        /// <summary>Устанавливает центр вращения в координатах модели.</summary>
        public void SetRotationCentre(ModelPoint3D modelPoint) => camera.SetRotationCentre(modelPoint);

        public bool IsVBObjectShown(string objsName) => vboController.IsVBObjectShown(objsName);
        public void SwitchOffVBObject(string objsName) => vboController.SwitchVBObject(objsName, false);
        public void SwitchOnVBObject(string objsName) => vboController.SwitchVBObject(objsName, true);

        public void FitObjectsToScreen()
        {
            camera.ResetPan();

            for (var i = 0; i < 3; i++)
            {
                var maxRad = 0f;
                foreach (var obj in vboController.GetVBObjs())
                {
                    foreach (var corner in obj.BoundingBox.GetCornerPoints())
                    {
                        var modelCorner = new ModelPoint3D(corner._x, corner._y, corner._z);
                        var sceneCorner = camera.GetSceneCoord(modelCorner);
                        var scr = camera.GetScreenCoord(sceneCorner);
                        var rad = (float)Math.Sqrt(scr.X * scr.X + scr.Y * scr.Y);
                        if (rad > maxRad) maxRad = rad;
                    }
                }

                if (maxRad == 0)
                    break;

                var factor = camera.Width > camera.Height
                    ? 1 / (maxRad / (camera.Height / 3f))
                    : 1 / (maxRad / (camera.Width / 3f));

                if (factor == 0) factor = 1;
                camera.Scale(factor);

                if (Math.Abs(factor - 1) < 0.1f)
                    break;
            }
        }

        /// <summary>В исходном коде метод интерфейса нигде не был реализован; здесь — разовый поворот по текущим RotationAxis/RotationAngle.</summary>
        public void RotateObjs() => camera.Rotate(RotationAxis, RotationAngle);

        /// <summary>Разовый поворот сцены на заданный угол вокруг оси — ответная часть кнопок поворота на 90°.</summary>
        public void RotateObjs(ViewAxis axis, float angle) => camera.Rotate(axis, angle);

        public void ScaleObjs(float scaleFactor) => camera.Scale(scaleFactor);

        public void PlaneObjs(ViewPlane plane) => camera.SetOnPlane(plane, camera.ScaleFactor);

        public void DisplayObjects() => DisplayObjects(0);

        /// <summary>Рисует сцену в буфер, предоставленный графическим хостом.</summary>
        public void DisplayObjects(int targetFramebuffer)
        {
            // Матрица проекции загружается каждый кадр: иначе после смены проекции/угла обзора без Resize
            // рисуется устаревшая матрица, а выбор объектов считает по текущей.
            UpdateProjection();
            renderer.Render(BuildContext(targetFramebuffer));
        }

        public IEnumerable<IVBObject> GetVBObjs() => vboController.GetVBObjs().Cast<IVBObject>();

        public bool DeleteVBObjects(string objsName) => vboController.DeleteVBObjects(objsName);
        public void DeleteAllVBObjects() => vboController.DeleteAllVBObjects();
        public void ChangeViewModeVBObjects(string objsName, ObjView objView) => vboController.ChangeViewModeVBObjects(objsName, objView);

        // Как в BaseForm.HideGeometryObj/FindGeometryObj: поиск по имени метода, добавившего объект
        // (ключ объекта — "<имя метода>:<guid>", см. GeometryKey).
        public void HideGeometryObj(string searchMethod)
        {
            if (searchMethod == nameof(DisplaySceneScale))
                sceneScaleLayer.Clear();
            if (searchMethod == nameof(DisplayReflectionPlane))
                reflectionPlaneLayer.Clear();
            geometryObjectsLayer.RemoveWhere(key => key.Contains(searchMethod));
        }

        public bool FindGeometryObj(string searchMethod) => geometryObjectsLayer.Any(key => key.Contains(searchMethod));

        public void HideAllGeometryObjs()
        {
            geometryObjectsLayer.Clear();
            sceneScaleLayer.Clear();
            reflectionPlaneLayer.Clear();
        }

        private static string GeometryKey([System.Runtime.CompilerServices.CallerMemberName] string method = "") =>
            $"{method}:{Guid.NewGuid()}";

        public void HideDisplayText3D() => textLayer.Labels.RemoveWhere(l => !l.IsScreenSpace);
        public void HideDisplayText2D() => textLayer.Labels.RemoveWhere(l => l.IsScreenSpace);

        public void DisplayText3D(string str, Color color, Point3D coord) =>
            textLayer.Labels.Add(new TextLabel { Text = str, Color = color, Position3D = coord, IsScreenSpace = false });

        public void DisplayText2D(string str, Color color, Point2D coord) =>
            textLayer.Labels.Add(new TextLabel { Text = str, Color = color, Position2D = coord, IsScreenSpace = true });

        /// <summary>
        /// Разбиение диапазона [min, max] на ranges элементов SceneScaleRange не было реализовано
        /// и в исходном коде (см. закомментированный SceneScale.Display/FillRange) — сюда не переносить нечего,
        /// возвращается объект шкалы без элементов легенды.
        /// </summary>
        public ISceneScale CreateScaleObject(float min, float max, decimal ranges, string title, string comments)
        {
            var scale = new SceneScale { Title = title, Info = comments };
            scaleItems[scale] = new List<SceneScaleRange>();
            return scale;
        }

        /// <summary>
        /// Перенос BaseForm.DisplaySceneScale(title, info): интервалы шкалы берутся из
        /// ResultsController.GetItems(), положение — из настроек (Scale_X_Coord/Scale_Y_Coord).
        /// </summary>
        public void DisplaySceneScale(string title, string info, IEnumerable<SceneScaleRange> items, int x, int y) =>
            sceneScaleLayer.Show(new SceneScale { Title = title, Info = info, Coord_X = x, Coord_Y = y }, items);

        public void DisplaySceneScale(ISceneScale scale)
        {
            if (scale is SceneScale concrete && scaleItems.TryGetValue(scale, out var items))
                sceneScaleLayer.Show(concrete, items);
        }

        public void DisplayLocalFrame(Frame frame)
        {
            geometryObjectsLayer.Add(GeometryKey(), ctx =>
            {
                var position = ctx.Camera.Position;
                GL.PushMatrix();
                GL.Translate(-position.X, -position.Y, -position.Z);
                GL.LineWidth(1.5f);
                GL.Begin(PrimitiveType.Lines);

                DrawAxis(frame.Centre, frame.Centre.Sum(frame.Dir_Z), 0, 0, 1f);
                DrawAxis(frame.Centre, frame.Centre.Sum(frame.Dir_Y), 0, 1f, 0);
                DrawAxis(frame.Centre, frame.Centre.Sum(frame.Dir_X), 1, 0.5f, 0);

                GL.End();
                GL.PopMatrix();
            });
        }

        private static void DrawAxis(Point3D from, Point3D to, float r, float g, float b)
        {
            GL.Color3(r, g, b);
            GL.Vertex3(from._x, from._y, from._z);
            GL.Color3(r, g, b);
            GL.Vertex3(to._x, to._y, to._z);
        }

        public void DisplayPath(Point3D[] points)
        {
            if (points.Length <= 1)
                return;

            var path = new ScenePath(points);
            geometryObjectsLayer.Add(GeometryKey(), ctx => path.Display(ctx.Camera.Position));

            var p0 = path[path.PointsQuantity - 2];
            var p1 = path[path.PointsQuantity - 1];
            DisplayText3D(path.Length.ToString(), Color.Black,
                new Point3D((p0._x + p1._x) / 2, (p0._y + p1._y) / 2, (p0._z + p1._z) / 2));
        }

        public void DisplayLine(Point3D p0, Point3D p1, Color objColor) => AddLine(p0, p1, objColor, GeometryKey());
        public void DisplaySpiral(Point3D p0, Point3D p1, Color objColor) => AddLine(p0, p1, objColor, GeometryKey());

        /// <summary>
        /// Перенос BaseForm.DisplayVector: отрезок из точки posit длиной length, масштаб которого
        /// не зависит от приближения камеры (делится на ScaleFactor).
        /// </summary>
        public void DisplayVector(Point3D length, Point3D posit, Color objColor)
        {
            geometryObjectsLayer.Add(GeometryKey(), ctx =>
            {
                var position = ctx.Camera.Position;
                var scale = 1 / ctx.ScaleFactor;
                GL.PushMatrix();
                GL.Translate(-position.X, -position.Y, -position.Z);
                GL.Translate(posit._x, posit._y, posit._z);
                GL.Scale(scale, scale, scale);
                GL.Color3(objColor.R, objColor.G, objColor.B);
                GL.LineWidth(5.0f);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(length._x, length._y, length._z);
                GL.End();
                GL.PopMatrix();
            });
        }

        /// <summary>
        /// Произвольный вспомогательный объект, рисуемый в проходе геометрии.
        /// Ключ <paramref name="name"/> — то же, что имя метода в HideGeometryObj/FindGeometryObj.
        /// </summary>
        public void DisplayGeometryObject(string name, Action<IRenderContext> draw) =>
            geometryObjectsLayer.Add(GeometryKey(name), draw);

        private void AddLine(Point3D p0, Point3D p1, Color objColor, string key)
        {
            geometryObjectsLayer.Add(key, ctx =>
            {
                var position = ctx.Camera.Position;
                GL.PushMatrix();
                GL.Translate(-position.X, -position.Y, -position.Z);
                GL.Color3(objColor.R, objColor.G, objColor.B);
                GL.LineWidth(5.0f);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(p0._x, p0._y, p0._z);
                GL.Vertex3(p1._x, p1._y, p1._z);
                GL.End();
                GL.PopMatrix();
            });
        }

        // DisplayConus/DisplaySphere/DisplayMarker создают GL-буферы меша на каждый вызов: меш передаётся
        // слою как принадлежащий объекту и удаляется при HideGeometryObj/HideAllGeometryObjs/Dispose.
        public void DisplayConus(float upperDiam, float bottomDiam, float length, Frame frame)
        {
            var mesh = meshFactory.CreateCylinder(bottomDiam / 2, upperDiam / 2, length, Color.Red);
            geometryObjectsLayer.Add(GeometryKey(), ctx =>
            {
                var position = ctx.Camera.Position;
                GL.PushMatrix();
                GL.Translate(-position.X, -position.Y, -position.Z);
                GL.Translate(frame.Centre._x, frame.Centre._y, frame.Centre._z);

                var angle = (float)(Math.Acos(Vector.GetCosAngleVectors(new Point3D(0, 0, 1), frame.Dir_Z)) * 180 / Math.PI);
                var axis = Vector.CrossProd(new Point3D(0, 0, 1), frame.Dir_Z);
                GL.Rotate(angle, axis._x, axis._y, axis._z);
                GL.Translate(0, 0, -length);

                mesh.Load();
                GL.PopMatrix();
            }, mesh);
        }

        public void DisplaySphere(float width, Frame frame)
        {
            var mesh = meshFactory.CreateSphere(width / 2, Color.Red);
            geometryObjectsLayer.Add(GeometryKey(), ctx =>
            {
                var position = ctx.Camera.Position;
                GL.PushMatrix();
                GL.Translate(-position.X, -position.Y, -position.Z);
                GL.Translate(frame.Centre._x, frame.Centre._y, frame.Centre._z);

                var angle = (float)(Math.Acos(Vector.GetCosAngleVectors(new Point3D(0, 0, 1), frame.Dir_Z)) * 180 / Math.PI);
                var axis = Vector.CrossProd(new Point3D(0, 0, 1), frame.Dir_Z);
                GL.Rotate(angle, axis._x, axis._y, axis._z);

                mesh.Load();
                GL.PopMatrix();
            }, mesh);
        }

        /// <summary>
        /// Сплошная сфера-маркер радиуса <paramref name="radius"/> в точке <paramref name="centre"/>
        /// (в BaseForm — gluSphere при показе направления группы).
        /// </summary>
        public void DisplayMarker(Point3D centre, float radius, Color color)
        {
            var mesh = meshFactory.CreateSphere(radius, color);
            geometryObjectsLayer.Add(GeometryKey(), ctx =>
            {
                var position = ctx.Camera.Position;
                GL.PushMatrix();
                GL.Translate(-position.X, -position.Y, -position.Z);
                GL.Translate(centre._x, centre._y, centre._z);
                mesh.Load();
                GL.PopMatrix();
            }, mesh);
        }

        public void DisplayDistance(Segment3D line)
        {
            geometryObjectsLayer.Add(GeometryKey(), ctx =>
            {
                var position = ctx.Camera.Position;
                GL.PushMatrix();
                GL.Translate(-position.X, -position.Y, -position.Z);
                GL.Color3(1f, 0, 0);
                GL.LineWidth(5.0f);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(line.P0._x, line.P0._y, line.P0._z);
                GL.Vertex3(line.P1._x, line.P1._y, line.P1._z);
                GL.End();
                GL.PopMatrix();
            });
        }

        public IVBObject FindVBObj(string objName) => vboController.FindVBObj(objName);

        public void UpdateProjection()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(camera.GetProjectionMatrix().AsColumnMajorArray());
            GL.MatrixMode(MatrixMode.Modelview);
        }

        public void SetTransparency(string objName, int alpha)
        {
            var vbo = vboController.FindVBObj(objName);
            if (vbo == null)
                return;

            var colors = vbo.PointsColors;
            for (var i = 3; i < colors.Length; i += 4)
                colors[i] = alpha / 255f;
            vbo.PointsColors = colors;
        }

        public void HideReflectionPlane() => reflectionPlaneLayer.Clear();

        public void DisplayReflectionPlane(string objName, float[] coeff) =>
            reflectionPlaneLayer.SetPlane(new Plane(new Point3D(coeff[0], coeff[1], coeff[2]), coeff[3]));

        public void CopyVBObjects(VBObject original, string copyName) => vboController.CopyVBObjects(original, copyName);

        /// <summary>Строит объект сечения по ограничивающему боксу видимых 3D-элементов — сам бокс собирает вызывающий код (Core не знает о "project").</summary>
        public void CreateClipPlane(BoundingBox bounds)
        {
            var data = ClipPlane.CreateBoundingBoxPlanes(bounds);
            var vbo = new ClipPlane("ClipPlane", data.Item1, data.Item2, data.Item3) { Renderer = clipPlaneRenderer };
            vboController.AddVbo(vbo);
        }

        public void DeleteClipPlane()
        {
            vboController.DeleteVBObjects("ClipPlane");
            clipPlaneLayer.Clear();
        }

        /// <summary>
        /// Задаёт плоскость сечения; сама перерисовка/пересчёт матриц идёт каждый кадр внутри
        /// ClipPlaneLayer.Draw (см. его комментарий) — плоскость не меняется от кадра к кадру,
        /// а вот матрица камеры меняется при вращении/панорамировании.
        /// </summary>
        public void ChangeClipPlane(Plane clipPlane) => clipPlaneLayer.SetPlane(clipPlane);

        /// <summary>
        /// Матрица объекта (модельная матрица) для VBO сечения: чистая математика без обращения
        /// к GL/VBO, вынесена из ChangeClipPlane отдельно, чтобы её можно было проверить тестами
        /// без живого GL-контекста (см. scene.avalonia.md — то же соображение, что и для камеры).
        /// </summary>
        public static Matrix<float> ComputeClipPlaneModelMatrix(Plane clipPlane, BoundingBox bounds)
        {
            var model = GlMatrix.Identity();
            var origin = clipPlane.Normal.Mult(clipPlane.Shifting);
            var sx = Math.Sign(clipPlane.Normal._x);
            var sy = Math.Sign(clipPlane.Normal._y);
            var sz = Math.Sign(clipPlane.Normal._z);
            var center = bounds.RightDownFar.Sum(bounds.LeftUpNear).Mult(0.5f);

            model = GlMatrix.Translate(model, center._x, center._y, center._z);
            model = GlMatrix.Translate(model, sx * origin._x, sy * origin._y, sz * origin._z);

            var angle = (float)(Math.Acos(Vector.GetCosAngleVectors(new Point3D(0, 0, -1), clipPlane.Normal)) * 180 / Math.PI);
            var axis = Vector.CrossProd(new Point3D(0, 0, -1), clipPlane.Normal);
            model = GlMatrix.Rotate(model, angle, axis._x, axis._y, axis._z);

            return model;
        }

        /// <summary>Геометрический подбор без обращения к "project"/IModelView — см. ScenePicker.</summary>
        public bool SelectByPoint(IEnumerable<Set> sets, ViewportPoint point, bool isSelected) =>
            picker.SelectByPoint(sets, point, isSelected);

        public void SelectByRect(IEnumerable<Set> sets, RectangleBox box, bool isSelected) =>
            picker.SelectByRect(sets, box, isSelected);

        public byte[] CaptureScreenshot(IFrameGrabber grabber) => grabber.Capture(new Viewport(camera.Width, camera.Height));

        private GlRenderContext BuildContext(int targetFramebuffer) => new GlRenderContext
        {
            Camera = camera,
            Viewport = new Viewport(camera.Width, camera.Height),
            Settings = settings,
            ScaleFactor = camera.ScaleFactor,
            TargetFramebuffer = targetFramebuffer
        };
    }
}
