using WinGUI.PropertiesPanel;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using static WinGUI.Methods.PlatformSpecific.PlatformSpecific;
using System.Linq;
using Project.Interfaces.Tasks;
using WinGUI.Properties;

namespace WinGUI
{
    public partial class BaseForm
    {
        enum GroupPropertyKeys { Name, Sort, Direction, ElementsNodes, CreateCond }
        enum CreateCondByGroup { Heat, Material, Media, Clamp, Load }
        public List<RowProperty> GetGroupProperty(IGroup obj)
        {
            var rows = new List<RowProperty>();

            rows.Add(new RowProperty(GroupPropertyKeys.Name.ToString(), 
                Resources.Header_groups_name,
                obj.Name));

            if (obj.ObjType == ObjType.Узел)
            {
                rows.Add(new RowProperty(GroupPropertyKeys.Sort.ToString(),
                    Resources.Header_groups_sort,
                    new ButtonPropertyValue(Resources.Отсортировать,
                    new Action(obj.SortByDistance))));

                rows.Add(new RowProperty(GroupPropertyKeys.Direction.ToString(),
                    Resources.Header_groups_direction,
                    new ButtonPropertyValue(Resources.Показать, 
                    new Action(async() => await NewMethod2(obj)))));

                rows.Add(new RowProperty(GroupPropertyKeys.Direction.ToString(),
                    Resources.Header_groups_direction,
                    new ButtonPropertyValue(Resources.Реверс,
                    new Action(obj.Reverse))));
            }
            else
            {
                rows.Add(new RowProperty(GroupPropertyKeys.ElementsNodes.ToString(),
                    Resources.Header_groups_elementsNodes,
                    new ButtonPropertyValue(Resources.Header_group_show,
                    new Action(() => ShowGroupWithNodes(obj)))));
            }

            // показываем возможные условия
            //var res = project.TaskData.Find(obj.Name);
            //if (res.Count() == 0)
                if(obj.ObjType != ObjType.Узел)
                    CreateElementsConditionsProperties(obj, rows);
                else
                    CreateNodesConditionsProperties(obj, rows);

            return rows;
        }

        private async Task NewMethod2(IGroup obj)
        {
            var objs = new List<IModelObject>() { obj.First(), obj.Last() };

            await Task.Run(new Action(() =>
            {
                DisplayGeometryObjectEvent = null;
                foreach (var item in objs)
                {
                    NewMethod3(item);
                    Task.Delay(500);
                    Invoke(new Action(DisplayObjects));
                }
            }));
        }

        private void NewMethod3(IModelObject item)
        {
            DisplayGeometryObjectEvent += new Action(() =>
            {
                var quadObj = gluNewQuadric(); // создаем новый объект
                                               // для создания сфер и цилиндров
                                               //Glu.gluQuadricOrientation(quadObj, Glu.GLU_OUTSIDE);
                GL.PushMatrix();
                GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
                GL.Color3(1f, 0, 0);
                GL.Translate(-Position._x, -Position._y, -Position._z);

                GL.Translate(item._x, item._y, item._z);


                //Glu.gluQuadricDrawStyle(quadObj, Glu.GLU_FILL); // устанавливаем
                gluSphere(quadObj, 1.5, 10, 10); // рисуем сферу
                                                 // радиусом 0.5
                GL.PopMatrix();
                gluDeleteQuadric(quadObj);
            });
        }

        private void CreateNodesConditionsProperties(IGroup obj, List<RowProperty> rows)
        {
            if (project.TaskKind == TaskKind.термическая)
            {
                rows.Add(new RowProperty(GroupPropertyKeys.CreateCond.ToString(),
                    Resources.Header_groups_createCond,
                    new DropDownPropertyValue("*",
                    new List<string>() 
                    {
                        CreateCondByGroup.Heat.ToString(),
                        CreateCondByGroup.Media.ToString()
                    })));
            }

            else
            {
                rows.Add(new RowProperty(GroupPropertyKeys.CreateCond.ToString(),
                    Resources.Header_groups_createCond,
                    new DropDownPropertyValue("*",
                    new List<string>() 
                    {
                        CreateCondByGroup.Clamp.ToString(),
                        CreateCondByGroup.Load.ToString()
                    })));
            }
        }

        private void CreateElementsConditionsProperties(IGroup obj, List<RowProperty> rows)
        {
            if (project.TaskType == TaskType.Volume)
            {
                if (obj.ObjType == ObjType.Элемент3D)
                {
                    if (project.TaskKind == TaskKind.термическая | 
                        project.TaskKind == (TaskKind.термическая | TaskKind.механическая))
                        rows.Add(new RowProperty(GroupPropertyKeys.CreateCond.ToString(),
                            Resources.Header_groups_createCond,
                            new DropDownPropertyValue("*",
                            new List<string>() 
                            {
                                CreateCondByGroup.Material.ToString(),
                                CreateCondByGroup.Heat.ToString(),
                            })));

                    else if (project.TaskKind == TaskKind.механическая)
                        rows.Add(new RowProperty(GroupPropertyKeys.CreateCond.ToString(),
                            Resources.Header_groups_createCond,
                            new DropDownPropertyValue("*",
                            new List<string>()
                            {
                                CreateCondByGroup.Material.ToString()
                            })));
                }

                else if (obj.ObjType == ObjType.Элемент2D)
                    if (project.TaskKind == TaskKind.термическая |
                        project.TaskKind == (TaskKind.термическая | TaskKind.механическая))
                        rows.Add(new RowProperty(GroupPropertyKeys.CreateCond.ToString(),
                            Resources.Header_groups_createCond,
                            new DropDownPropertyValue("*",
                            new List<string>()
                            {
                                CreateCondByGroup.Media.ToString()
                            })));
            }

            else if (project.TaskType == TaskType.AxiPlain | project.TaskType == TaskType.Plain)
            {
                if (obj.ObjType == ObjType.Элемент2D)
                    rows.Add(new RowProperty(GroupPropertyKeys.CreateCond.ToString(),
                        Resources.Header_groups_createCond,
                        new DropDownPropertyValue("*",
                        new List<string>()
                        {
                            CreateCondByGroup.Material.ToString()
                        })));

                else if (obj.ObjType == ObjType.Элемент1D)
                {
                    if (project.TaskKind == TaskKind.термическая |
                        project.TaskKind == (TaskKind.термическая | TaskKind.механическая))
                        rows.Add(new RowProperty(GroupPropertyKeys.CreateCond.ToString(),
                            Resources.Header_groups_createCond,
                            new DropDownPropertyValue("*",
                            new List<string>()
                            {
                                CreateCondByGroup.Material.ToString(),
                                CreateCondByGroup.Media.ToString()
                            })));

                    else if (project.TaskKind == TaskKind.механическая)
                        rows.Add(new RowProperty(GroupPropertyKeys.CreateCond.ToString(),
                            Resources.Header_groups_createCond,
                            new DropDownPropertyValue("*",
                            new List<string>()
                            {
                                CreateCondByGroup.Material.ToString()
                            })));
                } 
            }

            else if (project.TaskType == TaskType.Linear)
            {
                if (obj.ObjType == ObjType.Элемент1D)
                    rows.Add(new RowProperty(GroupPropertyKeys.CreateCond.ToString(),
                        Resources.Header_groups_createCond,
                        new DropDownPropertyValue("*",
                        new List<string>()
                        {
                            CreateCondByGroup.Material.ToString()
                        })));
            }

            else
            {
                if (obj.ObjType == ObjType.Элемент1D)
                    rows.Add(new RowProperty(GroupPropertyKeys.CreateCond.ToString(),
                        Resources.Header_groups_createCond,
                        new DropDownPropertyValue("*",
                        new List<string>()
                        {
                            CreateCondByGroup.Material.ToString()
                        })));

                else if (obj.ObjType == ObjType.Элемент2D)
                    rows.Add(new RowProperty(GroupPropertyKeys.CreateCond.ToString(),
                        Resources.Header_groups_createCond,
                        new DropDownPropertyValue("*",
                        new List<string>()
                        {
                            CreateCondByGroup.Material.ToString(),
                            CreateCondByGroup.Media.ToString()
                        })));

                else if (obj.ObjType == ObjType.Элемент3D)
                    rows.Add(new RowProperty(GroupPropertyKeys.CreateCond.ToString(),
                        Resources.Header_groups_createCond,
                        new DropDownPropertyValue("*",
                        new List<string>()
                        {
                            CreateCondByGroup.Material.ToString()
                        })));
            }
        }
    }
}