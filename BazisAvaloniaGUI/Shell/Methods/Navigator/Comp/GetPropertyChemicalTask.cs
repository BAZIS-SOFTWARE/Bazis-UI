using BazisAvaloniaGUI.Properties;
using Project.TaskParameters;
using System;
using System.Collections.Generic;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        enum ChemicalTaskPropertyKeys { MaxConсentration, MaxConсentrationValue }
        [Obsolete ("Не протестированно, так как химическая задача не реализована")]
        private List<RowProperty> GetPropertyChemicalTask(ChemicalParameters chemical)
        {
            var rows = new List<RowProperty>();
            rows.Add(new RowProperty(ChemicalTaskPropertyKeys.MaxConсentration.ToString(), Resources.Header_chemical_MaxConcentration, chemical.ChemicalConvergence.Is_Switched_Cm));
            if (chemical.ChemicalConvergence.Is_Switched_Cm)
                rows.Add(new RowProperty(ChemicalTaskPropertyKeys.MaxConсentrationValue.ToString(), Resources.Header_chemical_MaxConcentrationValue, chemical.ChemicalConvergence.Cm.ToString()));

            // Начальная концентрация задаётся через блок начального состояния (InitialState).
            return rows;
        }
    }
}
