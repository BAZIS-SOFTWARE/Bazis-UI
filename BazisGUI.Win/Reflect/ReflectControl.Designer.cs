using UserControlsEx;

namespace BazisGUI.Win.Reflect
{
    partial class ReflectControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReflectControl));
            radioButton6 = new System.Windows.Forms.RadioButton();
            radioButton5 = new System.Windows.Forms.RadioButton();
            radioButton4 = new System.Windows.Forms.RadioButton();
            radioButton3 = new System.Windows.Forms.RadioButton();
            label8 = new System.Windows.Forms.Label();
            trackBar1 = new ColorSlider();
            trackBar2 = new ColorSlider();
            trackBar3 = new ColorSlider();
            textBox1 = new TextBoxEx(components);
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            radioButton1 = new System.Windows.Forms.RadioButton();
            radioButton2 = new System.Windows.Forms.RadioButton();
            button1 = new System.Windows.Forms.Button();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            comboBox1 = new ComboBoxEx(components);
            label5 = new System.Windows.Forms.Label();
            btnCreateCopy = new System.Windows.Forms.Button();
            domainUpDown1 = new System.Windows.Forms.DomainUpDown();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // radioButton6
            // 
            resources.ApplyResources(radioButton6, "radioButton6");
            radioButton6.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            radioButton6.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            radioButton6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            radioButton6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            radioButton6.Name = "radioButton6";
            radioButton6.Tag = "0 0 -1 0";
            toolTip1.SetToolTip(radioButton6, resources.GetString("radioButton6.ToolTip"));
            radioButton6.UseVisualStyleBackColor = true;
            radioButton6.CheckedChanged += OnChoicePlane;
            radioButton6.Paint += OnPaint;
            // 
            // radioButton5
            // 
            resources.ApplyResources(radioButton5, "radioButton5");
            radioButton5.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            radioButton5.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            radioButton5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            radioButton5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            radioButton5.Name = "radioButton5";
            radioButton5.Tag = "0 0 1 0";
            toolTip1.SetToolTip(radioButton5, resources.GetString("radioButton5.ToolTip"));
            radioButton5.UseVisualStyleBackColor = true;
            radioButton5.CheckedChanged += OnChoicePlane;
            radioButton5.Paint += OnPaint;
            // 
            // radioButton4
            // 
            resources.ApplyResources(radioButton4, "radioButton4");
            radioButton4.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            radioButton4.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            radioButton4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            radioButton4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            radioButton4.Name = "radioButton4";
            radioButton4.Tag = "0 -1 0 0";
            toolTip1.SetToolTip(radioButton4, resources.GetString("radioButton4.ToolTip"));
            radioButton4.UseVisualStyleBackColor = true;
            radioButton4.CheckedChanged += OnChoicePlane;
            radioButton4.Paint += OnPaint;
            // 
            // radioButton3
            // 
            resources.ApplyResources(radioButton3, "radioButton3");
            radioButton3.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            radioButton3.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            radioButton3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            radioButton3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            radioButton3.Name = "radioButton3";
            radioButton3.Tag = "0 1 0 0";
            toolTip1.SetToolTip(radioButton3, resources.GetString("radioButton3.ToolTip"));
            radioButton3.UseVisualStyleBackColor = true;
            radioButton3.CheckedChanged += OnChoicePlane;
            radioButton3.Paint += OnPaint;
            // 
            // label8
            // 
            resources.ApplyResources(label8, "label8");
            label8.Name = "label8";
            toolTip1.SetToolTip(label8, resources.GetString("label8.ToolTip"));
            // 
            // trackBar1
            // 
            resources.ApplyResources(trackBar1, "trackBar1");
            trackBar1.BackColor = System.Drawing.Color.Transparent;
            trackBar1.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            trackBar1.LargeChange = 5U;
            trackBar1.Name = "trackBar1";
            trackBar1.ShowTextValue = true;
            trackBar1.SmallChange = 1U;
            trackBar1.TextValueColor = System.Drawing.Color.Black;
            trackBar1.ThumbRoundRectSize = new System.Drawing.Size(8, 8);
            toolTip1.SetToolTip(trackBar1, resources.GetString("trackBar1.ToolTip"));
            // 
            // trackBar3
            // 
            resources.ApplyResources(trackBar3, "trackBar3");
            trackBar3.BackColor = System.Drawing.Color.Transparent;
            trackBar3.BarInnerColor = System.Drawing.Color.Silver;
            trackBar3.BarOuterColor = System.Drawing.Color.Silver;
            trackBar3.BarPenColor = System.Drawing.Color.Black;
            trackBar3.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            tableLayoutPanel1.SetColumnSpan(trackBar3, 2);
            trackBar3.ElapsedInnerColor = System.Drawing.Color.Silver;
            trackBar3.ElapsedOuterColor = System.Drawing.Color.Silver;
            trackBar3.LargeChange = 5U;
            trackBar3.Maximum = 200;
            trackBar3.Name = "trackBar3";
            trackBar3.ShowTextValue = false;
            trackBar3.SmallChange = 1U;
            trackBar3.Tag = "";
            trackBar3.TextValueColor = System.Drawing.Color.Black;
            trackBar3.ThumbPenColor = System.Drawing.Color.Black;
            trackBar3.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            trackBar3.ThumbSize = 12;
            toolTip1.SetToolTip(trackBar3, resources.GetString("trackBar3.ToolTip"));
            trackBar3.Value = 100;
            trackBar3.ValueChanged += OnChangeNormal;
            // 
            // textBox1
            // 
            resources.ApplyResources(textBox1, "textBox1");
            textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tableLayoutPanel1.SetColumnSpan(textBox1, 2);
            textBox1.InputType = TXTBoxInputType.Float;
            textBox1.IsValidating = true;
            textBox1.Name = "textBox1";
            toolTip1.SetToolTip(textBox1, resources.GetString("textBox1.ToolTip"));
            textBox1.UserRegExCheck = null;
            textBox1.UserRegExCheckErrorMessage = null;
            textBox1.MouseMove += OnMouseMove;
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            toolTip1.SetToolTip(label4, resources.GetString("label4.ToolTip"));
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            label3.Tag = "";
            toolTip1.SetToolTip(label3, resources.GetString("label3.ToolTip"));
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            label2.Tag = "";
            toolTip1.SetToolTip(label2, resources.GetString("label2.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            label1.Tag = "";
            toolTip1.SetToolTip(label1, resources.GetString("label1.ToolTip"));
            // 
            // radioButton1
            // 
            resources.ApplyResources(radioButton1, "radioButton1");
            radioButton1.Checked = true;
            radioButton1.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            radioButton1.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            radioButton1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            radioButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            radioButton1.Name = "radioButton1";
            radioButton1.TabStop = true;
            radioButton1.Tag = "1 0 0 0";
            toolTip1.SetToolTip(radioButton1, resources.GetString("radioButton1.ToolTip"));
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += OnChoicePlane;
            radioButton1.Paint += OnPaint;
            // 
            // radioButton2
            // 
            resources.ApplyResources(radioButton2, "radioButton2");
            radioButton2.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            radioButton2.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            radioButton2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            radioButton2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            radioButton2.Name = "radioButton2";
            radioButton2.Tag = "-1 0 0 0";
            toolTip1.SetToolTip(radioButton2, resources.GetString("radioButton2.ToolTip"));
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += OnChoicePlane;
            radioButton2.Paint += OnPaint;
            // 
            // button1
            // 
            resources.ApplyResources(button1, "button1");
            tableLayoutPanel1.SetColumnSpan(button1, 2);
            button1.Name = "button1";
            toolTip1.SetToolTip(button1, resources.GetString("button1.ToolTip"));
            button1.UseVisualStyleBackColor = true;
            button1.Click += OnResetShifting;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(tableLayoutPanel1, "tableLayoutPanel1");
            tableLayoutPanel1.Controls.Add(radioButton6, 1, 2);
            tableLayoutPanel1.Controls.Add(radioButton5, 0, 2);
            tableLayoutPanel1.Controls.Add(radioButton4, 1, 1);
            tableLayoutPanel1.Controls.Add(radioButton3, 0, 1);
            tableLayoutPanel1.Controls.Add(trackBar1, 3, 0);
            tableLayoutPanel1.Controls.Add(trackBar2, 3, 1);
            tableLayoutPanel1.Controls.Add(trackBar3, 3, 2);
            tableLayoutPanel1.Controls.Add(textBox1, 3, 3);
            tableLayoutPanel1.Controls.Add(label4, 2, 3);
            tableLayoutPanel1.Controls.Add(label3, 2, 2);
            tableLayoutPanel1.Controls.Add(label2, 2, 1);
            tableLayoutPanel1.Controls.Add(label1, 2, 0);
            tableLayoutPanel1.Controls.Add(radioButton1, 0, 0);
            tableLayoutPanel1.Controls.Add(radioButton2, 1, 0);
            tableLayoutPanel1.Controls.Add(button1, 0, 3);
            tableLayoutPanel1.Controls.Add(comboBox1, 0, 5);
            tableLayoutPanel1.Controls.Add(label5, 0, 4);
            tableLayoutPanel1.Controls.Add(btnCreateCopy, 4, 5);
            tableLayoutPanel1.Controls.Add(domainUpDown1, 4, 6);
            tableLayoutPanel1.Controls.Add(label8, 2, 6);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            toolTip1.SetToolTip(tableLayoutPanel1, resources.GetString("tableLayoutPanel1.ToolTip"));
            // 
            // comboBox1
            // 
            resources.ApplyResources(comboBox1, "comboBox1");
            tableLayoutPanel1.SetColumnSpan(comboBox1, 4);
            comboBox1.FormattingEnabled = true;
            comboBox1.InputType = CMBInputType.Items;
            comboBox1.IsValidating = true;
            comboBox1.Name = "comboBox1";
            toolTip1.SetToolTip(comboBox1, resources.GetString("comboBox1.ToolTip"));
            comboBox1.UserRegExCheck = null;
            comboBox1.UserRegExCheckErrorMessage = null;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            tableLayoutPanel1.SetColumnSpan(label5, 4);
            label5.Name = "label5";
            toolTip1.SetToolTip(label5, resources.GetString("label5.ToolTip"));
            // 
            // btnCreateCopy
            // 
            resources.ApplyResources(btnCreateCopy, "btnCreateCopy");
            btnCreateCopy.Name = "btnCreateCopy";
            toolTip1.SetToolTip(btnCreateCopy, resources.GetString("btnCreateCopy.ToolTip"));
            btnCreateCopy.UseVisualStyleBackColor = true;
            btnCreateCopy.Click += OnSetCopyName;
            // 
            // domainUpDown1
            // 
            resources.ApplyResources(domainUpDown1, "domainUpDown1");
            domainUpDown1.Items.Add(resources.GetString("domainUpDown1.Items"));
            domainUpDown1.Items.Add(resources.GetString("domainUpDown1.Items1"));
            domainUpDown1.Items.Add(resources.GetString("domainUpDown1.Items2"));
            domainUpDown1.Items.Add(resources.GetString("domainUpDown1.Items3"));
            domainUpDown1.Name = "domainUpDown1";
            toolTip1.SetToolTip(domainUpDown1, resources.GetString("domainUpDown1.ToolTip"));
            // 
            // ReflectControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "ReflectControl";
            toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            Load += OnLoad;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label8;
        private ColorSlider trackBar1;
        private ColorSlider trackBar2;
        private ColorSlider trackBar3;
        private TextBoxEx textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button button1;
        private ComboBoxEx comboBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCreateCopy;
        private System.Windows.Forms.DomainUpDown domainUpDown1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
