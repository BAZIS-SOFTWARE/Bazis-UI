namespace BazisGUI.Win
{
    partial class AboutLicenseControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutLicenseControl));
            label1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            lblCompanyName = new System.Windows.Forms.Label();
            lblKeyInfo = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            lblServerAdress = new System.Windows.Forms.Label();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // lblCompanyName
            // 
            resources.ApplyResources(lblCompanyName, "lblCompanyName");
            lblCompanyName.Name = "lblCompanyName";
            // 
            // lblKeyInfo
            // 
            resources.ApplyResources(lblKeyInfo, "lblKeyInfo");
            lblKeyInfo.Name = "lblKeyInfo";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // lblServerAdress
            // 
            resources.ApplyResources(lblServerAdress, "lblServerAdress");
            lblServerAdress.Name = "lblServerAdress";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(tableLayoutPanel1, "tableLayoutPanel1");
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(lblServerAdress, 1, 2);
            tableLayoutPanel1.Controls.Add(lblCompanyName, 1, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 2);
            tableLayoutPanel1.Controls.Add(lblKeyInfo, 1, 1);
            tableLayoutPanel1.Controls.Add(label3, 0, 1);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // AboutLicenseControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "AboutLicenseControl";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.Label lblKeyInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblServerAdress;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}