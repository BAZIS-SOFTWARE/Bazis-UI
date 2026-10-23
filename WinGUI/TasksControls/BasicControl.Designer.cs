namespace WinGUI.TasksControls
{
    partial class BasicControl
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
            this.maxTMIter = new System.Windows.Forms.Label();
            this.txbIters = new UserControlsEx.TextBoxEx(this.components);
            this.lblSaveRateTM = new System.Windows.Forms.Label();
            this.txbSaveRate = new UserControlsEx.TextBoxEx(this.components);
            this.txbIniTemp = new UserControlsEx.TextBoxEx(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txbSaveRate, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblSaveRateTM, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txbIters, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.maxTMIter, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txbIniTemp, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(281, 251);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // maxTMIter
            // 
            this.maxTMIter.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.maxTMIter.AutoSize = true;
            this.maxTMIter.Location = new System.Drawing.Point(3, 28);
            this.maxTMIter.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.maxTMIter.Name = "maxTMIter";
            this.maxTMIter.Size = new System.Drawing.Size(106, 26);
            this.maxTMIter.TabIndex = 127;
            this.maxTMIter.Text = "Кол-во итераций на шаге";
            this.maxTMIter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.maxTMIter.UseWaitCursor = true;
            // 
            // txbIters
            // 
            this.txbIters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbIters.BackColor = System.Drawing.SystemColors.Window;
            this.txbIters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbIters.InputType = UserControlsEx.TXTBoxInputType.Integer;
            this.txbIters.IsValidating = true;
            this.txbIters.Location = new System.Drawing.Point(112, 31);
            this.txbIters.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbIters.Name = "txbIters";
            this.txbIters.Size = new System.Drawing.Size(149, 20);
            this.txbIters.TabIndex = 143;
            this.txbIters.Text = "2";
            this.txbIters.UserRegExCheck = null;
            this.txbIters.UserRegExCheckErrorMessage = null;
            // 
            // lblSaveRateTM
            // 
            this.lblSaveRateTM.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSaveRateTM.AutoSize = true;
            this.lblSaveRateTM.Location = new System.Drawing.Point(3, 111);
            this.lblSaveRateTM.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblSaveRateTM.Name = "lblSaveRateTM";
            this.lblSaveRateTM.Size = new System.Drawing.Size(91, 26);
            this.lblSaveRateTM.TabIndex = 144;
            this.lblSaveRateTM.Text = "Частота сохранений, шаг";
            this.lblSaveRateTM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSaveRateTM.UseWaitCursor = true;
            // 
            // txbSaveRate
            // 
            this.txbSaveRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbSaveRate.BackColor = System.Drawing.SystemColors.Window;
            this.txbSaveRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbSaveRate.InputType = UserControlsEx.TXTBoxInputType.Integer;
            this.txbSaveRate.IsValidating = true;
            this.txbSaveRate.Location = new System.Drawing.Point(112, 114);
            this.txbSaveRate.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbSaveRate.Name = "txbSaveRate";
            this.txbSaveRate.Size = new System.Drawing.Size(149, 20);
            this.txbSaveRate.TabIndex = 145;
            this.txbSaveRate.Text = "1";
            this.txbSaveRate.UserRegExCheck = null;
            this.txbSaveRate.UserRegExCheckErrorMessage = null;
            // 
            // txbIniTemp
            // 
            this.txbIniTemp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbIniTemp.BackColor = System.Drawing.SystemColors.Window;
            this.txbIniTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbIniTemp.InputType = ((UserControlsEx.TXTBoxInputType)((UserControlsEx.TXTBoxInputType.Float | UserControlsEx.TXTBoxInputType.Positive)));
            this.txbIniTemp.IsValidating = true;
            this.txbIniTemp.Location = new System.Drawing.Point(112, 198);
            this.txbIniTemp.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbIniTemp.Name = "txbIniTemp";
            this.txbIniTemp.Size = new System.Drawing.Size(149, 20);
            this.txbIniTemp.TabIndex = 146;
            this.txbIniTemp.Text = "20";
            this.txbIniTemp.UserRegExCheck = null;
            this.txbIniTemp.UserRegExCheckErrorMessage = null;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 195);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 26);
            this.label1.TabIndex = 147;
            this.label1.Text = "Начальная температура, C°";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.UseWaitCursor = true;
            // 
            // BasicControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BasicControl";
            this.Size = new System.Drawing.Size(281, 251);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label maxTMIter;
        private UserControlsEx.TextBoxEx txbIters;
        private System.Windows.Forms.Label lblSaveRateTM;
        private UserControlsEx.TextBoxEx txbSaveRate;
        private System.Windows.Forms.Label label1;
        private UserControlsEx.TextBoxEx txbIniTemp;
    }
}
