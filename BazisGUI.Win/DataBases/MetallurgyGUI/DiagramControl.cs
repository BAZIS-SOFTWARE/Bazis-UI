using BazisGUI.Win.Properties;
using MaterialDB;
using MaterialDB.MaterialData;
using MaterialDB.MaterialData.MetallurgicalData;
using PropertiesCalculator.PropertiesController.MetallurgicalModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UserControlsEx.Graph;

namespace BazisGUI.Win.DataBases.MetallurgyGUI
{
    public partial class DiagramControl : UserControl
    {
        CCTControl cctPanel;
        TTTControl tttPanel;
        PropertyData reactions;
        DataTable phaseData;
        string material;
        public DiagramControl(string material,PropertyData reactions, DataTable phaseData)
        {
            InitializeComponent();

            this.material = material;
            this.phaseData = phaseData;

            cctPanel = new CCTControl() { Dock = DockStyle.Fill };

            tttPanel = new TTTControl() { Dock = DockStyle.Fill };

            tableLayoutPanel1.Controls.Add(cctPanel, 1, 1);

            this.reactions = reactions;

            var coolingPhases = new List<string>();

            foreach (var reaction in reactions)
            {
                if (reaction.Key.StartsWith("Охлаждение"))
                {
                    coolingPhases.Add(reaction.Key.Split(' ', '-')[1]);
                }
            }

            var phases = phaseData.AsEnumerable().Select(r => r.Field<string>(0));
            foreach (var phase in phases)
            {
                if (coolingPhases.Contains(phase))
                {
                    cctPanel.AddPhase(phase);
                    tttPanel.AddPhase(phase);
                }                
            }
        }

        private void rbtCCT_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtCCT.Checked)
            {
                tableLayoutPanel1.Controls.Remove(tttPanel);
                tableLayoutPanel1.Controls.Add(cctPanel, 1, 1);
            }

        }

        private void rbtTTT_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtTTT.Checked)
            {
                tableLayoutPanel1.Controls.Remove(cctPanel);
                tableLayoutPanel1.Controls.Add(tttPanel, 1, 1);
            }
        }

        private void btnCalcDiag_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbtCCT.Checked)
                {
                    CalcCCTDiagram();
                }
                else
                {
                    CalcTTTDiagram();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void CalcTTTDiagram()
        {
            var tempering = reactions.Values.Where(x => x.Name.Split(' ')[1].Split('-')[0] == tttPanel.InitialPhase);

            var tempStep = 15.0f;
            var timeStep = 1.0f;
            var timeMax = tttPanel.MaxTime;
            var temp = tttPanel.IniTemp;
            var tempStop = tttPanel.FinTemp;
            var reacInitial = new Dictionary<string, List<DiagramGraphPoint>>();
            var reacFinal = new Dictionary<string, List<DiagramGraphPoint>>();

            var phases = new PhaseData(phaseData);
            foreach (var phase in phases)
            {
                if (phase.Name != tttPanel.InitialPhase)
                {
                    reacInitial.Add(phase.Name, new List<DiagramGraphPoint>());
                    reacFinal.Add(phase.Name, new List<DiagramGraphPoint>());
                }
            }

            while (temp > tempStop)
            {

                var model = new MetallurgicalModel();
                var processData = new ProcessData(tempering, new string[] { "Охлаждение" });                

                SetInitialCondition(phases, tttPanel.InitialPhase);

                var reacData = new Dictionary<string, List<DiagramGraphPoint>>();

                foreach (var phase in phases)
                    reacData.Add(phase.Name, new List<DiagramGraphPoint>());
                var time = 0.0f;
                while (time < timeMax)
                {
                    model.Calc(temp, timeStep, phases, processData);

                    time += timeStep;
                    var time_log10 = (float)Math.Log10(time);

                    foreach (var phase in phases)
                    {
                        var val = phase.Value;
                        if (reacData[phase.Name].Count == 0)
                            reacData[phase.Name].Add(new DiagramGraphPoint(time_log10, temp, val));
                        else if (reacData[phase.Name].Count > 0 && Math.Abs(reacData[phase.Name].LastOrDefault().Phase - val) > 0.01f)
                            reacData[phase.Name].Add(new DiagramGraphPoint(time_log10, temp, val));
                    }

                }
                temp -= tempStep;
                if (temp < tempStop)
                    break;

                foreach (var reac in reacData)
                {
                    if (reacInitial.ContainsKey(reac.Key) & reac.Value.Count > 1)
                    {
                        var fval = reac.Value.FirstOrDefault(x => x.Phase > tttPanel.MinPhase);
                        if (fval != null)
                            reacInitial[reac.Key].Add(fval);
                    }

                    if (reacFinal.ContainsKey(reac.Key) & reac.Value.Count > 1)
                    {
                        var lval = reac.Value.LastOrDefault(x => x.Phase < tttPanel.MaxPhase);
                        if (lval != null)
                            reacFinal[reac.Key].Add(lval);
                    }
                }
            }
            CreateTTTDiagram(reacInitial, reacFinal);
        }

        private void CalcCCTDiagram()
        {

            var coolings = reactions.Values.Where(x => x.Name.Split(' ')[1].Split('-')[0] == cctPanel.InitialPhase);

            var vel = cctPanel.MinVel;

            var velQ = (float)Math.Pow(cctPanel.MaxVel / cctPanel.MinVel, 1.0f/ (float)(cctPanel.VelNumber - 1));

            var tempVel = cctPanel.MinVel;
            var timeStep = 1.0f;
            var tempStart = cctPanel.IniTemp;
            var tempStop = cctPanel.FinTemp;
            var reacInitial = new Dictionary<string, List<DiagramGraphPoint>>();
            var reacFinal = new Dictionary<string, List<DiagramGraphPoint>>();
            var vels = new List<List<GraphPoint>>();

            var phases = new PhaseData(phaseData);
            foreach (var phase in phases)
            {
                if (phase.Name != cctPanel.InitialPhase)
                {
                    reacInitial.Add(phase.Name, new List<DiagramGraphPoint>());
                    reacFinal.Add(phase.Name, new List<DiagramGraphPoint>());
                }
            }

            var dicVel = new Dictionary<string, List<GraphPoint>>();

            for (int i = 0; i < cctPanel.VelNumber; i++)
            {
                var curVel = new List<GraphPoint>();
                var temp = tempStart;
                var time = 1.0f;

                var model = new MetallurgicalModel();
                var processData = new ProcessData(coolings, new string[] { "Охлаждение" });

                SetInitialCondition(phases, cctPanel.InitialPhase);

                var reacData = new Dictionary<string, List<DiagramGraphPoint>>();

                foreach (var phase in phases)
                    reacData.Add(phase.Name, new List<DiagramGraphPoint>());

                while (temp > tempStop)
                {
                    model.Calc(temp, timeStep, phases,processData);
                    var time_log10 = (float)Math.Log10(time);
                    curVel.Add(new GraphPoint(time_log10, temp));

                    foreach (var phase in phases)
                    {
                        var val = phase.Value;
                        if (reacData[phase.Name].Count == 0)
                            reacData[phase.Name].Add(new DiagramGraphPoint(time_log10, temp, val));
                        else if (reacData[phase.Name].Count > 0 && Math.Abs(reacData[phase.Name].LastOrDefault().Phase - val) > 0.01f)
                            reacData[phase.Name].Add(new DiagramGraphPoint(time_log10, temp, val));
                    }
                    temp += tempVel;
                    time++;
                }

                foreach (var reac in reacData)
                {
                    if (reacInitial.ContainsKey(reac.Key) & reac.Value.Count > 1)
                    {
                        var fval = reac.Value.FirstOrDefault(x => x.Phase > 0.025);
                        if (fval != null)
                            reacInitial[reac.Key].Add(fval);
                    }

                    if (reacFinal.ContainsKey(reac.Key) & reac.Value.Count > 1)
                    {
                        var lval = reac.Value.LastOrDefault(x => x.Phase > 0.05);
                        if (lval != null)
                            reacFinal[reac.Key].Add(lval);
                    }

                }
                dicVel.Add($"Скорость {vel}", curVel);

                vel = cctPanel.MinVel * (float)Math.Pow(velQ,i + 1);
                tempVel += vel;
            }

            CreateCCTDiagram(reacInitial, reacFinal, dicVel);
        }

        private void SetInitialCondition(PhaseData phases, string iniPhase)
        {
            foreach (var item in phases)
            {
                if (item.Name == iniPhase)
                    item.Value = 1.0f;
                else
                    item.Value = 0;
            }
        }

        private void CreateTTTDiagram(Dictionary<string, List<DiagramGraphPoint>> reacInitial, Dictionary<string, List<DiagramGraphPoint>> reacFinal)
        {
            var grDataRange = new List<GraphData>();
            Random rnd = new Random();
            var phases = phaseData.AsEnumerable().Select(r => r.Field<string>(0));
            foreach (var phaseName in phases)
            {
                var red = rnd.Next(0, 255);
                var green = rnd.Next(0, 255);
                var blue = rnd.Next(0, 255);

                var color = Color.FromArgb(red, green, blue);

                if (reacInitial.ContainsKey(phaseName) && reacInitial[phaseName].Count != 0)
                {
                    var data = new GraphData($"Реакция {phaseName} начало", color, "Время,сек", "Температура,°С", reacInitial[phaseName].ToArray());
                    data.ValueFlag = true;
                    data.Thickness = 3.5f;
                    grDataRange.Add(data);
                }

                if (reacFinal.ContainsKey(phaseName) && reacFinal[phaseName].Count != 0)
                {
                    var data = new GraphData($"Реакция {phaseName} конец", color, "Время,сек", "Температура,°С", reacFinal[phaseName].ToArray());
                    data.ValueFlag = true;
                    data.Thickness = 3.5f;
                    grDataRange.Add(data);
                }
            }
            if (grDataRange.Count() == 0)
                MessageBox.Show(
                    Resources.CreateDiagram_LackOfCalcDataWarning,
                    Localization.Localization.GetAttentionCaption(),
                    MessageBoxButtons.OK,MessageBoxIcon.Warning);
            else
            {
                graphContainer.CreateGraphData("TTT", grDataRange, new AxisFormat()
                { StepFormat = StepFormat.logarithmic, NumberOfSings = 0 }, new AxisFormat() { NumberOfSings = 2 });
            }

        }

        private void CreateCCTDiagram(Dictionary<string, List<DiagramGraphPoint>> reacInitial, Dictionary<string, List<DiagramGraphPoint>> reacFinal, Dictionary<string, List<GraphPoint>> dicVel)
        {
            var grDataRange = new List<GraphData>();
            Random rnd = new Random();
            var phases = phaseData.AsEnumerable().Select(r => r.Field<string>(0));
            foreach (var phaseName in phases)
            {
                var red = rnd.Next(0, 255);
                var green = rnd.Next(0, 255);
                var blue = rnd.Next(0, 255);

                var color = Color.FromArgb(red, green, blue);

                if (reacInitial.ContainsKey(phaseName) && reacInitial[phaseName].Count != 0)
                {
                    var data = new GraphData($"Реакция {phaseName} начало", color, "Время,сек", "Температура,°С", reacInitial[phaseName].ToArray());
                    data.ValueFlag = true;
                    data.Thickness = 3.5f;
                    grDataRange.Add(data);
                }

                if (reacFinal.ContainsKey(phaseName) && reacFinal[phaseName].Count != 0)
                {
                    var data = new GraphData($"Реакция {phaseName} конец", color, "Время,сек", "Температура,°С", reacFinal[phaseName].ToArray());
                    data.ValueFlag = true;
                    data.Thickness = 3.5f;
                    grDataRange.Add(data);
                }
            }

            foreach (var vel in dicVel)
            {
                if (vel.Value.Count > 1)
                {
                    var dataVel = new GraphData(vel.Key, Color.Black, "Время,сек", "Температура,°С", vel.Value.ToArray());
                    dataVel.ValueFlag = false;
                    grDataRange.Add(dataVel);
                }
            }

            if (grDataRange.Count() == 0)
                MessageBox.Show(
                    Resources.CreateDiagram_LackOfCalcDataWarning,
                    Localization.Localization.GetAttentionCaption(),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                graphContainer.CreateGraphData("CCT", grDataRange,
    new AxisFormat()
    { StepFormat = StepFormat.logarithmic, NumberOfSings = 0 },
    new AxisFormat() { NumberOfSings = 2 }
);
            }
        }       
    }
}
