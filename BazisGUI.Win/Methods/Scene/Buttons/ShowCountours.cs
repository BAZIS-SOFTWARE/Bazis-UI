using BazisGUI.SceneCore.Interfaces;
using BazisGUI.SceneCore.Interfaces;
using System;
using Geometry;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using BazisGUI.Win;
using System.Windows.Forms;
using Model.Interfaces;
using System.Linq;
using Model.Interfaces.MeshObjects;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {

        private void btnBorder_Click(object sender, EventArgs e)
        {
            try
            {

                if (!bool.Parse(btnBorder.Tag.ToString()))
                {
                    ChangeInsideObjects(false);
                    btnBorder.Tag = true;
            //        var surfElems = project.GetAllModelElements().Where(x => x is ISurfaceElement).
            //Select(x => (ISurfaceElement)x);
                    var linesNodes = project.FindBoundaryEdges();
                    var edges = project.CreateBoundaryEdges(linesNodes);
                    var linePresenter = presentersCreator.CreateLineObjectsPresenter(edges.ToList(), Color.DarkGray);
                    linePresenter.Name = "Boundary";
                    var vbo = CreateVBObject(linePresenter);
                    VBOController.AddVbo(vbo);
                }
                else
                {
                    btnBorder.Tag = false;
                    VBOController.DeleteVBObjects("Boundary");
                }
                    
                RequestRedraw();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
