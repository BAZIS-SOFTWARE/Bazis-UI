using Model.Interfaces;
using System;
using System.Windows.Forms;

namespace WinGUI.Measurement
{
    public partial class MeasuringSet : UserControl
    {

        public event Action<object, MeasureEventArgs> MakeMeasureEvent;
        public event Action<ObjType> PreparingMeasureEvent;

        MeasureKind measureKind;
        public MeasuringSet()
        {
            InitializeComponent();
        }

        private void Rbtn_Click(object sender, EventArgs e)
        {
            if (rbtVolume.Checked)
            {
                measureKind = MeasureKind.Volume;
                cmbMeasureObjects.Enabled = false;
                PreparingMeasureEvent?.Invoke(ObjType.Элемент3D);
            }

            else if (rbtSquare.Checked)
            {
                measureKind = MeasureKind.Square;
                cmbMeasureObjects.Enabled = false;
                PreparingMeasureEvent?.Invoke(ObjType.Элемент2D);
            }

            else if (rbtnPath.Checked)
            {
                measureKind = MeasureKind.Path;
                cmbMeasureObjects.Enabled = false;
                PreparingMeasureEvent?.Invoke(ObjType.Узел);
            }

            else
            {
                cmbMeasureObjects.Enabled = true;
                measureKind = MeasureKind.DistancePointToPoint;
                cmbMeasureObjects.SelectedIndex = 0;
                PreparingMeasureEvent?.Invoke(ObjType.Узел);
            }

        }

        private void btnMeasure_Click(object sender, EventArgs e)
        {
            MakeMeasureEvent(this, new MeasureEventArgs(measureKind));
        }

        private void cmbMeasureObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMeasureObjects.SelectedIndex == 0)
                measureKind = MeasureKind.DistancePointToPoint;
            else if (cmbMeasureObjects.SelectedIndex == 1)
                measureKind = MeasureKind.DistancePointToPlane;
        }
    }
}
