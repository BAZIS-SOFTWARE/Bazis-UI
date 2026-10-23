using WinGUI.SettingsControls;
using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Linq;
using System.Windows.Forms;
using Model.Interfaces;
using WinGUI.DataBases;
using WinGUI.Scene.Interfaces;
using Newtonsoft.Json;
using System.IO;

namespace WinGUI
{
    public partial class BaseForm
    {
        public void SaveConfig(SettingsConfig config)
        {
            try
            {
                //config.BackGroundColor = panelBackGroundColor.BackColor;
                //config.SelectObjectColor = pnlSelectionObjsColor.BackColor;
                //config.SelectGroupColor = pnlSelectionGroupColor.BackColor;

                //config.SolverPath = lblSolverPath.Text;
                //config.Lighting = chbLighting.Checked;
                //config.LighterPosition = lightingControl.BallPosition;
                //config.LightingIntensity = clslLigthingIntensity.Value;
                //config.Transparency = chbTransparency.Checked;
                //config.BackRibbers = chbBackRibbers.Checked;
                //config.Projection = chbOrtoProjection.Checked == true ?
                //ViewProjection.Parallel : ViewProjection.Perspective;

                //config.TransparencyValue = clslTransparency.Value;
                //config.NodeColor = pnlNodeColor.BackColor;
                //};

                //SaveSettingsEvent();

                var settingsSerializer = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented
                };

                var configString = JsonConvert.SerializeObject(config, settingsSerializer);

                var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                File.WriteAllText($@"{folder}\settingsConfig.json", configString);

                MessageBox.Show($@"Конфигурация сохранена в {folder}\settingsConfig.json");
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Ошибка сохранения конфигурации");
            }
        }
    }
}
