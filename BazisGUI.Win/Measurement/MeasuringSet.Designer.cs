namespace BazisGUI.Win.Measurement
{
    partial class MeasuringSet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeasuringSet));
            rbtnDistance = new System.Windows.Forms.RadioButton();
            rbtnPath = new System.Windows.Forms.RadioButton();
            rbtSquare = new System.Windows.Forms.RadioButton();
            rbtVolume = new System.Windows.Forms.RadioButton();
            cmbMeasureObjects = new System.Windows.Forms.ComboBox();
            btnMeasure = new System.Windows.Forms.Button();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            panel1 = new System.Windows.Forms.Panel();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // rbtnDistance
            // 
            resources.ApplyResources(rbtnDistance, "rbtnDistance");
            rbtnDistance.Name = "rbtnDistance";
            rbtnDistance.UseVisualStyleBackColor = true;
            rbtnDistance.Click += Rbtn_Click;
            // 
            // rbtnPath
            // 
            resources.ApplyResources(rbtnPath, "rbtnPath");
            rbtnPath.Name = "rbtnPath";
            rbtnPath.UseVisualStyleBackColor = true;
            rbtnPath.Click += Rbtn_Click;
            // 
            // rbtSquare
            // 
            resources.ApplyResources(rbtSquare, "rbtSquare");
            rbtSquare.Name = "rbtSquare";
            rbtSquare.UseVisualStyleBackColor = true;
            rbtSquare.Click += Rbtn_Click;
            // 
            // rbtVolume
            // 
            resources.ApplyResources(rbtVolume, "rbtVolume");
            rbtVolume.Name = "rbtVolume";
            rbtVolume.UseVisualStyleBackColor = true;
            rbtVolume.Click += Rbtn_Click;
            // 
            // cmbMeasureObjects
            // 
            resources.ApplyResources(cmbMeasureObjects, "cmbMeasureObjects");
            cmbMeasureObjects.FormattingEnabled = true;
            cmbMeasureObjects.Items.AddRange(new object[] { resources.GetString("cmbMeasureObjects.Items"), resources.GetString("cmbMeasureObjects.Items1") });
            cmbMeasureObjects.Name = "cmbMeasureObjects";
            cmbMeasureObjects.SelectedIndexChanged += cmbMeasureObjects_SelectedIndexChanged;
            // 
            // btnMeasure
            // 
            resources.ApplyResources(btnMeasure, "btnMeasure");
            btnMeasure.Name = "btnMeasure";
            btnMeasure.UseVisualStyleBackColor = true;
            btnMeasure.Click += btnMeasure_Click;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(tableLayoutPanel1, "tableLayoutPanel1");
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(cmbMeasureObjects, 1, 0);
            tableLayoutPanel1.Controls.Add(btnMeasure, 1, 1);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel1
            // 
            resources.ApplyResources(panel1, "panel1");
            panel1.Controls.Add(rbtVolume);
            panel1.Controls.Add(rbtnDistance);
            panel1.Controls.Add(rbtSquare);
            panel1.Controls.Add(rbtnPath);
            panel1.Name = "panel1";
            // 
            // MeasuringSet
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "MeasuringSet";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtnDistance;
        private System.Windows.Forms.RadioButton rbtnPath;
        private System.Windows.Forms.RadioButton rbtSquare;
        private System.Windows.Forms.RadioButton rbtVolume;
        private System.Windows.Forms.ComboBox cmbMeasureObjects;
        private System.Windows.Forms.Button btnMeasure;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
    }
}