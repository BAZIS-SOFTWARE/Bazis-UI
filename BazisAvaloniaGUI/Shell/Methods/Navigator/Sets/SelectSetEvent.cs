using BazisAvaloniaGUI.Extensions;
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
        enum SetPropertyKeys { Name, View, PrecisionOrder, AdjacentNodes, Group, Color }
        private void navigator_SelectSetEvent(string arg2)
        {
            try
            {
                ObjType objType;
                var setName = arg2.Split(' ')[1];
                // пока заглушим обработку объема
                if (arg2.Split(' ')[0].TryToEnum(out objType))
                {
                    if (objType == ObjType.Узел | objType == ObjType.Элемент1D | objType == ObjType.Элемент2D | objType == ObjType.Элемент3D)
                    {
                        var set = project.GetModelSetInfo(objType, setName);
                        if (set != null)
                        {
                            var rows = GetSetProperty(set);

                            if (objType != ObjType.Узел)
                            {
                                var firstObj = project.GetModelObject(objType, set.GetNumbers().First());
                                var level = project.GetModelElements(firstObj.Dim, setName).FirstOrDefault().Level;

                                rows.Add(new RowProperty(SetPropertyKeys.PrecisionOrder.ToString(),
                                    Resources.Header_set_precisionOrder,
                                    new DropDownPropertyValue(level, new List<string>() { "1", "2" })));

                                rows.Add(new RowProperty(SetPropertyKeys.AdjacentNodes.ToString(),
                                    Resources.Header_set_adjacentNodes,
                                    new ButtonPropertyValue(Resources.Header_set_show, () => { ShowAdjacenciesSet(objType, setName); })));

                                rows.Add(new RowProperty(SetPropertyKeys.Group.ToString(),
                                    Resources.Header_set_group,
                                    new ButtonPropertyValue(Resources.Header_set_create, () =>
                                    {
                                        var objects = set.GetNumbers().Select(num => project.GetModelObject(objType, num)).ToList();
                                        project.CreateGroup(set.Name, objects);
                                        var group = project.GetAllModelGroups().Last();
                                        console.PrintInfo($"{Resources.SelectSetEvent_CreateGroupBySet_Message}: {group.Name}", Color.Black);

                                        PresentGroupDataOnTree();
                                        OnGroupCreated?.Invoke(group.ObjType, group.Number, group.Name);
                                    })));
                            }
                            propertiesPanel.DrawTable(rows);
                        }

                    }
                    else if (objType == ObjType.Поверхность | objType == ObjType.Кривая | objType == ObjType.Точка)
                    {
                        var set = project.GetModelSetInfo(objType, setName);
                        var rows = GetSetProperty(set);
                        propertiesPanel.DrawTable(rows);
                    }
                }

                else
                {
                    var vol = project.GetModelVolumes().FirstOrDefault(x => x.Name == setName);
                    /*
                     * TO DO Реализовать для объема
                     */
                }


            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }

        }
    }
}
