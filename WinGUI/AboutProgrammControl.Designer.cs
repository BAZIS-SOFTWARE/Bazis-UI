namespace WinGUI
{
    partial class AboutProgrammControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutProgrammControl));
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            label1 = new System.Windows.Forms.Label();
            lblVersion = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            richTextBox = new System.Windows.Forms.RichTextBox();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(tableLayoutPanel, "tableLayoutPanel");
            tableLayoutPanel.Controls.Add(label1, 0, 0);
            tableLayoutPanel.Controls.Add(lblVersion, 0, 1);
            tableLayoutPanel.Controls.Add(pictureBox1, 1, 0);
            tableLayoutPanel.Controls.Add(richTextBox, 0, 2);
            tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // lblVersion
            // 
            resources.ApplyResources(lblVersion, "lblVersion");
            lblVersion.Name = "lblVersion";
            // 
            // pictureBox1
            // 
            tableLayoutPanel.SetColumnSpan(pictureBox1, 2);
            resources.ApplyResources(pictureBox1, "pictureBox1");
            pictureBox1.Name = "pictureBox1";
            tableLayoutPanel.SetRowSpan(pictureBox1, 4);
            pictureBox1.TabStop = false;
            // 
            // richTextBox
            // 
            richTextBox.BackColor = System.Drawing.SystemColors.Control;
            richTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(richTextBox, "richTextBox");
            richTextBox.Name = "richTextBox";
            tableLayoutPanel.SetRowSpan(richTextBox, 2);
            // 
            // AboutProgrammControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel);
            Name = "AboutProgrammControl";
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
