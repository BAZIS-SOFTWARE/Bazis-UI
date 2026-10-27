using global::Avalonia.Controls;
using ClipControl = BazisGUI.Avalonia.Clip.ClipControl;
using ObjView = BazisGUI.SceneCore.Interfaces.ObjView;
using global::Avalonia.Threading;
using BazisGUI.Avalonia.Clip;
using BazisGUI.Avalonia.CrossSection;
using BazisGUI.SceneCore;
using BazisGUI.SceneCore;
using BazisGUI.SceneCore.VBO;
using Geometry;
using Model.GeometryObjects;
using Model.Interfaces;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Color = System.Drawing.Color;

namespace BazisGUI.Avalonia.Shell
{
    // Из GUI/Methods/UtilityToolStrip.cs (скрытие и рассечение плоскостью, захват данных transform feedback)
    // и GUI/Methods/Scene/ClipPlane.cs. Всё, что работает с буферами и GL, выполняется в кадре сцены.
    internal partial class MainWindow
    {
        private void скрытьПлоскостьюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as MenuItem;
                // CheckOnClick: MenuItem с ToggleType = CheckBox переключает IsChecked до события Click.
                if (btn.IsChecked)
                {
                    if (project == null)
                    {
                        btn.IsChecked = false;
                        return;
                    }

                    var clip = new ClipControl();
                    var clipForm = new Window()
                    {
                        Name = "clipPlaneForm",
                        Topmost = true,
                        Icon = Icon,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        CanResize = false,
                        CanMaximize = false,
                        Title = Resources.UtilityToolStip_ClipForm_Text,
                        ShowInTaskbar = false,
                        FontFamily = FontFamily,
                        FontSize = FontSize,
                        Content = clip
                    };

                    ChangeClipMode(ClipMode.Default);
                    clip.SwitchOnOff += (v) =>
                    {
                        if (v)
                        {
                            ChangeClipMode(clip.Regime);
                            CreateClipPlane();
                        }
                        else
                        {
                            DeleteClipPlane();
                            ChangeClipMode(ClipMode.None);
                            RequestRedraw();
                        }
                    };
                    clip.ChangeClipMode += mode => ChangeClipMode(mode);
                    clip.ChangeLayerThickness += layerThickness =>
                        scene.Surface.Invoke(sceneController => sceneController.Advanced3DClipper.LayerThickness = layerThickness);
                    clip.SetClipPlaneEvent += plane =>
                    {
                        var scPlane = new Geometry.Plane(new Point3D(plane.X, plane.Y, plane.Z), plane.D);
                        DisplayClipPlane(scPlane);
                    };
                    clip.RedrawClipPlane += RequestRedraw;
                    clip.CaptureData += CaptureData;
                    clipForm.Closed += (o, ev) =>
                    {
                        DeleteClipPlane();
                        ChangeClipMode(ClipMode.None);
                        btn.IsChecked = false;
                        RequestRedraw();
                    };
                    clipForm.Show(this);
                    clipForm.Position = Position;
                }
                else
                {
                    OwnedWindows.FirstOrDefault(x => x.Name == "clipPlaneForm")?.Close();
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        /// <summary>
        /// Закрывает окно «Скрыть плоскостью» и сбрасывает отсечение, чтобы плоскость и режим
        /// не перешли на буферы следующего проекта (SceneProjectPresenter подключает активный clipper).
        /// </summary>
        private void CloseClipPlaneForm()
        {
            OwnedWindows.FirstOrDefault(x => x.Name == "clipPlaneForm")?.Close();
            scene.Surface.Invoke(sceneController =>
            {
                sceneController.DeleteClipPlane();
                sceneController.Advanced3DClipper.ClipMode = ClipMode.None;
                GL.Disable(EnableCap.ClipPlane0);
            });
        }

        /// <summary>
        /// Смена режима отсечения для 3д элементов (BaseForm.ChangeClipMode для каждого набора 3D-элементов).
        /// </summary>
        public void ChangeClipMode(ClipMode mode)
        {
            var names = project.GetModelSetsInfo(ObjType.Элемент3D).Select(set => set.Name).ToList();
            scene.Surface.Invoke(sceneController =>
            {
                var clipper = sceneController.Advanced3DClipper;
                clipper.ClipMode = mode;
                foreach (var name in names)
                {
                    if (sceneController.VboController.FindVBObj(name) is not SurfaceObjects el3d)
                        continue;

                    if (mode == ClipMode.None)
                    {
                        el3d.ActiveDrawingObject = null;
                        GL.Disable(EnableCap.ClipPlane0);
                    }
                    else
                        el3d.ActiveDrawingObject = clipper;
                }
            });
        }

        /// <summary>BaseForm.CreateClipPlane: плоскость строится по габаритам видимых 3D-элементов.</summary>
        public void CreateClipPlane()
        {
            var names = project.GetModelSetsInfo(ObjType.Элемент3D)
                .Where(set => GetVisibleNumbers(set).Any())
                .Select(set => set.Name)
                .ToList();
            scene.Surface.Invoke(sceneController =>
            {
                BoundingBox current = null;
                foreach (var name in names)
                {
                    var vbo = sceneController.VboController.FindVBObj(name);
                    if (vbo != null)
                        current = current == null ? vbo.BoundingBox.Merge(null) : current.Merge(vbo.BoundingBox);
                }

                if (current != null)
                    sceneController.CreateClipPlane(current);
            });
        }

        public void DeleteClipPlane() => scene.Surface.Invoke(sceneController => sceneController.DeleteClipPlane());

        public void DisplayClipPlane(Geometry.Plane plane) => scene.Surface.Invoke(sceneController => sceneController.ChangeClipPlane(plane));

        /// <summary>
        /// Кнопка «Захват»: прогоняет видимые 3D-элементы через отсекатель с transform feedback и оставляет
        /// видимыми только элементы, попавшие в сечение (BaseForm.CaptureData).
        /// </summary>
        public void CaptureData()
        {
            var sets = project.GetModelSetsInfo(ObjType.Элемент3D).Where(v => GetVisibleNumbers(v).Any()).ToArray();
            scene.Surface.Invoke(sceneController =>
            {
                var dataBuffers = new List<int>();
                var tboBuffers = new List<int>();
                var queries = new List<int>();
                var vbos = sets.Select(set => sceneController.VboController.FindVBObj(set.Name))
                    .Select(vbo => vbo != null && vbo.ViewState && vbo.ActiveDrawingObject is Advanced3DClipper ? vbo : null)
                    .ToArray();

                List<List<int>> indices;
                try
                {
                    CreateCaptureData(vbos, dataBuffers, tboBuffers, queries);
                    RunTransformFeedback(vbos, tboBuffers, queries);
                    indices = FetchData(vbos, dataBuffers, queries);
                }
                finally
                {
                    RemoveCaptureData(dataBuffers, tboBuffers, queries);
                }

                // Изменение видимости поднимает ModelView.Changed — применяем его вне кадра отрисовки.
                Dispatcher.UIThread.Post(() => CreateCaptureElements(sets, indices));
            });
        }

        private static void CreateCaptureData(VBObject[] vbos, List<int> dataBuffers, List<int> tboBuffers, List<int> queries)
        {
            foreach (var vbo in vbos.Where(vbo => vbo != null))
            {
                var data = GL.GenBuffer();
                dataBuffers.Add(data);
                var dataSize = vbo.CoordLength / 3;
                GL.BindBuffer(BufferTarget.ArrayBuffer, data);
                GL.BufferData(BufferTarget.ArrayBuffer, dataSize * sizeof(int), nint.Zero, BufferUsageHint.DynamicCopy);
                var tbo = GL.GenTransformFeedback();
                tboBuffers.Add(tbo);
                GL.BindTransformFeedback(TransformFeedbackTarget.TransformFeedback, tbo);
                GL.BindBufferBase(BufferRangeTarget.TransformFeedbackBuffer, 0, data);
                queries.Add(GL.GenQuery());
            }
        }

        private static void RunTransformFeedback(VBObject[] vbos, List<int> tboBuffers, List<int> queries)
        {
            GL.Enable(EnableCap.RasterizerDiscard);
            try
            {
                var index = 0;
                foreach (var vbo in vbos.Where(vbo => vbo != null))
                {
                    var last = vbo.ViewMode;
                    var pObj = (Advanced3DClipper)vbo.ActiveDrawingObject;
                    try
                    {
                        vbo.ViewMode = ObjView.Surface;
                        pObj.QueryId = queries[index];
                        pObj.TBOId = tboBuffers[index];
                        vbo.Load();
                    }
                    finally
                    {
                        vbo.ViewMode = last;
                        pObj.QueryId = 0;
                        pObj.TBOId = 0;
                    }
                    ++index;
                }
            }
            finally
            {
                GL.Disable(EnableCap.RasterizerDiscard);
            }
        }

        /// <summary>Индексы элементов каждого набора, попавших в сечение (null — набор не участвовал в захвате).</summary>
        private static List<List<int>> FetchData(VBObject[] vbos, List<int> dataBuffers, List<int> queries)
        {
            var list = new List<List<int>>();
            var index = 0;
            foreach (var vbo in vbos)
            {
                if (vbo == null)
                {
                    list.Add(null);
                    continue;
                }

                var surfVbo = vbo as SurfaceObjects;
                var indices = new List<int>();
                list.Add(indices);
                GL.GetQueryObject(queries[index], GetQueryObjectParam.QueryResult, out long items);
                if (items > 0 && surfVbo != null)
                {
                    var size = vbo.CoordLength / 3;
                    var data = new int[size];
                    VBO.GetSubData(dataBuffers[index], 0, size * sizeof(int), data);
                    var separators = new int[surfVbo.SeparatorsLength];
                    VBO.GetSubData(surfVbo.SeparatorBuffer, 0, surfVbo.SeparatorsLength * sizeof(int), separators);
                    var inIndex = 0;
                    for (var j = 1; j < separators.Length && items > 0; ++j)
                    {
                        var minElemIndex = separators[j - 1] * 3;
                        var maxElemIndex = separators[j] * 3;
                        var minIndex = data[inIndex];
                        while (inIndex < data.Length && data[inIndex] >= minElemIndex && data[inIndex] < maxElemIndex)
                            ++inIndex;
                        var offset = inIndex < data.Length ? 0 : 1;
                        if (minIndex != data[inIndex - offset])
                        {
                            var maxIndex = data[inIndex - 1] + 1;
                            items -= (maxIndex - minIndex) / 3;
                            indices.Add(j - 1);
                        }
                    }
                }
                ++index;
            }
            return list;
        }

        private static void RemoveCaptureData(List<int> dataBuffers, List<int> tboBuffers, List<int> queries)
        {
            GL.BindTransformFeedback(TransformFeedbackTarget.TransformFeedback, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            foreach (var query in queries)
                GL.DeleteQuery(query);
            foreach (var tbo in tboBuffers)
                GL.DeleteTransformFeedback(tbo);
            foreach (var data in dataBuffers)
                GL.DeleteBuffer(data);
        }

        /// <summary>
        /// Вариант захвата в виде видимых
        /// </summary>
        /// <param name="indices">Преобразованные индексы элементов, полученные из шейдера</param>
        private void CreateCaptureElements(Model.Interfaces.ObjectsCollections.ISetInfo[] sets, List<List<int>> indices)
        {
            using (project.BeginViewUpdate())
            {
                for (var index = 0; index < sets.Length; ++index)
                {
                    if (indices[index] == null)
                        continue;

                    var indexSet = indices[index].ToHashSet();
                    var indexElems = 0;
                    foreach (var element in project.GetModelElements(3, sets[index].Name))
                    {
                        var isVisible = indexSet.Contains(indexElems);
                        project.SetVisible(ObjType.Элемент3D, [element.Number], isVisible);
                        ++indexElems;
                    }
                }
            }
            RequestRedraw();
        }

        private void рассечьПлоскостьюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = (MenuItem)sender;
                if (btn.IsChecked)
                {
                    if (project == null)
                    {
                        btn.IsChecked = false;
                        return;
                    }

                    var crossSection = new CrossSectionControl();
                    var form = new Window()
                    {
                        Name = "CrossSectionForm",
                        Title = Resources.UtilityToolStrip_CrossSection_Text,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        CanResize = false,
                        Topmost = true,
                        ShowInTaskbar = false,
                        FontFamily = FontFamily,
                        FontSize = FontSize,
                        Content = crossSection
                    };
                    crossSection.RemoveCrossEvent += () =>
                    {
                        VBOController.DeleteVBObjects("crossSection");
                        RequestRedraw();
                    };
                    crossSection.SelectNodesEvent += () => SelectedObjects = SelectionType.Nodes;
                    crossSection.ErrorReported += message => console.PrintInfo(message, Color.Red);
                    crossSection.CreateCrossFromTextArgs += (ar1, ar2) =>
                    {
                        try
                        {
                            CreateSectionSurfacesFromCoords(ar2);
                        }
                        catch (Exception ex)
                        {
                            console.PrintInfo(ex.Message, Color.Red);
                        }
                    };
                    crossSection.CreateCrossFromNodesEvent += () =>
                    {
                        try
                        {
                            CreateSectionSurfacesFromNodes();
                        }
                        catch (Exception ex)
                        {
                            console.PrintInfo(ex.Message, Color.Red);
                        }
                    };
                    form.Closed += (ar1, ar2) =>
                    {
                        btn.IsChecked = false;
                        VBOController.DeleteVBObjects("crossSection");
                        RequestRedraw();
                    };
                    form.Show(this);
                    form.Position = Position;
                }
                else
                {
                    OwnedWindows.FirstOrDefault(x => x.Name == "CrossSectionForm")?.Close();
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void CreateSectionSurfacesFromNodes()
        {
            var selObjs = project.GetSelected(ObjType.Узел)
                .Select(number => project.GetModelObject(ObjType.Узел, number))
                .ToArray();

            if (selObjs.Length < 3)
            {
                console.PrintInfo(Resources.UtilityToolStrip_CreateCrossSection_InvalidNodeNumerErrorMessage, Color.Red);
                return;
            }

            var mP0 = selObjs[0].CalcCentr();
            var mP1 = selObjs[1].CalcCentr();
            var mP2 = selObjs[2].CalcCentr();

            var plane = new Geometry.Plane(mP0, mP1, mP2);
            ShowSection(plane);
        }

        public Geometry.Plane CreateSectionPlane(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            var mP0 = new Point3D(p0.X, p0.Y, p0.Z);
            var mP1 = new Point3D(p1.X, p1.Y, p1.Z);
            var mP2 = new Point3D(p2.X, p2.Y, p2.Z);
            return new Geometry.Plane(mP0, mP1, mP2);
        }

        private void CreateSectionSurfacesFromCoords(CreatePlaneFromTextArgs arg) =>
            ShowSection(CreateSectionPlane(arg.point1, arg.point2, arg.point3));

        /// <summary>
        /// Строит поверхность сечения модели плоскостью и выводит её на сцену. В BaseForm при вводе
        /// координат буфер создавался без добавления на сцену (CreateVBObject без AddVbo) — здесь
        /// сечение показывается в обоих случаях.
        /// </summary>
        private void ShowSection(Geometry.Plane plane)
        {
            var surface = project.GetSectionSurfaces(plane);
            var sections = new List<SurfaceFigure> { surface };

            var presenter = presentersCreator.CreateSurfaceObjectsPresenter(sections, Color.DarkGray);
            presenter.Name = "crossSection";
            VBOController.AddVbo(CreateVBObject(presenter));
            RequestRedraw();
        }
    }
}
