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
        public void PresentMeshData()
        {
            try
            {
                var searchMesh = navigator.TrySearchNodes(NodeName.Mesh, out List<TreeNode> mesh);

                var nodes = project.GetModelObjects(ObjType.Узел);

                // Наборы будут виртуальные
                if (nodes.Count() != 0)
                    if (searchMesh)
                    {
                        var v_node = navigator.CreateVirtualNode();
                        mesh[0].Nodes.Clear();
                        mesh[0].Nodes.Add(v_node);
                    }
                    else
                    {
                        var rn = navigator.CreateRealNode(NodeName.Mesh);

                        var v_node = navigator.CreateVirtualNode();
                        rn.Nodes.Add(v_node);

                        navigator.TrySearchNodes(NodeName.Project, out List<TreeNode> prNodes);
                        prNodes[0].Nodes.Add(rn);
                    }
                else
                {
                    if (searchMesh)
                        mesh.First().Remove();
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
