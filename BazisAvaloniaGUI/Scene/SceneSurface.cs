using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Rendering;
using Avalonia.Threading;
using BazisAvaloniaGUI.Localization;
using BazisGUI.Scene.Core;
using BazisGUI.Scene.Core.Capture;
using BazisGUI.Scene.Core.Input;
using BazisGUI.Scene.EventsArgs;
using BazisGUI.Scene.Interfaces;
using Geometry;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using OpenTK.Graphics.OpenGL;
using OperationalController;
using OperationalController.ModelScenePresentator;
using System.Collections.Concurrent;
using DrawingColor = System.Drawing.Color;

namespace BazisAvaloniaGUI.Scene;

/// <summary>
/// OpenGL-поверхность сцены: рендер, ввод (мышь/клавиатура) и выбор объектов.
/// Разметки нет — OpenGlControlBase рисуется кодом; контейнер/представление — SceneView.axaml.
/// </summary>
/// <remarks>
/// Всё, что меняет содержимое сцены (буферы наборов, текст, вспомогательная геометрия, плоскости),
/// ставится в одну очередь <see cref="Invoke"/> и выполняется в OnOpenGlRender, где GL-контекст текущий.
/// Единая очередь сохраняет порядок вызовов, как в BaseForm, где всё выполнялось синхронно.
/// </remarks>
internal class SceneSurface : OpenGlControlBase, ICustomHitTest
{
    // Как в BaseForm.ModelView_Changed: что требует пересборки буфера, а что — только перекраски.
    private const ModelViewChange RebuildChanges = ModelViewChange.Visibility | ModelViewChange.InsideSurfaces | ModelViewChange.ViewMode;
    private const ModelViewChange ColorChanges = ModelViewChange.Selection | ModelViewChange.SetColor | ModelViewChange.ObjectColor | ModelViewChange.SelectionColor | ModelViewChange.Transparency;

    private readonly SceneProjectPresenter presenter = new();
    private readonly SceneSelection selection = new();
    private readonly ConcurrentQueue<Action<SceneController>> sceneActions = new();
    private readonly ConcurrentQueue<TaskCompletionSource<byte[]>> frameCaptures = new();
    private SceneController? controller;
    private ProjectController? project;
    private bool wasInitialized;
    private int width;
    private int height;
    private SceneMouseButton pressedButton;

    /// <summary>Позиция нажатия правой кнопки — нужна, чтобы отличить клик от перетаскивания сцены.</summary>
    private Point rightPressPosition;

    /// <summary>Последняя позиция указателя на поверхности (DIP) — аналог WinForms ScreenMousePosition.</summary>
    private Point lastPointerPosition;

    /// <summary>Смещение (в пикселях), после которого движение считается перетаскиванием, а не дрожанием.</summary>
    private const double RightButtonDragThreshold = 4;

    /// <summary>Снимок экрана делается сразу после ближайшей отрисовки — из обработчика кнопки буфер ещё пуст.</summary>
    private string? screenShotPath;
    /// <summary>
    /// Запрошена смена точки поворота
    /// </summary>
    private bool rotatePointRequested;

    public event EventHandler<Exception>? ProjectDisplayFailed;
    public event Action<SceneSelectionResult>? SelectionApplied;

    /// <summary>Сцена сбросила выделение (Esc) — UI возвращает фильтр наборов на «Все объекты».</summary>
    public event Action? SelectionReset;

    /// <summary>Сцена получила проект — UI может перечитать его наборы.</summary>
    public event Action<ProjectController>? ProjectShown;

    /// <summary>Из выделения создана группа (пункт контекстного меню) — оболочка обновляет дерево групп.</summary>
    public event Action<IGroup>? GroupCreated;

    /// <summary>Выделенные объекты удалены (пункт контекстного меню) — оболочка обновляет дерево.</summary>
    public event Action? ObjectsRemoved;

    /// <summary>Сообщение для консоли окна (снимок экрана, результат операции и т.п.).</summary>
    public event Action<string, DrawingColor>? MessageReported;

    /// <summary>Тип объектов для выбора; null — «Объекты» (все наборы).</summary>
    public ObjType? SelectedObjectType
    {
        get => selectedObjectType;
        set
        {
            if (selectedObjectType == value)
                return;
            selectedObjectType = value;
            SelectedObjectTypeChanged?.Invoke(value);
        }
    }
    private ObjType? selectedObjectType;

    /// <summary>Сменился тип объектов для выбора (BaseForm.OnChangeSelectedObjectsEvent).</summary>
    public event Action<ObjType?>? SelectedObjectTypeChanged;

    public bool HideInsideSurfaces
    {
        get => project?.HideInsideSurfaces ?? false;
        set
        {
            if (project != null)
                project.SetHideInsideSurfaces(value);
        }
    }

    public SceneSurface()
    {
        AddHandler(ContextRequestedEvent, OnContextRequested, RoutingStrategies.Tunnel);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (project != null)
        {
            project.Message -= OnProjectMessage;
            project.Message += OnProjectMessage;
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        if (project != null)
            project.Message -= OnProjectMessage;
        base.OnDetachedFromVisualTree(e);
    }

    /// <summary>Отображение базиса (три оси + сфера в начале координат).</summary>
    public bool DisplayBasis
    {
        get => controller?.DisplayBasis ?? false;
        set => Invoke(scene => scene.DisplayBasis = value);
    }

    /// <summary>
    /// Выполняет действие над контроллером сцены на ближайшем кадре, когда GL-контекст текущий,
    /// и запрашивает этот кадр. Можно вызывать из любого потока.
    /// </summary>
    public void Invoke(Action<SceneController> action)
    {
        sceneActions.Enqueue(action);
        Redraw();
    }

    /// <summary>
    /// Как <see cref="Invoke"/>, но возвращает результат действия, когда оно выполнится на ближайшем кадре.
    /// </summary>
    public Task<T> InvokeAsync<T>(Func<SceneController, T> func)
    {
        var completion = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        Invoke(scene =>
        {
            try
            {
                completion.SetResult(func(scene));
            }
            catch (Exception error)
            {
                completion.SetException(error);
            }
        });
        return completion.Task;
    }

    /// <summary>
    /// Снимает в PNG кадр, нарисованный после выполнения всех ранее поставленных действий
    /// (в BaseForm — RenderNow + CreateScreenShot).
    /// </summary>
    public Task<byte[]> CaptureFrameAsync()
    {
        var completion = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
        frameCaptures.Enqueue(completion);
        Redraw();
        return completion.Task;
    }

    /// <summary>Запрашивает перерисовку сцены (BaseForm.RequestRedraw). Можно вызывать из любого потока.</summary>
    public void Redraw()
    {
        if (Dispatcher.UIThread.CheckAccess())
            RequestNextFrameRendering();
        else
            Dispatcher.UIThread.Post(RequestNextFrameRendering);
    }

    /// <summary>Вписывает объекты модели в окно сцены.</summary>
    public void FitToScreen() => Invoke(scene => scene.FitObjectsToScreen());

    /// <summary>Задаёт режим отображения (стороны / рёбра / стороны+рёбра) для наборов поверхностей и элементов.</summary>
    public void SetViewMode(ViewMode mode)
    {
        if (project == null)
            return;

        var modelView = project;
        using (modelView.BeginViewUpdate())
        {
            foreach (var type in new[] { ObjType.Поверхность, ObjType.Элемент2D, ObjType.Элемент3D })
                foreach (var set in project.GetModelSetsInfo(type))
                    modelView.SetViewMode(set, mode);
        }

        Redraw();
    }

    /// <summary>Показывает или скрывает контуры модели (граничные рёбра), как кнопка «Контуры» в WinForms.</summary>
    public void SetContoursVisible(bool visible)
    {
        if (project == null)
            return;

        var currentProject = project;
        Task.Run(() =>
        {
            var edges = new List<ILineObject<Model.MeshObjects.Node>>();
            if (visible)
            {
                var nodes = currentProject.FindBoundaryEdges();
                edges.AddRange(currentProject.CreateBoundaryEdges(nodes));
            }
            var linePresenter = new PresentersCreator().CreateLineObjectsPresenter(edges.ToList(), DrawingColor.DarkGray);
            linePresenter.Name = "Boundary";

            AddPresenter(linePresenter);
        });
    }

    /// <summary>
    /// Добавляет на сцену буфер, построенный по представлению вне наборов модели
    /// (точки сетки на кривых, поле результатов, сечение и т.п.), заменяя буфер с тем же именем.
    /// </summary>
    public void AddPresenter(IObjsPresenter objsPresenter) => Invoke(scene => presenter.Refresh(scene, objsPresenter));

    /// <summary>Удаляет буфер по имени (BaseForm.VBOController.DeleteVBObjects).</summary>
    public void DeleteObjects(string name) => Invoke(scene => scene.VboController.DeleteVBObjects(name));

    /// <summary>Удаляет все буферы сцены (BaseForm.VBOController.DeleteAllVBObjects).</summary>
    public void DeleteAllObjects() => Invoke(scene => scene.VboController.DeleteAllVBObjects());

    /// <summary>Пересоздаёт буферы наборов модели (BaseForm.CreateVBObjsByObjsType для перечисленных наборов).</summary>
    public void RefreshSets(IEnumerable<ISetInfo> sets)
    {
        var setList = sets.ToList();
        Invoke(scene =>
        {
            if (project != null)
                presenter.Refresh(project, scene, setList);
        });
    }

    /// <summary>Обновляет цвета или координаты существующего буфера (BaseForm.SetVBObjectAttribute).</summary>
    public void SetAttribute(IObjsPresenter objsPresenter, string attribName) =>
        Invoke(scene => presenter.SetAttribute(scene, objsPresenter, attribName));

    /// <summary>Просит снять текущий кадр в PNG: снимок делается сразу после ближайшей отрисовки.</summary>
    public void RequestScreenShot(string path)
    {
        screenShotPath = Path.GetFullPath(path);
        Redraw();
    }

    /// <summary>
    /// Создаёт группу из текущего выделения сцены — порт пункта «Создать новую группу»
    /// контекстного меню (WinForms BaseForm.создатьГруппуItem_Click).
    /// </summary>
    public void CreateGroupFromSelection()
    {
        if (project == null)
            return;

        try
        {
            // Как в WinForms: группа создаётся только из объектов одного типа (не «Объекты»).
            if (SelectedObjectType == null)
            {
                MessageReported?.Invoke($"{Localization.Resources.SceneEvents_CreateGroup_InvalidGroupTypeWarning}: {SceneViewLocalization.ObjectType(null)}", DrawingColor.Orange);
                return;
            }

            var selected = project.GetSelection().ToList();
            if (selected.Count == 0)
            {
                MessageReported?.Invoke(Localization.Resources.SceneView_CreateGroup_NothingSelected_Message, DrawingColor.Black);
                return;
            }

            project.CreateGroup(selected);

            // Как в WinForms: только что созданную группу берём последней в списке.
            var group = project.GetAllModelGroups().Last();
            MessageReported?.Invoke($"{Localization.Resources.SceneEvents_CreateGroup_SuccessCaption}: {group.Name}", DrawingColor.Black);
            GroupCreated?.Invoke(group);
        }
        catch (Exception error)
        {
            MessageReported?.Invoke(error.Message, DrawingColor.Red);
        }
    }

    /// <summary>
    /// Скрывает выделенные объекты и снимает с них выделение — порт пункта «Скрыть выбранное»
    /// контекстного меню (WinForms BaseForm.скрытьВыбранноеItem_Click).
    /// ModelView.HideSelected поднимает Changed, поэтому перестройку VBO делает подписка OnProjectMessage.
    /// </summary>
    public void HideSelected()
    {
        if (project == null)
            return;

        try
        {
            project.HideSelected();
        }
        catch (Exception error)
        {
            MessageReported?.Invoke(error.Message, DrawingColor.Red);
        }
    }

    /// <summary>
    /// Показывает все скрытые объекты — порт пункта «Показать все скрытые» контекстного меню
    /// (WinForms BaseForm.показатьСкрытыеItem_Click).
    /// ModelView.ShowAll поднимает Changed, поэтому перестройку VBO делает подписка OnProjectMessage.
    /// </summary>
    public void ShowHidden()
    {
        if (project == null)
            return;

        try
        {
            project.ShowAll();
        }
        catch (Exception error)
        {
            MessageReported?.Invoke(error.Message, DrawingColor.Red);
        }
    }

    /// <summary>
    /// Выводит в консоль сведения о выделенных объектах — порт пункта «Выбранные объекты» контекстного меню
    /// (WinForms BaseForm.menuItem_InfoSelectedObjects_Click).
    /// </summary>
    public void SelectedObjects()
    {
        if (project == null)
            return;

        try
        {
            var selected = project.GetSelection().ToList();
            var type = SceneViewLocalization.ObjectType(SelectedObjectType);
            var message = $"{Localization.Resources.SceneEvents_Info_Selected} {type}: {selected.Count}";
            if (selected.Count > 0)
                message += "\n" + string.Join("\n", selected.Select(x => x.ToString()));
            MessageReported?.Invoke(message, DrawingColor.Black);
        }
        catch (Exception error)
        {
            MessageReported?.Invoke(error.Message, DrawingColor.Red);
        }
    }

    /// <summary>
    /// Задаёт точку вращения камеры по ближайшей к камере вершине под курсором — порт пункта
    /// «Задать точку вращения» контекстного меню (WinForms BaseForm.menuItem_SetRotPoint_Click).
    /// </summary>
    public void RotationPointRequest()
    {
        rotatePointRequested = true;
        Redraw();
    }

    private void SetRotationPoint()
    {
        if(rotatePointRequested)
        {
            rotatePointRequested = false;
            if (controller == null || width == 0 || height == 0)
                return;

            try
            {
                var scaling = TopLevel.GetTopLevel(this)?.RenderScaling ?? 1;
                var mouseX = lastPointerPosition.X * scaling;
                var mouseY = lastPointerPosition.Y * scaling;
                var left = (int)(mouseX - width / 2f);
                var bottom = (int)(height / 2f - mouseY) - 10;
                var selectionBox = new RectangleBox(left, left + 10, bottom, bottom + 10);

                var camera = controller.GetCamera();

                var candidates = new List<(Point3D Model, float Depth)>();
                foreach (var vbo in controller.VboController.GetVBObjs())
                {
                    var coords = vbo.PointsCoords;
                    var count = coords.Length / 3;
                    for (var i = 0; i < count; i++)
                    {
                        var x = coords[3 * i + 0];
                        var y = coords[3 * i + 1];
                        var z = coords[3 * i + 2];

                        var scene = camera.GetSceenCoord(x, y, z);
                        if (selectionBox.IsPointInside(camera.GetScreenCoord(scene)))
                            candidates.Add((new Point3D(x, y, z), scene._z));
                    }
                }

                if (candidates.Count > 0)
                {
                    var nearest = candidates.OrderByDescending(c => c.Depth).First();
                    controller.SetRotationCentre(nearest.Model);
                }
            }
            catch (Exception error)
            {
                Dispatcher.UIThread.Post(() => MessageReported?.Invoke(error.Message, DrawingColor.Red));
            }
        }
    }

    /// <summary>
    /// Делает видимыми смежные (сопряжённые) геометрические объекты для выделенных — порт пункта
    /// «Показать сопряжённые» контекстного меню (WinForms BaseForm.показатьСопряженныеItem_Click).
    /// ModelView.SetVisible поднимает Changed, поэтому перестройку VBO делает подписка OnProjectMessage.
    /// </summary>
    public void ShowPaired()
    {
        if (project == null)
            return;

        try
        {
            var selected = project.GetSelection().ToList();
            using (project.BeginViewUpdate())
            {
                foreach (var item in selected)
                {
                    var (upperNumbers, lowerNumbers) = project.GetAdjacentGeometryObjects(item.Dim, item.Number);

                    if (item.Dim > 0)
                        ShowAdjacent((ObjType)(item.Dim - 1), lowerNumbers);

                    if (item.Dim < 2)
                        ShowAdjacent((ObjType)(item.Dim + 1), upperNumbers);
                }
            }
        }
        catch (Exception error)
        {
            MessageReported?.Invoke(error.Message, DrawingColor.Red);
        }

        void ShowAdjacent(ObjType type, IEnumerable<int> numbers)
        {
            foreach (var number in numbers)
            {
                var obj = project.GetModelObject(type, number);
                project.SetVisible(obj.ObjType, [obj.Number], true);
            }
        }
    }

    /// <summary>
    /// Удаляет выделенные объекты модели — порт пункта «Удалить выбранное» контекстного меню
    /// (WinForms BaseForm.menuItem_DeleteSelectedObjects_Click).
    /// Геометрию со сцены не удаляем (в WinForms — только через дерево).
    /// </summary>
    public void RemoveSelected()
    {
        if (project == null)
            return;

        if (SelectedObjectType is ObjType.Точка or ObjType.Кривая or ObjType.Поверхность)
            return;

        try
        {
            var selected = project.GetSelection().ToList();
            if (selected.Count == 0)
                return;

            var affectedSets = selected
                .Select(item => project.GetModelSetInfo(item.ObjType, item.Number))
                .OfType<ISetInfo>()
                .Distinct()
                .ToList();

            if (selected.Any(item => item.ObjType == ObjType.Узел))
                foreach (var elementType in new[] { ObjType.Элемент1D, ObjType.Элемент2D, ObjType.Элемент3D })
                    affectedSets.AddRange(project.GetModelSetsInfo(elementType));

            foreach (var item in selected)
                item.ExistState = false;

            project.ClearNotExistedModelData();

            RefreshSets(affectedSets.Distinct());

            MessageReported?.Invoke($"{Localization.Resources.SceneView_RemoveSelected_Message}: {selected.Count}", DrawingColor.Black);
            ObjectsRemoved?.Invoke();
        }
        catch (Exception error)
        {
            MessageReported?.Invoke(error.Message, DrawingColor.Red);
        }
    }

    /// <summary>Делает поверхность OpenGL доступной для событий указателя.</summary>
    bool ICustomHitTest.HitTest(Point point) => new Rect(Bounds.Size).Contains(point);

    /// <summary>Передаёт загруженный проект в поток рендера для создания VBO.</summary>
    public void ShowProject(ProjectController project)
    {
        if (this.project != null)
            this.project.Message -= OnProjectMessage;
        this.project = project;
        if (VisualRoot != null)
            project.Message += OnProjectMessage;
        Invoke(scene => presenter.Display(project, scene, fitToScreen: true));
        ProjectShown?.Invoke(project);
    }

    /// <summary>
    /// Пересоздаёт буферы всех наборов модели, сохраняя камеру (BaseForm: DeleteAllVBObjects + CreateVBObjects("Объекты")).
    /// Буферы вне модели (поля результатов, сечения) при этом удаляются, как и в BaseForm.
    /// </summary>
    public void RefreshProject()
    {
        Invoke(scene =>
        {
            if (project != null)
                presenter.Display(project, scene, fitToScreen: false);
        });
    }

    /// <summary>
    /// Выравнивает камеру по координатной плоскости (XY/XZ/YZ) и запрашивает перерисовку.
    /// Масштаб сохраняется: SceneController.PlaneObjs берёт текущий ScaleFactor камеры.
    /// </summary>
    public void SetPlane(ViewPlane plane) => Invoke(scene => scene.PlaneObjs(plane));

    /// <summary>
    /// Задаёт ось вращения для протягивания мышью: X/Y/Z — только вокруг этой оси, XYZ — свободный поворот.
    /// Перерисовка не нужна: вид не меняется, режим влияет только на последующие движения мыши.
    /// </summary>
    public void SetRotationAxis(ViewAxis axis)
    {
        if (controller == null)
            return;

        controller.RotationAxis = axis;
    }

    /// <summary>
    /// Разовый поворот сцены на заданный угол вокруг оси (кнопки поворота на 90°) с перерисовкой.
    /// </summary>
    public void RotateBy(ViewAxis axis, float angle) => Invoke(scene => scene.RotateObjs(axis, angle));

    /// <summary>
    /// Ставит изменённые наборы в очередь обновления на следующем кадре OpenGL
    /// (BaseForm.ModelView_Changed: пересборка или только перекраска буфера).
    /// </summary>
    private void OnProjectMessage(object? sender, ProjectMessageEventArgs e)
    {
        Invoke(scene =>
        {
            if (project == null || !ReferenceEquals(sender, project))
                return;

            var rebuild = new HashSet<ISetInfo>();
            var recolor = new HashSet<ISetInfo>();
            foreach (var change in e.Changes)
            {
                if (change is ViewChange viewChange)
                {
                    foreach (var set in viewChange.AffectedSets)
                    {
                        var kind = viewChange.GetChangeKind(set);
                        if ((kind & RebuildChanges) != 0)
                            rebuild.Add(set);
                        else if ((kind & ColorChanges) != 0)
                            recolor.Add(set);
                    }
                }
                else if (change is ProjectChange projectChange && projectChange.ChangeKind != ProjectChange.ProjectChangeKind.PropertyChanged
                    || change is GeoChange or MeshChange or SetDelete or SetRename)
                {
                    presenter.Display(project, scene, fitToScreen: false);
                    return;
                }
            }
            recolor.ExceptWith(rebuild);
            presenter.Refresh(project, scene, rebuild);
            presenter.Recolor(project, scene, recolor);
        });
    }
    protected override void OnOpenGlInit(GlInterface gl)
    {
        GL.LoadBindings(new AvaloniaGlBindingsContext(gl));
        controller = new SceneController();
        controller.GetCamera().Width = 1;
        controller.GetCamera().Height = 1;
        controller.RenderRequested += RequestRendering;
        controller.SelectionChanged += OnSelectionChanged;
        controller.Initialization();
        width = height = 0;

        // Контекст пересоздан (например, панель сцены переподключена) — буферы прежнего контекста потеряны.
        if (wasInitialized && project != null)
        {
            var currentProject = project;
            sceneActions.Enqueue(scene => presenter.Display(currentProject, scene, fitToScreen: false));
        }
        wasInitialized = true;
    }

    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        if (controller == null)
            return;

        var scaling = TopLevel.GetTopLevel(this)?.RenderScaling ?? 1;
        var nextWidth = Math.Max(1, (int)Math.Ceiling(Bounds.Width * scaling));
        var nextHeight = Math.Max(1, (int)Math.Ceiling(Bounds.Height * scaling));
        if (width != nextWidth || height != nextHeight)
        {
            width = nextWidth;
            height = nextHeight;
            controller.Resize(width, height);
        }

        while (sceneActions.TryDequeue(out var action))
        {
            try
            {
                action(controller);
            }
            catch (Exception error)
            {
                Dispatcher.UIThread.Post(() => ProjectDisplayFailed?.Invoke(this, error));
            }
        }

        SetRotationPoint();

        controller.DisplayObjects(fb);

        if (screenShotPath is { } path)
        {
            screenShotPath = null;
            CaptureScreenShot(path);
        }

        while (frameCaptures.TryDequeue(out var capture))
        {
            try
            {
                capture.SetResult(controller.CaptureScreenshot(new GlFrameGrabber()));
            }
            catch (Exception error)
            {
                capture.SetException(error);
            }
        }
    }

    /// <summary>Читает готовый кадр и сохраняет его в PNG по выбранному оболочкой пути.</summary>
    private void CaptureScreenShot(string path)
    {
        if (controller == null)
            return;

        try
        {
            var png = controller.CaptureScreenshot(new GlFrameGrabber());
            File.WriteAllBytes(path, png);
            Dispatcher.UIThread.Post(() => MessageReported?.Invoke($"{Localization.Resources.MakeScreenShot_ScreenShotTaken_Message}: {path}", DrawingColor.Black));
        }
        catch (Exception error)
        {
            Dispatcher.UIThread.Post(() => ProjectDisplayFailed?.Invoke(this, error));
        }
    }

    protected override void OnOpenGlDeinit(GlInterface gl)
    {
        if (controller == null)
            return;

        controller.RenderRequested -= RequestRendering;
        controller.SelectionChanged -= OnSelectionChanged;
        controller.VboController.DeleteAllVBObjects();
        controller.Dispose();
        controller = null;
    }

    protected override void OnOpenGlLost()
    {
        controller = null;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        Focus();
        if (controller == null)
            return;

        var point = e.GetCurrentPoint(this);
        lastPointerPosition = point.Position;
        var args = CreateMouseArgs(e);
        args.Button = point.Properties.IsLeftButtonPressed ? SceneMouseButton.Left
            : point.Properties.IsMiddleButtonPressed ? SceneMouseButton.Middle
            : point.Properties.IsRightButtonPressed ? SceneMouseButton.Right
            : SceneMouseButton.None;
        pressedButton = args.Button;
        if (pressedButton == SceneMouseButton.Right)
            rightPressPosition = point.Position;
        if (pressedButton != SceneMouseButton.None)
            e.Pointer.Capture(this);
        if (pressedButton == SceneMouseButton.Left && project != null)
        {
            var start = new System.Drawing.Point((int)args.Position._x, height - (int)args.Position._y);
            controller.SelectionRectangle.winScrenePosit = start;
            controller.SelectionRectangle.winScreneCoord = start;
        }
        controller.OnPointerPressed(args);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        lastPointerPosition = e.GetPosition(this);
        if (controller == null || pressedButton == SceneMouseButton.None)
            return;

        var args = CreateMouseArgs(e);
        args.Button = pressedButton;
        if (pressedButton == SceneMouseButton.Left && project != null)
        {
            controller.SelectionRectangle.winScreneCoord = new System.Drawing.Point((int)args.Position._x, height - (int)args.Position._y);
            RequestNextFrameRendering();
        }
        controller.OnPointerMoved(args);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (controller == null)
            return;

        var args = CreateMouseArgs(e);
        args.Button = e.InitialPressMouseButton switch
        {
            MouseButton.Left => SceneMouseButton.Left,
            MouseButton.Middle => SceneMouseButton.Middle,
            MouseButton.Right => SceneMouseButton.Right,
            _ => SceneMouseButton.None
        };
        controller.OnPointerReleased(args);
        if (args.Button == SceneMouseButton.Left)
        {
            controller.SelectionRectangle.Remove();
            RequestNextFrameRendering();
        }
        pressedButton = SceneMouseButton.None;
        if (e.Pointer.Captured == this)
            e.Pointer.Capture(null);
    }

    /// <summary>
    /// Подавляет автоматическое открытие контекстного меню, если правой кнопкой выполнялось
    /// перетаскивание сцены. Позицию отпускания берём из самого события, а не храним флаг движения.
    /// </summary>
    private void OnContextRequested(object? sender, ContextRequestedEventArgs e)
    {
        if (!e.TryGetPosition(this, out var releasePosition))
            return;

        var dx = releasePosition.X - rightPressPosition.X;
        var dy = releasePosition.Y - rightPressPosition.Y;
        if (dx * dx + dy * dy > RightButtonDragThreshold * RightButtonDragThreshold)
            e.Handled = true;
    }

    protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
    {
        base.OnPointerCaptureLost(e);
        if (pressedButton == SceneMouseButton.Middle)
            controller?.OnPointerReleased(new SceneMouseEventArgs { Button = SceneMouseButton.Middle });
        if (pressedButton == SceneMouseButton.Left && controller != null)
        {
            controller.SelectionRectangle.Remove();
            RequestNextFrameRendering();
        }
        pressedButton = SceneMouseButton.None;
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        if (controller == null)
            return;

        var args = CreateMouseArgs(e);
        args.Delta = (int)Math.Round(e.Delta.Y * 120);
        controller.OnWheelChanged(args);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (controller == null)
            return;

        var key = e.Key switch
        {
            Key.F => SceneKey.F,
            Key.C => SceneKey.C,
            Key.Escape => SceneKey.Escape,
            _ => SceneKey.None
        };

        switch (key)
        {
            // C — только запрос точки вращения (тяжёлая часть считается в OnOpenGlRender).
            case SceneKey.C:
                RotationPointRequest();
                return;

            // Esc обрабатывается главным окном после локальных обработчиков контролов.
            case SceneKey.Escape:
                return;
        }

        controller.OnKeyDown(new SceneKeyEventArgs
        {
            Key = key,
            Modifiers = CreateModifiers(e.KeyModifiers)
        });
    }

    /// <summary>
    /// Порт ветки Escape из WinForms BaseForm.GlControl_KeyDown: убирает вспомогательную геометрию
    /// и текст и снимает выделение со всех объектов. Пересборку наборов выполняет подписка OnProjectMessage.
    /// </summary>
    public void ClearSelection()
    {
        Invoke(scene =>
        {
            scene.HideAllGeometryObjs();
            scene.HideDisplayText2D();
            scene.HideDisplayText3D();
        });

        if (project != null)
        {
            try
            {
                using (project.BeginViewUpdate())
                    project.ClearSelection();
            }
            catch (Exception error)
            {
                MessageReported?.Invoke(error.Message, DrawingColor.Red);
            }
        }

        SelectionReset?.Invoke();
    }

    private void RequestRendering()
    {
        Dispatcher.UIThread.Post(RequestNextFrameRendering);
    }

    /// <summary>Применяет выбор кликом или рамкой к текущему проекту.</summary>
    private void OnSelectionChanged(object? sender, SelectObjectsEventArgs e)
    {
        if (project == null || controller == null)
            return;

        try
        {
            SelectionApplied?.Invoke(selection.Apply(project, controller, SelectedObjectType, e));
        }
        catch (Exception error)
        {
            ProjectDisplayFailed?.Invoke(this, error);
        }
    }

    private SceneMouseEventArgs CreateMouseArgs(PointerEventArgs e)
    {
        var position = e.GetPosition(this);
        var scaling = TopLevel.GetTopLevel(this)?.RenderScaling ?? 1;
        return new SceneMouseEventArgs
        {
            Position = new Point2D(position.X * scaling, position.Y * scaling),
            Modifiers = CreateModifiers(e.KeyModifiers)
        };
    }

    private SceneModifierKeys CreateModifiers(KeyModifiers modifiers)
    {
        var result = SceneModifierKeys.None;
        if (modifiers.HasFlag(KeyModifiers.Shift)) result |= SceneModifierKeys.Shift;
        if (modifiers.HasFlag(KeyModifiers.Control)) result |= SceneModifierKeys.Control;
        if (modifiers.HasFlag(KeyModifiers.Alt)) result |= SceneModifierKeys.Alt;
        return result;
    }
}
