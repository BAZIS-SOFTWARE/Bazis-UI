using BazisAvaloniaGUI.Navigator;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        public void PresentGeoData()
        {
            try
            {
                var searchGeo = navigator.TrySearchNodes(NodeName.Geometry, out List<TreeNode> geo);

                var points = project.GetModelObjects(ObjType.Точка);

                if (points.Count() != 0)
                {
                    if (searchGeo)
                    {
                        var v_node = navigator.CreateVirtualNode();
                        geo[0].Nodes.Clear();
                        geo[0].Nodes.Add(v_node);
                    }
                    else
                    {
                        var rn = navigator.CreateRealNode(NodeName.Geometry);

                        rn.Tag = "12,13,14";

                        var imgIndex = navigator.GetObjectImageIndex(NodeName.Geometry);

                        rn.ImageIndex = imgIndex;
                        rn.SelectedImageIndex = imgIndex;

                        var v_node = navigator.CreateVirtualNode();
                        rn.Nodes.Add(v_node);
                        navigator.TrySearchNodes(NodeName.Project, out List<TreeNode> prNodes);
                        prNodes[0].Nodes.Add(rn);
                    }
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
