using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Drawing;
using WinGUI.Scene.Interfaces;
using System.Linq;
using WinGUI.Localization;
using WinGUI.Properties;

namespace WinGUI.SettingsControls
{
    public partial class SettingsControl : UserControl//, ILocalizableHeaderControl
    {
        enum Culture { en, ru }
        //public event Action SaveSettingsEvent;
        public event Action<Color> SetSelectionObjectColorEvent;
        public event Action<Color> SetSelectionGroupColorEvent;
        public event Action<Color> SetBackGroundColorEvent;

        public event Action<Color> Set3DElemColorEvent;
        public event Action<Color> Set2DElemColorEvent;
        public event Action<Color> SetNodeColorEvent;

        public event Action<string> SetSolverPathEvent;
        public event Action<bool> SetLightingEvent;
        public Action<int> SetLightingIntensityEvent;
        public Action<Point> SetLighterPositionEvent;
        public Action<bool> SetTransparencyEvent;
        public Action<int> SetTransparencyValueEvent;

        public Action<bool> SetOrtoProjectionEvent;

        public Action<string> SetLanguageEvent;

        public SettingsControl()
        {
            InitializeComponent();

            lightingControl.SetBallPositionEvent += (ar) =>
            {
                SetLighterPositionEvent?.Invoke(ar);
            };

            cmbLanguage.Items.Clear();
            cmbLanguage.Items.AddRange(Enum.GetValues<Culture>().Select(GetLanguageByCulture).ToArray());
        }

        //public string GetLocalizableHeaderText() => 

        public void SetSettings(SettingsConfig settingsConfig)
        {
            panelBackGroundColor.BackColor = settingsConfig.BackGroundColor;
            pnlSelectionObjsColor.BackColor = settingsConfig.SelectObjectColor;
            pnlSelectionGroupColor.BackColor = settingsConfig.SelectGroupColor;
            lblSolverPath.Text = settingsConfig.SolverPath;
            chbLighting.Checked = settingsConfig.Lighting;
            chbBackRibbers.Checked = settingsConfig.BackRibbers;
            lightingControl.BallPosition = settingsConfig.LighterPosition;
            clslLigthingIntensity.Value = settingsConfig.LightingIntensity;
            chbTransparency.Checked = settingsConfig.Transparency;
            pnlNodeColor.BackColor = settingsConfig.NodeColor;
            chbOrtoProjection.Checked = settingsConfig.Projection == ViewProjection.Parallel ?
                true : false;
            clslTransparency.Value = settingsConfig.TransparencyValue;

            cmbLanguage.Text = GetLanguageByCulture(ParseCulture(settingsConfig.Language));
        }

        private Culture GetCultureByLanguage(string language)
        {
            switch (language)
            {
                case "Русский":
                    return Culture.ru;
                default:
                    return Culture.en;
            }
        }

        private string GetLanguageByCulture(Culture culture)
        {
            switch (culture)
            {
                case Culture.ru:
                    return "Русский";
                default:
                    return "English";
            }
        }

        private Culture ParseCulture(string culture)
        {
            switch (culture)
            {
                case "ru":
                    return Culture.ru;
                default:
                    return Culture.en;
            }
        }

        private void btnSelectObjectColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            pnlSelectionObjsColor.BackColor = dialog.Color;

            SetSelectionObjectColorEvent?.Invoke(pnlSelectionObjsColor.BackColor);
        }

        private void btnSelectGroupColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            pnlSelectionGroupColor.BackColor = dialog.Color;

            SetSelectionGroupColorEvent?.Invoke(pnlSelectionGroupColor.BackColor);
        }

        private void btnBackGroundColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            panelBackGroundColor.BackColor = dialog.Color;

            SetBackGroundColorEvent?.Invoke(panelBackGroundColor.BackColor);
        }

        private void btnSetSolverPath_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "(*.exe)|*.exe" };

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            lblSolverPath.Text = dialog.FileName;

            SetSolverPathEvent?.Invoke(lblSolverPath.Text);
        }

        private void chbLighting_Click(object sender, EventArgs e)
        {
            SetLightingEvent?.Invoke(chbLighting.Checked);
        }

        private void chbTransparency_Click(object sender, EventArgs e)
        {
            SetTransparencyEvent?.Invoke(chbTransparency.Checked);
        }

        private void chbBackRibbers_Click(object sender, EventArgs e)
        {

        }

        private void clslLigthingIntensity_Scroll(object sender, ScrollEventArgs e)
        {
            SetLightingIntensityEvent?.Invoke(e.NewValue);
        }

        private void clslTransparency_Scroll(object sender, ScrollEventArgs e)
        {
            SetTransparencyValueEvent?.Invoke(e.NewValue);
        }

        private void btnSelect3DElemColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            pnl3DElemColor.BackColor = dialog.Color;

            Set3DElemColorEvent?.Invoke(pnl3DElemColor.BackColor);
        }

        private void btnSelect2DElemColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            pnl2DElemColor.BackColor = dialog.Color;

            Set2DElemColorEvent?.Invoke(pnl2DElemColor.BackColor);
        }

        private void btnSelectNodeColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            pnlNodeColor.BackColor = dialog.Color;

            SetNodeColorEvent?.Invoke(pnlNodeColor.BackColor);
        }

        private void chbOrtoProjection_Click(object sender, EventArgs e)
        {
            if (chbOrtoProjection.Checked)
                SetOrtoProjectionEvent?.Invoke(true);
            else
                SetOrtoProjectionEvent?.Invoke(false);
        }

        private void cmbLanguage_TextChanged(object sender, EventArgs e)
        {
            SetLanguageEvent?.Invoke(GetCultureByLanguage(cmbLanguage.Text).ToString());
        }
    }
}
