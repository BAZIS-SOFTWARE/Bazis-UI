using WinGUI.PropertiesPanel;
using Project.TaskParameters;
using System;
using System.Collections.Generic;

namespace WinGUI
{
    public partial class BaseForm
    {
        enum ChemicalTaskPropertyKeys { MaxConсentration, MaxConсentrationValue }
        [Obsolete ("Не протестированно, так как химическая задача не реализована")]
        private List<RowProperty> GetPropertyChemicalTask(ChemicalParameters chemical)
        {
            var rows = new List<RowProperty>();
            rows.Add(new RowProperty(ChemicalTaskPropertyKeys.MaxConсentration.ToString(), Properties.Resources.Header_chemical_MaxConcentration, chemical.ChemicalConvergence.Is_Switched_Cm));
            if (chemical.ChemicalConvergence.Is_Switched_Cm)
                rows.Add(new RowProperty(ChemicalTaskPropertyKeys.MaxConсentrationValue.ToString(), Properties.Resources.Header_chemical_MaxConcentrationValue, chemical.ChemicalConvergence.Cm.ToString()));

            // Начальная концентрация задаётся через блок начального состояния (InitialState).
            return rows;
        }
    }
}
