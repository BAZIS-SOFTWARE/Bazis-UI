namespace BazisGUI.Win.DataBases.MetallurgyGUI
{
    partial class CCTControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CCTControl));
            txbMinVel = new System.Windows.Forms.TextBox();
            txbMaxVel = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            nudVels = new System.Windows.Forms.NumericUpDown();
            cmbPhases = new System.Windows.Forms.ComboBox();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            txbIniTemp = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            txbFinTemp = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)nudVels).BeginInit();
            SuspendLayout();
            // 
            // txbMinVel
            // 
            resources.ApplyResources(txbMinVel, "txbMinVel");
            txbMinVel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbMinVel.Name = "txbMinVel";
            // 
            // txbMaxVel
            // 
            resources.ApplyResources(txbMaxVel, "txbMaxVel");
            txbMaxVel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbMaxVel.Name = "txbMaxVel";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // nudVels
            // 
            resources.ApplyResources(nudVels, "nudVels");
            nudVels.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            nudVels.Name = "nudVels";
            nudVels.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // cmbPhases
            // 
            resources.ApplyResources(cmbPhases, "cmbPhases");
            cmbPhases.FormattingEnabled = true;
            cmbPhases.Name = "cmbPhases";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // txbIniTemp
            // 
            resources.ApplyResources(txbIniTemp, "txbIniTemp");
            txbIniTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbIniTemp.Name = "txbIniTemp";
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // txbFinTemp
            // 
            resources.ApplyResources(txbFinTemp, "txbFinTemp");
            txbFinTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbFinTemp.Name = "txbFinTemp";
            // 
            // CCTControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(txbFinTemp);
            Controls.Add(txbIniTemp);
            Controls.Add(cmbPhases);
            Controls.Add(nudVels);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txbMaxVel);
            Controls.Add(txbMinVel);
            Name = "CCTControl";
            ((System.ComponentModel.ISupportInitialize)nudVels).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txbMinVel;
        private System.Windows.Forms.TextBox txbMaxVel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudVels;
        private System.Windows.Forms.ComboBox cmbPhases;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txbIniTemp;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txbFinTemp;
    }
}
