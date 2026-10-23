# Перевод рендеринга 3D-сцены на Avalonia

Документ — пояснительная записка к диаграмме классов `ДК. Scene.Avalonia.mdpuml`
(картинка — `ДК. Scene.Avalonia.png`). Диаграмма показывает только состав классов и связи;
все обоснования, ограничения и подводные камни собраны здесь.

**Целевые платформы — Windows и Linux.** Это определяет и состав классов, и порядок работ:
кроссплатформенность здесь не «когда-нибудь потом», а требование, под которое
часть задач переезжает из отложенных в обязательные.

Соответствует текущему состоянию кода в `GUI/Scene`, `GUI/Methods/Scene` и `GUI/AvaloniaUI`.

## 1. Что мешает переходу сейчас

**Состояние вида хранится внутри OpenGL.** `BaseForm.ViewMatrix` читает `GL_MODELVIEW_MATRIX`
через `GL.GetFloat`, а `MoveCamera` и `RotateCamera` пишут в него через `GL.Translate` и
`GL.Rotate`. Любая операция камеры требует живого контекста и текущего потока рендера,
поэтому камеру нельзя ни вынести в объект, ни покрыть тестами. При этом интерфейсы
`ISceneCamera` и `ISceneControl.GetCamera()` уже написаны и ждут реализации.

**Сцена размазана по `BaseForm`.** Около сорока partial-файлов в `GUI/Methods/Scene/`:
рендер, ввод, пикинг, текст, компас, базис, сечение, скриншот. Видимость элементов
управляется подпиской и отпиской делегатов, причём снятие идёт перебором
`GetInvocationList()` и сравнением `del.Method.Name.Contains(searchMethod)` —
см. `HideText3D` и `HideGeometryObj`.

**Попутная находка.** `DisplayBasis` вызывает `gluNewQuadric()` на каждом кадре
и ни разу не вызывает `gluDeleteQuadric` — утечка неуправляемого ресурса.

**Что переносится дёшево.** `SelectByPoint` и `SelectByRect` уже чисто расчётные:
проекция координат плюс `Hull2DCreator`, вызовов GL в них нет. Им нужна только камера.
Весь VBO/FBO/шейдерный слой (`VBObject`, `VBOController`, `AverageColorRenderer`,
`Advanced3DClipper`, `ClipPlaneRenderer`, `ShaderProgramCreator`) платформо-независим:
вызовы OpenTK работают в любом контексте, на Linux в том числе.

## 2. Что ломается на Linux

Хорошая новость: Win32-зависимости сцены локализованы в трёх файлах, а не размазаны по коду.

| Что | Где | Замена |
| --- | --- | --- |
| `wglUseFontBitmapsW`, `wglGetCurrentDC` | `Methods/PlatformSpecific/PlatformSpecific.cs`, `SceneInitialization.ChangeTextFont` | `SkiaGlyphAtlasTextRenderer` + `GlyphAtlas` |
| `gdi32`: `SelectObject`, `DeleteObject` | там же | уходит вместе с подменой шрифта |
| `glu32`: `gluPerspective` | `UpdateProjection`, `CameraInitialization` | `CameraProjection` — матрица считается в коде |
| `glu32`: `gluNewQuadric`, `gluCylinder`, `gluSphere` | `DisplayBasis`, `DisplayCompass`, `DisplayConus`, `DisplaySphere` | `QuadricMeshFactory` |
| `new Font(...)`, `font.ToHfont()`, `FontStyle` | `SceneInitialization.ChangeTextFont` | `GlyphAtlas` поверх SkiaSharp |
| `image.Save(path, ImageFormat.Bmp)` | `Methods/Scene/Buttons/MakeScreenShot.cs` | `GlFrameGrabber`: `glReadPixels` + кодирование через SkiaSharp |
| `OpenTK.GLControl.GLControl` | `BaseForm.Designer.cs` | `SceneView : OpenGlControlBase` |
| `Gle` (`Scene/TaoExtension.cs`), `Tao.*.dll` | `GUI/Scene` | уже `[Obsolete]`, удалить |

Отдельно о `System.Drawing`. Типы `Color`, `Point`, `PointF`, `Rectangle`, `Size` живут
в сборке `System.Drawing.Primitives` и кроссплатформенны — сигнатуры `ISceneControl`,
`ISceneCamera`, `ISceneScale`, `IScaleItem`, `SceneScale`, `ScreenRectangle` менять не нужно.
А вот `Font`, `FontStyle`, `Bitmap`, `Image`, `ImageFormat` — это `System.Drawing.Common`,
которая начиная с .NET 8 на не-Windows бросает `PlatformNotSupportedException`
(конфигурационный переключатель `EnableUnixSupport`, существовавший в .NET 7, удалён).
Тянуть `System.Drawing.Common` в ядро нельзя.

**Главное следствие для планирования.** Текст и примитивы — не «этап 2 когда-нибудь»,
а обязательная часть работ: без них на Linux не запустится ничего.

**Второе следствие.** Переходная схема «Avalonia внутри WinForms» держится на пакете
`Avalonia.Win32.Interoperability`, который существует только под Windows. Значит
`WinFormsSceneHost` и `AvaloniaMasterHost` работают только на Windows, и Linux-сборка
появляется не раньше, чем оболочка целиком переедет на Avalonia — шаг 7 в разделе 5.
До этого момента Linux собирать и запускать нечего; проверять кроссплатформенность
стоит на отдельном стенде с голым окном Avalonia и `SceneView` внутри.

## 3. Выделение сборки

`BazisGUI.csproj` сейчас `net8.0-windows` с `UseWindowsForms`. Пока ядро сцены лежит
в этом проекте, ничто не мешает случайно затащить в него WinForms или Win32 обратно.

Поэтому ядро выносится в отдельный проект `BazisGUI.Scene.Core` с `TargetFramework net8.0`
(без суффикса `-windows`) и без `UseWindowsForms`. Компилятор начинает сам сторожить
границу: любая ссылка на `System.Windows.Forms`, `System.Drawing.Common` или Win32 P/Invoke
перестаёт собираться. Это дешевле и надёжнее, чем договорённости и код-ревью.

Состав проекта — всё содержимое пакета `BazisGUI/Scene/Core` на диаграмме плюс
переиспользуемый GL-слой из `GUI/Scene`. Зависимости: OpenTK, MathNet.Numerics,
`Geometry`, `Model.Interfaces`, SkiaSharp.

`MasterInterface.csproj` тоже `net8.0-windows` — для `IAvaloniaMaster` суффикс не нужен
и его стоит снять при случае, иначе внешние Avalonia-мастера не соберутся под Linux.

## 4. Схема перехода

### Контекст OpenGL на обеих платформах

Avalonia позволяет задать профиль контекста и там, и там, причём `GlVersion` принимает
флаг `isCompatibilityProfile`:

```csharp
// Windows
builder.With(new Win32PlatformOptions
{
    RenderingMode = [Win32RenderingMode.Wgl],
    WglProfiles = [new GlVersion(GlProfileType.OpenGL, 4, 6, isCompatibilityProfile: true)]
});

// Linux
builder.With(new X11PlatformOptions
{
    RenderingMode = [X11RenderingMode.Glx, X11RenderingMode.Software],
    GlProfiles = [new GlVersion(GlProfileType.OpenGL, 4, 6, isCompatibilityProfile: true),
                  new GlVersion(GlProfileType.OpenGL, 3, 3, isCompatibilityProfile: true)]
});
```

На Windows по умолчанию Avalonia использует ANGLE/GLES, где fixed-function не работает
вообще, поэтому `Win32RenderingMode.Wgl` обязателен. На Linux по умолчанию уже GLX,
менять режим не требуется — нужен только профиль.

Compatibility-профиль позволяет не переписывать `glMatrixMode`, `glRotate`, `glLoadMatrix`
и вызовы VBO с `glVertexPointer` на первых шагах. Но на Linux это подпорка, а не решение:
Mesa отдаёт compatibility-профиль 4.5/4.6 на современных драйверах (iris, radeonsi, llvmpipe),
однако на старых версиях и на части драйверов compat ограничен 3.0. Проприетарный драйвер
NVIDIA compat поддерживает полностью. Поэтому переход на собственные матрицы и шейдеры
планируется как обязательный шаг 8, а не как опция.

Вся эта настройка живёт в одном классе `GlPlatformConfigurator` с разветвлением
`ApplyWin32` / `ApplyX11`, который вызывается из `AvaloniaHost.Initialize`.

### Встраивание в текущую оболочку

Тем же приёмом, который уже применён для мастеров: новый `SceneView` кладётся
в `splitContainer2.Panel1` вместо `GLControl` через `WinFormsSceneHost : WinFormsAvaloniaControlHost` —
зеркало существующего `AvaloniaMasterHost`. Механизм встраивания и потоковая модель
описаны в `AvaloniaUI/Documents/AvaloniaMasterHosting.md`. Как сказано выше, это
Windows-only и временно.

## 5. Порядок работ

1. Завести проект `BazisGUI.Scene.Core` (`net8.0`) и перенести в него `GUI/Scene`.
2. `SceneCamera`, `CameraProjection`, `Viewport` — снять состояние вида с GL-стека
   и убрать `gluPerspective`.
3. `SceneRenderSettings`, `IRenderContext`, `ISceneLayer`, `SceneLayerCollection`.
4. Перенос `GUI/Methods/Scene/*.cs` в `SceneController` и слои; `BaseForm` перестаёт вызывать GL.
5. `ScenePicker` и `SceneInputController` со своими типами событий.
6. `SceneView`, `AvaloniaGlBindingsContext`, `GlPlatformConfigurator`;
   `SceneView` встраивается в WinForms через `WinFormsSceneHost`. С этого момента
   на Windows всё работает как раньше.
7. `SkiaGlyphAtlasTextRenderer` + `GlyphAtlas`, `QuadricMeshFactory`, `GlFrameGrabber` —
   снятие Win32 P/Invoke и `System.Drawing.Common`. Удаляются `PlatformSpecific.cs` и `Gle`.
   После этого ядро сцены собирается и работает под Linux.
8. `SceneViewModel` и `SceneToolbarViewModel`; оболочка — `MainWindow` вместо `BaseForm`,
   `WinFormsSceneHost` и `Avalonia.Win32.Interoperability` удаляются.
   **Здесь появляется рабочая Linux-сборка приложения целиком.**
9. Собственные матрицы и шейдеры вместо fixed-function; снимается требование
   compatibility-профиля, приложение перестаёт зависеть от возможностей драйвера.

Шаги 2–7 проверяются на Windows. Для раннего контроля кроссплатформенности имеет смысл
после шага 7 держать маленький стенд — окно Avalonia с одним `SceneView`, собираемое
под обе ОС; он ловит регрессии до того, как оболочка переедет.

## 6. Пояснения к классам диаграммы

### Ядро — `BazisGUI/Scene/Core`

`SceneController` принимает на себя те самые ~40 partial-файлов `GUI/Methods/Scene/*.cs`.
Реализация `ISceneControl` переезжает из `BaseForm`, сам контракт не меняется.
После переноса `BaseForm` не содержит ни одного вызова GL.

`SceneCamera` — ключевое изменение всего перехода. Видовая матрица переносится
из стека GL в поле класса. Камера перестаёт требовать живой контекст и становится
тестируемой, а `ScenePicker` получает возможность работать вне потока рендера.

`CameraProjection` заменяет `BaseForm.UpdateProjection()` и P/Invoke `glu32.gluPerspective`:
ортогональная и перспективная матрицы считаются в коде.

`SceneRenderSettings` выделяется из `SettingsConfig`, где сейчас в одном классе смешаны
настройки приложения (язык, путь к солверу, диапазоны шкалы) и настройки рендера
(цвет фона, освещение, прозрачность, подрезка).

`SceneRenderer` — замена `BaseForm.DisplayObjects()`. Важное отличие: `SwapBuffers()`
он не вызывает, буферами владеет композитор Avalonia.

`ISceneLayer` и `SceneLayerCollection` заменяют поля-события `DisplayBasisEvent`,
`DisplayCompassEvent`, `DisplayText3DEvent`, `DisplayText2DEvent`, `DisplayGeometryObjectEvent`.
У слоя есть имя и порядок, поэтому скрытие перестаёт быть поиском делегата по имени метода.

`ISceneTextRenderer` имеет две реализации. `WglBitmapFontTextRenderer` — перенос текущего
кода (`wglUseFontBitmapsW`, `GL.GenLists`, `GL.ListBase`, `GL.CallLists`); он Windows-only
и нужен только чтобы не ломать отображение на шагах 2–6.
`SkiaGlyphAtlasTextRenderer` вместе с `GlyphAtlas` — целевая реализация: глифы
растеризуются SkiaSharp в текстурный атлас, текст рисуется квадами.
SkiaSharp уже присутствует в дереве зависимостей Avalonia, отдельного пакета не потребуется.
Заодно снимается ограничение в 1150 глифов из `GL.GenLists(1150)`.

`IQuadricMeshFactory` / `QuadricMeshFactory` заменяют `gluNewQuadric`, `gluCylinder`
и `gluSphere` генерацией меша в `VBObject`. Попутно снимается описанная выше утечка
в `DisplayBasis`.

`SceneMouseEventArgs`, `SceneKeyEventArgs`, `SceneMouseButton`, `SceneModifierKeys` —
собственные типы вместо `System.Windows.Forms.MouseEventArgs`, `MouseButtons`, `Keys`
и `Control.ModifierKeys`. Без них ввод нельзя оторвать от WinForms.

`ScenePicker` переносится почти без правок. Из него убираются побочные эффекты —
`console.PrintInfo`, `propertiesPanel.DrawTable`, `DispatchSelection`; вместо них
`SceneController` поднимает события `SelectionChanged` и `InfoRequested`.

`GlFrameGrabber` читает кадр через `glReadPixels` и кодирует его SkiaSharp — вместо
`System.Drawing.Image.Save` в `btnMakeScreenShot_Click`. Формат стоит заодно сменить
с BMP на PNG.

### Существующий GL-слой — `BazisGUI/Scene`

На диаграмме показан свёрнуто. Иерархия `VBObject` / `SurfaceObjects` / `LineObjects` /
`PointObjects` / `ClipPlane` / `BoundingBox` / `ShaderProgramCreator` не дублируется —
см. `ДК. Scene.VBOController.mdpuml`.

Слой переиспользуется как есть, на Linux работает без правок. Единственное изменение:
`VBObject.Load()` и `VBO.Draw()` больше не опираются на текущие матрицы GL,
а получают их из `IRenderContext`.

### Хост — `BazisGUI/AvaloniaUI/Scene`

`SceneView : OpenGlControlBase` заменяет `OpenTK.GLControl.GLControl`. Четыре отличия,
которые надо учесть при переносе кода:

| В `GLControl` сейчас | В `SceneView` |
| --- | --- |
| рисуем в буфер по умолчанию (`0`) | рисуем в `fb`, переданный в `OnOpenGlRender` |
| `scene.SwapBuffers()` в конце кадра | вызывать нельзя, буферами владеет композитор |
| `Invalidate()` | `RequestNextFrameRendering()` |
| `scene.Width` / `scene.Height` | `Bounds` × `RenderScaling` |

`AvaloniaGlBindingsContext : OpenTK.IBindingsContext` — мост от
`GlInterface.GetProcAddress` к `GL.LoadBindings`. Именно он позволяет сохранить
все существующие вызовы OpenTK `GL.*` без переписывания, и он же одинаково работает
на WGL и на GLX.

`GlPlatformConfigurator` — настройка контекста из раздела 4, с разветвлением по ОС.

## 7. Что удаляется

| Класс / файл | Шаг |
| --- | --- |
| `Gle` (`Scene/TaoExtension.cs`), `Tao.OpenGl.dll`, `Tao.Platform.Windows.dll` | уже `[Obsolete]`, можно убирать сразу |
| `OpenTK.GLControl.GLControl` | 6 |
| `Methods/PlatformSpecific/PlatformSpecific.cs` | 7 |
| `WglBitmapFontTextRenderer` | 7 |
| `WinFormsSceneHost`, `AvaloniaMasterHost`, `TabButtonControlService`, `Avalonia.Win32.Interoperability` | 8 |
| `BaseForm` | 8 |

## 8. Ограничения и риски

Compatibility-профиль на Linux — подпорка с оговорками, описанными в разделе 4.
Пока шаг 9 не сделан, набор поддерживаемых драйверов ограничен, и на llvmpipe
производительность будет заметно хуже.

Прозрачность и подрезка (`AverageColorRenderer`, `Advanced3DClipper`) работают
со своими FBO. Нужно проверить, что переключение на `fb` Avalonia и обратно
восстанавливает состояние корректно — сейчас код рассчитан на то, что целевой
буфер всегда нулевой. На Linux это стоит перепроверить отдельно: поведение
привязки FBO у Mesa и у драйвера NVIDIA различается в мелочах.

Параметры платформы действуют на всё приложение: в том же контексте Avalonia
рисует и собственный интерфейс через Skia. Нагрузочно проверить стоит до того,
как на Avalonia переедет оболочка.

За пределами сцены кроссплатформенность упирается в остальные зависимости решения,
и это отдельная задача: нативные библиотеки (`gmsh`, `SQLite_x64`/`SQLite_x86`),
`Libs/ClientGUI.dll` и `Libs/LicenseInfo.dll` без исходников, IronPython в сценариях,
установщик на WiX (`Setup`, `InstallerAction`), `App.config` и пути с разделителем `\`.
Сцена может быть готова к Linux раньше, чем приложение целиком.

## Источники

- Диаграмма: `ДК. Scene.Avalonia.mdpuml`, `ДК. Scene.Avalonia.png`
- Подробная диаграмма с сеансом проекта: [ДК. Scene.Avalonia.Detail.mdpuml](<../../BazisAvaloniaGUI/Documents/ДК. Scene.Avalonia.Detail.mdpuml>)
- Смежные документы: `ДК. BaseForm.mdpuml`, `ДК. Scene.VBOController.mdpuml`,
  `AvaloniaUI/Documents/AvaloniaMasterHosting.md`
- Avalonia: [`OpenGlControlBase`](https://github.com/AvaloniaUI/Avalonia/blob/master/src/Avalonia.OpenGL/Controls/OpenGlControlBase.cs),
  [`Win32PlatformOptions`](https://github.com/AvaloniaUI/Avalonia/blob/master/src/Windows/Avalonia.Win32/Win32PlatformOptions.cs),
  [`X11PlatformOptions`](https://github.com/AvaloniaUI/Avalonia/blob/master/src/Avalonia.X11/X11Platform.cs),
  [`GlVersion`](https://github.com/AvaloniaUI/Avalonia/blob/master/src/Avalonia.OpenGL/GlVersion.cs)
