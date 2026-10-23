using BazisGUI.PropertiesPanel;
using Model.Interfaces;
using Project.Interfaces.Tasks;
using Project.Tasks;
using Project.Tasks.FrameCreators;
using Project.Tasks.Functions;
using Project.Tasks.Functions.Welding;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;

namespace BazisGUI
{
    public partial class BaseForm
    {
        public List<RowProperty> GetHeatProperty(HeatData obj, IEnumerable<IGroup> groups, List<string> _func)
        {
            var rows = new List<RowProperty>
            {
                new RowProperty("Мощность, Дж", obj.Heat),
                new RowProperty("Функция, F(t), F - Дж.", 
                new DropDownPropertyValue(obj.TimeFunction == null ? "*" : obj.TimeFunction.Name,_func)),
                new RowProperty("Группа элементов", new DropDownPropertyValue(obj.Group.Name, 
                groups.
                Where(x => x.ObjType == obj.Group.ObjType).
                Select(x => x.Name).ToList())),
                new RowProperty("Старт, сек.", obj.StartTime),
                new RowProperty("Стоп, сек.", obj.StopTime),
            };
            var funcNames = new List<string>() { "*", "SPH", "CIL", "Custom" };
            
            if (obj.FrameFunction != null)
            {
                rows.Add(new RowProperty
                (
                "Функция, F(v(x,y,z)), F - Дж.", 
                new DropDownPropertyValue(obj.FrameFunction.Name, funcNames
                )));
                rows.AddRange(GetFrameFunctionProperties(obj.FrameFunction));
                rows.AddRange(GetLocalFrameProperties(obj.FrameFunction.LocalFrame, groups));
            }
            else
            {
                rows.Add(new RowProperty
                (
                "Функция, F(v(x,y,z)), F - Дж.",
                new DropDownPropertyValue("*",funcNames
                )));
            }


            return rows;
        }
    }
}
