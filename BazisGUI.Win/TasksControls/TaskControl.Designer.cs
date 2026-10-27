using UserControlsEx;

namespace BazisGUI.Win.TasksControls
{
    partial class TaskControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxEx1 = new UserControlsEx.TextBoxEx(this.components);
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxEx2 = new UserControlsEx.TextBoxEx(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.solverSettingsControl = new BazisGUI.Win.TasksControls.SolverSettingsControl();
            this.timeSettingsControl = new BazisGUI.Win.TasksControls.TimeSettingsControl();
            this.basicControl = new BazisGUI.Win.TasksControls.BasicControl();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chemicalControl = new BazisGUI.Win.TasksControls.ChemicalControl();
            this.heatTaskControl = new BazisGUI.Win.TasksControls.HeatTaskControl_v2();
            this.mechTaskControl = new BazisGUI.Win.TasksControls.MechTaskControl_v2();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxEx1
            // 
            this.textBoxEx1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEx1.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxEx1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxEx1.InputType = UserControlsEx.TXTBoxInputType.Text;
            this.textBoxEx1.IsValidating = true;
            this.textBoxEx1.Location = new System.Drawing.Point(243, 1389);
            this.textBoxEx1.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxEx1.Name = "textBoxEx1";
            this.textBoxEx1.Size = new System.Drawing.Size(177, 20);
            this.textBoxEx1.TabIndex = 176;
            this.textBoxEx1.Text = "3";
            this.textBoxEx1.UserRegExCheck = null;
            this.textBoxEx1.UserRegExCheckErrorMessage = null;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(30, 1389);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(0);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(167, 17);
            this.checkBox1.TabIndex = 177;
            this.checkBox1.Text = "Макс.концентр. (dCt max), %";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 1444);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 13);
            this.label6.TabIndex = 178;
            this.label6.Text = "Начальная концентрация, %";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxEx2
            // 
            this.textBoxEx2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEx2.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxEx2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxEx2.InputType = UserControlsEx.TXTBoxInputType.Text;
            this.textBoxEx2.IsValidating = true;
            this.textBoxEx2.Location = new System.Drawing.Point(243, 1440);
            this.textBoxEx2.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxEx2.Name = "textBoxEx2";
            this.textBoxEx2.Size = new System.Drawing.Size(177, 20);
            this.textBoxEx2.TabIndex = 179;
            this.textBoxEx2.Text = "0.18";
            this.textBoxEx2.UserRegExCheck = null;
            this.textBoxEx2.UserRegExCheckErrorMessage = null;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.solverSettingsControl, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.timeSettingsControl, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.basicControl, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.92424F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.07576F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 211F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(664, 650);
            this.tableLayoutPanel1.TabIndex = 184;
            // 
            // solverSettingsControl
            // 
            this.solverSettingsControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.solverSettingsControl, 2);
            this.solverSettingsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.solverSettingsControl.Location = new System.Drawing.Point(0, 79);
            this.solverSettingsControl.Margin = new System.Windows.Forms.Padding(0);
            this.solverSettingsControl.Name = "solverSettingsControl";
            this.solverSettingsControl.Size = new System.Drawing.Size(664, 185);
            this.solverSettingsControl.TabIndex = 189;
            // 
            // timeSettingsControl
            // 
            this.timeSettingsControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.timeSettingsControl, 2);
            this.timeSettingsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeSettingsControl.Location = new System.Drawing.Point(0, 374);
            this.timeSettingsControl.Margin = new System.Windows.Forms.Padding(0);
            this.timeSettingsControl.Name = "timeSettingsControl";
            this.timeSettingsControl.Size = new System.Drawing.Size(664, 211);
            this.timeSettingsControl.TabIndex = 188;
            // 
            // basicControl
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.basicControl, 2);
            this.basicControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicControl.InitTemp = "20";
            this.basicControl.Iterations = "2";
            this.basicControl.Location = new System.Drawing.Point(0, 264);
            this.basicControl.Margin = new System.Windows.Forms.Padding(0);
            this.basicControl.Name = "basicControl";
            this.basicControl.SaveRate = "1";
            this.basicControl.Size = new System.Drawing.Size(664, 110);
            this.basicControl.TabIndex = 190;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(569, 606);
            this.btnSave.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 191;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.mechTaskControl);
            this.panel1.Controls.Add(this.heatTaskControl);
            this.panel1.Controls.Add(this.chemicalControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(664, 79);
            this.panel1.TabIndex = 192;
            // 
            // chemicalControl
            // 
            this.chemicalControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chemicalControl.InitConcentr = "0.18";
            this.chemicalControl.IsMaxConcentrSwitch = true;
            this.chemicalControl.Location = new System.Drawing.Point(0, 0);
            this.chemicalControl.Margin = new System.Windows.Forms.Padding(0);
            this.chemicalControl.MaxConcentr = "3";
            this.chemicalControl.Name = "chemicalControl";
            this.chemicalControl.Size = new System.Drawing.Size(664, 79);
            this.chemicalControl.TabIndex = 187;
            // 
            // heatTaskControl_v21
            // 
            this.heatTaskControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.heatTaskControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.heatTaskControl.DTMax = "1500";
            this.heatTaskControl.Location = new System.Drawing.Point(0, 0);
            this.heatTaskControl.Margin = new System.Windows.Forms.Padding(0);
            this.heatTaskControl.MaximumSize = new System.Drawing.Size(700, 0);
            this.heatTaskControl.Name = "heatTaskControl_v21";
            this.heatTaskControl.Size = new System.Drawing.Size(664, 79);
            this.heatTaskControl.TabIndex = 188;
            this.heatTaskControl.Tag = "260";
            // 
            // mechTaskControl
            // 
            this.mechTaskControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mechTaskControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mechTaskControl.Location = new System.Drawing.Point(0, 0);
            this.mechTaskControl.Margin = new System.Windows.Forms.Padding(0);
            this.mechTaskControl.MaxDU = "0.0005";
            this.mechTaskControl.MaxSiSt = "1.25";
            this.mechTaskControl.MaxU = "0.05";
            this.mechTaskControl.Name = "mechTaskControl";
            this.mechTaskControl.Size = new System.Drawing.Size(664, 79);
            this.mechTaskControl.TabIndex = 189;
            this.mechTaskControl.Tag = "300";
            // 
            // TaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.textBoxEx1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxEx2);
            this.MaximumSize = new System.Drawing.Size(700, 0);
            this.MinimumSize = new System.Drawing.Size(600, 650);
            this.Name = "TaskControl";
            this.Size = new System.Drawing.Size(664, 650);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextBoxEx textBoxEx1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label6;
        private TextBoxEx textBoxEx2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ChemicalControl chemicalControl;
        private SolverSettingsControl solverSettingsControl;
        private TimeSettingsControl timeSettingsControl;
        private BasicControl basicControl;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel1;
        private MechTaskControl_v2 mechTaskControl;
        private HeatTaskControl_v2 heatTaskControl;
    }
}
