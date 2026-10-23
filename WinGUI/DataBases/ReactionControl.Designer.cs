namespace WinGUI.DataBases
{
    partial class ReactionControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReactionControl));
            cmbInitialPhase = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            cmbFinalPhase = new System.Windows.Forms.ComboBox();
            dgv = new System.Windows.Forms.DataGridView();
            chbTimeDependent = new System.Windows.Forms.CheckBox();
            btnChangePhaseValue = new System.Windows.Forms.Button();
            btnDelPhaseValue = new System.Windows.Forms.Button();
            btnAddPhaseValue = new System.Windows.Forms.Button();
            txbPhaseValue = new System.Windows.Forms.TextBox();
            cmbPhaseName = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            SuspendLayout();
            // 
            // cmbInitialPhase
            // 
            resources.ApplyResources(cmbInitialPhase, "cmbInitialPhase");
            cmbInitialPhase.FormattingEnabled = true;
            cmbInitialPhase.Name = "cmbInitialPhase";
            cmbInitialPhase.SelectionChangeCommitted += cmbInitialPhase_SelectionChangeCommitted;
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
            // cmbFinalPhase
            // 
            resources.ApplyResources(cmbFinalPhase, "cmbFinalPhase");
            cmbFinalPhase.FormattingEnabled = true;
            cmbFinalPhase.Name = "cmbFinalPhase";
            cmbFinalPhase.SelectionChangeCommitted += cmbFinalPhase_SelectionChangeCommitted;
            // 
            // dgv
            // 
            resources.ApplyResources(dgv, "dgv");
            dgv.AllowUserToOrderColumns = true;
            dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Name = "dgv";
            dgv.ColumnHeaderMouseClick += dgv_ColumnHeaderMouseClick;
            // 
            // chbTimeDependent
            // 
            resources.ApplyResources(chbTimeDependent, "chbTimeDependent");
            chbTimeDependent.Name = "chbTimeDependent";
            chbTimeDependent.UseVisualStyleBackColor = true;
            chbTimeDependent.Click += chbTimeDependent_Click;
            // 
            // btnChangePhaseValue
            // 
            resources.ApplyResources(btnChangePhaseValue, "btnChangePhaseValue");
            btnChangePhaseValue.Name = "btnChangePhaseValue";
            btnChangePhaseValue.UseVisualStyleBackColor = true;
            btnChangePhaseValue.Click += btnChange_Click;
            // 
            // btnDelPhaseValue
            // 
            resources.ApplyResources(btnDelPhaseValue, "btnDelPhaseValue");
            btnDelPhaseValue.Name = "btnDelPhaseValue";
            btnDelPhaseValue.UseVisualStyleBackColor = true;
            btnDelPhaseValue.Click += btnDelTime_Click;
            // 
            // btnAddPhaseValue
            // 
            resources.ApplyResources(btnAddPhaseValue, "btnAddPhaseValue");
            btnAddPhaseValue.Name = "btnAddPhaseValue";
            btnAddPhaseValue.UseVisualStyleBackColor = true;
            btnAddPhaseValue.Click += btnAddTime_Click;
            // 
            // txbPhaseValue
            // 
            resources.ApplyResources(txbPhaseValue, "txbPhaseValue");
            txbPhaseValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbPhaseValue.Name = "txbPhaseValue";
            // 
            // cmbPhaseName
            // 
            resources.ApplyResources(cmbPhaseName, "cmbPhaseName");
            cmbPhaseName.FormattingEnabled = true;
            cmbPhaseName.Items.AddRange(new object[] { resources.GetString("cmbPhaseName.Items"), resources.GetString("cmbPhaseName.Items1"), resources.GetString("cmbPhaseName.Items2") });
            cmbPhaseName.Name = "cmbPhaseName";
            cmbPhaseName.SelectionChangeCommitted += cmbPhaseName_SelectionChangeCommitted;
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // ReactionControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(txbPhaseValue);
            Controls.Add(btnDelPhaseValue);
            Controls.Add(btnAddPhaseValue);
            Controls.Add(chbTimeDependent);
            Controls.Add(dgv);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnChangePhaseValue);
            Controls.Add(cmbPhaseName);
            Controls.Add(cmbFinalPhase);
            Controls.Add(cmbInitialPhase);
            Name = "ReactionControl";
            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ComboBox cmbInitialPhase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbFinalPhase;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.CheckBox chbTimeDependent;
        private System.Windows.Forms.Button btnAddPhaseValue;
        private System.Windows.Forms.Button btnChangePhaseValue;
        private System.Windows.Forms.Button btnDelPhaseValue;
        private System.Windows.Forms.TextBox txbPhaseValue;
        private System.Windows.Forms.ComboBox cmbPhaseName;
        private System.Windows.Forms.Label label3;
    }
}
