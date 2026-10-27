using BazisGUI.Win.Console;
using BazisGUI.SceneCore.Interfaces;
using BazisGUI.SceneCore.VBO;
using Model.Interfaces;
using Model.Interfaces.MeshObjects;
using Model.Interfaces.ObjectsCollections;
using OperationalController;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {

        private void btnShowSidesRibs_Click(object sender, EventArgs e)
        {
            ChangeViewModeObjects(ViewMode.LineSurface);
        }

        private void btnShowRibs_Click(object sender, EventArgs e)
        {
            ChangeViewModeObjects(ViewMode.Line);
        }

        private void btnShowSides_Click(object sender, EventArgs e)
        {
            ChangeViewModeObjects(ViewMode.Surface);
        }      

        private void ChangeViewModeObjects(ViewMode arg2)
        {
            try
            {
                using (project.BeginViewUpdate())
                {
                    foreach (var item in project.GetModelSetsInfo(ObjType.Поверхность))
                        project.SetViewMode(item, arg2);
                    foreach (var item in project.GetModelSetsInfo(ObjType.Элемент2D))
                        project.SetViewMode(item, arg2);
                    foreach (var item in project.GetModelSetsInfo(ObjType.Элемент3D))
                        project.SetViewMode(item, arg2);
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }



        private void btnShowNormals_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = (ToolStripButton)sender;
                if (btn.Checked)
                {
                    var surfElems = project.GetModelSurfaceElements(2);// ModelData.ObjectData.GetAllElements().Where(x => x is ISurfaceElement);
                    if (surfElems.Count() > 0)
                    {
                        var elemsNormals = project.CalcElemsNormals(3);

                        var linePresenter = presentersCreator.CreateLineObjectsPresenter(elemsNormals.ToList(), Color.DarkGray);
                        linePresenter.Name = "Normals";
                        var vbo = CreateVBObject(linePresenter);
                        VBOController.AddVbo(vbo);
                    }
                    else
                        throw new Exception("Для отображения нормалей модели не заданы объекты типа \"Элемент\"");
                }
                else
                {
                    VBOController.DeleteVBObjects("Normals");
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
