using UserControlsEx.Graph;

namespace WinGUI.DataBases.MechanicalGUI
{
    partial class HardeningControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HardeningControl));
            graphContainer = new GraphContainer();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            btnCalc = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            chbTemp = new System.Windows.Forms.CheckBox();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            cmbPhases = new System.Windows.Forms.ComboBox();
            txbTemp = new System.Windows.Forms.TextBox();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // graphContainer
            // 
            resources.ApplyResources(graphContainer, "graphContainer");
            graphContainer.Name = "graphContainer";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(tableLayoutPanel1, "tableLayoutPanel1");
            tableLayoutPanel1.Controls.Add(graphContainer, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(tableLayoutPanel2, "tableLayoutPanel2");
            tableLayoutPanel2.Controls.Add(btnCalc, 0, 1);
            tableLayoutPanel2.Controls.Add(panel1, 0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // btnCalc
            // 
            resources.ApplyResources(btnCalc, "btnCalc");
            btnCalc.Name = "btnCalc";
            btnCalc.UseVisualStyleBackColor = true;
            btnCalc.Click += btnCalc_Click;
            // 
            // panel1
            // 
            resources.ApplyResources(panel1, "panel1");
            panel1.Controls.Add(chbTemp);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(cmbPhases);
            panel1.Controls.Add(txbTemp);
            panel1.Name = "panel1";
            // 
            // chbTemp
            // 
            resources.ApplyResources(chbTemp, "chbTemp");
            chbTemp.Name = "chbTemp";
            chbTemp.UseVisualStyleBackColor = true;
            chbTemp.CheckedChanged += chbTemp_CheckedChanged;
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
            // cmbPhases
            // 
            resources.ApplyResources(cmbPhases, "cmbPhases");
            cmbPhases.FormattingEnabled = true;
            cmbPhases.Name = "cmbPhases";
            // 
            // txbTemp
            // 
            resources.ApplyResources(txbTemp, "txbTemp");
            txbTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbTemp.Name = "txbTemp";
            // 
            // HardeningControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "HardeningControl";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GraphContainer graphContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPhases;
        private System.Windows.Forms.TextBox txbTemp;
        private System.Windows.Forms.CheckBox chbTemp;
    }
}
