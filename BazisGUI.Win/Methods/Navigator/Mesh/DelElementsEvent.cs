using BazisGUI.Win.Extensions;
using BazisGUI.Win.PropertiesPanel;
using BazisGUI.Win.Utilities;
using GmshApi;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using OperationalController;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void DelElements(int obj)
        {
            try
            {
                ObjType objType;
                if (obj == 1)
                    objType = ObjType.Элемент1D;
                else if (obj == 2)
                    objType = ObjType.Элемент2D;
                else
                    objType = ObjType.Элемент3D;

                var names = project.GetModelSetsInfo(objType).
    Select(x => x.Name).ToList();
                foreach (var item in names)
                    project.DeleteModelSet(objType, item);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
