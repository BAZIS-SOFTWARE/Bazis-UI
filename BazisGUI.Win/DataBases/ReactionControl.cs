using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MaterialDB.MaterialData;
using BazisGUI.Win.Properties;

namespace BazisGUI.Win.DataBases
{
    public partial class ReactionControl : UserControl
    {
        Property reaction;
        Color selectionColor = Color.Orange;
        Color initialColor = Control.DefaultBackColor;
        public event Action<string,string> ChangeReactionName;
        public ReactionControl(string [] phaseNames, Property reaction)
        {
            InitializeComponent();

            dgv.EnableHeadersVisualStyles = false;

            this.reaction = reaction;

            cmbFinalPhase.Items.AddRange(phaseNames);
            cmbInitialPhase.Items.AddRange(phaseNames);

            var reacData = reaction.Name.Split(' ');
            cmbPhaseName.Text = reacData[0];

            var phases = reacData[1].Split('-');
            cmbInitialPhase.Text = phases[0];
            cmbFinalPhase.Text = phases[1];
            
            dgv.DataSource = reaction.DataTable;

            if (reaction.DataTable.Columns.Count > 2)
            {
                chbTimeDependent.Checked = true;
                btnAddPhaseValue.Enabled = true;
                btnChangePhaseValue.Enabled = true;
                btnDelPhaseValue.Enabled = true;
            }


            dgv_SetInitialBackColor();

            foreach (DataGridViewColumn column in dgv.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"(^([0]\.)(\d{0,3}[1-9])$)|(^[1]$)|(^[0]$)");
            if (!regex.IsMatch(txbPhaseValue.Text))
            {
                MessageBox.Show(
                    Resources.InvalidRegexPhaseMatchWarning,
                    Localization.Localization.GetAttentionCaption(),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);                
                return;
            }

            try
            {
                var column = new DataColumn($"Масс.Доли_{txbPhaseValue.Text}", typeof(float))
                { DefaultValue = 0 };
                reaction.DataTable.Columns.Add(column);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    if (dgv.Columns[i].HeaderCell.Style.BackColor == selectionColor)
                        if (!reaction.DataTable.Columns[dgv.Columns[i].Index].ColumnName.Split('_')[1].Equals(txbPhaseValue.Text))
                            reaction.DataTable.Columns[dgv.Columns[i].Index].ColumnName = $"Масс.Доли_{txbPhaseValue.Text}";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgv_SetInitialBackColor();

            if(e.ColumnIndex > 0)
            {
                txbPhaseValue.Text = dgv.Columns[e.ColumnIndex].HeaderText.Split('_')[1];
                dgv.Columns[e.ColumnIndex].HeaderCell.Style.BackColor = selectionColor;
            }
        }

        private void dgv_SetInitialBackColor()
        {
            foreach (DataGridViewColumn column in dgv.Columns)
                column.HeaderCell.Style.BackColor = initialColor;
        }

        private void cmbInitialPhase_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var oldReacName = reaction.Name;

            var newReacName = $"{cmbPhaseName.Text} {cmbInitialPhase.SelectedItem}-{cmbFinalPhase.Text}";

            ChangeReactionName?.Invoke(oldReacName, newReacName);
        }

        private void cmbFinalPhase_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var oldReacName = reaction.Name;            

            var newReacName = $"{cmbPhaseName.Text} {cmbInitialPhase.Text}-{cmbFinalPhase.SelectedItem}";            

            ChangeReactionName?.Invoke(oldReacName, newReacName);
        }

        private void btnDelTime_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (dgv.Columns[i].HeaderCell.Style.BackColor == selectionColor)
                    reaction.DataTable.Columns.RemoveAt(dgv.Columns[i].Index);
            }
        }

        private void cmbPhaseName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var oldReacName = reaction.Name;            

            var newReacName = $"{cmbPhaseName.SelectedItem} {cmbInitialPhase.Text}-{cmbFinalPhase.Text}";            

            ChangeReactionName?.Invoke(oldReacName, newReacName);
        }

        private void chbTimeDependent_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                Resources.EnteredDataDeletingWarning, 
                Localization.Localization.GetAttentionCaption(),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            
            if(result == DialogResult.OK)
            {
                for (int i = dgv.Columns.Count - 1; i > 0; i--)
                    reaction.DataTable.Columns.RemoveAt(i);

                if (chbTimeDependent.Checked)
                {
                    btnAddPhaseValue.Enabled = true;
                    btnChangePhaseValue.Enabled = true;
                    btnDelPhaseValue.Enabled = true;

                    var column0 = new DataColumn($"Масс.Доли_0.1", typeof(float))
                    { DefaultValue = 0 };
                    reaction.DataTable.Columns.Add(column0);
                    var column1 = new DataColumn($"Масс.Доли_1", typeof(float))
                    { DefaultValue = 0 };
                    reaction.DataTable.Columns.Add(column1);
                }
                else
                {
                    btnAddPhaseValue.Enabled = false;
                    btnDelPhaseValue.Enabled = false;
                    btnChangePhaseValue.Enabled = false;
                    var column0 = new DataColumn($"Масс.Доли", typeof(float))
                    { DefaultValue = 0 };
                    reaction.DataTable.Columns.Add(column0);
                }
            } 
        }
    }
}
