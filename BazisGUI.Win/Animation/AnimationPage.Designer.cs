using BazisGUI.Win.Player;
using UserControlsEx;

namespace BazisGUI.Win.Animation
{
    partial class AnimationPage
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.chbDelTempScrs = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txbDelayTime = new UserControlsEx.TextBoxEx(this.components);
            this.btnCreateAnimation = new System.Windows.Forms.Button();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.61539F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.38461F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 159F));
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.chbDelTempScrs, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.txbDelayTime, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnCreateAnimation, 2, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(391, 139);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 28);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Задержка между слайдами";
            // 
            // chbDelTempScrs
            // 
            this.chbDelTempScrs.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chbDelTempScrs.AutoSize = true;
            this.chbDelTempScrs.Checked = true;
            this.chbDelTempScrs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbDelTempScrs.Location = new System.Drawing.Point(206, 97);
            this.chbDelTempScrs.Margin = new System.Windows.Forms.Padding(4);
            this.chbDelTempScrs.Name = "chbDelTempScrs";
            this.chbDelTempScrs.Size = new System.Drawing.Size(15, 14);
            this.chbDelTempScrs.TabIndex = 6;
            this.chbDelTempScrs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chbDelTempScrs.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 91);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 26);
            this.label3.TabIndex = 7;
            this.label3.Text = "Удалить промежуточные результаты";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txbDelayTime
            // 
            this.txbDelayTime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txbDelayTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbDelayTime.InputType = UserControlsEx.TXTBoxInputType.Integer;
            this.txbDelayTime.IsValidating = true;
            this.txbDelayTime.Location = new System.Drawing.Point(203, 24);
            this.txbDelayTime.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.txbDelayTime.Name = "txbDelayTime";
            this.txbDelayTime.Size = new System.Drawing.Size(21, 20);
            this.txbDelayTime.TabIndex = 5;
            this.txbDelayTime.Text = "100";
            this.txbDelayTime.UserRegExCheck = null;
            this.txbDelayTime.UserRegExCheckErrorMessage = null;
            // 
            // btnCreateAnimation
            // 
            this.btnCreateAnimation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateAnimation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCreateAnimation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateAnimation.Location = new System.Drawing.Point(238, 90);
            this.btnCreateAnimation.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.btnCreateAnimation.Name = "btnCreateAnimation";
            this.btnCreateAnimation.Size = new System.Drawing.Size(146, 27);
            this.btnCreateAnimation.TabIndex = 3;
            this.btnCreateAnimation.Text = "Создать";
            this.btnCreateAnimation.UseVisualStyleBackColor = true;
            this.btnCreateAnimation.Click += new System.EventHandler(this.btnCreateAnimation_Click);
            // 
            // AnimationPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel4);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AnimationPage";
            this.Size = new System.Drawing.Size(391, 139);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chbDelTempScrs;
        private System.Windows.Forms.Label label3;
        private TextBoxEx txbDelayTime;
        private System.Windows.Forms.Button btnCreateAnimation;
    }
}
