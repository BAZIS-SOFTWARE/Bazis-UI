using WinGUI.Properties;
using WinGUI.PropertiesPanel;
using Project.TaskParameters;
using System.Collections.Generic;

namespace WinGUI
{
    public partial class BaseForm
    {
        enum TermalTaskPropertyKeys { MaxTemperture, MaxTempertureValue }
        private List<RowProperty> GetPropertyTermalTask(TermalParameters thermal)
        {
            var rows = new List<RowProperty>();
            rows.Add(new RowProperty(TermalTaskPropertyKeys.MaxTemperture.ToString(),
                Resources.Header_termalTask_maxTemperture,
                thermal.TermalConvergence.Is_Switched_Tm));
            if (thermal.TermalConvergence.Is_Switched_Tm)
                rows.Add(new RowProperty(TermalTaskPropertyKeys.MaxTempertureValue.ToString(),
                    WinGUI.Properties.Resources.Header_termalTask_maxTempertureValue,
                    thermal.TermalConvergence.Tm.ToString()));
            return rows;
        }
    }
}
