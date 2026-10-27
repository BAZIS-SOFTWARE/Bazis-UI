using BazisGUI.MasterInterface.Interfaces;
using System;
using System.Linq;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void HandleFunctionsMaster(IFunctionsHandling fh)
        {
            if (project?.FunctionsDB == null) throw new Exception("Не определена база функций");

            fh.SetFunctions(project?.FunctionsDB?.Keys?.ToArray() ?? Array.Empty<string>());

            OnChangeFunctions += (s, e) => fh.SetFunctions(e.Functions ?? Array.Empty<string>());
            OnProjectLoaded += () => fh.SetFunctions(project?.FunctionsDB?.Keys?.ToArray() ?? Array.Empty<string>());
        }
    }
}
