using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinGUI.AdvanceSelection.ControlsForSelect
{
    public partial class GeomSelect : UserControl
    {
        private readonly Dictionary<SelectionType, (bool volume, bool surface, bool curve)> _modes = new()
        {
            { SelectionType.Points, (true, true, true) },
            { SelectionType.Curves, (true, true, false) },
            { SelectionType.Surfaces, (true, false, false) }
        };
        public event Action CloseForm;

        public GeomSelect(SelectionType selectedObjects)
        {
            InitializeComponent();
            SetAvailableModes(selectedObjects);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        public void SetAvailableModes(SelectionType selectedObjects)
        {
            if (_modes.TryGetValue(selectedObjects, out var mode))
                SetApply(mode.volume, mode.surface, mode.curve);
            else
                CloseForm?.Invoke();
        }

        public int GetSelectDimension()
        {
            foreach (var rb in generalPanel.Controls.OfType<RadioButton>())
                if (rb.Checked)
                    return GetDim(rb.AccessibleName);
            return -1;
        }

        private void SetApply(bool volume, bool surface, bool curve)
        {
            rbtVolume.Enabled = volume;
            rbtSurface.Enabled = surface;
            rbtCurve.Enabled = curve;
        }

        private int GetDim(string rbtText) 
        {
            return Enum.Parse<SelectionType>(rbtText.Split("GeomSelect.")[1]) switch
            {
                SelectionType.Volumes => 3,
                SelectionType.Surfaces => 2,
                SelectionType.Curves => 1,
            };
        }
    }
}
