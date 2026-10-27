using BazisGUI.Win.Properties;
using System;
using System.Windows.Forms;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private string LoadPythonFile()
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Python Files|*.py";
                fileDialog.Title = Resources.LoadPythonFile_ВыберитеPythonФайл;

                if (fileDialog.ShowDialog() == DialogResult.OK)
                    return fileDialog.FileName;
            }
            return string.Empty;
        }
    }
}



