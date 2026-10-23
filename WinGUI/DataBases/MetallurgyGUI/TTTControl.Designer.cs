namespace WinGUI.DataBases.MetallurgyGUI
{
    partial class TTTControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TTTControl));
            txbFinTemp = new System.Windows.Forms.TextBox();
            txbIniTemp = new System.Windows.Forms.TextBox();
            nudTemps = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            txbMaxPhase = new System.Windows.Forms.TextBox();
            txbMinPhase = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            cmbPhases = new System.Windows.Forms.ComboBox();
            label5 = new System.Windows.Forms.Label();
            txbMaxTime = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)nudTemps).BeginInit();
            SuspendLayout();
            // 
            // txbFinTemp
            // 
            resources.ApplyResources(txbFinTemp, "txbFinTemp");
            txbFinTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbFinTemp.Name = "txbFinTemp";
            // 
            // txbIniTemp
            // 
            resources.ApplyResources(txbIniTemp, "txbIniTemp");
            txbIniTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbIniTemp.Name = "txbIniTemp";
            // 
            // nudTemps
            // 
            resources.ApplyResources(nudTemps, "nudTemps");
            nudTemps.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            nudTemps.Name = "nudTemps";
            nudTemps.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // txbMaxPhase
            // 
            resources.ApplyResources(txbMaxPhase, "txbMaxPhase");
            txbMaxPhase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbMaxPhase.Name = "txbMaxPhase";
            // 
            // txbMinPhase
            // 
            resources.ApplyResources(txbMinPhase, "txbMinPhase");
            txbMinPhase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbMinPhase.Name = "txbMinPhase";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // cmbPhases
            // 
            resources.ApplyResources(cmbPhases, "cmbPhases");
            cmbPhases.FormattingEnabled = true;
            cmbPhases.Name = "cmbPhases";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // txbMaxTime
            // 
            resources.ApplyResources(txbMaxTime, "txbMaxTime");
            txbMaxTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbMaxTime.Name = "txbMaxTime";
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            // 
            // TTTControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(txbMaxTime);
            Controls.Add(cmbPhases);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(txbFinTemp);
            Controls.Add(txbIniTemp);
            Controls.Add(nudTemps);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txbMaxPhase);
            Controls.Add(txbMinPhase);
            Name = "TTTControl";
            ((System.ComponentModel.ISupportInitialize)nudTemps).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txbFinTemp;
        private System.Windows.Forms.TextBox txbIniTemp;
        private System.Windows.Forms.NumericUpDown nudTemps;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbMaxPhase;
        private System.Windows.Forms.TextBox txbMinPhase;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbPhases;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txbMaxTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}
