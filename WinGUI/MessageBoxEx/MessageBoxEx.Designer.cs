namespace WinGUI.MessageBoxEx
{
    partial class MessageBoxEx
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageBoxEx));
            message = new System.Windows.Forms.Label();
            title = new System.Windows.Forms.Label();
            messagePanel = new System.Windows.Forms.Panel();
            messagePanel.SuspendLayout();
            SuspendLayout();
            // 
            // message
            // 
            resources.ApplyResources(message, "message");
            message.Name = "message";
            // 
            // title
            // 
            resources.ApplyResources(title, "title");
            title.BackColor = System.Drawing.Color.Transparent;
            title.Name = "title";
            // 
            // messagePanel
            // 
            resources.ApplyResources(messagePanel, "messagePanel");
            messagePanel.BackColor = System.Drawing.SystemColors.Control;
            messagePanel.Controls.Add(message);
            messagePanel.Name = "messagePanel";
            // 
            // MessageBoxEx
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Controls.Add(messagePanel);
            Controls.Add(title);
            Name = "MessageBoxEx";
            Paint += MessageBoxEx_Paint;
            messagePanel.ResumeLayout(false);
            messagePanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label message;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Panel messagePanel;
    }
}
