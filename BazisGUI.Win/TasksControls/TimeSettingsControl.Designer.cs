namespace BazisGUI.Win.TasksControls
{
    partial class TimeSettingsControl
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
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txbStartTime = new UserControlsEx.TextBoxEx(this.components);
            this.label11 = new System.Windows.Forms.Label();
            this.txbStopTime = new UserControlsEx.TextBoxEx(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.txbStartStep = new UserControlsEx.TextBoxEx(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.txbMinStep = new UserControlsEx.TextBoxEx(this.components);
            this.txbMaxStep = new UserControlsEx.TextBoxEx(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txbStartTime, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txbStopTime, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txbStartStep, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txbMinStep, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txbMaxStep, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(398, 442);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label8.Location = new System.Drawing.Point(3, 384);
            this.label8.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(154, 26);
            this.label8.TabIndex = 177;
            this.label8.Text = "Максимальный шаг расчета, сек";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label8.UseWaitCursor = true;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label10.Location = new System.Drawing.Point(3, 37);
            this.label10.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 168;
            this.label10.Text = "Время начала, сек";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txbStartTime
            // 
            this.txbStartTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbStartTime.BackColor = System.Drawing.SystemColors.Window;
            this.txbStartTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbStartTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txbStartTime.InputType = ((UserControlsEx.TXTBoxInputType)((UserControlsEx.TXTBoxInputType.Float | UserControlsEx.TXTBoxInputType.Positive)));
            this.txbStartTime.IsValidating = true;
            this.txbStartTime.Location = new System.Drawing.Point(159, 34);
            this.txbStartTime.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbStartTime.Name = "txbStartTime";
            this.txbStartTime.Size = new System.Drawing.Size(219, 20);
            this.txbStartTime.TabIndex = 169;
            this.txbStartTime.Tag = "0";
            this.txbStartTime.UserRegExCheck = null;
            this.txbStartTime.UserRegExCheckErrorMessage = null;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label11.Location = new System.Drawing.Point(3, 125);
            this.label11.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 13);
            this.label11.TabIndex = 170;
            this.label11.Text = "Время окончания, сек";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txbStopTime
            // 
            this.txbStopTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbStopTime.BackColor = System.Drawing.SystemColors.Window;
            this.txbStopTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbStopTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txbStopTime.InputType = ((UserControlsEx.TXTBoxInputType)((UserControlsEx.TXTBoxInputType.Float | UserControlsEx.TXTBoxInputType.Positive)));
            this.txbStopTime.IsValidating = true;
            this.txbStopTime.Location = new System.Drawing.Point(159, 122);
            this.txbStopTime.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbStopTime.Name = "txbStopTime";
            this.txbStopTime.Size = new System.Drawing.Size(219, 20);
            this.txbStopTime.TabIndex = 171;
            this.txbStopTime.Tag = "1";
            this.txbStopTime.UserRegExCheck = null;
            this.txbStopTime.UserRegExCheckErrorMessage = null;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.Location = new System.Drawing.Point(3, 213);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 13);
            this.label5.TabIndex = 172;
            this.label5.Text = "Начальный шаг расчета, сек";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.UseWaitCursor = true;
            // 
            // txbStartStep
            // 
            this.txbStartStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbStartStep.BackColor = System.Drawing.SystemColors.Window;
            this.txbStartStep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbStartStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txbStartStep.InputType = ((UserControlsEx.TXTBoxInputType)((UserControlsEx.TXTBoxInputType.Float | UserControlsEx.TXTBoxInputType.Positive)));
            this.txbStartStep.IsValidating = true;
            this.txbStartStep.Location = new System.Drawing.Point(159, 210);
            this.txbStartStep.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbStartStep.Name = "txbStartStep";
            this.txbStartStep.Size = new System.Drawing.Size(219, 20);
            this.txbStartStep.TabIndex = 175;
            this.txbStartStep.Tag = "2";
            this.txbStartStep.Text = "0.1";
            this.txbStartStep.UserRegExCheck = null;
            this.txbStartStep.UserRegExCheckErrorMessage = null;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label9.Location = new System.Drawing.Point(3, 295);
            this.label9.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(148, 26);
            this.label9.TabIndex = 176;
            this.label9.Text = "Минимальный шаг расчета, сек";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label9.UseWaitCursor = true;
            // 
            // txbMinStep
            // 
            this.txbMinStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbMinStep.BackColor = System.Drawing.SystemColors.Window;
            this.txbMinStep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbMinStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txbMinStep.InputType = ((UserControlsEx.TXTBoxInputType)((UserControlsEx.TXTBoxInputType.Float | UserControlsEx.TXTBoxInputType.Positive)));
            this.txbMinStep.IsValidating = true;
            this.txbMinStep.Location = new System.Drawing.Point(159, 298);
            this.txbMinStep.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbMinStep.Name = "txbMinStep";
            this.txbMinStep.Size = new System.Drawing.Size(219, 20);
            this.txbMinStep.TabIndex = 178;
            this.txbMinStep.Tag = "3";
            this.txbMinStep.Text = "0.00001";
            this.txbMinStep.UserRegExCheck = null;
            this.txbMinStep.UserRegExCheckErrorMessage = null;
            // 
            // txbMaxStep
            // 
            this.txbMaxStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbMaxStep.BackColor = System.Drawing.SystemColors.Window;
            this.txbMaxStep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbMaxStep.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txbMaxStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txbMaxStep.InputType = ((UserControlsEx.TXTBoxInputType)((UserControlsEx.TXTBoxInputType.Float | UserControlsEx.TXTBoxInputType.Positive)));
            this.txbMaxStep.IsValidating = true;
            this.txbMaxStep.Location = new System.Drawing.Point(159, 387);
            this.txbMaxStep.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbMaxStep.Name = "txbMaxStep";
            this.txbMaxStep.Size = new System.Drawing.Size(219, 20);
            this.txbMaxStep.TabIndex = 179;
            this.txbMaxStep.Tag = "4";
            this.txbMaxStep.Text = "100";
            this.txbMaxStep.UserRegExCheck = null;
            this.txbMaxStep.UserRegExCheckErrorMessage = null;
            // 
            // TimeSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TimeSettingsControl";
            this.Size = new System.Drawing.Size(398, 442);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label10;
        private UserControlsEx.TextBoxEx txbStartTime;
        private System.Windows.Forms.Label label11;
        private UserControlsEx.TextBoxEx txbStopTime;
        private System.Windows.Forms.Label label5;
        private UserControlsEx.TextBoxEx txbStartStep;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private UserControlsEx.TextBoxEx txbMinStep;
        private UserControlsEx.TextBoxEx txbMaxStep;
    }
}
