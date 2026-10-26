using WinGUI.Utilities;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinGUI.AdvanceSelection.ControlsForSelect
{
    public partial class MeshSelect : UserControl
    {
        private SelectInDirectionEventArgs directionConfig;
        private SelectInPlainEventArgs plainConfig;

        private readonly Dictionary<SelectionType, (bool set, bool surface, bool direction, bool other)> _modes = new()
        {
            { SelectionType.Elements1D, (true, false, false, false) },
            { SelectionType.Elements2D, (true, true, false, true) },
            { SelectionType.Elements3D, (true, false, false, false) },
            { SelectionType.Nodes,      (true, true, true, true) }
        };
        public event Action<SelectInDirectionEventArgs> SelectInDirection;
        public event Action CloseForm;
        private ObjType selectType;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public MeshSelect(SelectionType selectedObjects)
        {
            InitializeComponent();
            SetAvailableModes(selectedObjects);
        }

        public void SetAvailableModes(SelectionType selectedObjects)
        {
            if (_modes.TryGetValue(selectedObjects, out var mode))
            {
                SetApply(mode.set, mode.surface, mode.direction, mode.other);
                selectType = Converters.ConvertSelectionTypeToObjType(selectedObjects);
                directionConfig = null;
                plainConfig = null;
            }
            else
                CloseForm?.Invoke();
        }

        public object GetSelectedAdditionalMode()
        {
            if (rbtDirection.Checked)
            {
                if (directionConfig == null)
                    directionConfig = new SelectInDirectionEventArgs(ObjType.Узел, chbChangeDirection.Checked, float.Parse(txbAngle.Text));
                return directionConfig;
            }

            else if (rbtSurface.Checked)
            {
                if (plainConfig == null)
                    plainConfig = new SelectInPlainEventArgs(selectType, float.Parse(txbAngle.Text));
                return plainConfig;
            }
            else
                return selectType;
        }

        private void SetApply(bool set, bool surface, bool direction, bool other)
        {
            rbtSet.Enabled = set;
            rbtSurface.Enabled = surface;
            rbtDirection.Enabled = direction;
            lblAngle.Enabled = other;
            txbAngle.Enabled = other;
            chbChangeDirection.Enabled = other;
        }

        private void chbChangeDirection_CheckedChanged(object sender, EventArgs e)
        {
            directionConfig.Reverse = chbChangeDirection.Checked;
            //SelectInDirection(directionConfig);
        }

        private void Rbt_CheckedChanged(object sender, EventArgs e)
        {
            directionConfig = null;
            plainConfig = null;
        }
    }
}
