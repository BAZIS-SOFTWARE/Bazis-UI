using System.Collections.ObjectModel;
using global::Avalonia.Controls;
using global::Avalonia.Controls.Primitives;
using global::Avalonia.Interactivity;
using global::Avalonia.Threading;
using BazisGUI.Avalonia.Localization;
using BazisGUI.SceneCore.Interfaces;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using OperationalController;

namespace BazisGUI.Avalonia.Scene;

/// <summary>Пункт выпадающего списка наборов: тип объектов (null — все объекты) и его подпись.</summary>
internal sealed record SetButton(ObjType? Type, string Title);

/// <summary>
/// Представление сцены: разметка — SceneView.axaml, содержимое — SceneSurface.
/// Своей логики нет; работа со сценой идёт через <see cref="Surface"/>.
/// Пункты выпадающего списка наборов строятся по данным проекта.
/// </summary>
internal partial class SceneView : UserControl
{
    private readonly ObservableCollection<SetButton> setButtons = new();

    /// <summary>Признак перестройки списка: чтобы программная смена выбора не трогала сцену.</summary>
    private bool isUpdatingSets;

    /// <summary>Текущий режим вращения мышью; XYZ — свободный поворот (значение по умолчанию в SceneInputController).</summary>
    private ViewAxis rotationMode = ViewAxis.XYZ;

    public SceneView()
    {
        InitializeComponent();

        // Пункты выпадающего списка строятся по коллекции: сколько наборов вернёт проект — столько пунктов и будет.
        SetsCombo.ItemsSource = setButtons;
        SetsCombo.SelectionChanged += OnSetSelectionChanged;

        // Панели кнопок видов и механизмов отображения раскрываются обычными оверлеями внутри Panel (не Popup).
        // Присваивание вручную, т.к. у ToggleButton.IsChecked тип bool?, а у IsVisible — bool.
        ViewToggle.IsCheckedChanged += (_, _) =>
            ViewButtonsPanel.IsVisible = ViewToggle.IsChecked == true;

        DisplayStatesToggle.IsCheckedChanged += (_, _) =>
            DisplayStatesPanel.IsVisible = DisplayStatesToggle.IsChecked == true;

        Surface.ProjectShown += project =>
        {
            UpdateSets(project, resetSelection: true);
            ResetViewModeButtons();
        };
        Surface.SelectionReset += OnSelectionReset;
    }

    public SceneSurface Surface { get => surface; }

    /// <summary>Перечитывает наборы проекта и перестраивает пункты выпадающего списка.</summary>
    public void UpdateSets(ProjectController project, bool resetSelection = false)
    {
        var selectedType = resetSelection ? null : Surface.SelectedObjectType;
        // Пока список перестраивается, программная смена выбора не должна трогать сцену.
        isUpdatingSets = true;
        try
        {
            setButtons.Clear();
            setButtons.Add(new SetButton(null, SceneViewLocalization.ObjectType(null)));
            foreach (var set in project.GetAllModelSetsInfo()
                                        .Where(v => v.NumberOfObjects > 0)
                                        .Select(v => v.ObjType)
                                        .Distinct())
                setButtons.Add(new SetButton(set, SceneViewLocalization.ObjectType(set)));

            SetsCombo.SelectedItem = setButtons.FirstOrDefault(item => item.Type == selectedType) ?? setButtons[0];
        }
        finally
        {
            isUpdatingSets = false;
        }

        Surface.SelectedObjectType = ((SetButton)SetsCombo.SelectedItem!).Type;
    }

    /// <summary>Esc в сцене сбросил выделение — возвращаем фильтр наборов на «Все объекты».</summary>
    private void OnSelectionReset()
    {
        // Смена индекса вызовет OnSetSelectionChanged и обнулит Surface.SelectedObjectType.
        SetsCombo.SelectedIndex = 0;
        Surface.SelectedObjectType = null;
    }

    /// <summary>Выбор пункта списка: набор фильтрует сцену, «Все объекты» снимает фильтр.</summary>
    private void OnSetSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (isUpdatingSets || SetsCombo.SelectedItem is not SetButton item)
            return;

        Surface.SelectedObjectType = item.Type;
    }

    private void OnViewChangeClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;

        // Имена кнопок повторяют парные оси в порядке экранных осей (ZX/ZY), а ViewPlane — в порядке имён осей (XZ/YZ).
        var plane = button.Name switch
        {
            "btnXY" => ViewPlane.XY,
            "btnZX" => ViewPlane.XZ,
            "btnZY" => ViewPlane.YZ,
            _ => (ViewPlane?)null
        };

        if (plane is { } viewPlane)
            Surface.SetPlane(viewPlane);
    }

    /// <summary>Клик по кнопке режима поворота: вращение вокруг оси X/Y/Z, повторный клик — свободный поворот (XYZ).</summary>
    private void OnRotateChangeModeClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not ToggleButton button || AxisOf(button.Name) is not { } axis)
            return;

        // Повторный клик по активной оси снимает режим — возвращаем свободный поворот.
        rotationMode = rotationMode == axis ? ViewAxis.XYZ : axis;
        Surface.SetRotationAxis(rotationMode);

        // Обводка показывает активный режим. Синхронизация отложена, чтобы не зависеть от того,
        // успел ли ToggleButton переключить IsChecked до события Click.
        Dispatcher.UIThread.Post(() =>
        {
            foreach (var toggle in ViewButtonsPanel.Children.OfType<ToggleButton>())
                if (AxisOf(toggle.Name) is { } toggleAxis)
                    toggle.IsChecked = toggleAxis == rotationMode;
        });
    }

    /// <summary>Клик по кнопке разового поворота: горизонтальная — вокруг Y, вертикальная — вокруг X, угол 90°.</summary>
    private void OnRotateByRightAngle(object? sender, RoutedEventArgs e)
    {
        if (sender is not Control control || RightAngleOf(control.Name) is not { } rotation)
            return;

        Surface.RotateBy(rotation.Axis, rotation.Angle);

        // Кнопка — разовое действие, а не режим: рамку-обводку не фиксируем.
        if (control is ToggleButton toggle)
            Dispatcher.UIThread.Post(() => toggle.IsChecked = false);
    }

    /// <summary>Ось поворота по имени кнопки режима (null — кнопка не про режим поворота).</summary>
    private static ViewAxis? AxisOf(string? buttonName) => buttonName switch
    {
        "btnRotX" => ViewAxis.X,
        "btnRotY" => ViewAxis.Y,
        "btnRotZ" => ViewAxis.Z,
        _ => null
    };

    /// <summary>
    /// Ось и угол разового поворота по имени кнопки. Соответствие как в WinForms
    /// (btnRotHor90 → ViewAxis.Y, btnRotVert90 → ViewAxis.X), угол всегда 90°.
    /// Варианты написания: btnRotHor/btnRotHor90 и btnRotVer/btnRotVert/btnRotVert90.
    /// </summary>
    private static (ViewAxis Axis, float Angle)? RightAngleOf(string? buttonName) => buttonName switch
    {
        "btnRotHor" => (ViewAxis.Y, 90f),
        "btnRotVer" => (ViewAxis.X, 90f),
        _ => null
    };

    /// <summary>«Вписать в экран»: масштаб и центр камеры по габаритам модели.</summary>
    private void OnFitToScreenClick(object? sender, RoutedEventArgs e) => Surface.FitToScreen();

    /// <summary>«Снимок экрана»: оболочка выбирает путь сохранения PNG.</summary>
    public event Action? ScreenShotRequested;

    private void OnMakeScreenShotClick(object? sender, RoutedEventArgs e) => ScreenShotRequested?.Invoke();

    /// <summary>«Внутренние объекты»: показать/скрыть внутренние поверхности модели.</summary>
    private void OnShowInsideObjectsClick(object? sender, RoutedEventArgs e) =>
        Surface.HideInsideSurfaces = !Surface.HideInsideSurfaces;

    /// <summary>Режим отображения: стороны / рёбра / стороны+рёбра — кнопки взаимоисключающие.</summary>
    private void OnViewModeClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not ToggleButton button)
            return;

        var mode = button.Name switch
        {
            "btnShowSides" => ViewMode.Surface,
            "btnShowRibs" => ViewMode.Line,
            _ => ViewMode.LineSurface
        };

        foreach (var other in DisplayStatesPanel.Children.OfType<ToggleButton>())
            if (IsViewModeButton(other.Name))
                other.IsChecked = ReferenceEquals(other, button);

        Surface.SetViewMode(mode);
    }

    /// <summary>
    /// Новый проект показывается в режимах отображения из своего представления, а не в режиме,
    /// выбранном кнопками для прежнего проекта, — снимаем отметку со всех кнопок режима.
    /// </summary>
    private void ResetViewModeButtons()
    {
        foreach (var button in DisplayStatesPanel.Children.OfType<ToggleButton>())
            if (IsViewModeButton(button.Name))
                button.IsChecked = false;
    }

    /// <summary>Имя кнопки относится к выбору режима отображения (а не к базису/контурам).</summary>
    private static bool IsViewModeButton(string? buttonName) =>
        buttonName is "btnShowSides" or "btnShowRibs" or "btnShowSidesRibs";

    /// <summary>«Базис»: показ осей и сферы в начале координат.</summary>
    private void OnBasisClick(object? sender, RoutedEventArgs e)
    {
        if (sender is ToggleButton button)
            Surface.DisplayBasis = button.IsChecked == true;
    }

    /// <summary>«Контуры»: показ граничных рёбер модели.</summary>
    private void OnContoursClick(object? sender, RoutedEventArgs e)
    {
        if (sender is ToggleButton button)
            Surface.SetContoursVisible(button.IsChecked == true);
    }

    /// <summary>Кнопка дополнительного выбора (btnAdvSelection): окно режимов выбора открывает оболочка.</summary>
    public event Action? AdvancedSelectionRequested;

    private void OnAdvSelectClick(object? sender, RoutedEventArgs e) => AdvancedSelectionRequested?.Invoke();

    /// <summary>Пункт контекстного меню «Create a new group»: создать группу из выделенного.</summary>
    private void OnCreateGroup(object? sender, RoutedEventArgs e) => Surface.CreateGroupFromSelection();

    /// <summary>Пункт контекстного меню «Hide selected»: скрыть выделенные объекты.</summary>
    private void OnHideSelected(object? sender, RoutedEventArgs e) => Surface.HideSelected();

    /// <summary>Пункт контекстного меню «Show all hidden»: показать все скрытые объекты.</summary>
    private void OnShowHidden(object? sender, RoutedEventArgs e) => Surface.ShowHidden();

    /// <summary>Пункт контекстного меню «Selected objects»: сведения о выделенных объектах.</summary>
    private void OnSelectedObjects(object? sender, RoutedEventArgs e) => Surface.SelectedObjects();

    /// <summary>Пункт контекстного меню «Set rotation point»: центр вращения по точке под курсором.</summary>
    private void OnSetRotationPoint(object? sender, RoutedEventArgs e) => Surface.RotationPointRequest();

    /// <summary>Пункт контекстного меню «Show paired»: показать смежные объекты выделенных.</summary>
    private void OnShowPaired(object? sender, RoutedEventArgs e) => Surface.ShowPaired();

    /// <summary>Пункт контекстного меню «Remove selected»: удалить выделенные объекты модели.</summary>
    private void OnRemoveSelected(object? sender, RoutedEventArgs e) => Surface.RemoveSelected();
}
