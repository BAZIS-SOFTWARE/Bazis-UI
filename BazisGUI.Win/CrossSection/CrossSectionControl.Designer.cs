namespace BazisGUI.Win.CrossSection
{
    partial class CrossSectionControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrossSectionControl));
            btnCreateCross = new System.Windows.Forms.Button();
            chbSelectPoints = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            txbPoint1 = new System.Windows.Forms.TextBox();
            txbPoint2 = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            txbPoint3 = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            rbtXY = new System.Windows.Forms.RadioButton();
            rbtXZ = new System.Windows.Forms.RadioButton();
            rbtYZ = new System.Windows.Forms.RadioButton();
            tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            btnRemoveCross = new System.Windows.Forms.Button();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // btnCreateCross
            // 
            resources.ApplyResources(btnCreateCross, "btnCreateCross");
            btnCreateCross.Name = "btnCreateCross";
            btnCreateCross.UseVisualStyleBackColor = true;
            btnCreateCross.Click += btnCreatePlane_Click;
            // 
            // chbSelectPoints
            // 
            resources.ApplyResources(chbSelectPoints, "chbSelectPoints");
            chbSelectPoints.Name = "chbSelectPoints";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // txbPoint1
            // 
            resources.ApplyResources(txbPoint1, "txbPoint1");
            txbPoint1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tableLayoutPanel3.SetColumnSpan(txbPoint1, 2);
            txbPoint1.Name = "txbPoint1";
            // 
            // txbPoint2
            // 
            resources.ApplyResources(txbPoint2, "txbPoint2");
            txbPoint2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tableLayoutPanel3.SetColumnSpan(txbPoint2, 2);
            txbPoint2.Name = "txbPoint2";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // txbPoint3
            // 
            resources.ApplyResources(txbPoint3, "txbPoint3");
            txbPoint3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tableLayoutPanel3.SetColumnSpan(txbPoint3, 2);
            txbPoint3.Name = "txbPoint3";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // rbtXY
            // 
            resources.ApplyResources(rbtXY, "rbtXY");
            rbtXY.Name = "rbtXY";
            rbtXY.TabStop = true;
            rbtXY.UseVisualStyleBackColor = true;
            rbtXY.CheckedChanged += RbtXY_CheckedChanged;
            // 
            // rbtXZ
            // 
            resources.ApplyResources(rbtXZ, "rbtXZ");
            rbtXZ.Name = "rbtXZ";
            rbtXZ.TabStop = true;
            rbtXZ.UseVisualStyleBackColor = true;
            rbtXZ.CheckedChanged += RbtXZ_CheckedChanged;
            // 
            // rbtYZ
            // 
            resources.ApplyResources(rbtYZ, "rbtYZ");
            rbtYZ.Name = "rbtYZ";
            rbtYZ.TabStop = true;
            rbtYZ.UseVisualStyleBackColor = true;
            rbtYZ.CheckedChanged += RbtYZ_CheckedChanged;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(tableLayoutPanel3, "tableLayoutPanel3");
            tableLayoutPanel3.Controls.Add(rbtYZ, 0, 5);
            tableLayoutPanel3.Controls.Add(rbtXZ, 0, 4);
            tableLayoutPanel3.Controls.Add(rbtXY, 0, 3);
            tableLayoutPanel3.Controls.Add(label3, 0, 2);
            tableLayoutPanel3.Controls.Add(txbPoint3, 1, 2);
            tableLayoutPanel3.Controls.Add(label2, 0, 1);
            tableLayoutPanel3.Controls.Add(txbPoint2, 1, 1);
            tableLayoutPanel3.Controls.Add(label1, 0, 0);
            tableLayoutPanel3.Controls.Add(txbPoint1, 1, 0);
            tableLayoutPanel3.Controls.Add(btnCreateCross, 1, 6);
            tableLayoutPanel3.Controls.Add(chbSelectPoints, 1, 3);
            tableLayoutPanel3.Controls.Add(btnRemoveCross, 2, 6);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // btnRemoveCross
            // 
            resources.ApplyResources(btnRemoveCross, "btnRemoveCross");
            btnRemoveCross.Name = "btnRemoveCross";
            btnRemoveCross.UseVisualStyleBackColor = true;
            btnRemoveCross.Click += btnRemoveCross_Click;
            // 
            // CrossSectionControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel3);
            Name = "CrossSectionControl";
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnCreateCross;
        private System.Windows.Forms.CheckBox chbSelectPoints;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbPoint1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton rbtYZ;
        private System.Windows.Forms.RadioButton rbtXZ;
        private System.Windows.Forms.RadioButton rbtXY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbPoint3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbPoint2;
        private System.Windows.Forms.Button btnRemoveCross;
    }
}
