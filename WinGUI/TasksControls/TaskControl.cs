using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using PreProc;
using Project.TaskParameters;

namespace WinGUI.TasksControls
{
    public partial class TaskControl : UserControl, ITaskControl
    {
        private GeneralParameters parameters;

        public event Action<string> BtnSave_ClickEvent;

        public TaskControl()
        {
            InitializeComponent();
        }

        public void InputData(GeneralParameters _parameters)
        {
            parameters = _parameters;

            if(_parameters is ChemicalParameters cmp)
            {
                chemicalControl.BringToFront();
                chemicalControl.MaxConcentr = cmp.ChemicalConvergence.Cm.ToString();
                chemicalControl.IsMaxConcentrSwitch = cmp.ChemicalConvergence.Is_Switched_Cm;
            }
            else if(_parameters is TermalParameters tmp)
            {
                heatTaskControl.BringToFront();
                heatTaskControl.DTMax = tmp.TermalConvergence.Tm.ToString();
            }
            else
            {
                mechTaskControl.BringToFront();
                var mp = _parameters as MechanicalParameters;
                mechTaskControl.MaxDU = mp.MechanicalConvergence.DUm.ToString();
                mechTaskControl.MaxU = mp.MechanicalConvergence.Um.ToString();
                mechTaskControl.MaxSiSt = mp.MechanicalConvergence.PlasticityCriterion.ToString();
            }

            timeSettingsControl.SetTimeSettings(_parameters.TimeSettings);
            solverSettingsControl.SetSolverSettings(_parameters.SolverSettings);

            basicControl.Iterations = parameters.Iterations.ToString();
            basicControl.SaveRate = _parameters.SaveRate.ToString();
        }

        public bool GetValidationResult()
        {

            var checks = new List<bool>();

            checks.Add(timeSettingsControl.GetValidationResult());
            checks.Add(solverSettingsControl.GetValidationResult());

            if(parameters is ChemicalParameters cmp)
                checks.Add(chemicalControl.GetValidationResult());
            
            return checks.All(x => x);
        }      

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (GetValidationResult())
            {
                var timeSettings = timeSettingsControl.GetTimeSettings();
                var solverSettings = solverSettingsControl.GetSolverSettings();

                if (parameters is ChemicalParameters cmp)
                {
                    cmp.ChemicalConvergence.Is_Switched_Cm = chemicalControl.IsMaxConcentrSwitch;
                    cmp.ChemicalConvergence.Cm = Convert.ToSingle(chemicalControl.MaxConcentr);
                }

                else if (parameters is TermalParameters tmp)
                {
                    tmp.TermalConvergence.Tm = Convert.ToSingle(heatTaskControl.DTMax);
                }
                else
                {
                    var mp = parameters as MechanicalParameters;
                    mp.MechanicalConvergence.DUm = Convert.ToSingle(mechTaskControl.MaxDU);
                    mp.MechanicalConvergence.Um = Convert.ToSingle(mechTaskControl.MaxU);
                    mp.MechanicalConvergence.PlasticityCriterion = Convert.ToSingle(mechTaskControl.MaxSiSt);
                }

                parameters.Iterations = Convert.ToInt32(basicControl.Iterations);
                parameters.SaveRate = Convert.ToInt32(basicControl.SaveRate);

                // Начальные значения в Project 6.x задаются по группам через InitialState
                // и этим окном не редактируются — только через панель свойств.

                parameters.SolverSettings = solverSettings;
                parameters.TimeSettings = timeSettings;

                var settingsSerializer = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Newtonsoft.Json.Formatting.Indented
                };

                var parLine = JsonConvert.SerializeObject(parameters, settingsSerializer);

                BtnSave_ClickEvent?.Invoke(parLine);
            }
        }
    }
}
