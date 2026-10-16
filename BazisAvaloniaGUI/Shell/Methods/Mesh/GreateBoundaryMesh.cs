using BazisAvaloniaGUI.Localization;
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
        private void наПоверхности2DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CreateBoundaryMesh(1);
                PresentModelObjectsForSelection();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void наПоверхности3DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CreateBoundaryMesh(2);
                PresentModelObjectsForSelection();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
        public void CreateBoundaryMesh(int dim)
        {
            ObjType objType;
            if (dim == 2)
                objType = ObjType.Элемент3D;
            else if (dim == 1)
                objType = ObjType.Элемент2D;
            else
                return;
            var els = project.GetModelObjects(objType);
            if (els.Count() == 0)
                console.PrintInfo($"{Resources.GenerateBoundaryMesh_Generate2DOn3DSurfaces_NoObjects_Message} {dim + 1}D {Resources.GenerateBoundaryMesh_Generate2DOn3DSurfaces_Elements_Message}", Color.Red);
            else
            {
                if (dim == 2)
                    objType = ObjType.Элемент2D;
                else
                    objType = ObjType.Элемент1D;

                bool resFlag;
                if (dim == 2)
                    resFlag = project.Create2DForm3D();
                else
                    resFlag = project.Create1DFrom2D();

                if (resFlag)
                {
                    var set = project.GetModelSetsInfo(objType).LastOrDefault();

                    if (set != null)
                    {
                        var v = navigator.CreateVirtualNode(NodeName.Sets);
                        navigator.TrySearchNodes(NodeName.Mesh, out List<TreeNode> nodes);
                        nodes.First().Nodes.Add(v);
                    }

                    var pre = project.CreateModelObjectsPresentor(set);
                    var vbo = CreateVBObject(pre);
                    VBOController.AddVbo(vbo);

                    RequestRedraw();
                    console.PrintInfo($"{Resources.GenerateBoundaryMesh_CreateBoundaryMeh_ObjectsGenerated_Message} {set.ObjType.ToString()}: {set.NumberOfObjects}. {Resources.GenerateBoundaryMesh_CreateBoundaryMeh_CheckRecommendation_Message}", Color.Orange);
                }
            }
        }
    }
}
