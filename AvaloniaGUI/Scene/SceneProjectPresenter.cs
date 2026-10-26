using SceneCore;
using SceneCore;
using SceneCore.Interfaces;
using SceneCore.VBO;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using OperationalController;

namespace AvaloniaGUI.Scene;

internal class SceneProjectPresenter
{
    /// <summary>
    /// Имя буфера плоскости сечения (SceneController.CreateClipPlane). Пока открыто окно сечения, общая
    /// очистка сцены его не удаляет — снимается он только явно (DeleteClipPlane при закрытии окна).
    /// </summary>
    public const string ClipPlaneName = "ClipPlane";

    /// <summary>
    /// Наборы модели, буферы которых сейчас принадлежат сцене (построены Display/RefreshSets, в том числе
    /// пустые — все объекты набора скрыты). Если буфер набора удалён извне (вместо модели показано поле
    /// результатов), набор отсюда убирается, и изменения представления не возвращают его на сцену.
    /// Используется только в потоке рендера.
    /// </summary>
    private readonly HashSet<string> modelSets = new();

    /// <summary>Создаёт GL-объекты для видимых наборов загруженного проекта.</summary>
    public void Display(IProjectController project, SceneController scene, bool fitToScreen = true)
    {
        DeleteAll(scene);

        Refresh(project, scene, project.GetAllModelSetsInfo());
        if (fitToScreen)
            scene.FitObjectsToScreen();
    }

    /// <summary>
    /// Удаляет все буферы сцены, кроме плоскости сечения (BaseForm.VBOController.DeleteAllVBObjects).
    /// </summary>
    public void DeleteAll(SceneController scene)
    {
        var vboController = scene.VboController;
        var names = vboController.GetVBObjs()
            .Select(vbo => vbo.ObjName)
            .Where(name => name != ClipPlaneName)
            .ToList();
        foreach (var name in names)
            vboController.DeleteVBObjects(name);
        modelSets.Clear();
    }

    /// <summary>Удаляет буфер по имени; набор модели перестаёт обновляться по изменениям представления.</summary>
    public void Delete(SceneController scene, string name)
    {
        scene.VboController.DeleteVBObjects(name);
        modelSets.Remove(name);
    }

    /// <summary>
    /// Перестраивает буферы наборов по изменению представления (OnProjectMessage). Наборы, которых сейчас
    /// нет на сцене (удалены ради поля результатов), не трогает — иначе модель перекрыла бы результаты.
    /// </summary>
    public void RefreshShown(IProjectController project, SceneController scene, IEnumerable<ISetInfo> sets) =>
        Refresh(project, scene, sets.Where(set => modelSets.Contains(set.Name)));

    /// <summary>
    /// Перестраивает GL-объекты только для наборов с изменившимся представлением.
    /// Как BaseForm.RefreshModelSetBuffer: объект рисования (прозрачность, отсекатель) переходит к новому буферу.
    /// </summary>
    public void Refresh(IProjectController project, SceneController scene, IEnumerable<ISetInfo> sets)
    {
        var vboController = scene.VboController;
        foreach (var set in sets)
        {
            modelSets.Add(set.Name);
            var drawingObject = vboController.FindVBObj(set.Name)?.ActiveDrawingObject;
            vboController.DeleteVBObjects(set.Name);
            if (set.NumberOfObjects == 0)
                continue;

            var vbo = CreateVbo(project.CreateModelObjectsPresentor(set), scene);
            if (vbo == null)
                continue;

            if (drawingObject != null)
                vbo.ActiveDrawingObject = drawingObject;
            else if (set.ObjType == ObjType.Элемент3D && scene.Advanced3DClipper.ClipMode != ClipMode.None)
                vbo.ActiveDrawingObject = scene.Advanced3DClipper;

            vboController.AddVbo(vbo);
        }
    }

    /// <summary>
    /// Обновляет цвета буферов наборов без пересборки геометрии — BaseForm.RecolorModelSetBuffer.
    /// Если буфера набора на сцене нет (например, вместо модели показано поле результатов), ничего не делает.
    /// </summary>
    public void Recolor(IProjectController project, SceneController scene, IEnumerable<ISetInfo> sets)
    {
        foreach (var set in sets)
            SetAttribute(scene, project.CreateModelObjectsPresentor(set), "цвет");
    }

    /// <summary>
    /// Перенос BaseForm.SetVBObjectAttribute: обновляет цвета ("цвет") или координаты буфера,
    /// созданного ранее по тому же представлению.
    /// </summary>
    public void SetAttribute(SceneController scene, IObjsPresenter presenter, string attribName)
    {
        var vbo = scene.VboController.FindVBObj(presenter.Name);
        if (vbo == null || presenter.Count() == 0)
            return;

        if (attribName == "цвет")
            vbo.PointsColors = presenter.CreateVertexes(vbo.ColorLength, "цвет");
        else
            vbo.PointsCoords = presenter.CreateVertexes(vbo.CoordLength, "координаты");
    }

    /// <summary>Перестройка кастомных GL-объектов (например, контуров или полей результатов) по существующему презентатору.</summary>
    public void Refresh(SceneController scene, IObjsPresenter presenter)
    {
        var vboController = scene.VboController;
        vboController.DeleteVBObjects(presenter.Name);
        // Буфер с этим именем больше не набор модели — изменения представления его не перестраивают
        modelSets.Remove(presenter.Name);
        if (presenter.Count() == 0)
            return;
        var vbo = CreateVbo(presenter, scene);
        if (vbo == null)
            return;

        vboController.AddVbo(vbo);
    }

    /// <summary>
    /// Создаёт GL-объект из презентера: разбор массивов и выбор типа объекта (BaseForm.TryCreateVBObject).
    /// Возвращает null, если у презентера нет вершин (рисовать нечего).
    /// </summary>
    public VBObject? CreateVbo(IObjsPresenter presenter, SceneController scene)
    {
        var vboController = scene.VboController;
        var indexes = presenter.CreateIndexes();
        var pointers = presenter.CreatePointers(indexes.Item1);
        if (pointers.Length == 0)
            return null;

        var coordinates = presenter.CreateVertexes(indexes.Item2, "координаты");
        var colors = presenter.CreateVertexes(indexes.Item3, "цвет");
        var normals = presenter.CreateVertexes(indexes.Item2, "нормаль");
        var edges = presenter.CreateEdgeFlags(indexes.Item4);
        var name = presenter.Name;

        VBObject vbo;
        if (presenter.PresenterType == PresenterType.Surface)
        {
            if (presenter is not ISurfaceObjsPresenter surface)
                throw new InvalidOperationException("Surface presenter is required.");

            var separators = surface.CreateSeparators();
            var view = presenter.ViewMode switch
            {
                ViewMode.Line => ObjView.Lines,
                ViewMode.LineSurface => ObjView.LinesSurface,
                _ => ObjView.Surface
            };
            vbo = vboController.CreateSurfaceVBObjects(pointers, coordinates, colors, normals, edges, name, separators, view);
        }
        else if (presenter.PresenterType == PresenterType.Line)
            vbo = vboController.CreateLineVBObjects(pointers, coordinates, colors, normals, edges, name);
        else
            vbo = vboController.CreatePointVBObjects(pointers, coordinates, colors, normals, name);

        var transparency = scene.AverageColorRenderer;
        vbo.ActiveDrawingObject = transparency != null && transparency.IsEnable ? transparency : null;
        return vbo;
    }
}
