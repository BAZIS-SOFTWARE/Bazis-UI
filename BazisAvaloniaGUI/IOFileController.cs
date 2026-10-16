using System.IO;

namespace BazisAvaloniaGUI
{
    public static class IOFileController
    {
        public static bool CopyFile(string fileName, string oldFolder, string newFolder)
        {
            var oldfilePath = Path.Combine(oldFolder, fileName);

            if (File.Exists(oldfilePath))
            {
                var newfilePath = Path.Combine(newFolder, fileName);

                File.Create(newfilePath).Close();
                File.Copy(oldfilePath, newfilePath, true);
                return true;
            }
            else return false;
        }
    }
}
