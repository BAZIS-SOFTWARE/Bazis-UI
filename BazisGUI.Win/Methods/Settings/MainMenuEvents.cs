using BazisGUI.Win.DataBases;
using BazisGUI.Win.Properties;
using BazisGUI.SceneCore.Interfaces;
using BazisGUI.Win.SettingsControls;
using Model.Interfaces;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var btn = sender as ToolStripMenuItem;
            var name = Resources.BaseForm_настройкиToolStripMenuItem_Click_Settings;

            if (btn.Checked)
            {
                var settings = new SettingsControl();
                settings.Leave += Settings_Leave;
                settings.SetSettings(settingsConfig);
                SetSettingsToConfig(settings);
                //Resources.SettingsControl_headerName_text;
                TabButtonsService.AddControl(name, settings);
            }
            else
                TabButtonsService.RemoveControl(name);
        }

        private void Settings_Leave(object sender, EventArgs e)
        {
            SaveConfig(settingsConfig);
        }

        private void SetSettingsToConfig(SettingsControl settings)
        {

            settings.SetSelectionGroupColorEvent += (ar) =>

            {
                settingsConfig.SelectGroupColor = ar;
            };
            settings.SetSelectionObjectColorEvent += (ar) =>
            { 
                settingsConfig.SelectObjectColor = ar;
                ApplySelectionColor();
            };

            settings.SetNodeColorEvent += (ar) =>
            {
                settingsConfig.NodeColor = ar;
                var setInfo = project.GetModelSetsInfo(ObjType.Узел).FirstOrDefault();
                if (setInfo != null)
                    project.SetColor(setInfo, ar);
            };

            settings.SetSolverPathEvent += (ar) =>
            {
                settingsConfig.SolverPath = ar;
            };
            settings.SetBackGroundColorEvent += (ar) =>
            {
                settingsConfig.BackGroundColor = ar;
                sceneController.BackGroundColor = ar; // сам averageColorRenderer.BackgroundColor обновит sceneController
                RequestRedraw();
            };


            settings.SetLightingEvent += (ar) =>
            {
                settingsConfig.Lighting = ar;
                sceneController.IsLighting = ar; // тоже синхронизирует averageColorRenderer.IsLighting
                RequestRedraw();
            };

            settings.SetTransparencyEvent += (ar) =>
            {
                settingsConfig.Transparency = ar;
                averageColorRenderer.IsEnable = ar;
                // Альфа ModelView задаётся только в режиме прозрачности (GetModelViewTransparency)
                project?.SetTransparency(GetModelViewTransparency());
                ClearAllDataOnScene();
                if(project != null)
                    CreateVBObjects("Объекты");
                RequestRedraw();
            };

            settings.SetOrtoProjectionEvent += (ar) =>
            {
                settingsConfig.Projection = ar ? ViewProjection.Parallel : ViewProjection.Perspective;
                sceneController.Projection = settingsConfig.Projection;
                UpdateProjection();
                RequestRedraw();
            };

            settings.SetTransparencyValueEvent += (ar1) =>
            {
                settingsConfig.TransparencyValue = ar1;
                if (project != null)
                    project.SetTransparency(GetModelViewTransparency());
            };

            settings.SetLightingIntensityEvent += (ar) =>
            {
                settingsConfig.LightingIntensity = ar;
                var lightAttenuation = 1 - ar / 100.0f;
                GL.Light(LightName.Light0, LightParameter.LinearAttenuation, lightAttenuation);
                RequestRedraw();
            };


            settings.SetLighterPositionEvent += (ar) =>
            {
                var kx = (float)(Width / settings.Width);
                var ky = (float)(Height / settings.Height);

                var x = ar.X * kx;
                var y = ar.Y * ky;

                settingsConfig.LighterPosition.X = (int)x;
                settingsConfig.LighterPosition.Y = (int)y;
                sceneController.LightTranslateX = x;
                sceneController.LightTranslateY = y;

                RequestRedraw();
            };

            settings.SetLanguageEvent += (ar) => 
            {
                if (settingsConfig.Language != ar)
                {
                    MessageBox.Show(Resources.MainMenuEvents_ChangeLanguage_Message, Localization.Localization.GetAttentionCaption());
                    settingsConfig.Language = ar;
                }
            };
        }
    }
}
