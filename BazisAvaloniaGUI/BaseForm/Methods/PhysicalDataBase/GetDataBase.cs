using BazisAvaloniaGUI.Localization;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;

namespace BazisAvaloniaGUI
{
    internal partial class BaseForm
    {
        public T GetDataBase<T>(string dbName, string dbPath)
        {
            var filePath = FindFileByPath(dbPath, dbName);
            if (filePath == null)
            {
                console.PrintInfo($"{Resources.GetDAtaBase_FindFileByPath_DBNotFound_Message_Part1} {dbName} {Resources.GetDAtaBase_FindFileByPath_DBNotFound_Message_Part2} {dbPath}", Color.Orange);
                return default;
            }

            else
            {
                var settingsSerializer = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented,
                };

                return JsonConvert.DeserializeObject<T>
    (File.ReadAllText(Path.Combine(dbPath, dbName)), settingsSerializer);
            }
        }
    }
}
