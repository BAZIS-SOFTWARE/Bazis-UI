using Geometry;
using BazisGUI.Avalonia.Localization;
using BazisGUI.Avalonia.Properties;
using Model.Interfaces;
using Project.Interfaces.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace BazisGUI.Avalonia.Shell
{
    internal partial class MainWindow
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
                    new Action(() => NewMethod2(obj)))));

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
            if (obj.ObjType != ObjType.Узел)
                CreateElementsConditionsProperties(obj, rows);
            else
                CreateNodesConditionsProperties(obj, rows);

            return rows;
        }

        /// <summary>
        /// Показ направления группы: сферы в первом и последнем объектах группы
        /// (в BaseForm — gluSphere через DisplayGeometryObjectEvent).
        /// </summary>
        // Синхронный метод: исключение доходит до обработчика щелчка панели свойств,
        // а не теряется в async void-лямбде.
        private void NewMethod2(IGroup obj)
        {
            // Пустая группа: First()/Last() бросили бы InvalidOperationException.
            if (!obj.Any())
            {
                console.PrintInfo(Resources.ConsoleEvents_ConsoleInEvent_ObjectNotFound_Message, Color.Orange);
                return;
            }

            var objs = new List<IModelObject>() { obj.First(), obj.Last() };

            HideAllGeometryObjects();
            foreach (var item in objs)
            {
                var centre = new Point3D(item._x, item._y, item._z);
                scene.Surface.Invoke(sceneController => sceneController.DisplayMarker(centre, 1.5f, Color.Red));
            }
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
