using WinGUI.AvaloniaUI.Masters;
using MasterInterface;
using MasterInterface.Interfaces;
using System.Windows.Forms;

namespace WinGUI
{
    public partial class BaseForm
    {
        /// <summary>
        /// Открывает WinForms-мастер в области мастеров.
        /// </summary>
        /// <remarks>
        /// Точка входа для существующих мастеров, унаследованных от <see cref="BaseMaster"/>,
        /// в том числе загружаемых из внешних сборок через <see cref="ImportMasterDLL"/>.
        /// </remarks>
        /// <param name="master">Открываемый мастер.</param>
        public void OpenMaster(BaseMaster master)
        {
            InitializeMaster(master);
            ShowMaster(master.MasterName, master);
        }

        /// <summary>
        /// Открывает Avalonia-мастер в области мастеров.
        /// </summary>
        /// <remarks>
        /// От WinForms-варианта отличается только способом размещения: представление мастера
        /// оборачивается во временный <see cref="AvaloniaMasterHost"/>.
        /// </remarks>
        /// <param name="master">Открываемый мастер.</param>
        public void OpenMaster(IAvaloniaMaster master)
        {
            InitializeMaster(master);
            ShowMaster(master.MasterName, new AvaloniaMasterHost(master));
        }

        /// <summary>
        /// Размещает представление мастера в области мастеров.
        /// </summary>
        /// <param name="masterName">Имя мастера, используемое как заголовок вкладки.</param>
        /// <param name="presentation">Представление мастера или его хост.</param>
        private void ShowMaster(string masterName, Control presentation)
        {
            presentation.Dock = DockStyle.Fill;
            presentation.Name = $"cntr{masterName}";
            presentation.Text = $"cntr{masterName}";
            presentation.Size = cntrНавигатор.Size;
            presentation.Location = cntrНавигатор.Location;
            presentation.Anchor = cntrНавигатор.Anchor;

            TabButtonsService.AddControl(presentation.Name, presentation);
        }
    }
}
