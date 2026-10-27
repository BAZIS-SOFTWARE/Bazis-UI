# Функции BazisGUI

Функции сгруппированы по [бизнес-задачам](BusinessTasks.md). «Есть путь в коде» означает, что в `BazisGUI.Avalonia` присутствуют обработчик и вызовы соответствующего компонента; это не означает проверки всех форматов файлов, моделей и операционных систем. Технические связи и входные данные описаны в [Implementation.md](Implementation.md).

| ID | Задача | Функция | Состояние и граница | Основной путь |
| --- | --- | --- | --- | --- |
| F01 | B1 | Создание, открытие и сохранение проекта | Есть путь в коде; сохранение использует `.bpf` или `.bpf2` | [MainWindow.cs](../BazisGUI.Avalonia/Shell/MainWindow.cs) |
| F02 | B1 | Импорт CAD и сетки | Есть разделение по расширению: проект, CAD или сетка; CAD требует доступной нативной библиотеки Gmsh | [MainWindow.cs](../BazisGUI.Avalonia/Shell/MainWindow.cs) |
| F03 | B1 | Создание геометрии и генерация сетки | Команды создания геометрии и генерации 2D/3D-сетки вызывают `ProjectController` | [GeometryBuilder.cs](../BazisGUI.Avalonia/Shell/Methods/Geom/GeometryBuilder.cs), [GenerateMesh2DEvent.cs](../BazisGUI.Avalonia/Shell/Methods/Mesh/GenerateMesh2DEvent.cs), [GenerateMesh3DEvent.cs](../BazisGUI.Avalonia/Shell/Methods/Mesh/GenerateMesh3DEvent.cs) |
| F04 | B1, B5 | Отображение модели и выбор объектов | Сцена строит буферы по данным контроллера; выбор точкой и рамкой использует только видимые объекты | [SceneProjectPresenter.cs](../BazisGUI.Avalonia/Scene/SceneProjectPresenter.cs), [SceneSelection.cs](../BazisGUI.Avalonia/Scene/SceneSelection.cs) |
| F05 | B2 | Редактирование задач и свойств | Есть обработчики создания задач, выбора условий и редактирования панели свойств; полнота отдельных видов физики зависит от CORE | [Create.cs](../BazisGUI.Avalonia/Shell/Methods/Tasks/Create.cs), [ChangeCompProperties.cs](../BazisGUI.Avalonia/Shell/Methods/PropertyPanel/ChangeCompProperties.cs) |
| F06 | B3 | Формирование инструкций расчёта | GUI вызывает `PreProc.CalcCompDataV2` и показывает полученные инструкции | [GenerateTSF.cs](../BazisGUI.Avalonia/Shell/Methods/Comp/GenerateTSF.cs) |
| F07 | B3 | Запуск и остановка внешнего решателя | Перед запуском сохраняет проект и проверяет наличие файлов; путь к решателю задаётся настройкой | [StartStopConp.cs](../BazisGUI.Avalonia/Shell/Methods/Comp/StartStopConp.cs) |
| F08 | B4 | Загрузка и показ результатов | Читает базу результатов, строит отображаемые объекты и цветовую шкалу | [ResultsMainMenuEvents.cs](../BazisGUI.Avalonia/Shell/Methods/Results/ResultsMainMenuEvents.cs), [SelectResultsEvent.cs](../BazisGUI.Avalonia/Shell/Methods/Navigator/Results/SelectResultsEvent.cs) |
| F09 | B4 | Графики и анимация | Есть построение графика выбранной величины по временным срезам и создание GIF | [CreatePlot.cs](../BazisGUI.Avalonia/Shell/Methods/Results/CreatePlot.cs), [CreateAnimation.cs](../BazisGUI.Avalonia/Shell/Methods/Results/CreateAnimation.cs) |
| F10 | B5 | Общая сцена и проверка её алгоритмов | `BazisGUI.SceneCore` обслуживает оба GUI; отдельный тестовый проект покрывает камеру, матрицы, ввод и выбор | [BazisGUI.SceneCore.csproj](../BazisGUI.SceneCore/BazisGUI.SceneCore.csproj), [BazisGUI.SceneCore.Tests.csproj](../BazisGUI.SceneCore.Tests/BazisGUI.SceneCore.Tests.csproj) |

## Известные границы

- `BazisGUI.Win` сохраняется для совместимости, но дальнейшая разработка интерфейса ведётся в `BazisGUI.Avalonia` ([AGENTS.md](../AGENTS.md)).
- `BazisGUI.SceneCore` предоставляет геометрический выбор; видимость, применение выделения и операции над проектом остаются в GUI ([ScenePicker.cs](../BazisGUI.SceneCore/Core/Picking/ScenePicker.cs), [SceneSelection.cs](../BazisGUI.Avalonia/Scene/SceneSelection.cs)).
- Скорость выбора большого числа объектов определяется полным обходом переданных объектов и проекцией их координат; наличие ленивого перечисления ограничивает промежуточное хранение, но не устраняет этот обход ([ScenePicker.cs](../BazisGUI.SceneCore/Core/Picking/ScenePicker.cs)).
- Работа CAD, расчёта и чтения результатов требует доступности соответствующих внешних библиотек, файлов и исполняемого решателя. Наличие обработчика в GUI не гарантирует поддержку всех вариантов данных.
