using UserControlsEx;

namespace WinGUI.SettingsControls
{
    partial class SettingsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsControl));
            lblSolverPath = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            chbLighting = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            chbBackRibbers = new System.Windows.Forms.CheckBox();
            btnBackGroundColor = new System.Windows.Forms.Button();
            panelBackGroundColor = new System.Windows.Forms.Panel();
            clslLigthingIntensity = new ColorSlider();
            label2 = new System.Windows.Forms.Label();
            clslTransparency = new ColorSlider();
            chbTransparency = new System.Windows.Forms.CheckBox();
            tabControlEx1 = new TabControlEx();
            tbScene = new System.Windows.Forms.TabPage();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            lightingControl = new LightingControl();
            chbOrtoProjection = new System.Windows.Forms.CheckBox();
            label5 = new System.Windows.Forms.Label();
            tbObjects = new System.Windows.Forms.TabPage();
            tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            btnSelectNodeColor = new System.Windows.Forms.Button();
            btnSelect2DElemColor = new System.Windows.Forms.Button();
            btnSelectColor = new System.Windows.Forms.Button();
            btnSelectGroupColor = new System.Windows.Forms.Button();
            pnlSelectionObjsColor = new System.Windows.Forms.Panel();
            pnlSelectionGroupColor = new System.Windows.Forms.Panel();
            btnSelect3DElemColor = new System.Windows.Forms.Button();
            pnl3DElemColor = new System.Windows.Forms.Panel();
            pnl2DElemColor = new System.Windows.Forms.Panel();
            pnlNodeColor = new System.Windows.Forms.Panel();
            tbSolver = new System.Windows.Forms.TabPage();
            tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            label3 = new System.Windows.Forms.Label();
            tabPage1 = new System.Windows.Forms.TabPage();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            lblLanguage = new System.Windows.Forms.Label();
            cmbLanguage = new System.Windows.Forms.ComboBox();
            tabControlEx1.SuspendLayout();
            tbScene.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tbObjects.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tbSolver.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tabPage1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // lblSolverPath
            // 
            resources.ApplyResources(lblSolverPath, "lblSolverPath");
            lblSolverPath.Name = "lblSolverPath";
            lblSolverPath.Click += btnSetSolverPath_Click;
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // chbLighting
            // 
            resources.ApplyResources(chbLighting, "chbLighting");
            chbLighting.Name = "chbLighting";
            chbLighting.UseVisualStyleBackColor = true;
            chbLighting.Click += chbLighting_Click;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // chbBackRibbers
            // 
            resources.ApplyResources(chbBackRibbers, "chbBackRibbers");
            chbBackRibbers.Name = "chbBackRibbers";
            chbBackRibbers.UseVisualStyleBackColor = true;
            chbBackRibbers.Click += chbBackRibbers_Click;
            // 
            // btnBackGroundColor
            // 
            resources.ApplyResources(btnBackGroundColor, "btnBackGroundColor");
            btnBackGroundColor.Name = "btnBackGroundColor";
            btnBackGroundColor.UseVisualStyleBackColor = true;
            btnBackGroundColor.Click += btnBackGroundColor_Click;
            // 
            // panelBackGroundColor
            // 
            resources.ApplyResources(panelBackGroundColor, "panelBackGroundColor");
            panelBackGroundColor.BackColor = System.Drawing.Color.White;
            panelBackGroundColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelBackGroundColor.Name = "panelBackGroundColor";
            // 
            // clslLigthingIntensity
            // 
            resources.ApplyResources(clslLigthingIntensity, "clslLigthingIntensity");
            clslLigthingIntensity.BackColor = System.Drawing.Color.Transparent;
            clslLigthingIntensity.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            clslLigthingIntensity.LargeChange = 5U;
            clslLigthingIntensity.Name = "clslLigthingIntensity";
            clslLigthingIntensity.ShowTextValue = true;
            clslLigthingIntensity.SmallChange = 1U;
            clslLigthingIntensity.TextValueColor = System.Drawing.Color.Black;
            clslLigthingIntensity.ThumbRoundRectSize = new System.Drawing.Size(8, 8);
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // clslTransparency
            // 
            resources.ApplyResources(clslTransparency, "clslTransparency");
            clslTransparency.BackColor = System.Drawing.Color.Transparent;
            clslTransparency.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            clslTransparency.LargeChange = 5U;
            clslTransparency.Name = "clslTransparency";
            clslTransparency.ShowTextValue = true;
            clslTransparency.SmallChange = 1U;
            clslTransparency.TextValueColor = System.Drawing.Color.Black;
            clslTransparency.ThumbRoundRectSize = new System.Drawing.Size(8, 8);
            // 
            // chbTransparency
            // 
            resources.ApplyResources(chbTransparency, "chbTransparency");
            chbTransparency.Name = "chbTransparency";
            chbTransparency.UseVisualStyleBackColor = true;
            chbTransparency.Click += chbTransparency_Click;
            // 
            // tabControlEx1
            // 
            tabControlEx1.Controls.Add(tbScene);
            tabControlEx1.Controls.Add(tbObjects);
            tabControlEx1.Controls.Add(tbSolver);
            tabControlEx1.Controls.Add(tabPage1);
            resources.ApplyResources(tabControlEx1, "tabControlEx1");
            tabControlEx1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            tabControlEx1.FontColor = System.Drawing.Color.Black;
            tabControlEx1.Name = "tabControlEx1";
            tabControlEx1.SelectColor = System.Drawing.SystemColors.Control;
            tabControlEx1.SelectedIndex = 0;
            tabControlEx1.UnSelectColor = System.Drawing.Color.LightGray;
            // 
            // tbScene
            // 
            tbScene.Controls.Add(tableLayoutPanel1);
            resources.ApplyResources(tbScene, "tbScene");
            tbScene.Name = "tbScene";
            tbScene.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(tableLayoutPanel1, "tableLayoutPanel1");
            tableLayoutPanel1.Controls.Add(label4, 0, 5);
            tableLayoutPanel1.Controls.Add(label2, 0, 2);
            tableLayoutPanel1.Controls.Add(chbTransparency, 1, 5);
            tableLayoutPanel1.Controls.Add(lightingControl, 0, 3);
            tableLayoutPanel1.Controls.Add(chbLighting, 1, 2);
            tableLayoutPanel1.Controls.Add(btnBackGroundColor, 0, 0);
            tableLayoutPanel1.Controls.Add(panelBackGroundColor, 1, 0);
            tableLayoutPanel1.Controls.Add(chbBackRibbers, 1, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 1);
            tableLayoutPanel1.Controls.Add(clslLigthingIntensity, 0, 4);
            tableLayoutPanel1.Controls.Add(clslTransparency, 0, 6);
            tableLayoutPanel1.Controls.Add(chbOrtoProjection, 1, 7);
            tableLayoutPanel1.Controls.Add(label5, 0, 7);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // lightingControl
            // 
            lightingControl.BackColor = System.Drawing.SystemColors.ControlLightLight;
            lightingControl.BallPosition = new System.Drawing.Point(0, 0);
            lightingControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tableLayoutPanel1.SetColumnSpan(lightingControl, 2);
            resources.ApplyResources(lightingControl, "lightingControl");
            lightingControl.Name = "lightingControl";
            // 
            // chbOrtoProjection
            // 
            resources.ApplyResources(chbOrtoProjection, "chbOrtoProjection");
            chbOrtoProjection.Name = "chbOrtoProjection";
            chbOrtoProjection.UseVisualStyleBackColor = true;
            chbOrtoProjection.Click += chbOrtoProjection_Click;
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // tbObjects
            // 
            tbObjects.Controls.Add(tableLayoutPanel4);
            resources.ApplyResources(tbObjects, "tbObjects");
            tbObjects.Name = "tbObjects";
            tbObjects.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(tableLayoutPanel4, "tableLayoutPanel4");
            tableLayoutPanel4.Controls.Add(btnSelectNodeColor, 0, 4);
            tableLayoutPanel4.Controls.Add(btnSelect2DElemColor, 0, 3);
            tableLayoutPanel4.Controls.Add(btnSelectColor, 0, 0);
            tableLayoutPanel4.Controls.Add(btnSelectGroupColor, 0, 1);
            tableLayoutPanel4.Controls.Add(pnlSelectionObjsColor, 1, 0);
            tableLayoutPanel4.Controls.Add(pnlSelectionGroupColor, 1, 1);
            tableLayoutPanel4.Controls.Add(btnSelect3DElemColor, 0, 2);
            tableLayoutPanel4.Controls.Add(pnl3DElemColor, 1, 2);
            tableLayoutPanel4.Controls.Add(pnl2DElemColor, 1, 3);
            tableLayoutPanel4.Controls.Add(pnlNodeColor, 1, 4);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // btnSelectNodeColor
            // 
            resources.ApplyResources(btnSelectNodeColor, "btnSelectNodeColor");
            btnSelectNodeColor.Name = "btnSelectNodeColor";
            btnSelectNodeColor.UseVisualStyleBackColor = true;
            btnSelectNodeColor.Click += btnSelectNodeColor_Click;
            // 
            // btnSelect2DElemColor
            // 
            resources.ApplyResources(btnSelect2DElemColor, "btnSelect2DElemColor");
            btnSelect2DElemColor.Name = "btnSelect2DElemColor";
            btnSelect2DElemColor.UseVisualStyleBackColor = true;
            btnSelect2DElemColor.Click += btnSelect2DElemColor_Click;
            // 
            // btnSelectColor
            // 
            resources.ApplyResources(btnSelectColor, "btnSelectColor");
            btnSelectColor.Name = "btnSelectColor";
            btnSelectColor.UseVisualStyleBackColor = true;
            btnSelectColor.Click += btnSelectObjectColor_Click;
            // 
            // btnSelectGroupColor
            // 
            resources.ApplyResources(btnSelectGroupColor, "btnSelectGroupColor");
            btnSelectGroupColor.Name = "btnSelectGroupColor";
            btnSelectGroupColor.UseVisualStyleBackColor = true;
            btnSelectGroupColor.Click += btnSelectGroupColor_Click;
            // 
            // pnlSelectionObjsColor
            // 
            resources.ApplyResources(pnlSelectionObjsColor, "pnlSelectionObjsColor");
            pnlSelectionObjsColor.BackColor = System.Drawing.Color.LawnGreen;
            pnlSelectionObjsColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlSelectionObjsColor.Name = "pnlSelectionObjsColor";
            // 
            // pnlSelectionGroupColor
            // 
            resources.ApplyResources(pnlSelectionGroupColor, "pnlSelectionGroupColor");
            pnlSelectionGroupColor.BackColor = System.Drawing.Color.Yellow;
            pnlSelectionGroupColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlSelectionGroupColor.Name = "pnlSelectionGroupColor";
            // 
            // btnSelect3DElemColor
            // 
            resources.ApplyResources(btnSelect3DElemColor, "btnSelect3DElemColor");
            btnSelect3DElemColor.Name = "btnSelect3DElemColor";
            btnSelect3DElemColor.UseVisualStyleBackColor = true;
            btnSelect3DElemColor.Click += btnSelect3DElemColor_Click;
            // 
            // pnl3DElemColor
            // 
            resources.ApplyResources(pnl3DElemColor, "pnl3DElemColor");
            pnl3DElemColor.BackColor = System.Drawing.Color.Yellow;
            pnl3DElemColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnl3DElemColor.Name = "pnl3DElemColor";
            // 
            // pnl2DElemColor
            // 
            resources.ApplyResources(pnl2DElemColor, "pnl2DElemColor");
            pnl2DElemColor.BackColor = System.Drawing.Color.Yellow;
            pnl2DElemColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnl2DElemColor.Name = "pnl2DElemColor";
            // 
            // pnlNodeColor
            // 
            resources.ApplyResources(pnlNodeColor, "pnlNodeColor");
            pnlNodeColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlNodeColor.Name = "pnlNodeColor";
            // 
            // tbSolver
            // 
            tbSolver.Controls.Add(tableLayoutPanel5);
            resources.ApplyResources(tbSolver, "tbSolver");
            tbSolver.Name = "tbSolver";
            tbSolver.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(tableLayoutPanel5, "tableLayoutPanel5");
            tableLayoutPanel5.Controls.Add(lblSolverPath, 1, 0);
            tableLayoutPanel5.Controls.Add(label3, 0, 0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(tableLayoutPanel2);
            resources.ApplyResources(tabPage1, "tabPage1");
            tabPage1.Name = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(tableLayoutPanel2, "tableLayoutPanel2");
            tableLayoutPanel2.Controls.Add(lblLanguage, 0, 0);
            tableLayoutPanel2.Controls.Add(cmbLanguage, 1, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // lblLanguage
            // 
            resources.ApplyResources(lblLanguage, "lblLanguage");
            lblLanguage.Name = "lblLanguage";
            // 
            // cmbLanguage
            // 
            resources.ApplyResources(cmbLanguage, "cmbLanguage");
            cmbLanguage.FormattingEnabled = true;
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.TextChanged += cmbLanguage_TextChanged;
            // 
            // SettingsControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tabControlEx1);
            Name = "SettingsControl";
            tabControlEx1.ResumeLayout(false);
            tbScene.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tbObjects.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tbSolver.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            tabPage1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chbLighting;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbBackRibbers;
        private System.Windows.Forms.Button btnBackGroundColor;
        private System.Windows.Forms.Panel panelBackGroundColor;
        private LightingControl lightingControl;
        private ColorSlider clslLigthingIntensity;
        private ColorSlider clslTransparency;
        private System.Windows.Forms.CheckBox chbTransparency;
        private System.Windows.Forms.Label lblSolverPath;
        private System.Windows.Forms.Label label2;
        private TabControlEx tabControlEx1;
        private System.Windows.Forms.TabPage tbScene;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabPage tbObjects;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnSelectColor;
        private System.Windows.Forms.Button btnSelectGroupColor;
        private System.Windows.Forms.Panel pnlSelectionObjsColor;
        private System.Windows.Forms.Panel pnlSelectionGroupColor;
        private System.Windows.Forms.Button btnSelectNodeColor;
        private System.Windows.Forms.Button btnSelect2DElemColor;
        private System.Windows.Forms.Button btnSelect3DElemColor;
        private System.Windows.Forms.Panel pnl2DElemColor;
        private System.Windows.Forms.Panel pnl3DElemColor;
        private System.Windows.Forms.Panel pnlNodeColor;
        private System.Windows.Forms.TabPage tbSolver;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chbOrtoProjection;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.ComboBox cmbLanguage;
    }
}
