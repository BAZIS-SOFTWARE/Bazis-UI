namespace WinGUI.TasksControls
{
    partial class ChemicalControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txbMaxConcentr = new UserControlsEx.TextBoxEx(this.components);
            this.chbMaxConcentr = new System.Windows.Forms.CheckBox();
            this.txbIniConcentr = new UserControlsEx.TextBoxEx(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.txbMaxConcentr, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.chbMaxConcentr, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txbIniConcentr, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(569, 646);
            this.tableLayoutPanel1.TabIndex = 185;
            // 
            // txbMaxConcentr
            // 
            this.txbMaxConcentr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbMaxConcentr.BackColor = System.Drawing.SystemColors.Window;
            this.txbMaxConcentr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbMaxConcentr.InputType = UserControlsEx.TXTBoxInputType.Text;
            this.txbMaxConcentr.IsValidating = true;
            this.txbMaxConcentr.Location = new System.Drawing.Point(227, 151);
            this.txbMaxConcentr.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbMaxConcentr.Name = "txbMaxConcentr";
            this.txbMaxConcentr.Size = new System.Drawing.Size(322, 20);
            this.txbMaxConcentr.TabIndex = 180;
            this.txbMaxConcentr.Text = "3";
            this.txbMaxConcentr.UserRegExCheck = null;
            this.txbMaxConcentr.UserRegExCheckErrorMessage = null;
            // 
            // chbMaxConcentr
            // 
            this.chbMaxConcentr.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chbMaxConcentr.AutoSize = true;
            this.chbMaxConcentr.Checked = true;
            this.chbMaxConcentr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbMaxConcentr.Enabled = false;
            this.chbMaxConcentr.Location = new System.Drawing.Point(3, 153);
            this.chbMaxConcentr.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.chbMaxConcentr.Name = "chbMaxConcentr";
            this.chbMaxConcentr.Size = new System.Drawing.Size(167, 17);
            this.chbMaxConcentr.TabIndex = 181;
            this.chbMaxConcentr.Text = "Макс.концентр. (dCt max), %";
            this.chbMaxConcentr.UseVisualStyleBackColor = true;
            // 
            // txbIniConcentr
            // 
            this.txbIniConcentr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbIniConcentr.BackColor = System.Drawing.SystemColors.Window;
            this.txbIniConcentr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbIniConcentr.InputType = UserControlsEx.TXTBoxInputType.Text;
            this.txbIniConcentr.IsValidating = true;
            this.txbIniConcentr.Location = new System.Drawing.Point(227, 474);
            this.txbIniConcentr.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbIniConcentr.Name = "txbIniConcentr";
            this.txbIniConcentr.Size = new System.Drawing.Size(322, 20);
            this.txbIniConcentr.TabIndex = 183;
            this.txbIniConcentr.Text = "0.18";
            this.txbIniConcentr.UserRegExCheck = null;
            this.txbIniConcentr.UserRegExCheckErrorMessage = null;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 478);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 182;
            this.label1.Text = "Начальная концентрация, %";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChemicalControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ChemicalControl";
            this.Size = new System.Drawing.Size(569, 646);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private UserControlsEx.TextBoxEx txbMaxConcentr;
        private System.Windows.Forms.CheckBox chbMaxConcentr;
        private UserControlsEx.TextBoxEx txbIniConcentr;
        private System.Windows.Forms.Label label1;
    }
}
