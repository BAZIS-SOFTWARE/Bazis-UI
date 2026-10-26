# BazisGUI

BazisGUI — графический клиент для подготовки расчётной модели, запуска CAE-расчёта и просмотра результатов на базе BazisCORE. Основное приложение — `AvaloniaGUI`. Прежний клиент `WinGUI` остаётся в решении и поддерживается. Численный расчёт выполняет отдельный решатель, путь к которому задаётся в настройках приложения.

## Возможности

- Создание, открытие и сохранение проектов `.bpf`/`.bpf2`, импорт CAD и сетки.
- Работа с геометрией, сеткой, физическими задачами, свойствами и граничными условиями.
- Отображение модели и выбор видимых объектов в трёхмерной сцене.
- Формирование инструкций расчёта, запуск и остановка внешнего решателя.
- Просмотр полей результатов, графиков и анимации.

Наличие команды в интерфейсе не означает поддержки любого формата или физической задачи. Реализованные пути и их ограничения перечислены в [каталоге функций](Documents/Features.md).

## Состав решения

| Проект | Назначение |
| --- | --- |
| [AvaloniaGUI](AvaloniaGUI/AvaloniaGUI.csproj) | Основной клиент: окно, навигатор, свойства, команды проекта и отображение сцены |
| [SceneCore](SceneCore/SceneCore.csproj) | Общие для двух клиентов камера, ввод, OpenGL-отрисовка, слои и геометрический выбор |
| [WinGUI](WinGUI/WinGUI.csproj) | Поддерживаемый прежний клиент на Windows Forms |
| [MasterInterface](MasterInterface/MasterInterface.csproj), [UserControlsEx](UserControlEx/UserControlsEx.csproj) | Компоненты прежнего клиента |
| [ProjectConverter](ProjectConverter/ProjectConverter.csproj) | Вспомогательное приложение для Windows |
| [InstallerAction](InstallerAction/InstallerAction.csproj), [Setup](Setup/Setup.vdproj) | Компоненты установщика Windows |
| [SceneCore.Tests](SceneCore.Tests/SceneCore.Tests.csproj), [AvaloniaGUI.Tests](AvaloniaGUI.Tests/AvaloniaGUI.Tests.csproj), [WinGUI.Tests](WinGUI.Tests/WinGUI.Tests.csproj) | Тестовые проекты |

Инженерные данные и операции над ними предоставляет BazisCORE: геометрия, модель и сетка, проектные свойства и условия, препроцессор и постпроцессор. `SceneCore` получает координаты отображаемых объектов через собственные структуры и не зависит от пакета `Model`; применение выбора к проекту остаётся в GUI. Связи компонентов подробнее показаны в [архитектуре решения](Documents/Architecture.md).

## Требования и запуск

Для сборки основного клиента нужен .NET SDK 8 и доступ к NuGet-пакетам из файлов проектов. В решении также используются библиотеки из каталога `Libs`. Для отображения сцены требуется графическое окружение с совместимым OpenGL-контекстом: [точка входа AvaloniaGUI](AvaloniaGUI/Program.cs) настраивает WGL на Windows и GLX/EGL на Linux. CAD-операциям нужна совместимая нативная библиотека Gmsh (`.dll` на Windows или `.so` на Linux). Для запуска расчёта нужно указать путь к исполняемому решателю в настройках приложения.

Из корня репозитория соберите и запустите основной клиент:

```powershell
dotnet build AvaloniaGUI/AvaloniaGUI.csproj
dotnet run --project AvaloniaGUI/AvaloniaGUI.csproj
```

На Linux в [проекте AvaloniaGUI](AvaloniaGUI/AvaloniaGUI.csproj) предусмотрены конфигурации `Debug-Linux` и `Release-Linux` с идентификатором среды `linux-x64`:

```bash
dotnet build AvaloniaGUI/AvaloniaGUI.csproj -c Debug-Linux
dotnet run --project AvaloniaGUI/AvaloniaGUI.csproj -c Debug-Linux
```

Проверка отдельных частей решения:

```powershell
dotnet test SceneCore.Tests/SceneCore.Tests.csproj
dotnet test AvaloniaGUI.Tests/AvaloniaGUI.Tests.csproj
```

`WinGUI`, `WinGUI.Tests` и `ProjectConverter` используют `net8.0-windows`. Проекты `MasterInterface` и `UserControlsEx` используют .NET Framework 4.8, а `Setup` — проект установщика Visual Studio. Поэтому сборку всего [BazisGUI.sln](BazisGUI.sln) следует рассматривать отдельно от сборки основного клиента.

## Документация

- [Бизнес-задачи](Documents/BusinessTasks.md), [функции и ограничения](Documents/Features.md), [архитектура](Documents/Architecture.md), [пути реализации](Documents/Implementation.md).
- [Основные потоки AvaloniaGUI](AvaloniaGUI/Documents/Overview.md) и [диаграммы классов AvaloniaGUI](AvaloniaGUI/Documents/AvaloniaGUI.mdpuml).
- [Основные потоки WinGUI](WinGUI/Documents/Overview.md) и [диаграммы классов WinGUI](WinGUI/Documents/WinGUI.mdpuml).
- [Диаграммы SceneCore](SceneCore/Documents/SceneCore.mdpuml).

## Лицензия

Условия распространения приведены в [LICENSE.txt](LICENSE.txt): GNU Affero General Public License v3.
