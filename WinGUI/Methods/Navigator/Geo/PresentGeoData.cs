using WinGUI.Extensions;
using WinGUI.Navigator;
using WinGUI.PropertiesPanel;
using WinGUI.Utilities;
using GmshApi;
using Model.Interfaces;
using OperationalController;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinGUI
{
    public partial class BaseForm
    {
        public void PresentGeoData()
        {
            try
            {
                //navigator.BeginUpdate();

                var searchGeo = navigator.TrySearchNodes(NodeName.Geometry, out List<TreeNode> geo);

                var points = project.GetModelObjects(ObjType.Точка);

                if (points.Count() != 0)
                {
                    //var types = new List<ObjType>() { ObjType.Точка, ObjType.Кривая, ObjType.Поверхность };
                    if (searchGeo)
                    {
                        var v_node = navigator.CreateVirtualNode();
                        geo[0].Nodes.Clear();
                        geo[0].Nodes.Add(v_node);
                    }
                    //PresentObjects(geo.First(), types);
                    else
                    {
                        var rn = navigator.CreateRealNode(NodeName.Geometry);

                        //navigator.SetContextMenu(rn);

                        rn.Tag = "12,13,14";

                        var imgIndex = navigator.GetObjectImageIndex(NodeName.Geometry);

                        rn.ImageIndex = imgIndex;
                        rn.SelectedImageIndex = imgIndex;

                        var v_node = navigator.CreateVirtualNode();
                        rn.Nodes.Add(v_node);
                        //PresentObjects(rn, types);
                        navigator.TrySearchNodes(NodeName.Project, out List<TreeNode> prNodes);
                        prNodes[0].Nodes.Add(rn);
                    }
                    //PresentVolumeInfo();
                }

                else
                {
                    if (searchGeo)
                        geo.First().Remove();
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
