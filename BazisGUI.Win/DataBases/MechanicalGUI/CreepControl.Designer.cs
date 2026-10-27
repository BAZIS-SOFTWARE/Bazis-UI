using UserControlsEx.Graph;

namespace BazisGUI.Win.DataBases.MechanicalGUI
{
    partial class CreepControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreepControl));
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            graphContainer1 = new GraphContainer();
            panel1 = new System.Windows.Forms.Panel();
            label6 = new System.Windows.Forms.Label();
            cmbPhases = new System.Windows.Forms.ComboBox();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            txbTemp = new System.Windows.Forms.TextBox();
            txbTime = new System.Windows.Forms.TextBox();
            txbForce = new System.Windows.Forms.TextBox();
            txbLength = new System.Windows.Forms.TextBox();
            txbDiam = new System.Windows.Forms.TextBox();
            btnCalc = new System.Windows.Forms.Button();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(tableLayoutPanel1, "tableLayoutPanel1");
            tableLayoutPanel1.Controls.Add(graphContainer1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel1, 1, 0);
            tableLayoutPanel1.Controls.Add(btnCalc, 1, 1);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // graphContainer1
            // 
            resources.ApplyResources(graphContainer1, "graphContainer1");
            graphContainer1.Name = "graphContainer1";
            tableLayoutPanel1.SetRowSpan(graphContainer1, 2);
            // 
            // panel1
            // 
            resources.ApplyResources(panel1, "panel1");
            panel1.Controls.Add(label6);
            panel1.Controls.Add(cmbPhases);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(txbTemp);
            panel1.Controls.Add(txbTime);
            panel1.Controls.Add(txbForce);
            panel1.Controls.Add(txbLength);
            panel1.Controls.Add(txbDiam);
            panel1.Name = "panel1";
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
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
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
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
            // txbTemp
            // 
            resources.ApplyResources(txbTemp, "txbTemp");
            txbTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbTemp.Name = "txbTemp";
            // 
            // txbTime
            // 
            resources.ApplyResources(txbTime, "txbTime");
            txbTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbTime.Name = "txbTime";
            // 
            // txbForce
            // 
            resources.ApplyResources(txbForce, "txbForce");
            txbForce.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbForce.Name = "txbForce";
            // 
            // txbLength
            // 
            resources.ApplyResources(txbLength, "txbLength");
            txbLength.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbLength.Name = "txbLength";
            // 
            // txbDiam
            // 
            resources.ApplyResources(txbDiam, "txbDiam");
            txbDiam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txbDiam.Name = "txbDiam";
            // 
            // btnCalc
            // 
            resources.ApplyResources(btnCalc, "btnCalc");
            btnCalc.Name = "btnCalc";
            btnCalc.UseVisualStyleBackColor = true;
            btnCalc.Click += btnCalc_Click;
            // 
            // CreepControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "CreepControl";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private GraphContainer graphContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbTime;
        private System.Windows.Forms.TextBox txbForce;
        private System.Windows.Forms.TextBox txbLength;
        private System.Windows.Forms.TextBox txbDiam;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txbTemp;
        private System.Windows.Forms.ComboBox cmbPhases;
        private System.Windows.Forms.Label label6;
    }
}
