# Задача: аргументы события `IModelView.Changed` и выборочное обновление VBO

Инструкция для реализации в режиме Code. Работа идёт в двух репозиториях, BazisCore и Gerbera, и в каждом заканчивается отдельным коммитом.

## 1. Зачем

Сейчас `ModelView` поднимает `Changed` с `EventArgs.Empty`, а `BaseForm.ModelView_Changed` на каждое изменение вызывает `RefreshModelViewBuffers()`. Тот удаляет и заново создаёт VBO **всех наборов всех типов**. Если выделить один узел, заново строятся индексы, координаты и нормали всех элементов и пересоздаются GL-буферы.

Что нужно сделать с буфером набора, зависит от того, что изменилось:

| Что изменилось | Что меняется в буфере | Действие в Gerbera |
| --- | --- | --- |
| видимость, внутренние грани, режим отображения набора | состав объектов или граней, способ отрисовки | пересобрать VBO набора (`RefreshModelSetBuffer`) |
| выделение, цвет набора, цвет выделения, прозрачность | только цвета, число объектов то же | перекрасить: `SetVBObjectAttribute(presenter, "цвет")` |
| ничего для этого набора | — | не трогать |

Цель: событие сообщает, **что изменилось и у каких наборов**, а Gerbera по этой информации трогает только затронутые наборы и делает самое дешёвое достаточное действие.

## 2. Где код

- BazisCore: `C:\BazisComponents\BazisCore`
  - `OperationalController/ModelView/ModelView.cs`, `ObjectsView.cs`
  - `OperationalController/Interfaces/IModelView.cs`
  - `OperationalController/Documents/ModelView.md`: проектные решения по виду, обязательно прочитать разделы «Что получает презентер», «Где живёт цикл отбора», «Внутренние грани объёмных элементов»
  - тесты: `TestModelCore` (MSTest), папка `ControllerTests`
- Gerbera: `C:\Gerbera`
  - `GUI/Methods/ModelViewOperations.cs`: `SubscribeToModelView`, `ModelView_Changed`, `RefreshModelViewBuffers`, `RefreshModelSetBuffer`
  - `GUI/Methods/Scene/SetVBObjectAttribute.cs`: обновление атрибута «цвет» или «координаты» существующего VBO
  - `GUI/Documents/DisplayObjects.md`: предыдущий шаг (`RequestRedraw()`)

Обе папки должны быть доступны в сессии Code.

**Связь репозиториев — NuGet, а не ссылка на проект.** `GUI/BazisGUI.csproj` подключает `OperationalController` пакетом (`PackageReference Include="OperationalController" Version="4.9.7"`). В `OperationalController.csproj` включены `GeneratePackageOnBuild` и `PackageOutputPath = c:\BazisComponents\IntPackages_v2\$(Configuration)`. Изменения BazisCore попадут в Gerbera только через новую версию пакета (см. шаг 3.8).

Номера строк в этом документе не приводятся: код мог измениться на шаге `RequestRedraw()`. Ищи по именам методов.

## 3. Правила кодирования (обязательны)

Из пользовательских настроек и `AGENTS.md` обоих репозиториев:

- не использовать явную типизацию переменных там, где это не нужно (`var`);
- параметры метода писать в одну строку, без переноса столбиком;
- не использовать приведение вниз (downcasting), разве что без него никак;
- придерживаться SOLID и ООП;
- не создавать статических классов и методов, если это не диктует производительность;
- не вызывать метод внутри круглых скобок другого метода: результат сначала присвоить переменной;
- не дублировать ссылки на существующие объекты в полях классов;
- методы, возвращающие коллекции, писать через `yield return` и использовать в `foreach`;
- не возвращать кортежи;
- цепочки LINQ — не длиннее трёх методов;
- не использовать глобальные переменные;
- XML-комментарии `<summary>` на русском, как в соседнем коде.

## 4. Шаг 1 — BazisCore

### 4.1. Перечисление вида изменения

`OperationalController/ModelView/ModelViewChange.cs`:

```csharp
/// <summary>
/// Вид изменения состояния представления.
/// </summary>
[Flags]
public enum ModelViewChange
{
    None = 0,
    Selection = 1,
    Visibility = 2,
    SetColor = 4,
    ViewMode = 8,
    SelectionColor = 16,
    Transparency = 32,
    InsideSurfaces = 64
}
```

### 4.2. Аргументы события

`OperationalController/ModelView/ModelViewChangedEventArgs.cs`. Неизменяемый объект: конструктор копирует переданные данные. Словари наружу не отдаются, вместо них методы-вопросы.

Изменения хранятся на трёх уровнях:

- **общие** — касаются всех наборов: прозрачность, внутренние грани;
- **по типу объектов** — выделение, видимость, цвет выделения;
- **по набору** — цвет и режим отображения набора.

Итоговое изменение набора — объединение трёх уровней. Такая раскладка точнее одного общего набора флагов: в пакете «выделить узлы + скрыть элементы» узлы только перекрасятся, а пересоберутся лишь элементы.

```csharp
/// <summary>
/// Описывает изменение состояния представления: что изменилось и у каких наборов.
/// </summary>
public class ModelViewChangedEventArgs : EventArgs
{
    readonly ModelViewChange common;
    readonly Dictionary<ObjType, ModelViewChange> byType;
    readonly Dictionary<ISetInfo, ModelViewChange> bySet;

    public ModelViewChangedEventArgs(ModelViewChange common, IReadOnlyDictionary<ObjType, ModelViewChange> byType, IReadOnlyDictionary<ISetInfo, ModelViewChange> bySet)
    {
        this.common = common;
        this.byType = new Dictionary<ObjType, ModelViewChange>(byType);
        this.bySet = new Dictionary<ISetInfo, ModelViewChange>(bySet, ReferenceEqualityComparer.Instance);
    }

    /// <summary>Возвращает все изменения, затрагивающие заданный набор.</summary>
    public ModelViewChange GetChanges(ISetInfo setInfo)
    {
        byType.TryGetValue(setInfo.ObjType, out var typeChanges);
        bySet.TryGetValue(setInfo, out var setChanges);
        return common | typeChanges | setChanges;
    }

    /// <summary>Есть ли у набора хотя бы одно из указанных изменений.</summary>
    public bool HasAny(ISetInfo setInfo, ModelViewChange changes)
    {
        var setChanges = GetChanges(setInfo);
        return (setChanges & changes) != ModelViewChange.None;
    }
}
```

Если компилятор не примет `IReadOnlyDictionary` в конструкторе `Dictionary` с компаратором, подойдёт `IEnumerable<KeyValuePair<,>>` или копирование в цикле. Ключ `ISetInfo` — по ссылке, как у `ObjectsView.setStyles`: наборы переименовываются, и сравнивать их по имени нельзя.

### 4.3. Накопитель изменений внутри `ModelView`

Вместо флага `hasChanges` в `ModelView` добавить закрытый вложенный класс, например `PendingChanges`, по образцу уже существующего `UpdateScope`:

- поля: общий `ModelViewChange`, `Dictionary<ObjType, ModelViewChange>`, `Dictionary<ISetInfo, ModelViewChange>` (с `ReferenceEqualityComparer.Instance`);
- `Add(ModelViewChange change, ObjType objType)`, `Add(ModelViewChange change, ISetInfo setInfo)`, `AddCommon(ModelViewChange change)` — объединяют флаги через `|=`;
- `IsEmpty`;
- `CreateEventArgs()` — создаёт аргументы (они копируют данные);
- `Clear()`.

`Raise(bool changed)` заменить перегрузками:

```csharp
private void Raise(bool changed, ModelViewChange change, ObjType objType)
private void Raise(bool changed, ModelViewChange change, ISetInfo setInfo)
private void RaiseCommon(bool changed, ModelViewChange change)
```

Логика у всех одна:

1. если `!changed`, выйти;
2. добавить изменение в накопитель;
3. если `updateDepth == 0`, вызвать `Flush()`.

`Flush()` при пустом накопителе ничего не делает. Иначе он создаёт аргументы, очищает накопитель и поднимает `Changed`. Порядок важен: сначала очистить накопитель, потом поднять событие, потому что обработчик может снова изменить вид. `EndUpdate()` при `updateDepth == 0` и непустом накопителе вызывает `Flush()`.

### 4.4. Что сообщает каждый метод

В циклах по `views` перейти с `views.Values` на пары, чтобы тип был известен: `foreach (var pair in views)`.

| Член `ModelView` | Изменение | Уровень |
| --- | --- | --- |
| `Select`, `Deselect`, `ToggleSelection`, `ClearSelection(objType)` | `Selection` | тип |
| `SetSelection` | `Selection` | каждый тип, у которого вид изменился (включая очищенные) |
| `ClearSelection()` | `Selection` | каждый тип, у которого вид изменился |
| `SetVisible` | `Visibility` | тип |
| `ShowAll` | `Visibility` | каждый изменённый тип |
| `HideSelected` | `Visibility \| Selection` | каждый изменённый тип |
| `SetSetColor` | `SetColor` | набор |
| `SetViewMode` | `ViewMode` | набор |
| `SelectionColor` (set) | `SelectionColor` | каждый тип из `SelectedTypes`: только у них есть объекты этого цвета. Если выделения нет, накопитель остаётся пустым, и событие не поднимается: перерисовывать нечего |
| `Transparency` (set) | `Transparency` | общий |
| `HideInsideSurfaces` (set) | `InsideSurfaces` | общий: флаг общемодельный (см. `ModelView.md`) |
| `Prune` → `ObjectsView.Prune` | `Visibility \| Selection` | каждый изменённый тип |
| `Prune` → `PruneSetStyles` | `Visibility` | тип (консервативно: пересборка) |
| `Reset` | `Visibility \| Selection` | каждый изменённый тип |

### 4.5. Интерфейс

В `IModelView` заменить `event EventHandler Changed;` на `event EventHandler<ModelViewChangedEventArgs> Changed;` и обновить XML-комментарий: какие изменения и на каком уровне сообщаются.

Совместимость: существующий обработчик `ModelView_Changed(object sender, EventArgs e)` продолжит подписываться, потому что C# допускает контравариантность параметров при преобразовании группы методов. Поэтому шаг 1 не ломает Gerbera, даже если шаг 2 ещё не сделан.

### 4.6. Тесты

Новый файл `TestModelCore/ControllerTests/ModelViewChangedTests.cs` (MSTest). Как создать `ModelView` над моделью, посмотри в существующих тестах, например `TestModelScenePresentor.cs` и `TestServices.cs`. Проверить:

- `Select` одного типа даёт `Selection` для наборов этого типа и `None` для наборов других типов;
- повторный `Select` тех же номеров не поднимает событие;
- пакет `BeginUpdate` с `Select(узлы)` и `SetVisible(элементы, false)` поднимает **одно** событие: у наборов узлов `Selection` без `Visibility`, у наборов элементов `Visibility`;
- `SetSetColor(set)` даёт `SetColor` только у этого набора, у других наборов того же типа — `None`;
- `Transparency` даёт `Transparency` у всех наборов;
- `HideSelected` даёт `Visibility | Selection`;
- вложенные `BeginUpdate` дают одно событие на выходе из внешнего;
- обработчик, меняющий вид внутри `Changed`, получает следующее событие отдельно, а не теряет его.

### 4.7. Документация

В `OperationalController/Documents/ModelView.md` добавить раздел «Аргументы события `Changed`». В нём описать три уровня, таблицу из 4.4 и принцип: **аргументы говорят, что изменилось в представлении, а не что делать с буферами — про VBO BazisCore не знает**.

В разделе «Чем это лучше предыдущих попыток» поправить фразу о том, что презентер набора «пересоздаётся на `Changed`»: теперь это зависит от вида изменения.

### 4.8. Версия пакета

Поднять `<Version>` в `OperationalController.csproj`: появились новые публичные типы, и изменился тип события, поэтому минорная версия — 4.9.7 → 4.10.0. **Не пересобирать под тем же номером**: NuGet возьмёт старый пакет из кеша. Собрать проект, чтобы пакет появился в `c:\BazisComponents\IntPackages_v2\<Configuration>`, и убедиться, что эта папка есть среди источников NuGet у решения Gerbera (`dotnet nuget list source` или `nuget.config`).

Запустить тесты `TestModelCore`. По `AGENTS.md` BazisCore устаревший проект `TestModel` на компиляцию не проверять.

Коммит в BazisCore.

## 5. Шаг 2 — Gerbera

### 5.1. Пакет

Найти все проекты решения `BazisGUISolution.sln`, которые ссылаются на `OperationalController`, и поднять версию на 4.10.0. Если NuGet выдаст NU1605 (понижение версии) из-за транзитивных зависимостей `PreProc`/`PostProc`, сообщить об этом, а не подавлять предупреждение.

### 5.2. Обработчик

В `ModelViewOperations.cs`:

```csharp
const ModelViewChange RebuildChanges = ModelViewChange.Visibility | ModelViewChange.InsideSurfaces | ModelViewChange.ViewMode;
const ModelViewChange ColorChanges = ModelViewChange.Selection | ModelViewChange.SetColor | ModelViewChange.SelectionColor | ModelViewChange.Transparency;

/// <summary>
/// Обновляет буферы наборов, затронутых изменением представления, и запрашивает кадр.
/// </summary>
private void ModelView_Changed(object sender, ModelViewChangedEventArgs e)
{
    if (IsDisposed || project == null)
        return;

    if (InvokeRequired)
    {
        BeginInvoke(new Action(() => ModelView_Changed(sender, e)));
        return;
    }

    foreach (var setInfo in GetModelSets())
    {
        if (e.HasAny(setInfo, RebuildChanges))
            RefreshModelSetBuffer(setInfo);
        else if (e.HasAny(setInfo, ColorChanges))
            RecolorModelSetBuffer(setInfo);
    }

    RequestRedraw();
}

/// <summary>
/// Возвращает все наборы расчётной модели.
/// </summary>
private IEnumerable<ISetInfo> GetModelSets()
{
    foreach (ObjType objType in Enum.GetValues(typeof(ObjType)))
    {
        foreach (var setInfo in project.GetModelSetsInfo(objType))
            yield return setInfo;
    }
}

/// <summary>
/// Обновляет цвета буфера набора без пересборки геометрии.
/// </summary>
private void RecolorModelSetBuffer(ISetInfo setInfo)
{
    var presenter = project.CreateModelObjectsPresentor(setInfo);
    SetVBObjectAttribute(presenter, "цвет");
}
```

- Константы (`const`) допустимы: это значения времени компиляции, а не статическое состояние или статические методы, которых правила просят избегать.
- `RefreshModelViewBuffers()` удалить, если после замены у него не осталось вызывающих.
- `RefreshModelSetBuffer(setInfo)` не менять: им пользуется и `MergeElementSets`.
- Режим отображения идёт через пересборку, а не через `ChangeViewModeVBObjects`: так не нужно отдельно сопоставлять `ViewMode` и `ObjView` для точечных и линейных наборов, а пересобирается всё равно только один набор.

### 5.3. Почему перекраска безопасна и когда нет

`SetVBObjectAttribute` пишет новый массив цветов длиной `vboObjs.ColorLength` поверх существующего буфера. Это корректно, только если состав видимых объектов набора с момента создания буфера не изменился. Поэтому в обработчике пересборка проверяется первой: если у набора в этом же событии есть `Visibility`, `InsideSurfaces` или `ViewMode`, выполняется пересборка.

Если у набора нет VBO (все объекты скрыты или набор пуст), `FindVBObj` вернёт `null`, и перекраска ничего не сделает. Это правильно.

Проверь, что `presenter.CreateVertexes(length, "цвет")` при перекраске возвращает массив той же длины, что и при создании буфера в `TryCreateVBObject`. Если длина цветов зависит от чего-то кроме состава объектов, сообщи об этом до продолжения.

### 5.4. Главный риск: код, полагавшийся на полную пересборку

До этого изменения **любое** событие `Changed` пересобирало все наборы. Некоторые обработчики меняют модель (удаляют или объединяют объекты) и затем вызывают `ModelView.Prune()`, `ClearSelection()` или `Reset()`. Если явного обновления VBO у них нет, буферы, возможно, обновлялись только благодаря этому побочному эффекту. Теперь пересоберутся только наборы с изменённым видом.

Проверить каждого вызывающего `ModelView.Prune()`, `ModelView.Reset()` и `project.ClearNotExistedModelData()`. Как минимум:

- `Scene/SceneEvents.cs` — `menuItem_DeleteSelectedObjects_Click`;
- `Navigator/Object/ShowHideDelObject.cs` — `navigator_DelObjectEvent`, ветка сетки;
- `Console/MergeElementSets.cs`.

Там, где модель изменилась, VBO затронутых наборов должны обновляться явно (`RefreshModelSetBuffer` или `DeleteVBObjects`/`CreateVBObjects`), а кадр — запрашиваться через `RequestRedraw()`. Не восстанавливать полную пересборку «на всякий случай», а обновлять именно то, что изменилось. Список найденных мест и принятых решений привести в итоговом отчёте.

### 5.5. Документация

В `GUI/Documents/DisplayObjects.md` в разделе 7 («Следующий шаг») отметить, что передача затронутых наборов в `Changed` выполнена, и кратко описать правило «пересобрать или перекрасить».

Коммит в Gerbera.

## 6. Проверка

1. Решение BazisCore собирается, тесты `TestModelCore` проходят, включая новые.
2. `BazisGUISolution.sln` собирается с новой версией пакета без новых предупреждений.
3. Вручную на проекте с сеткой из нескольких наборов разных типов:
   - выделение узла кликом и рамкой (с Shift и без): меняется цвет только выделенного, остальное не мигает и не пропадает;
   - выделение элементов на крупной сетке: отклик заметно быстрее, чем до изменения;
   - скрыть выбранное / показать скрытые;
   - показ и скрытие наборов и групп в навигаторе;
   - смена цвета узлов в настройках (цвет набора);
   - смена цвета выделения при существующем выделении;
   - ползунок прозрачности;
   - кнопка «Показать внутренние объекты» (`HideInsideSurfaces`);
   - удаление выделенных объектов со сцены и из навигатора (раздел 5.4): удалённое исчезает, остальное на месте;
   - объединение наборов элементов из консоли.

## 7. Чего не делать

- Не передавать в событие номера объектов. У `VBObject` нет соответствия «номер → смещение в буфере», частичное обновление буфера — отдельная задача.
- Не добавлять в BazisCore ничего про VBO, буферы и способ обновления.
- Не менять презентеры и правило цвета в `ModelView.GetColor`.
- Не трогать `RequestRedraw()` и `DisplayObjects()`: это закрытый предыдущий шаг.

## 8. Итоговый отчёт

В конце перечислить:

- изменённые файлы в обоих репозиториях;
- новую версию пакета;
- результаты сборки и тестов;
- найденные в разделе 5.4 места и что сделано в каждом;
- всё, что пришлось решить по-другому, чем здесь написано, и почему.
