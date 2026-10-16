using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using Model.Interfaces;
using Project.Tasks;
using System.Collections.Generic;
using System.Drawing;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        enum MediaPropertyKeys { CondType }
        public List<RowProperty> GetMediaProperty(MediaData obj, IEnumerable<IGroup> groups, List<string> funcTables)
        {
            var rows = GetCondProperty(obj, groups, funcTables);

            rows.Add(new RowProperty(MediaPropertyKeys.CondType.ToString(), Resources.Header_cond_media_condType,
                new DropDownPropertyValue(obj.MediaType,
                new List<string>()
                {
                    MediaType.ConstantTemp.ToString(),
                    MediaType.HeatFlux.ToString()
                }))
            { Color = Color.Gainsboro });

            return rows;

        }
    }
}
