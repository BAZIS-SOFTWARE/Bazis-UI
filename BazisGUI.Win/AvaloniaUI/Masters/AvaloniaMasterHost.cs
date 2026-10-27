using Avalonia.Win32.Interoperability;
using BazisGUI.Win.AvaloniaUI.Hosting;
using BazisGUI.MasterInterface.Interfaces;
using System;

namespace BazisGUI.Win.AvaloniaUI.Masters
{
    /// <summary>
    /// Временный WinForms-хост представления Avalonia-мастера.
    /// </summary>
    /// <remarks>
    /// Существует только потому, что оболочка приложения пока построена на WinForms:
    /// область мастеров (<see cref="TabButtonControlService"/>) умеет работать лишь с
    /// <see cref="System.Windows.Forms.Control"/>. Класс подлежит удалению вместе с
    /// переводом оболочки на Avalonia, при этом сами мастера переписывать не потребуется.
    /// <para>
    /// Базовый <see cref="WinFormsAvaloniaControlHost"/> берёт на себя изменение размеров,
    /// передачу фокуса, ввод и освобождение Avalonia-представления при <c>Dispose</c>,
    /// а работает он только при общем UI-потоке WinForms и Avalonia (см. <see cref="AvaloniaHost"/>).
    /// </para>
    /// </remarks>
    internal sealed class AvaloniaMasterHost : WinFormsAvaloniaControlHost
    {
        /// <summary>
        /// Создаёт хост с представлением указанного мастера.
        /// </summary>
        /// <param name="master">Мастер, представление которого нужно разместить.</param>
        /// <exception cref="ArgumentNullException">Возникает, если <paramref name="master"/> равен <c>null</c>.</exception>
        public AvaloniaMasterHost(IAvaloniaMaster master)
        {
            if (master == null) throw new ArgumentNullException(nameof(master));

            Content = master.CreateView();
        }
    }
}
