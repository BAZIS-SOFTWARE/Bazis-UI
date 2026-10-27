using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.MasterInterface.Interfaces
{
    public interface ILocalazableMaster : IMasterInterface
    {
        string GetCurrentUICultureMasterName();
    }
}
