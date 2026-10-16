using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using Model.Interfaces;
using Project.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        enum LoadPropertyKeys { Type }
        public enum LoadKindKeys { Force, Pressure }
        public List<RowProperty> GetLoadProperty(LoadData obj, List<string> funcTables, IEnumerable<IGroup> groups)
        {
            var rows = GetCondProperty(obj, groups, funcTables);
            rows.Add(new RowProperty(LoadPropertyKeys.Type.ToString(),
                Resources.Header_load_type,
                new DropDownPropertyValue(obj.Kind, Enum.GetValues<LoadKindKeys>().Select(x => x.ToString()).ToList()))
            { Color = Color.Gainsboro });

            return rows;
        }
    }
}
