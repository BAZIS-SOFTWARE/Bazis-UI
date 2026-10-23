using WinGUI.Properties;
using WinGUI.PropertiesPanel;
using Project.Interfaces.Tasks;
using Project.Tasks.Functions.FrameFunctions;
using System.Collections.Generic;

namespace WinGUI
{
    public partial class BaseForm
    {
        enum FrameFunctionPropertyKeys { SPHWidth, CILLength, CILUpperDiam, CILBottomDiam, CFFFile }
        private List<RowProperty> GetFrameFunctionProperties(IFrameFunction function)
        {
            var rows = new List<RowProperty>();

            if (function is SPH sph)
            {
                rows.Add(new RowProperty(FrameFunctionPropertyKeys.SPHWidth.ToString(), 
                    Resources.Header_frameFunction_SPH_width,
                    sph.Width));
            }

            else if (function is CIL cil)
            {
                rows.Add(new RowProperty(FrameFunctionPropertyKeys.CILLength.ToString(),
                    Resources.Header_frameFunction_CIL_length,
                    cil.Length));

                rows.Add(new RowProperty(FrameFunctionPropertyKeys.CILUpperDiam.ToString(),
                    Resources.Header_frameFunction_CIL_upperDiam,
                    cil.UpperDiam));

                rows.Add(new RowProperty(FrameFunctionPropertyKeys.CILBottomDiam.ToString(),
                    Resources.Header_frameFunction_CIL_bottomDiam,
                    cil.BottomDiam));
            }

            else
            {
                var cf = function as CustomFrameFunction;
                rows.Add(new RowProperty(FrameFunctionPropertyKeys.CFFFile.ToString(), Resources.Header_frameFunction_CFF_file, cf?.ExecutedFile ?? "*"));
            }

            return rows;
        }
    }
}
