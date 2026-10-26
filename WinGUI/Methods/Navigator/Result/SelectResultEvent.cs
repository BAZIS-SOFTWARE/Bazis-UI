using WinGUI.Properties;
using WinGUI.PropertiesPanel;
using ResultDB.IO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;

namespace WinGUI
{
    public partial class BaseForm
    {
        private void navigator_SelectResultEvent(string arg2)
        {
            try
            {
                checkPlayerControl.StopChecking();
                CheckPlayerControl_StopCheckingEvent(checkPlayerControl);
                var row = new RowProperty(ResultPropertyKeys.Result.ToString(), Resources.Header_result_result, arg2);
                propertiesPanel.DrawTable(new List<RowProperty>() { row });

                var loader = new LoadResultsFileDB();
                var times = loader.GetValues($@"{ResultDbPath}", "nodes", "Time");
                checkPlayerControl.StartValue = 0;
                checkPlayerControl.StopValue = times.Count() - 1;
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }
    }
}
