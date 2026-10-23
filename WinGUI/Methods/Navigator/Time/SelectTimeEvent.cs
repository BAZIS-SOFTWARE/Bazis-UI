using SceneCore.VBO;
using ResultDB;
using ResultDB.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WinGUI
{
    public partial class BaseForm
    {
        private void navigator_SelectTimeEvent(string arg1, double arg2)
        {
            // TO DO

            var loader = new LoadResultsFileDB();
            var tables = new List<string>()
            {
                ResultType.nodes.ToString(),
                ResultType.elements.ToString()
            };
            var res = loader.GetResult(ResultDbPath, tables, (float)arg2);
            ShowResults(res, arg1);
        }
        public void ShowResults(Result result, string resName)
        {
            try
            {
                var tableName = ResultType.nodes.ToString();

                if (settingsConfig.MergeResultsValue)
                    MergeResults(result);

                if (settingsConfig.ShowResultsField)
                {
                    if (!settingsConfig.IsScaleMaxMinManual)
                    {
                        var res = GetMaxMin(result, tableName, resName);
                        var intervals = settingsConfig.Scale_Intervals;
                        var pre = settingsConfig.Scale_Precision;
                        resultsController.FillRange(res.Item2, res.Item1, intervals, pre);
                        //scale.FillRange(res.Item2, res.Item1, settingsConfig.Scale_Intervals);
                    }

                    ClearAllGeometryDataOnScene();
                    ClearAllMeshDataOnScene();
 

                    if (settingsConfig.ShowResultsScale)
                    {
                        HideGeometryObj("DisplaySceneScale");
                        var title = result.Name;
                        var info = $"{resName} {result.Time}";
                        DisplaySceneScale(title,info);
                    }

                    PresentResultsField(result, resName, tableName);
                }

                if (settingsConfig.ShowNodeResultsValue)
                {
                    DisplayText3DEvent = null;
                    ShowResultValue(ResultType.nodes, resName, result);
                }


                if (settingsConfig.ShowElementsResultsValue)
                {
                    DisplayText3DEvent = null;
                    ShowResultValue(ResultType.elements, resName, result);
                }

                RequestRedraw();

            }
            catch (Exception ex)
            {
                console.PrintInfo(Localization.Localization.GetErrorWithSource(ex), Color.Red);
            }
        }
    }
}
