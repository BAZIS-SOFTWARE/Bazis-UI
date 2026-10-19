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
        enum ClampPopertyKeys { Type }
        public enum ClampKindKeys { Hard, Flexable, Symmetric, Contact }
        public List<RowProperty> GetClampProperty(ClampData obj, IEnumerable<IGroup> groups, List<string> funcTables)
        {
            var rows = GetCondProperty(obj, groups, funcTables);
            rows.Add(new RowProperty(ClampPopertyKeys.Type.ToString(),
                Resources.Header_clamp_type,
                new DropDownPropertyValue(obj.Kind, Enum.GetValues<ClampKindKeys>().Select(x => x.ToString()).ToList()))
            { Color = Color.Gainsboro });

            return rows;
        }
    }
}
