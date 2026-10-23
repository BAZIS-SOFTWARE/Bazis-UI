using AvaloniaGUI.Chamfer.ViewModels;
using AvaloniaGUI.Chamfer.Views;
using Avalonia.Threading;
using Avalonia.Controls;
using System;

namespace AvaloniaGUI.Chamfer.Services
{
    /// <summary>
    /// Вспомогательный сервис для управления окном построения фасок.
    /// </summary>
    /// <remarks>
    /// Сервис отвечает за создание и отображение <see cref="ChamferWindow"/> с соответствующим
    /// <see cref="ChamferViewModel"/>. Все обращения выполняются в UI-потоке Avalonia.
    /// Экземпляр сервиса принадлежит главному окну.
    /// При закрытии окна сервис очищает превью операции через переданный <see cref="IChamferOperationService"/>
    /// и уведомляет вызывающую сторону через переданное действие, что позволяет ей синхронизировать
    /// собственное состояние с фактическим состоянием окна.
    /// </remarks>
    internal sealed class ChamferWindowService
    {
        /// <summary>
        /// Текущий открытый экземпляр окна фаски или <c>null</c>, если окно не отображается.
        /// </summary>
        private ChamferWindow currentWindow;

        /// <summary>
        /// Отобразить окно построения фаски.
        /// </summary>
        /// <param name="owner">Главное окно, вместе с которым закрывается редактор фаски.</param>
        /// <param name="operationService">
        /// Сервис операции фаски, предоставляющий логику построения и методы управления превью.
        /// Не может быть <c>null</c>.
        /// </param>
        /// <param name="closed">
        /// Необязательное действие, вызываемое после закрытия окна любым способом,
        /// в том числе системной кнопкой закрытия. Вызывается в UI-потоке Avalonia,
        /// поэтому вызывающая сторона отвечает за переход в собственный UI-поток.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Бросается, если параметр <paramref name="operationService"/> равен <c>null</c>.
        /// </exception>
        public void Show(Window owner, IChamferOperationService operationService, Action closed = null)
        {
            ArgumentNullException.ThrowIfNull(owner);
            if (operationService == null)
                throw new System.ArgumentNullException(nameof(operationService));

            Dispatcher.UIThread.VerifyAccess();
            // Одновременно допускается только одно окно построения фаски.
            if (currentWindow != null)
            {
                currentWindow.Activate();
                return;
            }

            var viewModel = new ChamferViewModel(operationService);
            var window = new ChamferWindow
            {
                DataContext = viewModel
            };
            currentWindow = window;
            // Окно закрывается не по нажатию «Добавить», а после успешного построения фаски
            // (вызывающая сторона вызывает Close); при ошибке оно остаётся открытым для правки параметров.

            window.Closed += (_, _) =>
            {
                // Ссылка очищается до уведомления, чтобы окно можно было открыть повторно.
                if (ReferenceEquals(currentWindow, window))
                    currentWindow = null;

                operationService.ClearPreview();
                closed?.Invoke();
            };

            window.Show(owner);
        }

        /// <summary>
        /// Закрыть текущее окно фаски, если оно открыто.
        /// </summary>
        public void Close()
        {
            Dispatcher.UIThread.VerifyAccess();
            currentWindow?.Close();
        }
    }
}
