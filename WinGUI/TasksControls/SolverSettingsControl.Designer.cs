namespace WinGUI.TasksControls
{
    partial class SolverSettingsControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSolver = new UserControlsEx.ComboBoxEx(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txbSolverIterations = new UserControlsEx.TextBoxEx(this.components);
            this.txbPrecision = new UserControlsEx.TextBoxEx(this.components);
            this.txbRelaxation = new UserControlsEx.TextBoxEx(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbPriority = new UserControlsEx.ComboBoxEx(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbSolver, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txbSolverIterations, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txbPrecision, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txbRelaxation, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.cmbPriority, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(426, 446);
            this.tableLayoutPanel1.TabIndex = 176;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 216);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 13);
            this.label4.TabIndex = 144;
            this.label4.Text = "Точность решения, у.ед.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 38);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 129;
            this.label3.Text = "Алгоритм решения";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbSolver
            // 
            this.cmbSolver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSolver.FormattingEnabled = true;
            this.cmbSolver.InputType = UserControlsEx.CMBInputType.Items;
            this.cmbSolver.IsValidating = true;
            this.cmbSolver.Items.AddRange(new object[] {
            "BiCGStab",
            "ConjugateGradient",
            "SymmetricUtdu"});
            this.cmbSolver.Location = new System.Drawing.Point(170, 34);
            this.cmbSolver.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.cmbSolver.Name = "cmbSolver";
            this.cmbSolver.Size = new System.Drawing.Size(236, 21);
            this.cmbSolver.TabIndex = 140;
            this.cmbSolver.Text = "ConjugateGradient";
            this.cmbSolver.UserRegExCheck = null;
            this.cmbSolver.UserRegExCheckErrorMessage = null;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 127);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 130;
            this.label1.Text = "Кол-во итераций решения";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txbSolverIterations
            // 
            this.txbSolverIterations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbSolverIterations.BackColor = System.Drawing.SystemColors.Window;
            this.txbSolverIterations.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbSolverIterations.InputType = UserControlsEx.TXTBoxInputType.Integer;
            this.txbSolverIterations.IsValidating = true;
            this.txbSolverIterations.Location = new System.Drawing.Point(170, 123);
            this.txbSolverIterations.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbSolverIterations.Name = "txbSolverIterations";
            this.txbSolverIterations.Size = new System.Drawing.Size(236, 20);
            this.txbSolverIterations.TabIndex = 139;
            this.txbSolverIterations.Text = "100";
            this.txbSolverIterations.UserRegExCheck = null;
            this.txbSolverIterations.UserRegExCheckErrorMessage = null;
            // 
            // txbPrecision
            // 
            this.txbPrecision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbPrecision.BackColor = System.Drawing.SystemColors.Window;
            this.txbPrecision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbPrecision.InputType = ((UserControlsEx.TXTBoxInputType)((UserControlsEx.TXTBoxInputType.Float | UserControlsEx.TXTBoxInputType.Positive)));
            this.txbPrecision.IsValidating = true;
            this.txbPrecision.Location = new System.Drawing.Point(170, 212);
            this.txbPrecision.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbPrecision.Name = "txbPrecision";
            this.txbPrecision.Size = new System.Drawing.Size(236, 20);
            this.txbPrecision.TabIndex = 143;
            this.txbPrecision.Text = "0.01";
            this.txbPrecision.UserRegExCheck = null;
            this.txbPrecision.UserRegExCheckErrorMessage = null;
            // 
            // txbRelaxation
            // 
            this.txbRelaxation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txbRelaxation.BackColor = System.Drawing.SystemColors.Window;
            this.txbRelaxation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbRelaxation.InputType = ((UserControlsEx.TXTBoxInputType)((UserControlsEx.TXTBoxInputType.Float | UserControlsEx.TXTBoxInputType.Positive)));
            this.txbRelaxation.IsValidating = true;
            this.txbRelaxation.Location = new System.Drawing.Point(170, 301);
            this.txbRelaxation.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.txbRelaxation.Name = "txbRelaxation";
            this.txbRelaxation.Size = new System.Drawing.Size(236, 20);
            this.txbRelaxation.TabIndex = 145;
            this.txbRelaxation.Text = "1.05";
            this.txbRelaxation.UserRegExCheck = null;
            this.txbRelaxation.UserRegExCheckErrorMessage = null;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 305);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 13);
            this.label7.TabIndex = 146;
            this.label7.Text = "Коэф. релаксации (w)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 394);
            this.label12.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 13);
            this.label12.TabIndex = 147;
            this.label12.Text = "Приоритет";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbPriority
            // 
            this.cmbPriority.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPriority.DisplayMember = "0";
            this.cmbPriority.FormattingEnabled = true;
            this.cmbPriority.InputType = UserControlsEx.CMBInputType.Items;
            this.cmbPriority.IsValidating = true;
            this.cmbPriority.Items.AddRange(new object[] {
            "Низкий",
            "НижеСреднего",
            "Средний",
            "ВышеСреднего",
            "Высокий",
            "Наивысший"});
            this.cmbPriority.Location = new System.Drawing.Point(170, 390);
            this.cmbPriority.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.cmbPriority.Name = "cmbPriority";
            this.cmbPriority.Size = new System.Drawing.Size(236, 21);
            this.cmbPriority.TabIndex = 148;
            this.cmbPriority.Text = "Наивысший";
            this.cmbPriority.UserRegExCheck = null;
            this.cmbPriority.UserRegExCheckErrorMessage = null;
            // 
            // SolverSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SolverSettingsControl";
            this.Size = new System.Drawing.Size(426, 446);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private UserControlsEx.ComboBoxEx cmbSolver;
        private System.Windows.Forms.Label label1;
        private UserControlsEx.TextBoxEx txbSolverIterations;
        private UserControlsEx.TextBoxEx txbPrecision;
        private System.Windows.Forms.Label label4;
        private UserControlsEx.TextBoxEx txbRelaxation;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label12;
        private UserControlsEx.ComboBoxEx cmbPriority;
    }
}
