using System.Windows.Forms;

namespace WinGUI.AdvanceSelection.ControlsForSelect
{
    partial class GeomSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeomSelect));
            generalPanel = new TableLayoutPanel();
            rbtVolume = new RadioButton();
            rbtSurface = new RadioButton();
            rbtCurve = new RadioButton();
            generalPanel.SuspendLayout();
            SuspendLayout();
            // 
            // generalPanel
            // 
            resources.ApplyResources(generalPanel, "generalPanel");
            generalPanel.Controls.Add(rbtVolume);
            generalPanel.Controls.Add(rbtSurface);
            generalPanel.Controls.Add(rbtCurve);
            generalPanel.Name = "generalPanel";
            // 
            // rbtVolume
            // 
            resources.ApplyResources(rbtVolume, "rbtVolume");
            rbtVolume.Checked = true;
            rbtVolume.ForeColor = System.Drawing.SystemColors.ControlText;
            rbtVolume.Name = "rbtVolume";
            rbtVolume.AccessibleName = "GeomSelect.Volumes";
            rbtVolume.TabStop = true;
            rbtVolume.Tag = "";
            // 
            // rbtSurface
            // 
            resources.ApplyResources(rbtSurface, "rbtSurface");
            rbtSurface.Name = "rbtSurface";
            rbtSurface.AccessibleName = "GeomSelect.Surfaces";
            rbtSurface.TabStop = true;
            // 
            // rbtCurve
            // 
            resources.ApplyResources(rbtCurve, "rbtCurve");
            rbtCurve.Name = "rbtCurve";
            rbtCurve.AccessibleName = "GeomSelect.Curves";
            rbtCurve.TabStop = true;
            // 
            // GeomSelect
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(generalPanel);
            Name = "GeomSelect";
            generalPanel.ResumeLayout(false);
            generalPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel generalPanel;

        private RadioButton rbtVolume;
        private RadioButton rbtSurface;
        private RadioButton rbtCurve;
    }
}
