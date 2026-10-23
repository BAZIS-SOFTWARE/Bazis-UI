# Консоль Avalonia

`ConsoleControl` — перенос `GUI/Console/ConsoleControl.cs` с теми же членами:
`PrintInfo`, `PrintHistory`, `ExecuteCmdFile`, `ProcessCommandLine`, `SetValue`,
`KeyDownEventHadler`, `ClearAll_Click`, `btnBackGroundInfo_Click`, `btnStartMacro_Click`,
`btnDictionary_Click`, событиями `ConsoleCommandEnteredEvent` и `CommandsListRequestedEvent`.
`ConsoleHistory`, `FieldsParser`, `Token` и перечисления `Enums/*` скопированы без изменений логики.

Обработка команд, как в WinForms, находится в оболочке: `Shell/Methods/Console/ExecuteCommand.cs`
и соседние файлы повторяют `GUI/Methods/Console/*` (имена методов, порядок операций, проверки).

Отличия, вызванные Avalonia:

- вместо `RichTextBox` вывод — список цветных строк `Messages`, ввод — последняя строка поля
  (`TextBox` без рамки и фона); после Enter команда остаётся в выводе отдельной строкой;
- как в `HighlightPhrase`, заголовок сессии зелёный целиком, у `PrintInfo` цветом выделяется
  только сообщение (префикс ` > ` чёрный); `LinkLabel` с путём журнала стоит в первой строке;
- заголовок `PinnedPage` (15 px, крестик → `ControlCollapseEvent`) нарисован самой консолью;
  кнопки `toolStripEx1` с картинками из `ConsoleControl.resx` лежат в `Assets`, подсказки —
  в `ConsoleControl*.resx`;
- диалоги файлов и цвета асинхронные (`StorageProvider`, `ColorSelectionDialog`);
- `PrintInfo` из фонового потока передаётся в `Dispatcher.UIThread`;
- путь журнала собирается через `Path.Combine` (сборка также под Linux), ссылка на журнал
  открывается через `Launcher`.
