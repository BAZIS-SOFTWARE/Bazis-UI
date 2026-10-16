using System.Globalization;
using System.Resources;

namespace BazisAvaloniaGUI.Databases
{
    internal static class DatabaseControlResources
    {
        public static string Get(string control, string key)
        {
            var resources = new ResourceManager($"BazisAvaloniaGUI.Databases.{control}", typeof(DatabaseControlResources).Assembly);
            return resources.GetString(key, CultureInfo.CurrentUICulture) ?? resources.GetString(key, CultureInfo.InvariantCulture) ?? key;
        }
    }
}
