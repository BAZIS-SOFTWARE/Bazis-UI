using MaterialDB.MaterialData;
using MaterialDB.Utilities;
using PropertiesCalculator.PropertiesCalculator.MechanicalModels;
using PropertiesCalculator.PropertiesController.Interfaces;
using PropertiesCalculator.PropertiesController.MechanicalModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UserControlsEx.Graph;

namespace WinGUI.DataBases.MechanicalGUI
{
    public partial class HardeningControl : UserControl
    {
        private DataTable yieldTable;
        private DataTable ultimateTable;
        private DataTable hardKoeffTable;
        private DataTable phaseTable;
        private int hardModel;
        IHardeningModel<float> hardeningModel;

        public HardeningControl(PropertyData mechProps, PropertyData genProps)
        {
            InitializeComponent();

            yieldTable = mechProps["Предел текучести"].DataTable;
            ultimateTable = mechProps["Предел прочности"].DataTable;
            hardKoeffTable = mechProps["Коэффициент упрочнения"].DataTable;
            var modelNumberTable = genProps["Модель упрочнения"].DataTable;
            hardModel = Convert.ToInt32(modelNumberTable.Rows[0]["Модель упрочнения"]);
            phaseTable = genProps["Структура"].DataTable;

            
            var phases = phaseTable.AsEnumerable().Select(r => r.Field<string>(0));
            foreach (var phase in phases)
                cmbPhases.Items.Add(phase);


            if (hardModel == 1)
                hardeningModel = new LinearHardeningModel();
            else
                hardeningModel = new ExponentialHardeningModel();
        }

        private List<GraphData> CaclHardeningForTemp()
        {
            var temps = yieldTable.AsEnumerable().Select(r => Convert.ToSingle(r[0])).ToList();

            var temp = float.Parse(txbTemp.Text);

            var temp1 = temps.Find(x => Math.Abs(x - temp) < 1e-4 | x > temp);
            var temp1_index = temps.IndexOf(temp1);
            var temp0_index = temp1_index - 1;
            var temp0 = temps[temp0_index];

            var grDataRange = new List<GraphData>();

            float st0, su0, m0;
            GetPhysicalData(temp0_index, out st0, out su0, out m0);
            float st1, su1, m1;
            GetPhysicalData(temp1_index, out st1, out su1, out m1);

            var deps = 1.0f / yieldTable.Rows.Count;

            var st = InterpolationSearch.InterpolatedValue(new float[] { temp0, temp1 },new float[] { st0, st1 }, temp);
            var su = InterpolationSearch.InterpolatedValue(new float[] { temp0, temp1 }, new float[] { su0, su1 },  temp);
            var m = InterpolationSearch.InterpolatedValue(new float[] { temp0, temp1 }, new float[] { m0, m1 }, temp);

            var points = CalcHardnessForStructure(st, su, m, deps, hardModel);

            var grData = new GraphData("Упрочнение", Color.Orange, "", "", points.ToArray());
            grDataRange.Add(grData);

            return grDataRange;
        }

        private void GetPhysicalData(int temp_index, out float st, out float su, out float m)
        {
            st = Convert.ToSingle(yieldTable.Rows[temp_index][cmbPhases.Text]);
            su = Convert.ToSingle(ultimateTable.Rows[temp_index][cmbPhases.Text]);
            m = Convert.ToSingle(hardKoeffTable.Rows[temp_index][cmbPhases.Text]);
        }

        private List<GraphData> CaclHardeningForTemps()
        {
            var temps = yieldTable.AsEnumerable().Select(r => r.Field<float>(0)); ;

            var grDataRange = new List<GraphData>();

            for (int i = 0; i < yieldTable.Rows.Count; i++)
            {
                var st = Convert.ToSingle(yieldTable.Rows[i][cmbPhases.Text]);
                var sr = Convert.ToSingle(ultimateTable.Rows[i][cmbPhases.Text]);
                var m = Convert.ToSingle(hardKoeffTable.Rows[i][cmbPhases.Text]);
                var deps = 1.0f / yieldTable.Rows.Count;
                var points = CalcHardnessForStructure(st, sr, m, deps, hardModel);

                var grData = new GraphData("Упрочнение", Color.Orange, "", "", points.ToArray());
                grDataRange.Add(grData);
            }

            return grDataRange;
        }

        private List<GraphPoint> CalcHardnessForStructure(float st, float sr, float m, float d_eps, int modelNumber)
        {
            var points = new List<GraphPoint>();
            var eps = 0.0f;

            while (eps <= 1)
            {
                var stress = st + hardeningModel.Calc(st, m, sr, eps);
                points.Add(new GraphPoint(eps, stress));

                eps += d_eps;
            }

            return points;
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            try
            {
                List<GraphData> data;
                if (chbTemp.Checked)
                    data = CaclHardeningForTemp();
                else
                    data = CaclHardeningForTemps();
                graphContainer.CreateGraphData("Упрочнение", data, new AxisFormat(), new AxisFormat());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
       
        }

        private void chbTemp_CheckedChanged(object sender, EventArgs e)
        {
            if (chbTemp.Checked)
                txbTemp.Enabled = true;
            else txbTemp.Enabled = false;
        }
    }
}
