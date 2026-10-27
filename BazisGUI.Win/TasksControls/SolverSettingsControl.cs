using PreProc;
using PreProc.Interfaces;
using Project.TaskParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BazisGUI.Win.TasksControls
{
    public partial class SolverSettingsControl : UserControl
    {
        public SolverSettingsControl()
        {
            InitializeComponent();
        }

        public bool GetValidationResult()
        {
            var checks = new List<bool>()
            {
                cmbPriority.IsValueValid(),
                cmbSolver.IsValueValid(),
                txbPrecision.IsValueValid(),
                txbRelaxation.IsValueValid(),
                txbSolverIterations.IsValueValid()
            };
            return checks.All(x => x);
        }

        // Окно относится к устаревшему редактору tsf (BaseForm.EditTSFFile помечен Obsolete).
        // Здесь только адаптация к Project 6.x: Solver стал перечислением и доступен на чтение,
        // поэтому настройки пересоздаются конструктором. Хранение матрицы этим окном не правится.
        private MatrixStorageKind matrixStorage = MatrixStorageKind.SymmetricCsr;

        internal SolverSettings GetSolverSettings()
        {
            Enum.TryParse<LinearSolverKind>(cmbSolver.Text, out var solver);

            return new SolverSettings(solver, matrixStorage)
            {
                MaxIter = Convert.ToInt32(txbSolverIterations.Text),
                Precision = Convert.ToSingle(txbPrecision.Text),
                Relaxation = Convert.ToSingle(txbRelaxation.Text),
                Priority = cmbPriority.Text
            };
      
        }

        internal void SetSolverSettings(SolverSettings solverSettings)
        {
            txbPrecision.Text = solverSettings.Precision.ToString();
            txbRelaxation.Text = solverSettings.Relaxation.ToString();
            txbSolverIterations.Text = solverSettings.MaxIter.ToString();
            cmbPriority.Text = solverSettings.Priority;
            cmbSolver.Text = solverSettings.Solver.ToString();
            matrixStorage = solverSettings.MatrixStorage;
        }
    }
}
