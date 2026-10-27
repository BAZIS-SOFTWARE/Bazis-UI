using BazisGUI.MasterInterface.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        public void HandlePreparedDataMaster(IPreparedDataLoader pdlh)
        {
            pdlh.SetDataFromConditionsStrings(project.GetAllCondData()?.Select(x => x?.ToString()) ?? Array.Empty<string>());
            OnProjectLoaded += () => pdlh.SetDataFromConditionsStrings(project.GetAllCondData()?.Select(x => x?.ToString()) ?? Array.Empty<string>());
        }
    }
}
