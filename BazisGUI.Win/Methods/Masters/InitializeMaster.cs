using BazisGUI.MasterInterface.Interfaces;
using System;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        /// <summary>
        /// Подключает мастер к приложению: исполнитель команд, базовые события и capability-интерфейсы.
        /// </summary>
        /// <remarks>
        /// Метод не зависит от UI-фреймворка мастера и работает одинаково для WinForms- и Avalonia-мастеров.
        /// За размещение мастера в интерфейсе отвечает вызывающий код (см. перегрузки <c>OpenMaster</c>).
        /// </remarks>
        /// <param name="master">Подключаемый мастер.</param>
        /// <exception cref="ArgumentNullException">Возникает, если <paramref name="master"/> равен <c>null</c>.</exception>
        /// <exception cref="Exception">Возникает, если проект не определён.</exception>
        public void InitializeMaster(IBaseMaster master)
        {
            if (master == null) throw new ArgumentNullException(nameof(master));
            if (!project.HasProject) throw new Exception("Project is not open.");

            master.SetCommandExecutor(ExecuteCommand);

            if (master is IFunctionsHandling fh) HandleFunctionsMaster(fh);

            if (master is IMaterialsHandling mh) HandleMaterialsMaster(mh);

            if (master is IGroupHandling gh) HandleGroupsMaster(gh);

            if (master is IPreparedDataLoader pdlh) HandlePreparedDataMaster(pdlh);

            HandleBaseMaster(master);
        }
    }
}
