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
        private void HandleMaterialsMaster(IMaterialsHandling mh)
        {
            if (project?.MaterialsDB == null) throw new Exception("Не определена база материалов");

            mh.SetMaterials(project?.MaterialsDB?.Keys?.ToArray() ?? Array.Empty<string>());

            OnChangeMaterials += (s, e) => mh.SetMaterials(e.Materials ?? Array.Empty<string>());
            OnProjectLoaded += () => mh.SetMaterials(project?.MaterialsDB?.Keys?.ToArray() ?? Array.Empty<string>());
        }
    }
}
