using UserControlsEx;

namespace BazisGUI.Win.TasksControls
{
    partial class MechTaskControl_v2
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
            this.label6 = new System.Windows.Forms.Label();
            this.chbPlastisity = new System.Windows.Forms.CheckBox();
            this.chbUMax = new System.Windows.Forms.CheckBox();
            this.txbMaxSiSt = new UserControlsEx.TextBoxEx(this.components);
            this.txbMaxU = new UserControlsEx.TextBoxEx(this.components);
            this.txbMaxDU = new UserControlsEx.TextBoxEx(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(3, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 153;
            this.label6.Text = "Макс. разница dU, >0";
            // 
            // chbPlastisity
            // 
            this.chbPlastisity.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chbPlastisity.AutoSize = true;
            this.chbPlastisity.Checked = true;
            this.chbPlastisity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbPlastisity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chbPlastisity.Location = new System.Drawing.Point(3, 92);
            this.chbPlastisity.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.chbPlastisity.Name = "chbPlastisity";
            this.chbPlastisity.Size = new System.Drawing.Size(175, 17);
            this.chbPlastisity.TabIndex = 152;
            this.chbPlastisity.Text = "Пласт. деформации Si/St, >1 ";
            this.chbPlastisity.UseVisualStyleBackColor = true;
            // 
            // chbUMax
            // 
            this.chbUMax.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chbUMax.AutoSize = true;
            this.chbUMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chbUMax.Location = new System.Drawing.Point(3, 51);
            this.chbUMax.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.chbUMax.Name = "chbUMax";
            this.chbUMax.Size = new System.Drawing.Size(159, 17);
            this.chbUMax.TabIndex = 135;
            this.chbUMax.Text = "Макс. перемещения U, >0";
            this.chbUMax.UseVisualStyleBackColor = true;
            // 
            // txbMaxSiSt
            // 
            this.txbMaxSiSt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbMaxSiSt.BackColor = System.Drawing.SystemColors.Window;
            this.txbMaxSiSt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbMaxSiSt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txbMaxSiSt.InputType = ((UserControlsEx.TXTBoxInputType)((UserControlsEx.TXTBoxInputType.Float | UserControlsEx.TXTBoxInputType.Positive)));
            this.txbMaxSiSt.IsValidating = true;
            this.txbMaxSiSt.Location = new System.Drawing.Point(212, 90);
            this.txbMaxSiSt.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbMaxSiSt.Name = "txbMaxSiSt";
            this.txbMaxSiSt.Size = new System.Drawing.Size(298, 20);
            this.txbMaxSiSt.TabIndex = 134;
            this.txbMaxSiSt.Text = "1.25";
            this.txbMaxSiSt.UserRegExCheck = null;
            this.txbMaxSiSt.UserRegExCheckErrorMessage = null;
            // 
            // txbMaxU
            // 
            this.txbMaxU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbMaxU.BackColor = System.Drawing.SystemColors.Window;
            this.txbMaxU.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbMaxU.Enabled = false;
            this.txbMaxU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txbMaxU.InputType = ((UserControlsEx.TXTBoxInputType)((UserControlsEx.TXTBoxInputType.Float | UserControlsEx.TXTBoxInputType.Positive)));
            this.txbMaxU.IsValidating = true;
            this.txbMaxU.Location = new System.Drawing.Point(212, 50);
            this.txbMaxU.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbMaxU.Name = "txbMaxU";
            this.txbMaxU.Size = new System.Drawing.Size(298, 20);
            this.txbMaxU.TabIndex = 133;
            this.txbMaxU.Text = "0.05";
            this.txbMaxU.UserRegExCheck = null;
            this.txbMaxU.UserRegExCheckErrorMessage = null;
            // 
            // txbMaxDU
            // 
            this.txbMaxDU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbMaxDU.BackColor = System.Drawing.SystemColors.Window;
            this.txbMaxDU.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbMaxDU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txbMaxDU.InputType = ((UserControlsEx.TXTBoxInputType)((UserControlsEx.TXTBoxInputType.Float | UserControlsEx.TXTBoxInputType.Positive)));
            this.txbMaxDU.IsValidating = true;
            this.txbMaxDU.Location = new System.Drawing.Point(212, 10);
            this.txbMaxDU.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbMaxDU.Name = "txbMaxDU";
            this.txbMaxDU.Size = new System.Drawing.Size(298, 20);
            this.txbMaxDU.TabIndex = 132;
            this.txbMaxDU.Text = "0.0005";
            this.txbMaxDU.UserRegExCheck = null;
            this.txbMaxDU.UserRegExCheckErrorMessage = null;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.txbMaxDU, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.chbPlastisity, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chbUMax, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txbMaxU, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txbMaxSiSt, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(530, 121);
            this.tableLayoutPanel1.TabIndex = 154;
            // 
            // MechTaskControl_v2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.MaximumSize = new System.Drawing.Size(700, 0);
            this.Name = "MechTaskControl_v2";
            this.Size = new System.Drawing.Size(530, 121);
            this.Tag = "300";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chbPlastisity;
        private System.Windows.Forms.CheckBox chbUMax;
        private TextBoxEx txbMaxSiSt;
        private TextBoxEx txbMaxU;
        private TextBoxEx txbMaxDU;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
