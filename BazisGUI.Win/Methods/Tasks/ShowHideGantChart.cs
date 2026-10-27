using BazisGUI.Win.GantChart;
using Project.Interfaces.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void показатьНаДиаграммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as ToolStripMenuItem;
                if (btn.Checked)
                {
                    var ganttContol = new cntrГант();
                    //BazisGUI.Win.Properties.Resources.cntrГант_headerName_text;
                    ganttContol.AddConds(project.GetAllCondData());
                    TabButtonsService.AddControl(btn.Text, ganttContol);
                }

                else
                    TabButtonsService.RemoveControl(btn.Text);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
