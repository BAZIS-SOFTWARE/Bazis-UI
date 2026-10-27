using System.Globalization;
using System.Resources;

namespace BazisGUI.Avalonia.Databases
{
    internal static class DatabaseControlResources
    {
        public static string Get(string control, string key)
        {
            var resources = new ResourceManager($"{typeof(DatabaseControlResources).Namespace}.{control}", typeof(DatabaseControlResources).Assembly);
            return resources.GetString(key, CultureInfo.CurrentUICulture) ?? resources.GetString(key, CultureInfo.InvariantCulture) ?? key;
        }
    }
}
