# BazisGUI.Avalonia

BazisGUI.Avalonia — основная оболочка CAE на Avalonia. Точка входа — [`Program.cs`](../Program.cs), главное окно — [`Shell/MainWindow.cs`](../Shell/MainWindow.cs). Обработчики `MainWindow` распределены по `Shell/Methods/`; это части одного класса, а не отдельные контроллеры.

## Навигация по диаграммам

| Файл | Содержание |
| --- | --- |
| [BazisGUI.Avalonia.mdpuml](BazisGUI.Avalonia.mdpuml) | Оболочка, панели, данные и внешние точки входа |
| [BazisGUI.Avalonia-1.mdpuml](BazisGUI.Avalonia-1.mdpuml) | Представление проекта на сцене |
| [BazisGUI.Avalonia-2.mdpuml](BazisGUI.Avalonia-2.mdpuml) | Навигатор и его дерево |
| [BazisGUI.Avalonia-3.mdpuml](BazisGUI.Avalonia-3.mdpuml) | Панель свойств и консоль |
| [BazisGUI.Avalonia-4.mdpuml](BazisGUI.Avalonia-4.mdpuml) | Запросы данных и действия навигатора |

## Основные потоки данных

1. `MainWindow` открывает и сохраняет проект, связывает панели, навигатор и сцену. Код оболочки — [`MainWindow.cs`](../Shell/MainWindow.cs) и [`MainWindow.Designer.cs`](../Shell/MainWindow.Designer.cs); файловые операции — [`IODataController.cs`](../IODataController.cs).
2. [`SceneView`](../Scene/SceneView.axaml.cs) содержит `SceneSurface` и перестраивает выбор набора после `ProjectShown`. `SceneSurface` ставит действия над OpenGL в очередь `Invoke` и исполняет её в `OnOpenGlRender`; [`SceneProjectPresenter`](../Scene/SceneProjectPresenter.cs) обновляет буферы по данным проекта, [`SceneSelection`](../Scene/SceneSelection.cs) применяет выбор объектов.
3. [`NavigatorControl`](../Navigator/NavigatorControl.cs) передаёт действия через события в `MainWindow`. Узлы и коллекция узлов определены в [`TreeNode.cs`](../Navigator/TreeNode.cs); обработчики оболочки — в `Shell/Methods/Navigator/`.
4. [`PropertiesPanelControl`](../Properties/PropertiesPanelControl.cs) отображает `RowProperty` и отправляет изменения обработчикам из `Shell/Methods/PropertyPanel/`.
5. [`ConsoleControl`](../Console/ConsoleControl.cs) хранит сообщения и передаёт команды оболочке через `ConsoleCommandEnteredEvent`; их обработчики находятся в `Shell/Methods/Console/`.

`ProjectController`, `IProjectController` и `SceneController` принадлежат подключённым проектам. На диаграммах указаны только используемые точки входа; внутреннее устройство подключённых проектов здесь не повторяется. Источники: [`BazisGUI.Avalonia.csproj`](../BazisGUI.Avalonia.csproj), [`MainWindow.cs`](../Shell/MainWindow.cs), [`SceneSurface.cs`](../Scene/SceneSurface.cs).

