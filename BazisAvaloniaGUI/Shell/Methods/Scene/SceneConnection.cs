using BazisAvaloniaGUI.Extensions;
using BazisAvaloniaGUI.Scene;
using BazisAvaloniaGUI.Utilities;
using BazisGUI.Scene.Core;
using BazisGUI.Scene.Core.Rendering;
using BazisGUI.Scene.Interfaces;
using Geometry;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using OpenTK.Graphics.OpenGL;
using OperationalController;
using System;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    public enum SelectionType { Select, Objects, Figures, Points, Curves, Surfaces, Volumes, Nodes, Elements, Elements1D, Elements2D, Elements3D }

    /// <summary>
    /// Точки подключения сцены. Отображение, буферы, выбор объектов и кнопки сцены реализует SceneView;
    /// здесь сохранены имена методов BaseForm, через которые прикладная логика обращается к сцене,
    /// и они переадресованы в публичный API SceneView (очередь <see cref="SceneSurface.Invoke"/>).
    /// </summary>
    internal partial class MainWindow
    {
        private SceneBuffers sceneBuffers;

        /// <summary>Аналог BaseForm.VBOController: операции с буферами отдельных наборов.</summary>
        private SceneBuffers VBOController => sceneBuffers ??= new SceneBuffers(scene);

        internal sealed class SceneBuffers(SceneView scene)
        {
            public void DeleteVBObjects(string objsName) => scene.Surface.DeleteObjects(objsName);

            public void DeleteAllVBObjects() => scene.Surface.DeleteAllObjects();

            // В BaseForm сюда передаётся VBObject, созданный CreateVBObject; буфер создаётся в GL-контексте
            // сцены, поэтому передаётся представление (presenter), а буфер строится на ближайшем кадре.
            public void AddVbo(IObjsPresenter presenter)
            {
                if (presenter != null)
                    scene.Surface.AddPresenter(presenter);
            }
        }

        /// <summary>В BaseForm создаёт VBObject; в Avalonia буферы создаёт сцена, поэтому возвращается представление.</summary>
        public IObjsPresenter CreateVBObject(IObjsPresenter presenter) => presenter;

        IProjectController subscribedModelView;

        /// <summary>Передаёт проект сцене (в BaseForm — подписка на ModelView.Changed для обновления буферов).</summary>
        private void SubscribeToModelView()
        {
            var modelView = project;
            if (ReferenceEquals(subscribedModelView, modelView))
                return;

            subscribedModelView = modelView;
            // В BaseForm здесь ApplyModelViewSettings() (цвет выделения и ModelView.Transparency из settingsConfig).
            // Полные настройки представления, включая прозрачность, применяет PresentProject; здесь — цвет выделения.
            ApplySelectionColor();
            if (project != null)
                scene.Surface.ShowProject(project);
        }

        public void CreateVBObjects(string objects)
        {
            if (project == null)
                return;

            if (objects == "Объекты")
            {
                scene.Surface.RefreshSets(project.GetAllModelSetsInfo());
            }
            else if (objects == "Элементы")
            {
                CreateVBObjsByObjsType(ObjType.Элемент1D);
                CreateVBObjsByObjsType(ObjType.Элемент2D);
                CreateVBObjsByObjsType(ObjType.Элемент3D);
            }
            else if (objects == "Геометрия")
            {
                CreateVBObjsByObjsType(ObjType.Кривая);
                CreateVBObjsByObjsType(ObjType.Поверхность);
            }
            else
                CreateVBObjsByObjsType(objects.ToEnum<ObjType>());
        }

        public void CreateVBObjsByObjsType(ObjType objType)
        {
            if (project != null)
                scene.Surface.RefreshSets(project.GetModelSetsInfo(objType).Where(set => set.NumberOfObjects > 0));
        }

        public void DeleteVBObjects(string objects)
        {
            if (objects == "Объекты")
                VBOController.DeleteAllVBObjects();
            else if (objects == "Элементы")
            {
                DeleteVBObjsByObjsType(ObjType.Элемент1D);
                DeleteVBObjsByObjsType(ObjType.Элемент2D);
                DeleteVBObjsByObjsType(ObjType.Элемент3D);
            }
            else if (objects == "Геометрия")
            {
                DeleteVBObjsByObjsType(ObjType.Точка);
                DeleteVBObjsByObjsType(ObjType.Кривая);
                DeleteVBObjsByObjsType(ObjType.Поверхность);
            }
            else
                DeleteVBObjsByObjsType(objects.ToEnum<ObjType>());
        }

        public void DeleteVBObjsByObjsType(ObjType objType)
        {
            if (project == null)
                return;

            foreach (var setInfo in project.GetModelSetsInfo(objType))
                VBOController.DeleteVBObjects(setInfo.Name);
        }

        private void RefreshModelSetBuffer(ISetInfo setInfo) => scene.Surface.RefreshSets([setInfo]);

        public void SetVBObjectAttribute(IObjsPresenter presenter, string attribName) => scene.Surface.SetAttribute(presenter, attribName);

        /// <summary>BaseForm.ColorObjects: обновляет цвета буферов наборов заданного типа без пересборки.</summary>
        internal void ColorObjects(string objTypeStr)
        {
            if (objTypeStr == "Объекты")
            {
                foreach (var type in Enum.GetValues<ObjType>())
                    ColorVBObjsByObjsType(type);
            }
            else if (objTypeStr == "Элементы")
            {
                ColorVBObjsByObjsType(ObjType.Элемент1D);
                ColorVBObjsByObjsType(ObjType.Элемент2D);
                ColorVBObjsByObjsType(ObjType.Элемент3D);
            }
            else
            {
                var objType = Enum.TryParse(objTypeStr, out ObjType parsed) ? parsed :
                    throw new Exception($"{Resources.ColorObjects_ObjectConversion_Exception} {objTypeStr}");
                ColorVBObjsByObjsType(objType);
            }

            RequestRedraw();
        }

        public void ColorVBObjsByObjsType(ObjType objType)
        {
            foreach (var setInfo in project.GetModelSetsInfo(objType))
                if (setInfo.NumberOfObjects > 0)
                    SetVBObjectAttribute(project.CreateModelObjectsPresentor(setInfo), "цвет");
        }

        public void ClearAllGeometryDataOnScene() => DeleteVBObjects("Геометрия");

        public void ClearAllMeshDataOnScene()
        {
            DeleteVBObjsByObjsType(ObjType.Узел);
            DeleteVBObjects("Элементы");
        }

        public void ClearAllDataOnScene()
        {
            HideAllGeometryObjects();
            HideAllText2D();
            HideAllText3D();
            VBOController.DeleteAllVBObjects();
        }

        /// <summary>BaseForm: DisplayGeometryObjectEvent = null — убрать всю вспомогательную геометрию.</summary>
        public void HideAllGeometryObjects() => scene.Surface.Invoke(sceneController => sceneController.HideAllGeometryObjs());

        /// <summary>BaseForm: DisplayText3DEvent = null — убрать все подписи в координатах модели.</summary>
        public void HideAllText3D() => scene.Surface.Invoke(sceneController => sceneController.HideDisplayText3D());

        /// <summary>BaseForm: DisplayText2DEvent = null — убрать все подписи в координатах окна.</summary>
        public void HideAllText2D() => scene.Surface.Invoke(sceneController => sceneController.HideDisplayText2D());

        /// <summary>Убирает вспомогательную геометрию, добавленную методом с именем <paramref name="searchMethod"/>.</summary>
        public void HideGeometryObj(string searchMethod) => scene.Surface.Invoke(sceneController => sceneController.HideGeometryObj(searchMethod));

        // Сцена (SceneSurface) сама запрашивает кадр при изменении ModelView и при каждом действии над сценой.
        public void RequestRedraw() => scene.Surface.Redraw();

        public void FitObjectsToScreen() => scene.Surface.FitToScreen();

        public void DisplayText3D(string str, Color color, Point3D coord) =>
            scene.Surface.Invoke(sceneController => sceneController.DisplayText3D(str, color, coord));

        public void DisplayText2D(string str, Color color, Point2D coord) =>
            scene.Surface.Invoke(sceneController => sceneController.DisplayText2D(str, color, coord));

        public void DisplayDistance(Segment3D line) =>
            scene.Surface.Invoke(sceneController => sceneController.DisplayDistance(line));

        public void DisplayLocalFrame(Frame frame) =>
            scene.Surface.Invoke(sceneController => sceneController.DisplayLocalFrame(frame));

        public void DisplayPath(Point3D[] points) =>
            scene.Surface.Invoke(sceneController => sceneController.DisplayPath(points));

        public void DisplayVector(Point3D length, Point3D posit, Color objColor) =>
            scene.Surface.Invoke(sceneController => sceneController.DisplayVector(length, posit, objColor));

        public void DisplaySpiral(Point3D p0, Point3D p1, Color objColor) =>
            scene.Surface.Invoke(sceneController => sceneController.DisplaySpiral(p0, p1, objColor));

        public void DisplayConus(float upperDiam, float bottomDiam, float length, Frame frame) =>
            scene.Surface.Invoke(sceneController => sceneController.DisplayConus(upperDiam, bottomDiam, length, frame));

        public void DisplaySphere(float width, Frame frame) =>
            scene.Surface.Invoke(sceneController => sceneController.DisplaySphere(width, frame));

        /// <summary>
        /// Вспомогательный объект с собственной отрисовкой (в BaseForm — подписка на DisplayGeometryObjectEvent).
        /// <paramref name="name"/> — ключ для <see cref="HideGeometryObj"/>.
        /// </summary>
        public void DisplayGeometryObject(string name, Action<IRenderContext> draw) =>
            scene.Surface.Invoke(sceneController => sceneController.DisplayGeometryObject(name, draw));

        /// <summary>Тип объектов для выбора на сцене (в BaseForm хранится в кнопке btnSelect).</summary>
        public SelectionType SelectedObjects
        {
            get => scene.Surface.SelectedObjectType.HasValue
                ? Converters.ConvertObjTypeToSelectionType(scene.Surface.SelectedObjectType.Value)
                : SelectionType.Objects;
            set
            {
                scene.Surface.SelectedObjectType = Converters.TryConvertSelectionTypeToObjType(value, out var objType) ? objType : null;
                SetBackColorToAllObjects();
            }
        }

        // Кнопки выбора типа объектов (btnSelect и objButtons) — часть SceneView.
        public void PresentModelObjectsForSelection()
        {
            if (project != null)
                scene.UpdateSets(project);
        }

        internal void SetBackColorToAllObjects()
        {
            if (project == null)
                return;

            // Как в исходном BaseForm: сбрасываются и выделение, и подсветка групп/условий
            using (project.BeginViewUpdate())
            {
                project.ClearSelection();
                project.ClearColor();
            }
        }

        /// <summary>Признак подписки на пересоздание контроллера сцены (см. <see cref="SubscribeToSceneRecreation"/>).</summary>
        private bool sceneRecreationSubscribed;

        /// <summary>
        /// При пересоздании GL-контекста сцена создаёт новый контроллер с настройками по умолчанию —
        /// передаём ему все настройки из settingsConfig заново. Подписка однократная; вызывается из
        /// <see cref="ApplySceneSettings"/>, который выполняется при запуске (SetGeneralSettings).
        /// </summary>
        private void SubscribeToSceneRecreation()
        {
            if (sceneRecreationSubscribed)
                return;

            sceneRecreationSubscribed = true;
            scene.Surface.ControllerRecreated += () => ApplySceneSettings(applyProjection: true, applyLight: true);
        }

        /// <summary>
        /// Перенос части BaseForm.SetGeneralSettings и обработчиков настроек: цвет фона, освещение,
        /// прозрачность отрисовки, проекция, угол обзора, шаг вращения, положение и интенсивность
        /// источника света передаются сцене.
        /// </summary>
        /// <param name="applyProjection">
        /// Проекция из настроек. В BaseForm UpdateProjection читал settingsConfig.Projection на каждом
        /// вызове, в том числе при запуске, поэтому по умолчанию проекция применяется всегда.
        /// </param>
        /// <param name="applyLight">
        /// Положение и интенсивность источника света из настроек (по умолчанию применяются, в том числе при запуске).
        /// </param>
        private void ApplySceneSettings(bool applyProjection = true, bool applyLight = true)
        {
            SubscribeToSceneRecreation();
            var config = settingsConfig;
            scene.Surface.Invoke(sceneController =>
            {
                sceneController.BackGroundColor = config.BackGroundColor;
                sceneController.IsLighting = config.Lighting;
                if (sceneController.AverageColorRenderer != null)
                {
                    sceneController.AverageColorRenderer.IsEnable = config.Transparency;
                    sceneController.AverageColorRenderer.ShowSurfaceBackEdges = config.BackRibbers;
                }
                if (applyProjection)
                    sceneController.Projection = config.Projection;
                // Нулевые/отрицательные значения (повреждённый или старый файл настроек) не применяем
                if (config.AngleOfProjection > 0)
                    sceneController.AngleOfProjection = config.AngleOfProjection;
                if (config.RotationAngle > 0)
                    sceneController.RotationAngle = config.RotationAngle;
                sceneController.UpdateProjection();
                if (applyLight)
                {
                    sceneController.LightTranslateX = config.LighterPosition.X;
                    sceneController.LightTranslateY = config.LighterPosition.Y;
                    GL.Light(LightName.Light0, LightParameter.LinearAttenuation, 1 - config.LightingIntensity / 100.0f);
                }
            });
        }

        /// <summary>
        /// Реакция оболочки на выбор, выполненный сценой: та часть SelectByPoint/SelectByRect из BaseForm,
        /// которая выводит сообщение в консоль и свойства объекта в панель.
        /// </summary>
        private void scene_SelectionApplied(SceneSelectionResult result)
        {
            try
            {
                if (project == null)
                    return;

                ApplySelectionColor();
                if (result.IsPoint)
                {
                    // BaseForm.SelectObjects(Point2D): без попадания ничего не выводится.
                    if (result.Type is not { } type)
                        return;

                    if (IsAdvancedSelectionOpened)
                        DispatchSelection([result.Number], result.IsSelected);
                    else
                    {
                        var setInfo = project.GetModelSetInfo(type, result.Number);
                        console.PrintInfo($"{Resources.SelectByPoint_ObjectSelected_Message} : {Localization.Localization.GetSelectionTypeLocalization(Converters.ConvertObjTypeToSelectionType(type))} {result.Number}", Color.Black);
                        CreateObjectProperties(setInfo, result.Number);
                    }
                }
                else
                {
                    var objStr = Declination(result.Count);
                    if (result.IsSelected)
                        console.PrintInfo($"{Resources.SelectByRect_Selected_Message} {result.Count} {objStr}", Color.Black);
                    else
                        console.PrintInfo($"{Resources.SelectByRect_Hidden_Message} {result.Count} {objStr}", Color.Black);
                }

                // В BaseForm вызывается из SelectObjects (SceneEvents.cs) после выбора точкой или рамкой.
                sceneSelectionChangedAction?.Invoke();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        /// <summary>Группа создана из выделения на сцене (контекстное меню) — BaseForm.создатьГруппуItem_Click.</summary>
        private void scene_GroupCreated(IGroup group)
        {
            PresentGroupDataOnTree();
            OnGroupCreated?.Invoke(group.ObjType, group.Number, group.Name);
        }

        /// <summary>Выделенные объекты удалены со сцены — BaseForm.menuItem_DeleteSelectedObjects_Click.</summary>
        private void scene_ObjectsRemoved()
        {
            PresentMeshData();
            PresentGroupDataOnTree();
            PresentCondDataOnTree();
        }
    }
}
