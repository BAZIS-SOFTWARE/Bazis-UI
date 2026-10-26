# BazisGUI

BazisGUI — графическое приложение для подготовки и анализа инженерных расчётов на базе библиотек BazisCORE. В нём можно открыть или создать проект, работать с геометрией и сеткой, задать условия задачи, подготовить инструкции для внешнего решателя и исследовать результаты. Основной клиент решения — `AvaloniaGUI`; `WinGUI` сохраняется как прежняя оболочка.

Подробнее: [бизнес-задачи](Documents/BusinessTasks.md), [функции и ограничения](Documents/Features.md), [архитектура](Documents/Architecture.md), [реализация](Documents/Implementation.md).

## Основные компоненты

| Компонент | Назначение |
| --- | --- |
| [AvaloniaGUI](AvaloniaGUI/AvaloniaGUI.csproj) | Окно приложения, навигатор, свойства, команды проекта и отображение сцены |
| [SceneCore](SceneCore/SceneCore.csproj) | Камера, OpenGL-рендеринг, ввод, слои и геометрический выбор объектов |
| [WinGUI](WinGUI/WinGUI.csproj) | Прежний клиент Windows, использующий общий `SceneCore` |
| BazisCORE | Внешние библиотеки модели, проекта, CAD, препроцессора и результатов |
| [Тесты](SceneCore.Tests/SceneCore.Tests.csproj) | Отдельные проекты `SceneCore.Tests`, `AvaloniaGUI.Tests` и `WinGUI.Tests` |

`SceneCore` не зависит от пакета `Model`: GUI передаёт ему координаты видимых объектов через `Set`. Вычисление попадания и глубины остаётся в сцене ([ScenePicker.cs](SceneCore/Core/Picking/ScenePicker.cs)).

## Сборка и запуск

Нужен .NET SDK 8 и доступ к пакетам, указанным в файлах проектов. Для CAD-операций нужна совместимая нативная библиотека Gmsh, а для расчёта — отдельно настроенный исполняемый решатель. Графическая сцена требует совместимого контекста OpenGL. Точка входа `AvaloniaGUI` настраивает WGL на Windows и GLX/EGL на Linux ([Program.cs](AvaloniaGUI/Program.cs)).

Из корня репозитория:

```powershell
dotnet build AvaloniaGUI/AvaloniaGUI.csproj
dotnet run --project AvaloniaGUI/AvaloniaGUI.csproj
dotnet test SceneCore.Tests/SceneCore.Tests.csproj
```

`WinGUI` ориентирован на `net8.0-windows`; его можно собрать отдельно командой `dotnet build WinGUI/WinGUI.csproj` на Windows. Состав всего решения приведён в [BazisGUI.sln](BazisGUI.sln). Проверка полной сборки решения включает также вспомогательные проекты и установщик.

## Работа приложения

Проект можно создать, открыть или сохранить в `.bpf`/`.bpf2`; импорт CAD и сетки обрабатывается отдельно ([MainWindow.cs](AvaloniaGUI/Shell/MainWindow.cs)). Команды геометрии и генерации сетки обращаются к контроллеру BazisCORE. Перед запуском расчёта приложение сохраняет проект, проверяет связанные файлы, формирует инструкции и запускает внешний процесс по пути из настроек ([StartStopConp.cs](AvaloniaGUI/Shell/Methods/Comp/StartStopConp.cs)). Результаты читаются из базы и могут быть показаны на сцене, в графике или анимации.

Наличие команды в интерфейсе не означает поддержки всех видов физических задач и входных форматов. Подтверждённые пути и ограничения перечислены в [Features.md](Documents/Features.md).

## Лицензия

Условия распространения находятся в [LICENSE.txt](LICENSE.txt) — GNU Affero General Public License v3.
