using BazisGUI.Win.Properties;
using BazisGUI.MasterInterface.Interfaces;
using Model.Interfaces;
using Project.Interfaces.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        public void HandleBaseMaster(IBaseMaster master)
        {
            master.UpdateSceneEvent += (s, e) =>
            {
                ClearAllDataOnScene();
                foreach (var item in Enum.GetValues<ObjType>())
                    CreateVBObjsByObjsType(item);
            };
            master.PrintInfoEvent += (s, e) => console.PrintInfo(e.Message, e.Color);
            master.GenerateConditionsEvent += (s, e) =>
            {
                var res = MessageBox.Show(Resources.HandleBaseMaster_Handle_CondCreation_Message,
                    Localization.Localization.GetAttentionCaption(), MessageBoxButtons.YesNo);
                if (res == DialogResult.No)
                    return;

                project.ClearTaskData();
                foreach (var item in e.InputStrings)
                {
                    var args = item.Split(':').Select(x => x.Trim()).ToArray();
                    var kind = Enum.Parse<DataKind>(args[0]);
                    var cond = project.Create(kind, args[1]);
                    project.AddTaskData(cond);

                }
                console.PrintInfo(Resources.HandleBaseMaster_Success_Message, Color.Green);
            };
        }
    }
}
