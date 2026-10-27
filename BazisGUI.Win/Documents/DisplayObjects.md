# Перерисовка сцены: `RequestRedraw()` вместо прямых вызовов `DisplayObjects()`

Пояснения к схеме `DisplayObjects.mdpuml`. Номера строк даны по состоянию кода на 23.09.2026.

## 1. Что сейчас

`BaseForm.DisplayObjects()` (`GUI/Methods/Scene/DisplayObjects.cs`) рисует кадр синхронно и сразу же вызывает `scene.SwapBuffers()`. Обработчики зовут его напрямую, около 110 раз в 47 файлах. Отсюда три проблемы:

- **Лишние кадры.** Одно действие рисует несколько кадров подряд. Выделение рамкой даёт три: `ModelView.Changed`, затем `SelectByRect.cs:47`, затем `MouseEvents.cs:132`.
- **Зависимость от побочных эффектов.** Обработчик не всегда знает, будет ли перерисовка. `MergeElementSets` явно ничего не рисует и полагается на то, что `ModelView.ClearSelection()` поднимет `Changed`. Если ничего не было выделено, события нет, и на экране остаётся старое состояние.
- **Несовместимость с Avalonia.** В `OpenGlControlBase` кадр нельзя нарисовать по требованию из обработчика. Можно только попросить следующий (`RequestNextFrameRendering()`), см. `scene.avalonia.md`, раздел 6.

## 2. Что предлагается

Один путь отрисовки, `scene.Paint → DisplayObjects()`, и два способа его запросить.

```csharp
/// <summary>
/// Запрашивает перерисовку сцены. Несколько запросов до ближайшего WM_PAINT дают один кадр.
/// </summary>
public void RequestRedraw()
{
    if (IsDisposed)
        return;

    if (InvokeRequired)
    {
        BeginInvoke(new Action(RequestRedraw));
        return;
    }

    scene.Invalidate();
}

/// <summary>
/// Рисует кадр синхронно, до возврата из метода. Только для захвата изображения с экрана.
/// </summary>
public void RenderNow()
{
    scene.Refresh();
}
```

После перехода `DisplayObjects()` становится `private` и вызывается только из подписки `scene.Paint` в `SceneInitialization.cs:63`. Это закрывает путь для новых прямых вызовов. `BaseForm` не реализует `ISceneControl` (его реализует только `SceneController`), поэтому смена видимости ничего наружу не ломает.

`ModelView_Changed` (`ModelViewOperations.cs:49`) вместо `DisplayObjects()` вызывает `RequestRedraw()`.

## 3. Кто будет вызывать `RequestRedraw()` (`<<caller>>`)

Здесь меняется то, о чём `IModelView` не знает: камера, VBO, слои, текст, геометрические события, настройки рендера. Явный запрос кадра нужен, поэтому `DisplayObjects()` заменяется на `RequestRedraw()` один к одному.

| Группа | Файл: строка | Что меняется |
| --- | --- | --- |
| Сцена: ввод | `Scene/MouseEvents.cs`: 44, 50, 61, 74, 132, 138 | камера, рамка выделения, точка вращения |
| | `Scene/SceneEvents.cs`: 139, 208, 259, 264 | центр вращения, Esc (сброс событий текста и геометрии), C, F |
| Сцена: панели | `Scene/Buttons/ViewToolStrip.cs`: 23, 29, 35, 54, 73, 92, 98, 104 | камера |
| | `Scene/Buttons/FitToScreen.cs:21`, `ShowBasis.cs:40`, `ShowCountours.cs:42`, `DisplayToolStrip.cs:85` | камера, слой базиса, VBO контуров и нормалей |
| | `Scene/Buttons/AdvanceSelection.cs:124`, `Scene/VBObjects/ColorObjects.cs:35` | атрибут «цвет» VBO |
| Настройки | `Settings/MainMenuEvents.cs`: 69, 77, 87, 95, 110, 127 | фон, освещение, прозрачность, проекция, интенсивность, источник света |
| Навигатор | `Navigator/Sets/DelSetEvent.cs:57`, `Mesh/DelElementsEvent.cs:39`, `Objects/DelAllObjectsEvent.cs:28` | удаление VBO |
| | `Navigator/Object/ShowHideDelObject.cs:74` | удаление геометрии |
| | `Navigator/NavigatorEvents.cs`: 51, 68 | пересоздание VBO при снятии результатов |
| | `Navigator/Geo/SelectGeoEvent .cs`: 52, 59, 66, 72, 94, 118 | 3D-текст номеров, VBO `transPoints` |
| | `Navigator/Objects/GetSurfaceProperty.cs:60` | 3D-текст номеров |
| | `Navigator/Time/SelectTimeEvent.cs:74` (`ShowResults`) | поле результатов, шкала, текст |
| Консоль | `Console/ConsoleEvents.cs`: 41, 139, 175 | новый VBO, координаты VBO |
| | `Console/BeamConnection.cs:35`, `FindCoincidentNodes.cs:51`, `NodesShiftCoordinate.cs:51` | новые или пересозданные VBO |
| | `Console/MergeElementSets.cs` — **вызова нет, добавить** после `RefreshModelSetBuffer(set)` | пересоздание VBO наборов |
| Сетка и геометрия | `Mesh/GenerateMesh2DEvent.cs:37`, `GenerateMesh3DEvent.cs:44`, `GreateBoundaryMesh.cs:81`, `QuadMesh.cs:23`, `RefineMesh.cs:25` | новые VBO сетки |
| | `Geom/GeometryBuilder.cs`: 140, 223; `Geom/CreateChamfer.cs`: 21, 44; `Scene/PreviewChamfer.cs:195` | VBO геометрии, предпросмотр фаски |
| | `PropertyPanel/ChangeMeshObjectProperty.cs`: 46, 70 | координаты VBO |
| Инструменты и результаты | `UtilityToolStrip.cs`: 56, 68, 172, 261, 282 | измерения: линии, текст |
| | `UtilityToolStrip.cs`: 311, 345, 386 | VBO сечения |
| | `UtilityToolStrip.cs`: 462, 479, 489, 549 | плоскость отсечения, `ClipControl.RedrawClipPlane`, `CaptureData` |
| | `Results/Reflect.cs`: 49, 62, 75, 89 | габарит, плоскость и копии VBO при отзеркаливании |
| | `Results/CreatePlot.cs`: 23, 38, 48, 98; `CreateDiagramm.cs:22` | подсказка на экране, пересоздание VBO, 3D-текст |
| | `Results/CreateAnimation.cs:39`, `Player/Checking.cs:100` | сброс событий текста и геометрии при закрытии |
| | `BasePage.cs`: 83, 108 | 2D-подсказка асинхронной команды |
| BaseForm | `BaseForm.cs`: 115, 466, 518, 718 | загрузка, создание, открытие проекта, добавление сетки |

## 4. Какие вызовы удаляются (`<<drop>>`)

Эти обработчики меняют только `IModelView`: выделение, видимость, цвет выделения. Кадр уже запросит `ModelView_Changed`, поэтому явный вызов удаляется, а не заменяется.

| Файл: строка | Почему лишний |
| --- | --- |
| `Navigator/Mesh/SelectMeshEvent .cs`: 36, 43, 63, 70, 90, 97 | `ShowElements()` сводится к `ModelView.SetVisible` |
| `Navigator/Mesh/SelectMeshEvent .cs`: 50, 77, 104 | `DelElements()` уже запрашивает кадр (строка 39) |
| `Navigator/Mesh/ShowHideElementsEvent.cs:34` | только `ModelView.SetVisible` |
| `Navigator/Cond/SelectCondEvent.cs:64` | `SelectionColor` и `SetSelection` |
| `Navigator/Sets/SelectSetEvent.cs:46` | `ShowAdjacenciesSet()` сводится к `ModelView.SetVisible` |
| `Scene/SelectObjects/SelectByRect.cs:47` | `Select`/`Deselect` внутри `BeginUpdate`; после него кадр ещё раз запрашивает `MouseUp` |
| `Scene/Buttons/AdvanceSelection.cs`: 248, 275 | `Select`/`Deselect` |
| `Scene/Buttons/AdvanceSelection.cs:303`, `Scene/Buttons/SelectObjects.cs:30` | `SetBackColorToAllObjects()` сводится к `ClearSelection()`; если выделения не было, рисовать нечего |

Уже переделаны и явных вызовов не содержат: `ChangeGroupViewState`, `SelectGroupEvent`, `EditGroup`, `ShowGroupWithNodes`, `ChangeSetViewState`, `ShowAdjSetEvent`, `SelectObjectEvent`, `ChangeAllObjectsViewStateEvents`, `ShowAdjac`, `FindFreeNodesEvent`, `ConsoleEvents.FindObject/FindVolElems`, `SceneEvents` (скрыть и показать выбранное), `ShowInsideObjects`.

Если обработчик меняет и `IModelView`, и что-то ещё (например, Esc в `SceneEvents.cs:208` сбрасывает `DisplayText*Event`), вызов остаётся и заменяется на `RequestRedraw()`: лишний запрос склеится с запросом от `Changed` в один кадр.

## 5. Где кадр нужен сразу (`<<sync>>`)

`CreateAnimation.CreateGIFAnimation()` в цикле вызывает `ShowResults()`, а затем `CreateScreenShot()`. Снимок делается через `Graphics.CopyFromScreen`, то есть копируется то, что уже на экране. Если `ShowResults()` только запросит кадр, к моменту снимка он ещё не нарисован, и все кадры GIF окажутся одинаковыми.

Поэтому в цикле между `ShowResults(result, resName)` и `CreateScreenShot()` нужен `RenderNow()`. Сам `ShowResults()` вызывает `RequestRedraw()`: при выборе времени в навигаторе синхронный кадр не нужен.

`btnMakeScreenShot_Click` снимает уже показанный кадр и перерисовки не требует.

## 6. Риски и проверка

- **Отзывчивость при перетаскивании.** `WM_PAINT` в Windows имеет низкий приоритет. При вращении и панорамировании кадры идут по мере опустошения очереди сообщений, а движения мыши тоже склеиваются, так что обычно картинка не отстаёт. Если на больших моделях появится рывок, в трёх ветках `GlControl_MouseMove` можно вызывать `RenderNow()` вместо `RequestRedraw()`.
- **Потоки.** `BasePage.AsyncMethodContainer`, `FindCoincidentNodes` и `CreatePlot.SelectContainerAsync` выполняются рядом с `Task.Run`. `RequestRedraw()` сам переходит в UI-поток через `BeginInvoke`, поэтому отдельный `Invoke` ради перерисовки не нужен.
- **Код, который читает буфер кадра.** Кроме анимации, это стоит проверить для `UtilityToolStrip.CaptureData` (transform feedback) и `SceneController.CaptureScreenshot`. Первый пишет в свои буферы до перерисовки, второй сейчас не вызывается.
- **Поведение, не связанное с переходом.** Точка вращения не рисуется при нажатии средней кнопки: `DisplayRotationPointEvent` больше никто не вызывает, а `RotationPointLayer.IsVisible` из WinForms не переключается. Это нужно исправлять отдельно, через `sceneController` в `GlControl_MouseDown/MouseUp`.

Что проверить вручную после замены: вращение, панорамирование и масштаб; выделение кликом и рамкой, со Shift и без; Esc, C и F; показ и скрытие наборов и групп в навигаторе; удаление набора; генерацию сетки; отсечение плоскостью; отзеркаливание; измерения; смену фона, освещения и прозрачности; GIF-анимацию (кадры должны различаться).

## 7. Следующий шаг (`<<next>>`)

Хост подписывается на `SceneController.RenderRequested` через `sceneController.RenderRequested += RequestRedraw;` в `SceneInitialization`. После этого:

- ввод мыши можно передать в `SceneInputController`: он уже поднимает `Invalidated → RenderRequested`;
- слои и `VBOController` могут сами сообщать об изменениях, и группа «Сцена: панели» постепенно перестаёт вызывать `RequestRedraw()`;
- при переходе на `SceneView` (Avalonia) тело `RequestRedraw()` меняется на `RequestNextFrameRendering()`, а обработчики не меняются.

Отдельный шаг — передавать в `IModelView.Changed` затронутые наборы, чтобы `RefreshModelViewBuffers()` не пересобирал VBO всех наборов на каждое изменение. От схемы запроса кадра он не зависит.

**Сделано** (`ModelViewChanged.md`). `IModelView.Changed` теперь несёт `ModelViewChangedEventArgs` — что изменилось и у каких наборов. `ModelView_Changed` в `ModelViewOperations.cs` для каждого набора расчётной модели решает по правилу «пересобрать или перекрасить»:

- видимость, внутренние грани, режим отображения набора изменились → `RefreshModelSetBuffer` (пересобрать VBO набора: состав объектов или способ отрисовки могли поменяться);
- иначе, если изменились выделение, цвет набора, цвет выделения или прозрачность → `RecolorModelSetBuffer` (`SetVBObjectAttribute(presenter, "цвет")`: число объектов то же, буфер просто перекрашивается);
- иначе набор не трогается.

`RefreshModelViewBuffers()` убран за ненадобностью. Места, где модель менялась в обход выделения вида (значит, `Prune()`/`ClearSelection()` могли не отметить набор изменившимся), теперь обновляют свои буферы явно — см. раздел 5.4 `ModelViewChanged.md`.
