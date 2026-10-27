namespace BazisGUI.Win.Clip
{
    partial class ClipControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClipControl));
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            button2 = new System.Windows.Forms.Button();
            textBox2 = new System.Windows.Forms.TextBox();
            radioButton6 = new System.Windows.Forms.RadioButton();
            radioButton5 = new System.Windows.Forms.RadioButton();
            radioButton4 = new System.Windows.Forms.RadioButton();
            radioButton3 = new System.Windows.Forms.RadioButton();
            checkBox1 = new System.Windows.Forms.CheckBox();
            panel1 = new System.Windows.Forms.Panel();
            domainUpDown1 = new System.Windows.Forms.DomainUpDown();
            label5 = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            radioButton1 = new System.Windows.Forms.RadioButton();
            radioButton2 = new System.Windows.Forms.RadioButton();
            button1 = new System.Windows.Forms.Button();
            panel2 = new System.Windows.Forms.Panel();
            radioButton9 = new System.Windows.Forms.RadioButton();
            radioButton8 = new System.Windows.Forms.RadioButton();
            radioButton7 = new System.Windows.Forms.RadioButton();
            label6 = new System.Windows.Forms.Label();
            colorSlider1 = new UserControlsEx.ColorSlider();
            colorSlider2 = new UserControlsEx.ColorSlider();
            colorSlider3 = new UserControlsEx.ColorSlider();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(tableLayoutPanel1, "tableLayoutPanel1");
            tableLayoutPanel1.Controls.Add(button2, 0, 4);
            tableLayoutPanel1.Controls.Add(textBox2, 3, 4);
            tableLayoutPanel1.Controls.Add(radioButton6, 1, 2);
            tableLayoutPanel1.Controls.Add(radioButton5, 0, 2);
            tableLayoutPanel1.Controls.Add(radioButton4, 1, 1);
            tableLayoutPanel1.Controls.Add(radioButton3, 0, 1);
            tableLayoutPanel1.Controls.Add(checkBox1, 0, 6);
            tableLayoutPanel1.Controls.Add(panel1, 1, 6);
            tableLayoutPanel1.Controls.Add(textBox1, 3, 5);
            tableLayoutPanel1.Controls.Add(label4, 2, 5);
            tableLayoutPanel1.Controls.Add(label3, 2, 2);
            tableLayoutPanel1.Controls.Add(label2, 2, 1);
            tableLayoutPanel1.Controls.Add(label1, 2, 0);
            tableLayoutPanel1.Controls.Add(radioButton1, 0, 0);
            tableLayoutPanel1.Controls.Add(radioButton2, 1, 0);
            tableLayoutPanel1.Controls.Add(button1, 0, 5);
            tableLayoutPanel1.Controls.Add(panel2, 0, 3);
            tableLayoutPanel1.Controls.Add(label6, 2, 4);
            tableLayoutPanel1.Controls.Add(colorSlider1, 3, 0);
            tableLayoutPanel1.Controls.Add(colorSlider2, 3, 1);
            tableLayoutPanel1.Controls.Add(colorSlider3, 3, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // button2
            // 
            tableLayoutPanel1.SetColumnSpan(button2, 2);
            resources.ApplyResources(button2, "button2");
            button2.Name = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            resources.ApplyResources(textBox2, "textBox2");
            textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.MouseDown += OnMouseDown;
            textBox2.MouseMove += OnMouseMove;
            textBox2.MouseUp += OnMouseUp;
            // 
            // radioButton6
            // 
            resources.ApplyResources(radioButton6, "radioButton6");
            radioButton6.Checked = true;
            radioButton6.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            radioButton6.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            radioButton6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            radioButton6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            radioButton6.Name = "radioButton6";
            radioButton6.TabStop = true;
            radioButton6.Tag = "100 100 0";
            radioButton6.UseVisualStyleBackColor = true;
            radioButton6.Click += OnChoicePlane;
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
            radioButton5.Tag = "100 100 200";
            radioButton5.UseVisualStyleBackColor = true;
            radioButton5.Click += OnChoicePlane;
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
            radioButton4.Tag = "100 0 100";
            radioButton4.UseVisualStyleBackColor = true;
            radioButton4.Click += OnChoicePlane;
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
            radioButton3.Tag = "100 200 100";
            radioButton3.UseVisualStyleBackColor = true;
            radioButton3.Click += OnChoicePlane;
            radioButton3.Paint += OnPaint;
            // 
            // checkBox1
            // 
            resources.ApplyResources(checkBox1, "checkBox1");
            tableLayoutPanel1.SetColumnSpan(checkBox1, 2);
            checkBox1.Name = "checkBox1";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.Click += OnEnableClipPlane;
            // 
            // panel1
            // 
            tableLayoutPanel1.SetColumnSpan(panel1, 2);
            panel1.Controls.Add(domainUpDown1);
            panel1.Controls.Add(label5);
            resources.ApplyResources(panel1, "panel1");
            panel1.Name = "panel1";
            // 
            // domainUpDown1
            // 
            resources.ApplyResources(domainUpDown1, "domainUpDown1");
            domainUpDown1.Items.Add(resources.GetString("domainUpDown1.Items"));
            domainUpDown1.Items.Add(resources.GetString("domainUpDown1.Items1"));
            domainUpDown1.Items.Add(resources.GetString("domainUpDown1.Items2"));
            domainUpDown1.Items.Add(resources.GetString("domainUpDown1.Items3"));
            domainUpDown1.Name = "domainUpDown1";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // textBox1
            // 
            resources.ApplyResources(textBox1, "textBox1");
            textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.MouseDown += OnMouseDown;
            textBox1.MouseMove += OnMouseMove;
            textBox1.MouseUp += OnMouseUp;
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            label4.MouseMove += OnMouseMove;
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            label3.Tag = "";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            label2.Tag = "";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            label1.Tag = "";
            // 
            // radioButton1
            // 
            resources.ApplyResources(radioButton1, "radioButton1");
            radioButton1.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            radioButton1.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            radioButton1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            radioButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            radioButton1.Name = "radioButton1";
            radioButton1.Tag = "200 100 100";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.Click += OnChoicePlane;
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
            radioButton2.Tag = "0 100 100";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.Click += OnChoicePlane;
            radioButton2.Paint += OnPaint;
            // 
            // button1
            // 
            tableLayoutPanel1.SetColumnSpan(button1, 2);
            resources.ApplyResources(button1, "button1");
            button1.Name = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += OnResetShifting;
            // 
            // panel2
            // 
            tableLayoutPanel1.SetColumnSpan(panel2, 4);
            panel2.Controls.Add(radioButton9);
            panel2.Controls.Add(radioButton8);
            panel2.Controls.Add(radioButton7);
            resources.ApplyResources(panel2, "panel2");
            panel2.Name = "panel2";
            // 
            // radioButton9
            // 
            resources.ApplyResources(radioButton9, "radioButton9");
            radioButton9.Name = "radioButton9";
            radioButton9.Tag = "Layered";
            radioButton9.UseVisualStyleBackColor = true;
            radioButton9.Click += OnChangeDrawMode;
            // 
            // radioButton8
            // 
            resources.ApplyResources(radioButton8, "radioButton8");
            radioButton8.Name = "radioButton8";
            radioButton8.Tag = "KeepElement";
            radioButton8.UseVisualStyleBackColor = true;
            radioButton8.Click += OnChangeDrawMode;
            // 
            // radioButton7
            // 
            resources.ApplyResources(radioButton7, "radioButton7");
            radioButton7.Checked = true;
            radioButton7.Name = "radioButton7";
            radioButton7.TabStop = true;
            radioButton7.Tag = "Default";
            radioButton7.UseVisualStyleBackColor = true;
            radioButton7.Click += OnChangeDrawMode;
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // colorSlider1
            // 
            colorSlider1.BackColor = System.Drawing.Color.Transparent;
            colorSlider1.BarInnerColor = System.Drawing.Color.Silver;
            colorSlider1.BarOuterColor = System.Drawing.Color.Silver;
            colorSlider1.BarPenColor = System.Drawing.Color.Black;
            colorSlider1.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            colorSlider1.ElapsedInnerColor = System.Drawing.Color.Silver;
            colorSlider1.ElapsedOuterColor = System.Drawing.Color.Silver;
            colorSlider1.LargeChange = 5U;
            resources.ApplyResources(colorSlider1, "colorSlider1");
            colorSlider1.Maximum = 200;
            colorSlider1.Name = "colorSlider1";
            colorSlider1.ShowTextValue = false;
            colorSlider1.SmallChange = 1U;
            colorSlider1.TextValueColor = System.Drawing.Color.Black;
            colorSlider1.ThumbPenColor = System.Drawing.Color.Black;
            colorSlider1.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            colorSlider1.ThumbSize = 12;
            colorSlider1.Value = 100;
            colorSlider1.ValueChanged += OnChangeValue;
            // 
            // colorSlider2
            // 
            colorSlider2.BackColor = System.Drawing.Color.Transparent;
            colorSlider2.BarInnerColor = System.Drawing.Color.Silver;
            colorSlider2.BarOuterColor = System.Drawing.Color.Silver;
            colorSlider2.BarPenColor = System.Drawing.Color.Black;
            colorSlider2.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            colorSlider2.ElapsedInnerColor = System.Drawing.Color.Silver;
            colorSlider2.ElapsedOuterColor = System.Drawing.Color.Silver;
            colorSlider2.LargeChange = 5U;
            resources.ApplyResources(colorSlider2, "colorSlider2");
            colorSlider2.Maximum = 200;
            colorSlider2.Name = "colorSlider2";
            colorSlider2.ShowTextValue = false;
            colorSlider2.SmallChange = 1U;
            colorSlider2.TextValueColor = System.Drawing.Color.Black;
            colorSlider2.ThumbPenColor = System.Drawing.Color.Black;
            colorSlider2.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            colorSlider2.ThumbSize = 12;
            colorSlider2.Value = 100;
            colorSlider2.ValueChanged += OnChangeValue;
            // 
            // colorSlider3
            // 
            colorSlider3.BackColor = System.Drawing.Color.Transparent;
            colorSlider3.BarInnerColor = System.Drawing.Color.Silver;
            colorSlider3.BarOuterColor = System.Drawing.Color.Silver;
            colorSlider3.BarPenColor = System.Drawing.Color.Black;
            colorSlider3.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            colorSlider3.ElapsedInnerColor = System.Drawing.Color.Silver;
            colorSlider3.ElapsedOuterColor = System.Drawing.Color.Silver;
            colorSlider3.LargeChange = 5U;
            resources.ApplyResources(colorSlider3, "colorSlider3");
            colorSlider3.Maximum = 200;
            colorSlider3.Name = "colorSlider3";
            colorSlider3.ShowTextValue = false;
            colorSlider3.SmallChange = 1U;
            colorSlider3.TextValueColor = System.Drawing.Color.Black;
            colorSlider3.ThumbPenColor = System.Drawing.Color.Black;
            colorSlider3.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            colorSlider3.ThumbSize = 12;
            colorSlider3.Value = 0;
            colorSlider3.ValueChanged += OnChangeValue;
            // 
            // ClipControl
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "ClipControl";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DomainUpDown domainUpDown1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label6;
        private UserControlsEx.ColorSlider colorSlider1;
        private UserControlsEx.ColorSlider colorSlider2;
        private UserControlsEx.ColorSlider colorSlider3;
        private System.Windows.Forms.Button button2;
    }
}
