using Avalonia;
using Avalonia.Themes.Fluent;
using Avalonia.Threading;
using Avalonia.Win32.Interoperability;
using System;
using System.Globalization;
using System.Threading;

namespace WinGUI.AvaloniaUI.Hosting
{
    /// <summary>
    /// Жизненный цикл Avalonia внутри WinForms-приложения.
    /// </summary>
    /// <remarks>
    /// Avalonia инициализируется в том же потоке, из которого вызван <see cref="Initialize"/>,
    /// то есть в UI-потоке WinForms. Общий UI-поток обязателен для встраиваемого содержимого:
    /// <c>WinFormsAvaloniaControlHost</c> создаёт Avalonia-представление в потоке WinForms-контрола,
    /// и если диспетчер Avalonia живёт в другом потоке, встроенное дерево никогда не проходит
    /// компоновку и отрисовку (проверено: размеры содержимого остаются нулевыми).
    /// Собственный цикл сообщений Avalonia при этом не запускается — очередь диспетчера
    /// прокачивается циклом сообщений WinForms (<c>Application.Run</c>).
    /// </remarks>
    internal static class AvaloniaHost
    {
        private static bool isInitialized;

        /// <summary>
        /// Настраивает Avalonia в текущем потоке. Повторные вызовы игнорируются.
        /// </summary>
        /// <remarks>
        /// Вызывать до запуска цикла сообщений WinForms и только из UI-потока приложения.
        /// </remarks>
        public static void Initialize()
        {
            if (isInitialized)
                return;

            // Владельцем SynchronizationContext остаётся WinForms: приложение целиком построено
            // на его семантике маршалинга, а Avalonia использует Dispatcher.UIThread напрямую.
            AvaloniaSynchronizationContext.AutoInstall = false;

            AppBuilder.Configure<BazisAvaloniaApplication>()
                .UsePlatformDetect()
                .SetupWithoutStarting();

            // WinForms перехватывает клавиатурные сообщения раньше Avalonia,
            // поэтому окна и встроенные представления Avalonia без этого фильтра не получают ввод с клавиатуры.
            System.Windows.Forms.Application.AddMessageFilter(new WinFormsAvaloniaMessageFilter());

            isInitialized = true;
        }

        /// <summary>
        /// Выполнить действие в UI-потоке Avalonia.
        /// </summary>
        /// <remarks>
        /// Языковая культура задаётся для потока, поэтому текущая языковая культура вызывающего
        /// потока переносится в UI-поток Avalonia. Благодаря этому окна Avalonia отображаются
        /// на языке, выбранном в настройках приложения.
        /// </remarks>
        /// <param name="action">Действие, выполняемое в UI-потоке Avalonia.</param>
        public static void Post(Action action)
        {
            if (!isInitialized)
                throw new InvalidOperationException("Avalonia is not initialized.");

            var uiCulture = CultureInfo.CurrentUICulture;

            Dispatcher.UIThread.Post(() =>
            {
                Thread.CurrentThread.CurrentUICulture = uiCulture;
                action();
            });
        }

        private sealed class BazisAvaloniaApplication : Application
        {
            public override void Initialize()
            {
                Styles.Add(new FluentTheme());
            }
        }
    }
}
