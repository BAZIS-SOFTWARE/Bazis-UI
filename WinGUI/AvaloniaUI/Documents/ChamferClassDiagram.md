# Диаграмма классов модуля Add Chamfer

Диаграмма соответствует текущей реализации в `GUI/AvaloniaUI/Chamfer`.

```mermaid
classDiagram
    direction LR

    class BaseForm {
        <<WinForms Form>>
        +OnChamferByAngleRequested(length, angle) event
        +OnChamferByLengthsRequested(length1, length2) event
        -addChamferToolStripMenuItem_Click(sender, e)
        -RequestChamferByAngle(length, angle)
        -RequestChamferByLengths(length1, length2)
    }

    class AvaloniaHost {
        <<static>>
        -initializedEvent ManualResetEventSlim
        -isInitialized bool
        -initializationException Exception
        +Initialize()
        +Post(action)
        -RunAvalonia()
    }

    class ChamferWindowService {
        <<static>>
        +Show(operationService)
    }

    class IChamferOperationService {
        <<interface>>
        +AddByAngle(length, angle)
        +AddByLengths(length1, length2)
    }

    class SynchronizationContextChamferOperationService {
        -synchronizationContext SynchronizationContext
        -addByAngle Action~double, double~
        -addByLengths Action~double, double~
        +AddByAngle(length, angle)
        +AddByLengths(length1, length2)
    }

    class ChamferViewModel {
        -operationService IChamferOperationService
        +AngleLength string
        +Angle string
        +FirstLength string
        +SecondLength string
        +Mode ChamferMode
        +IsAngleMode bool
        +IsLengthsMode bool
        +CloseRequested event
        +SelectModeCommand IRelayCommand
        +AddCommand IRelayCommand
        -SelectMode(mode)
        -Add()
        -CanAdd() bool
        -TryParseNumber(text, value) bool
    }

    class ChamferMode {
        <<enumeration>>
        Angle
        Lengths
    }

    class ChamferWindow {
        <<Avalonia Window>>
        +DataContext object
        -OnTitleBarPointerPressed(sender, e)
        -OnCloseClick(sender, e)
    }

    class ChamferWindow_axaml {
        <<View>>
        TextBox.Text ↔ ViewModel properties
        Button.Command → ViewModel commands
        IsVisible ← mode flags
    }

    BaseForm ..> SynchronizationContextChamferOperationService : создаёт с callback-ами
    BaseForm ..> ChamferWindowService : Show()
    SynchronizationContextChamferOperationService ..|> IChamferOperationService
    ChamferWindowService ..> AvaloniaHost : Post()
    ChamferWindowService ..> ChamferViewModel : создаёт
    ChamferWindowService ..> ChamferWindow : создаёт и показывает
    ChamferViewModel --> IChamferOperationService : вызывает
    ChamferViewModel --> ChamferMode : хранит режим
    ChamferWindow --> ChamferViewModel : DataContext
    ChamferWindow_axaml ..> ChamferViewModel : Binding / Command
    ChamferWindowService ..> ChamferViewModel : CloseRequested
```

## Ответственность классов

- `AvaloniaHost` — единая инициализация Avalonia и диспетчеризация действий на её STA UI-поток.
- `ChamferWindowService` — composition root модуля: создаёт ViewModel и окно, задаёт `DataContext` и связывает запрос закрытия.
- `ChamferViewModel` — состояние полей, выбранный режим, проверка чисел и команды.
- `IChamferOperationService` — граница между Avalonia-модулем и прикладной логикой BazisGUI.
- `SynchronizationContextChamferOperationService` — переходной адаптер, возвращающий запрос на WinForms UI-поток без зависимости от `BaseForm`.
- `ChamferWindow` и `.axaml` — представление и только оконное поведение: отображение, перетаскивание и закрытие.
- `ChamferMode` — внутреннее состояние UI; за пределы Chamfer-модуля не передаётся.
