using System.Linq;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        /// <summary>
        /// Убирает мастер из области мастеров и освобождает его представление.
        /// </summary>
        /// <remarks>
        /// <see cref="TabButtonControlService"/> только удаляет контрол из контейнера,
        /// поэтому освобождение выполняется здесь: для Avalonia-мастера это закрывает
        /// встроенное представление и связанное с ним окно платформы.
        /// </remarks>
        /// <param name="masterName">Имя мастера, переданное при открытии.</param>
        public void CloseMaster(string masterName)
        {
            if (!TabButtonsService.GetNames().Contains(masterName))
                return;

            var presentation = TabButtonsService.GetControl(masterName);
            TabButtonsService.RemoveControl(masterName);
            presentation.Dispose();
        }
    }
}
