using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        enum ProjectMeshPropertyKeys { Nodes, Elements1D, Elements2D, Elements3D, Show, Hide, Delete }
        private void navigator_SelectMeshEvent()
        {
            try
            {
                List<RowProperty> rows = new List<RowProperty>
                {
                    // Узлы
                    new RowProperty(ProjectMeshPropertyKeys.Nodes.ToString(),
                    Resources.Header_projectMesh_nodes,
                    project.GetModelObjects(ObjType.Узел).Count(),
                    true),

                    // Элементы 1D
                    new RowProperty(ProjectMeshPropertyKeys.Elements1D.ToString(),
                    Resources.Header_projectMesh_elements1D,
                    project.GetAllModelElements().Where(x => x.ObjType == ObjType.Элемент1D).Count(),
                    true),

                    new RowProperty(ProjectMeshPropertyKeys.Show.ToString(), "",
                    new ButtonPropertyValue(Resources.Header_projectMesh_show,() =>
                    {
                        ShowElements(1,true);
                    })),

                    new RowProperty(ProjectMeshPropertyKeys.Hide.ToString(), "",
                    new ButtonPropertyValue(Resources.Header_projectMesh_hide,() =>
                    {
                        ShowElements(1, false);
                    })),

                    new RowProperty(ProjectMeshPropertyKeys.Delete.ToString(), "",
                    new ButtonPropertyValue(Resources.Header_projectMesh_delete,() =>
                    {
                        DelElements(1);
                    })),

                    // Элементы 2D
                    new RowProperty(ProjectMeshPropertyKeys.Elements2D.ToString(),
                    Resources.Header_projectMesh_elements2D,
                    project.GetAllModelElements().Where(x => x.ObjType == ObjType.Элемент2D).Count(),
                    true),

                    new RowProperty(ProjectMeshPropertyKeys.Show.ToString(), "",
                    new ButtonPropertyValue(Resources.Header_projectMesh_show, () =>
                    {
                        ShowElements(2, true);
                    })),

                    new RowProperty(ProjectMeshPropertyKeys.Hide.ToString(), "",
                    new ButtonPropertyValue(Resources.Header_projectMesh_hide, () =>
                    {
                        ShowElements(2, false);
                    })),

                    new RowProperty(ProjectMeshPropertyKeys.Delete.ToString(), "",
                    new ButtonPropertyValue(Resources.Header_projectMesh_delete, () =>
                    {
                        DelElements(2);
                    })),

                    // Элементы 3D
                    new RowProperty(ProjectMeshPropertyKeys.Elements3D.ToString(),
                    Resources.Header_projectMesh_elements3D,
                    project.GetAllModelElements().Where(x => x.ObjType == ObjType.Элемент3D).Count(),
                    true),

                    new RowProperty(ProjectMeshPropertyKeys.Show.ToString(), "",
                    new ButtonPropertyValue(Resources.Header_projectMesh_show, () =>
                    {
                        ShowElements(3, true);
                    })),

                    new RowProperty(ProjectMeshPropertyKeys.Hide.ToString(), "",
                    new ButtonPropertyValue(Resources.Header_projectMesh_hide, () =>
                    {
                        ShowElements(3, false);
                    })),

                    new RowProperty(ProjectMeshPropertyKeys.Delete.ToString(), "",
                    new ButtonPropertyValue(Resources.Header_projectMesh_delete, () =>
                    {
                        DelElements(3);
                    }))
                };

                propertiesPanel.DrawTable(rows);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }
    }
}
