# BazisGUI.Win

BazisGUI.Win — поддерживаемая оболочка Windows Forms для работы с проектом CAE. Точка входа — [`Program.cs`](../Program.cs), главное окно — [`BaseForm.cs`](../BaseForm.cs). `BaseForm` разбит на множество `partial`-файлов в `Methods/`; все они образуют один класс, поэтому на диаграммах он показан один раз или с пометкой рассматриваемой части.

## Навигация по диаграммам

| Файл | Содержание |
| --- | --- |
| [BazisGUI.Win.mdpuml](BazisGUI.Win.mdpuml) | Оболочка, панели, данные и внешние точки входа |
| [BazisGUI.Win-1.mdpuml](BazisGUI.Win-1.mdpuml) | Рисование сцены |
| [BazisGUI.Win-2.mdpuml](BazisGUI.Win-2.mdpuml) | Навигатор и события выбора |
| [BazisGUI.Win-3.mdpuml](BazisGUI.Win-3.mdpuml) | Панель свойств, консоль и панель задачи |
| [BazisGUI.Win-4.mdpuml](BazisGUI.Win-4.mdpuml) | Запросы данных и действия навигатора |

## Основные потоки данных

1. `BaseForm` создаёт и загружает проект, затем обновляет панели и сцену. Сохранение и загрузка находятся в [`BaseForm.cs`](../BaseForm.cs), операции с файлами и настройками — в [`IODataController.cs`](../IODataController.cs).
2. Навигатор передаёт действия пользователя событиями в обработчики `BaseForm`; состояние дерева меняется через [`NavigatorControl.cs`](../Navigator/NavigatorControl.cs) и `Methods/Navigator/`.
3. Панель свойств выдаёт `PropertyUpdateEvent`, `ReDrawEvent` и `ValidateValue`. Их обработчики находятся в `Methods/PropertyPanel/`, представление таблицы — в [`PropertiesPanelControl.cs`](../PropertiesPanel/PropertiesPanelControl.cs).
4. `BaseForm.RequestRedraw()` запрашивает кадр через `Invalidate()`. `DisplayObjects()` вызывает `SceneController.DisplayObjects()` и затем обработчики добавочного рисования. Синхронный `RenderNow()` используется для захвата кадра; см. [`DisplayObjects.cs`](../Methods/Scene/DisplayObjects.cs) и [`SceneInitialization.cs`](../Methods/Scene/SceneInitialization.cs).
5. Консоль сообщает о командах событием `ConsoleCommandEnteredEvent`; ввод и история расположены в [`ConsoleControl.cs`](../Console/ConsoleControl.cs), обработчики — в `Methods/Console/`.

`ProjectController` и `SceneController` принадлежат подключённым проектам. На диаграммах указаны только используемые точки входа; их внутреннее устройство здесь не повторяется. Источники: [`BazisGUI.Win.csproj`](../BazisGUI.Win.csproj), [`BaseForm.cs`](../BaseForm.cs), [`SceneInitialization.cs`](../Methods/Scene/SceneInitialization.cs).

Ранее подготовленные диаграммы `ДК.*`, `ДА.*`, `UseCases.mdpuml` и `DisplayObjects.mdpuml` оставлены отдельно: часть из них описывает сценарии переноса и предполагаемые компоненты, а приведённый выше набор фиксирует текущие классы.
