using UserControlsEx;

namespace WinGUI.Console
{
    partial class ConsoleControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleControl));
            tlscOut = new System.Windows.Forms.ToolStripContainer();
            rtxbField = new System.Windows.Forms.RichTextBox();
            toolStripEx1 = new ToolStripEx();
            spbDictionary = new System.Windows.Forms.ToolStripButton();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            btnStartMacro = new System.Windows.Forms.ToolStripButton();
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            toolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem16 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            toolStripMenuItem32 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem33 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem34 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem35 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem36 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem37 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem38 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem39 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem47 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem48 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem49 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem50 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem51 = new System.Windows.Forms.ToolStripMenuItem();
            изменитьКоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            переместитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            векторabcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            повернутьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            осьxyzУголToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem54 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem55 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem56 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem57 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem58 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem59 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem60 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem61 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem62 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem63 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem64 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem65 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem66 = new System.Windows.Forms.ToolStripMenuItem();
            tlscOut.ContentPanel.SuspendLayout();
            tlscOut.RightToolStripPanel.SuspendLayout();
            tlscOut.SuspendLayout();
            toolStripEx1.SuspendLayout();
            SuspendLayout();
            // 
            // tlscOut
            // 
            tlscOut.BottomToolStripPanelVisible = false;
            // 
            // tlscOut.ContentPanel
            // 
            tlscOut.ContentPanel.Controls.Add(rtxbField);
            resources.ApplyResources(tlscOut.ContentPanel, "tlscOut.ContentPanel");
            resources.ApplyResources(tlscOut, "tlscOut");
            tlscOut.LeftToolStripPanelVisible = false;
            tlscOut.Name = "tlscOut";
            // 
            // tlscOut.RightToolStripPanel
            // 
            tlscOut.RightToolStripPanel.Controls.Add(toolStripEx1);
            tlscOut.RightToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // tlscOut.TopToolStripPanel
            // 
            resources.ApplyResources(tlscOut.TopToolStripPanel, "tlscOut.TopToolStripPanel");
            tlscOut.TopToolStripPanelVisible = false;
            // 
            // rtxbField
            // 
            rtxbField.BackColor = System.Drawing.SystemColors.Control;
            rtxbField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(rtxbField, "rtxbField");
            rtxbField.Name = "rtxbField";
            rtxbField.KeyDown += KeyDownEventHadler;
            // 
            // toolStripEx1
            // 
            toolStripEx1.BackGroundColor = System.Drawing.Color.Gainsboro;
            resources.ApplyResources(toolStripEx1, "toolStripEx1");
            toolStripEx1.FrameColor = System.Drawing.Color.DarkGray;
            toolStripEx1.GeneralFrame = true;
            toolStripEx1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripEx1.IconLocation = new System.Drawing.Point(3, 3);
            toolStripEx1.ImageRectangleSize = new System.Drawing.Point(16, 16);
            toolStripEx1.ItemBackGroundColor = System.Drawing.Color.FromArgb(228, 228, 228);
            toolStripEx1.ItemFrame = true;
            toolStripEx1.ItemLocation = new System.Drawing.Point(1, 1);
            toolStripEx1.ItemPressColor = System.Drawing.Color.Black;
            toolStripEx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { spbDictionary, toolStripButton1, toolStripButton2, btnStartMacro });
            toolStripEx1.ItemSelectColor = System.Drawing.Color.Gray;
            toolStripEx1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            toolStripEx1.Name = "toolStripEx1";
            toolStripEx1.SplitButtonClickWidth = 13;
            toolStripEx1.SplitButtonHeight = 40;
            toolStripEx1.SplitButtonTriangleSize = 6;
            toolStripEx1.Stretch = true;
            toolStripEx1.TextBoxFrame = true;
            toolStripEx1.TextBoxHeight = 0;
            // 
            // spbDictionary
            // 
            resources.ApplyResources(spbDictionary, "spbDictionary");
            spbDictionary.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            spbDictionary.Image = Properties.Resources.helpContent;
            spbDictionary.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            spbDictionary.Name = "spbDictionary";
            spbDictionary.Click += btnDictionary_Click;
            // 
            // toolStripButton1
            // 
            resources.ApplyResources(toolStripButton1, "toolStripButton1");
            toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Click += btnBackGroundInfo_Click;
            // 
            // toolStripButton2
            // 
            resources.ApplyResources(toolStripButton2, "toolStripButton2");
            toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Click += ClearAll_Click;
            // 
            // btnStartMacro
            // 
            resources.ApplyResources(btnStartMacro, "btnStartMacro");
            btnStartMacro.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnStartMacro.Name = "btnStartMacro";
            btnStartMacro.Click += btnStartMacro_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // toolStripMenuItem14
            // 
            toolStripMenuItem14.Name = "toolStripMenuItem14";
            resources.ApplyResources(toolStripMenuItem14, "toolStripMenuItem14");
            // 
            // toolStripMenuItem15
            // 
            toolStripMenuItem15.Name = "toolStripMenuItem15";
            resources.ApplyResources(toolStripMenuItem15, "toolStripMenuItem15");
            // 
            // toolStripMenuItem16
            // 
            toolStripMenuItem16.Name = "toolStripMenuItem16";
            resources.ApplyResources(toolStripMenuItem16, "toolStripMenuItem16");
            // 
            // toolStripTextBox1
            // 
            toolStripTextBox1.Name = "toolStripTextBox1";
            resources.ApplyResources(toolStripTextBox1, "toolStripTextBox1");
            // 
            // toolStripMenuItem32
            // 
            toolStripMenuItem32.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem33 });
            resources.ApplyResources(toolStripMenuItem32, "toolStripMenuItem32");
            toolStripMenuItem32.Name = "toolStripMenuItem32";
            // 
            // toolStripMenuItem33
            // 
            toolStripMenuItem33.Name = "toolStripMenuItem33";
            resources.ApplyResources(toolStripMenuItem33, "toolStripMenuItem33");
            // 
            // toolStripMenuItem34
            // 
            toolStripMenuItem34.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem35 });
            resources.ApplyResources(toolStripMenuItem34, "toolStripMenuItem34");
            toolStripMenuItem34.Name = "toolStripMenuItem34";
            // 
            // toolStripMenuItem35
            // 
            toolStripMenuItem35.Name = "toolStripMenuItem35";
            resources.ApplyResources(toolStripMenuItem35, "toolStripMenuItem35");
            // 
            // toolStripMenuItem36
            // 
            resources.ApplyResources(toolStripMenuItem36, "toolStripMenuItem36");
            toolStripMenuItem36.Name = "toolStripMenuItem36";
            // 
            // toolStripMenuItem37
            // 
            resources.ApplyResources(toolStripMenuItem37, "toolStripMenuItem37");
            toolStripMenuItem37.Name = "toolStripMenuItem37";
            // 
            // toolStripMenuItem38
            // 
            toolStripMenuItem38.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem39 });
            toolStripMenuItem38.Name = "toolStripMenuItem38";
            resources.ApplyResources(toolStripMenuItem38, "toolStripMenuItem38");
            // 
            // toolStripMenuItem39
            // 
            toolStripMenuItem39.Name = "toolStripMenuItem39";
            resources.ApplyResources(toolStripMenuItem39, "toolStripMenuItem39");
            // 
            // toolStripMenuItem47
            // 
            toolStripMenuItem47.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem48, toolStripMenuItem50 });
            toolStripMenuItem47.Name = "toolStripMenuItem47";
            resources.ApplyResources(toolStripMenuItem47, "toolStripMenuItem47");
            // 
            // toolStripMenuItem48
            // 
            toolStripMenuItem48.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem49 });
            toolStripMenuItem48.Name = "toolStripMenuItem48";
            resources.ApplyResources(toolStripMenuItem48, "toolStripMenuItem48");
            // 
            // toolStripMenuItem49
            // 
            toolStripMenuItem49.Name = "toolStripMenuItem49";
            resources.ApplyResources(toolStripMenuItem49, "toolStripMenuItem49");
            // 
            // toolStripMenuItem50
            // 
            toolStripMenuItem50.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem51 });
            toolStripMenuItem50.Name = "toolStripMenuItem50";
            resources.ApplyResources(toolStripMenuItem50, "toolStripMenuItem50");
            // 
            // toolStripMenuItem51
            // 
            toolStripMenuItem51.Name = "toolStripMenuItem51";
            resources.ApplyResources(toolStripMenuItem51, "toolStripMenuItem51");
            // 
            // изменитьКоToolStripMenuItem
            // 
            изменитьКоToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { переместитьToolStripMenuItem, повернутьToolStripMenuItem });
            изменитьКоToolStripMenuItem.Name = "изменитьКоToolStripMenuItem";
            resources.ApplyResources(изменитьКоToolStripMenuItem, "изменитьКоToolStripMenuItem");
            // 
            // переместитьToolStripMenuItem
            // 
            переместитьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { векторabcToolStripMenuItem });
            переместитьToolStripMenuItem.Name = "переместитьToolStripMenuItem";
            resources.ApplyResources(переместитьToolStripMenuItem, "переместитьToolStripMenuItem");
            // 
            // векторabcToolStripMenuItem
            // 
            векторabcToolStripMenuItem.Name = "векторabcToolStripMenuItem";
            resources.ApplyResources(векторabcToolStripMenuItem, "векторabcToolStripMenuItem");
            // 
            // повернутьToolStripMenuItem
            // 
            повернутьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { осьxyzУголToolStripMenuItem });
            повернутьToolStripMenuItem.Name = "повернутьToolStripMenuItem";
            resources.ApplyResources(повернутьToolStripMenuItem, "повернутьToolStripMenuItem");
            // 
            // осьxyzУголToolStripMenuItem
            // 
            осьxyzУголToolStripMenuItem.Name = "осьxyzУголToolStripMenuItem";
            resources.ApplyResources(осьxyzУголToolStripMenuItem, "осьxyzУголToolStripMenuItem");
            // 
            // toolStripMenuItem54
            // 
            toolStripMenuItem54.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem55, toolStripMenuItem57, toolStripMenuItem59 });
            resources.ApplyResources(toolStripMenuItem54, "toolStripMenuItem54");
            toolStripMenuItem54.MergeAction = System.Windows.Forms.MergeAction.Replace;
            toolStripMenuItem54.Name = "toolStripMenuItem54";
            // 
            // toolStripMenuItem55
            // 
            toolStripMenuItem55.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem56 });
            toolStripMenuItem55.Name = "toolStripMenuItem55";
            resources.ApplyResources(toolStripMenuItem55, "toolStripMenuItem55");
            // 
            // toolStripMenuItem56
            // 
            toolStripMenuItem56.Name = "toolStripMenuItem56";
            resources.ApplyResources(toolStripMenuItem56, "toolStripMenuItem56");
            // 
            // toolStripMenuItem57
            // 
            toolStripMenuItem57.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem58 });
            toolStripMenuItem57.Name = "toolStripMenuItem57";
            resources.ApplyResources(toolStripMenuItem57, "toolStripMenuItem57");
            // 
            // toolStripMenuItem58
            // 
            toolStripMenuItem58.Name = "toolStripMenuItem58";
            resources.ApplyResources(toolStripMenuItem58, "toolStripMenuItem58");
            // 
            // toolStripMenuItem59
            // 
            toolStripMenuItem59.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem60 });
            toolStripMenuItem59.Name = "toolStripMenuItem59";
            resources.ApplyResources(toolStripMenuItem59, "toolStripMenuItem59");
            // 
            // toolStripMenuItem60
            // 
            toolStripMenuItem60.Name = "toolStripMenuItem60";
            resources.ApplyResources(toolStripMenuItem60, "toolStripMenuItem60");
            // 
            // toolStripMenuItem61
            // 
            toolStripMenuItem61.Name = "toolStripMenuItem61";
            resources.ApplyResources(toolStripMenuItem61, "toolStripMenuItem61");
            toolStripMenuItem61.Click += NewItem_Click;
            // 
            // toolStripMenuItem62
            // 
            toolStripMenuItem62.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem63 });
            toolStripMenuItem62.Name = "toolStripMenuItem62";
            resources.ApplyResources(toolStripMenuItem62, "toolStripMenuItem62");
            // 
            // toolStripMenuItem63
            // 
            toolStripMenuItem63.Name = "toolStripMenuItem63";
            resources.ApplyResources(toolStripMenuItem63, "toolStripMenuItem63");
            // 
            // toolStripMenuItem64
            // 
            toolStripMenuItem64.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem65 });
            toolStripMenuItem64.Name = "toolStripMenuItem64";
            resources.ApplyResources(toolStripMenuItem64, "toolStripMenuItem64");
            // 
            // toolStripMenuItem65
            // 
            toolStripMenuItem65.Name = "toolStripMenuItem65";
            resources.ApplyResources(toolStripMenuItem65, "toolStripMenuItem65");
            toolStripMenuItem65.Click += NewItem_Click;
            // 
            // toolStripMenuItem66
            // 
            toolStripMenuItem66.Name = "toolStripMenuItem66";
            resources.ApplyResources(toolStripMenuItem66, "toolStripMenuItem66");
            // 
            // ConsoleControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tlscOut);
            //HeaderName = GetLocalizableHeaderText();
            Name = "ConsoleControl";
            Load += ConsoleControl_Load;
            tlscOut.ContentPanel.ResumeLayout(false);
            tlscOut.RightToolStripPanel.ResumeLayout(false);
            tlscOut.RightToolStripPanel.PerformLayout();
            tlscOut.ResumeLayout(false);
            tlscOut.PerformLayout();
            toolStripEx1.ResumeLayout(false);
            toolStripEx1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem14;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem15;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem16;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripContainer tlscOut;
        private System.Windows.Forms.RichTextBox rtxbField;
        private ToolStripEx toolStripEx1;
        private System.Windows.Forms.ToolStripButton spbDictionary;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem32;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem33;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem34;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem35;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem36;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem37;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem38;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem39;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem47;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem48;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem49;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem50;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem51;
        private System.Windows.Forms.ToolStripMenuItem изменитьКоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem переместитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem векторabcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem повернутьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem осьxyzУголToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem54;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem55;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem56;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem57;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem58;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem59;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem60;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem61;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem62;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem63;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem64;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem65;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem66;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton btnStartMacro;
    }
}
