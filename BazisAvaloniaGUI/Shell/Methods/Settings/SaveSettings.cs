using BazisAvaloniaGUI.SettingsControls;
using Newtonsoft.Json;
using System;
using System.IO;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        /// <summary>Конфигурация в том виде, в каком она последний раз сохранена (или открыта) — для поиска изменений.</summary>
        private string savedConfigSnapshot;

        /// <summary>Окно закрывается — сообщения о сохранении настроек не показываются.</summary>
        private bool isClosing;

        private static string SerializeConfig(SettingsConfig config)
        {
            var settingsSerializer = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(config, settingsSerializer);
        }

        /// <summary>Запоминает текущее состояние конфигурации как сохранённое.</summary>
        private void RememberSavedConfig() => savedConfigSnapshot = SerializeConfig(settingsConfig);

        /// <summary>
        /// Сохраняет конфигурацию и сообщает об этом, только если она изменилась с последнего сохранения.
        /// Уход фокуса со страницы настроек без изменений ничего не делает.
        /// </summary>
        private void SaveConfigIfChanged(bool showMessage = true)
        {
            if (SerializeConfig(settingsConfig) != savedConfigSnapshot)
                SaveConfig(settingsConfig, showMessage);
        }

        public void SaveConfig(SettingsConfig config, bool showMessage = true)
        {
            try
            {
                var configString = SerializeConfig(config);

                var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                File.WriteAllText(Path.Combine(folder, "settingsConfig.json"), configString);
                savedConfigSnapshot = configString;

                if (showMessage)
                    MessageBox.Show(this, $@"Конфигурация сохранена в {Path.Combine(folder, "settingsConfig.json")}");
            }
            catch (Exception)
            {
                if (showMessage)
                    MessageBox.Show(this, $@"Ошибка сохранения конфигурации");
            }
        }
    }
}
