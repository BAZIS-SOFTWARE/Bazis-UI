using UserControlsEx.Graph;

namespace WinGUI.DataBases.MetallurgyGUI
{
    partial class DiagramControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagramControl));
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            panel1 = new System.Windows.Forms.Panel();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            rbtCCT = new System.Windows.Forms.RadioButton();
            rbtTTT = new System.Windows.Forms.RadioButton();
            btnCalcDiag = new System.Windows.Forms.Button();
            graphContainer = new GraphContainer();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(tableLayoutPanel1, "tableLayoutPanel1");
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(btnCalcDiag, 1, 2);
            tableLayoutPanel1.Controls.Add(graphContainer, 0, 1);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel1
            // 
            resources.ApplyResources(panel1, "panel1");
            tableLayoutPanel1.SetColumnSpan(panel1, 2);
            panel1.Controls.Add(tableLayoutPanel2);
            panel1.Name = "panel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(tableLayoutPanel2, "tableLayoutPanel2");
            tableLayoutPanel2.Controls.Add(rbtCCT, 1, 0);
            tableLayoutPanel2.Controls.Add(rbtTTT, 0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // rbtCCT
            // 
            resources.ApplyResources(rbtCCT, "rbtCCT");
            rbtCCT.Checked = true;
            rbtCCT.Name = "rbtCCT";
            rbtCCT.TabStop = true;
            rbtCCT.UseVisualStyleBackColor = true;
            rbtCCT.CheckedChanged += rbtCCT_CheckedChanged;
            // 
            // rbtTTT
            // 
            resources.ApplyResources(rbtTTT, "rbtTTT");
            rbtTTT.Name = "rbtTTT";
            rbtTTT.UseVisualStyleBackColor = true;
            rbtTTT.CheckedChanged += rbtTTT_CheckedChanged;
            // 
            // btnCalcDiag
            // 
            resources.ApplyResources(btnCalcDiag, "btnCalcDiag");
            btnCalcDiag.Name = "btnCalcDiag";
            btnCalcDiag.UseVisualStyleBackColor = true;
            btnCalcDiag.Click += btnCalcDiag_Click;
            // 
            // graphContainer
            // 
            resources.ApplyResources(graphContainer, "graphContainer");
            graphContainer.Name = "graphContainer";
            // 
            // DiagramControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "DiagramControl";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rbtCCT;
        private System.Windows.Forms.RadioButton rbtTTT;
        private System.Windows.Forms.Button btnCalcDiag;
        private GraphContainer graphContainer;
    }
}
