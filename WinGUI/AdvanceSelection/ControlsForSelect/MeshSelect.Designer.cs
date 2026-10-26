using System.Drawing;
using System.Windows.Forms;
using UserControlsEx;

namespace WinGUI.AdvanceSelection.ControlsForSelect
{
    partial class MeshSelect
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeshSelect));
            generalPanel = new TableLayoutPanel();
            rbtSet = new RadioButton();
            rbtSurface = new RadioButton();
            rbtDirection = new RadioButton();
            lblAngle = new Label();
            txbAngle = new TextBoxEx(components);
            chbChangeDirection = new CheckBox();
            generalPanel.SuspendLayout();
            SuspendLayout();
            // 
            // generalPanel
            // 
            resources.ApplyResources(generalPanel, "generalPanel");
            generalPanel.Controls.Add(rbtSet, 0, 0);
            generalPanel.Controls.Add(rbtSurface, 0, 1);
            generalPanel.Controls.Add(rbtDirection, 0, 2);
            generalPanel.Controls.Add(lblAngle, 0, 3);
            generalPanel.Controls.Add(txbAngle, 1, 3);
            generalPanel.Controls.Add(chbChangeDirection, 0, 4);
            generalPanel.Name = "generalPanel";
            // 
            // rbtSet
            // 
            resources.ApplyResources(rbtSet, "rbtSet");
            rbtSet.Checked = true;
            generalPanel.SetColumnSpan(rbtSet, 2);
            rbtSet.Name = "rbtSet";
            rbtSet.TabStop = true;
            rbtSet.CheckedChanged += Rbt_CheckedChanged;
            // 
            // rbtSurface
            // 
            resources.ApplyResources(rbtSurface, "rbtSurface");
            generalPanel.SetColumnSpan(rbtSurface, 2);
            rbtSurface.Name = "rbtSurface";
            rbtSurface.TabStop = true;
            rbtSurface.CheckedChanged += Rbt_CheckedChanged;
            // 
            // rbtDirection
            // 
            resources.ApplyResources(rbtDirection, "rbtDirection");
            generalPanel.SetColumnSpan(rbtDirection, 2);
            rbtDirection.Name = "rbtDirection";
            rbtDirection.TabStop = true;
            rbtDirection.CheckedChanged += Rbt_CheckedChanged;
            // 
            // lblAngle
            // 
            resources.ApplyResources(lblAngle, "lblAngle");
            lblAngle.Name = "lblAngle";
            // 
            // txbAngle
            // 
            resources.ApplyResources(txbAngle, "txbAngle");
            txbAngle.BorderStyle = BorderStyle.FixedSingle;
            txbAngle.InputType = TXTBoxInputType.Text;
            txbAngle.IsValidating = true;
            txbAngle.Name = "txbAngle";
            txbAngle.UserRegExCheck = null;
            txbAngle.UserRegExCheckErrorMessage = null;
            // 
            // chbChangeDirection
            // 
            resources.ApplyResources(chbChangeDirection, "chbChangeDirection");
            generalPanel.SetColumnSpan(chbChangeDirection, 2);
            chbChangeDirection.Name = "chbChangeDirection";
            chbChangeDirection.UseVisualStyleBackColor = true;
            chbChangeDirection.CheckedChanged += chbChangeDirection_CheckedChanged;
            // 
            // MeshSelect
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(generalPanel);
            Name = "MeshSelect";
            generalPanel.ResumeLayout(false);
            generalPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }




        #endregion

        private TableLayoutPanel generalPanel;

        private RadioButton rbtSet;
        private RadioButton rbtSurface;
        private RadioButton rbtDirection;

        private Label lblAngle;
        private TextBoxEx txbAngle;
        private CheckBox chbChangeDirection;
    }
}
