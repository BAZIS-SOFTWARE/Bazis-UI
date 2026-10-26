# Функции BazisGUI

Функции сгруппированы по [бизнес-задачам](BusinessTasks.md). «Есть путь в коде» означает, что в `AvaloniaGUI` присутствуют обработчик и вызовы соответствующего компонента; это не означает проверки всех форматов файлов, моделей и операционных систем. Технические связи и входные данные описаны в [Implementation.md](Implementation.md).

| ID | Задача | Функция | Состояние и граница | Основной путь |
| --- | --- | --- | --- | --- |
| F01 | B1 | Создание, открытие и сохранение проекта | Есть путь в коде; сохранение использует `.bpf` или `.bpf2` | [MainWindow.cs](../AvaloniaGUI/Shell/MainWindow.cs) |
| F02 | B1 | Импорт CAD и сетки | Есть разделение по расширению: проект, CAD или сетка; CAD требует доступной нативной библиотеки Gmsh | [MainWindow.cs](../AvaloniaGUI/Shell/MainWindow.cs) |
| F03 | B1 | Создание геометрии и генерация сетки | Команды создания геометрии и генерации 2D/3D-сетки вызывают `ProjectController` | [GeometryBuilder.cs](../AvaloniaGUI/Shell/Methods/Geom/GeometryBuilder.cs), [GenerateMesh2DEvent.cs](../AvaloniaGUI/Shell/Methods/Mesh/GenerateMesh2DEvent.cs), [GenerateMesh3DEvent.cs](../AvaloniaGUI/Shell/Methods/Mesh/GenerateMesh3DEvent.cs) |
| F04 | B1, B5 | Отображение модели и выбор объектов | Сцена строит буферы по данным контроллера; выбор точкой и рамкой использует только видимые объекты | [SceneProjectPresenter.cs](../AvaloniaGUI/Scene/SceneProjectPresenter.cs), [SceneSelection.cs](../AvaloniaGUI/Scene/SceneSelection.cs) |
| F05 | B2 | Редактирование задач и свойств | Есть обработчики создания задач, выбора условий и редактирования панели свойств; полнота отдельных видов физики зависит от CORE | [Create.cs](../AvaloniaGUI/Shell/Methods/Tasks/Create.cs), [ChangeCompProperties.cs](../AvaloniaGUI/Shell/Methods/PropertyPanel/ChangeCompProperties.cs) |
| F06 | B3 | Формирование инструкций расчёта | GUI вызывает `PreProc.CalcCompDataV2` и показывает полученные инструкции | [GenerateTSF.cs](../AvaloniaGUI/Shell/Methods/Comp/GenerateTSF.cs) |
| F07 | B3 | Запуск и остановка внешнего решателя | Перед запуском сохраняет проект и проверяет наличие файлов; путь к решателю задаётся настройкой | [StartStopConp.cs](../AvaloniaGUI/Shell/Methods/Comp/StartStopConp.cs) |
| F08 | B4 | Загрузка и показ результатов | Читает базу результатов, строит отображаемые объекты и цветовую шкалу | [ResultsMainMenuEvents.cs](../AvaloniaGUI/Shell/Methods/Results/ResultsMainMenuEvents.cs), [SelectResultsEvent.cs](../AvaloniaGUI/Shell/Methods/Navigator/Results/SelectResultsEvent.cs) |
| F09 | B4 | Графики и анимация | Есть построение графика выбранной величины по временным срезам и создание GIF | [CreatePlot.cs](../AvaloniaGUI/Shell/Methods/Results/CreatePlot.cs), [CreateAnimation.cs](../AvaloniaGUI/Shell/Methods/Results/CreateAnimation.cs) |
| F10 | B5 | Общая сцена и проверка её алгоритмов | `SceneCore` обслуживает оба GUI; отдельный тестовый проект покрывает камеру, матрицы, ввод и выбор | [SceneCore.csproj](../SceneCore/SceneCore.csproj), [SceneCore.Tests.csproj](../SceneCore.Tests/SceneCore.Tests.csproj) |

## Известные границы

- `WinGUI` сохраняется для совместимости, но дальнейшая разработка интерфейса ведётся в `AvaloniaGUI` ([AGENTS.md](../AGENTS.md)).
- `SceneCore` предоставляет геометрический выбор; видимость, применение выделения и операции над проектом остаются в GUI ([ScenePicker.cs](../SceneCore/Core/Picking/ScenePicker.cs), [SceneSelection.cs](../AvaloniaGUI/Scene/SceneSelection.cs)).
- Скорость выбора большого числа объектов определяется полным обходом переданных объектов и проекцией их координат; наличие ленивого перечисления ограничивает промежуточное хранение, но не устраняет этот обход ([ScenePicker.cs](../SceneCore/Core/Picking/ScenePicker.cs)).
- Работа CAD, расчёта и чтения результатов требует доступности соответствующих внешних библиотек, файлов и исполняемого решателя. Наличие обработчика в GUI не гарантирует поддержку всех вариантов данных.
