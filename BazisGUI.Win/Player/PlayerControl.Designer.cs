using UserControlsEx;

namespace BazisGUI.Win.Player
{
    partial class PlayerControl
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerControl));
            btnStopCheck = new System.Windows.Forms.Button();
            btnCheckDinamic = new System.Windows.Forms.Button();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            colorSlider = new ColorSlider();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnStopCheck
            // 
            btnStopCheck.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnStopCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            btnStopCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnStopCheck.Image = (System.Drawing.Image)resources.GetObject("btnStopCheck.Image");
            btnStopCheck.Location = new System.Drawing.Point(43, 3);
            btnStopCheck.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnStopCheck.MaximumSize = new System.Drawing.Size(31, 31);
            btnStopCheck.Name = "btnStopCheck";
            btnStopCheck.Size = new System.Drawing.Size(31, 31);
            btnStopCheck.TabIndex = 24;
            btnStopCheck.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            btnStopCheck.UseVisualStyleBackColor = true;
            btnStopCheck.Click += StopChecking_Click;
            // 
            // btnCheckDinamic
            // 
            btnCheckDinamic.Dock = System.Windows.Forms.DockStyle.Fill;
            btnCheckDinamic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCheckDinamic.Image = (System.Drawing.Image)resources.GetObject("btnCheckDinamic.Image");
            btnCheckDinamic.Location = new System.Drawing.Point(4, 3);
            btnCheckDinamic.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnCheckDinamic.MaximumSize = new System.Drawing.Size(31, 31);
            btnCheckDinamic.Name = "btnCheckDinamic";
            btnCheckDinamic.Size = new System.Drawing.Size(31, 31);
            btnCheckDinamic.TabIndex = 25;
            btnCheckDinamic.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            btnCheckDinamic.UseVisualStyleBackColor = true;
            btnCheckDinamic.Click += StartChecking_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.Controls.Add(btnCheckDinamic, 0, 0);
            tableLayoutPanel1.Controls.Add(btnStopCheck, 1, 0);
            tableLayoutPanel1.Controls.Add(colorSlider, 2, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            tableLayoutPanel1.Size = new System.Drawing.Size(251, 37);
            tableLayoutPanel1.TabIndex = 27;
            // 
            // colorSlider
            // 
            colorSlider.BackColor = System.Drawing.Color.Transparent;
            colorSlider.BarInnerColor = System.Drawing.Color.Silver;
            colorSlider.BarOuterColor = System.Drawing.Color.Silver;
            colorSlider.BarPenColor = System.Drawing.Color.Black;
            colorSlider.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            colorSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            colorSlider.ElapsedInnerColor = System.Drawing.Color.Silver;
            colorSlider.ElapsedOuterColor = System.Drawing.Color.Silver;
            colorSlider.LargeChange = 5U;
            colorSlider.Location = new System.Drawing.Point(82, 3);
            colorSlider.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            colorSlider.MaximumSize = new System.Drawing.Size(0, 31);
            colorSlider.Name = "colorSlider";
            colorSlider.ShowTextValue = true;
            colorSlider.Size = new System.Drawing.Size(165, 31);
            colorSlider.SmallChange = 1U;
            colorSlider.TabIndex = 26;
            colorSlider.Text = "colorSlider";
            colorSlider.TextValueColor = System.Drawing.Color.Black;
            colorSlider.ThumbPenColor = System.Drawing.Color.Black;
            colorSlider.ThumbRoundRectSize = new System.Drawing.Size(1, 1);
            colorSlider.ThumbSize = 12;
            // 
            // PlayerControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(tableLayoutPanel1);
            Margin = new System.Windows.Forms.Padding(0);
            MinimumSize = new System.Drawing.Size(251, 35);
            Name = "PlayerControl";
            Size = new System.Drawing.Size(251, 37);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button btnStopCheck;
        private ColorSlider colorSlider;
        private System.Windows.Forms.Button btnCheckDinamic;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
