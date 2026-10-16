using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using Project.TaskParameters;
using System.Collections.Generic;

namespace BazisAvaloniaGUI.Shell
{

    internal partial class MainWindow
    {
        enum MechanicalPropertyKeys { MaxDiference, MaxMove, MaxMoveValue, PlasticDeformation, PlasticDeformationValue }
        private List<RowProperty> GetPropertyMechanicalTask(MechanicalParameters mechanical) 
        {
            var rows = new List<RowProperty>
            {
                new RowProperty(MechanicalPropertyKeys.MaxDiference.ToString(),
                Resources.Header_mechanicalTask_maxDiference,
                mechanical.MechanicalConvergence.DUm.ToString()),

                new RowProperty(MechanicalPropertyKeys.MaxMove.ToString(),
                Resources.Header_mechanicalTask_maxMove,
                mechanical.MechanicalConvergence.Is_Switched_Um)
            };

            if (mechanical.MechanicalConvergence.Is_Switched_Um)
                rows.Add(new RowProperty(MechanicalPropertyKeys.MaxMoveValue.ToString(),
                    Resources.Header_mechanicalTask_maxMoveValue,
                    mechanical.MechanicalConvergence.Um.ToString()));

            rows.Add(new RowProperty(MechanicalPropertyKeys.PlasticDeformation.ToString(), 
                Resources.Header_mechanicalTask_maxPlasticDeformation,
                mechanical.MechanicalConvergence.Is_Physically_NonLinear));

            if(mechanical.MechanicalConvergence.Is_Physically_NonLinear)
                rows.Add(new RowProperty(MechanicalPropertyKeys.PlasticDeformationValue.ToString(),
                    Resources.Header_mechanicalTask_plasticDeformationValue,
                    mechanical.MechanicalConvergence.PlasticityCriterion.ToString()));

            return rows;
        }
    }
}
