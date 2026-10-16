using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using Model.Interfaces;
using Project.Interfaces.Tasks;
using Project.Tasks;
using Project.Tasks.Materials;
using System.Collections.Generic;
using System.Drawing;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        enum MaterialPropertyKeys { Material, Diametr, Thickness }
        public List<RowProperty> GetMatProperty(MatData obj, List<string> mat, IEnumerable<IGroup> groups, List<string> funcTables)
        {
            var rows = GetCondProperty(obj, groups, funcTables);

            rows.Add(new RowProperty(MaterialPropertyKeys.Material.ToString(), Resources.Header_cond_material_material, new DropDownPropertyValue(obj.Material.Name, mat))
            { Color = Color.Gainsboro });

            // подумать над этим....
            if (obj is BeamMatData bmat)
                rows.Add(new RowProperty(MaterialPropertyKeys.Diametr.ToString(), Resources.Header_cond_material_diametr, bmat.Diameter));
            else if (obj is PlateMatData pmat)
                rows.Add(new RowProperty(MaterialPropertyKeys.Thickness.ToString(), Resources.Header_cond_material_thickness, pmat.Thickness));

            return rows;
        }
    }
}
