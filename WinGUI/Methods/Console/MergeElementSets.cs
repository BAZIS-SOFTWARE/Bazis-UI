using System;
using System.Linq;
using Model.Interfaces;
using System.Drawing;
using WinGUI.Navigator;
using ResultDB.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using UserControlsEx.Graph;
using Geometry;
using Model.MeshObjects;
using WinGUI.Console;
using WinGUI.Scene.VBO;

namespace WinGUI
{
    public partial class BaseForm
    {
        private async void MergeEventSets(string objTypeStr, string masterSet, string slaveSet)
        {
            try
            {
                // выбор объектов
                ObjType objType;
                if (!ObjType.TryParse(objTypeStr, out objType))
                    throw new Exception("Неизвестный тип объектов");

                // Выделение снимается до слияния: событие Changed от ClearSelection
                // перекрашивает буферы наборов по их текущему составу, а состав
                // master/slave меняется только ниже, явным пересозданием буферов.
                project.ClearSelection();

                project.MergeElements(objType, masterSet, slaveSet);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
