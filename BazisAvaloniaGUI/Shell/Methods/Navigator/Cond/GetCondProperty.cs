using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using Model.Interfaces;
using Project.Interfaces.Tasks;
using Project.Tasks;
using Project.Tasks.Functions;
using Project.Tasks.Functions.FrameFunctions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        enum CondPropertyKeys { ObjectsGroup, Value, Function, Parameter, ParameterValue, Table, File, Direction, StartTime, StopTime, CoordinateSystem }
        public List<RowProperty> GetCondProperty(CondData obj, IEnumerable<IGroup> groups, List<string> funcTables)
        {
            var rows = new List<RowProperty>();

            rows.Add(new RowProperty(CondPropertyKeys.ObjectsGroup.ToString(),
                Resources.Header_cond_objectsGroup,
                new DropDownPropertyValue(obj.Group.Name,
                groups.Select(x => x.Name).ToList())));

            rows.Add(new RowProperty(CondPropertyKeys.Value.ToString(),
                Resources.Header_cond_value, obj.Value)
            { Color = Color.Gainsboro });

            var funcNames = Enum.GetNames(typeof(FuncName)).ToList();
            funcNames.Add("*");

            string funcValue = "*";

            if (obj.Function != null)
            {
                if (obj.Function is CustomFrameFunction)
                    funcNames.Add(obj.Function.Name);
                funcValue = obj.Function.Name;
            }

            rows.Add(new RowProperty(CondPropertyKeys.Function.ToString(),
                Resources.Header_cond_function,
                new DropDownPropertyValue(funcValue, funcNames))
            { Color = Color.Gainsboro });

            if (obj.Function != null)
            {
                var pars = obj.Function?.GetParameters();
                foreach (var item in pars)
                {
                    var parAr = item.ToString().Split("=");

                    rows.Add(new RowProperty(CondPropertyKeys.Parameter.ToString(),
                        string.Format(Resources.Header_cond_parameter_placeHolder, parAr[0]),
                        new DropDownPropertyValue(item.ParameterKind, Enum.GetNames(typeof(ParameterKind)).ToList()),
                        item.ParameterType == ParameterType.Variable));

                    rows.Add(new RowProperty(CondPropertyKeys.ParameterValue.ToString(),
                        string.Format(Resources.Header_cond_parameterValue_placeHolder, parAr[0]),
                        item.ParameterKind == ParameterKind.Table
                        ? new DropDownPropertyValue((item as TableParameter).Table.Name, funcTables)
                        : parAr[1],
                        item.ParameterType == ParameterType.Variable));

                    if (item.ParameterKind == ParameterKind.Table)
                    {
                        var tablePar = item as TableParameter;
                        rows.Add(new RowProperty(CondPropertyKeys.Table.ToString(),
                            string.Format(Resources.Header_cond_table_placeHolder, tablePar.Table.Name),
                            new DropDownPropertyValue(tablePar.Parameter.Name, pars.Select(x => x.Name).ToList())));
                    }
                }
            }

            var dirNames = Enum.GetNames(typeof(Direction)).ToList();
            rows.Add(new RowProperty(CondPropertyKeys.Direction.ToString(),
                Resources.Header_cond_direction,
                new DropDownPropertyValue(obj.Direction, dirNames))
            { Color = Color.Gainsboro });

            rows.Add(new RowProperty(CondPropertyKeys.StartTime.ToString(),
                Resources.Header_cond_start,
                obj.StartTime)
            { Color = Color.Gainsboro });
            rows.Add(new RowProperty(CondPropertyKeys.StopTime.ToString(),
                Resources.Header_cond_stop, obj.StopTime)
            { Color = Color.Gainsboro });

            rows.Add(new RowProperty(CondPropertyKeys.CoordinateSystem.ToString(),
                Resources.Header_cond_coordinateSystem,
                new DropDownPropertyValue(obj.LocalFrame?.Name ?? "*",
                new List<string>() { "MRF", "SRF", "*" }))
            { Color = Color.Gainsboro });

            if (obj.LocalFrame != null)
                rows.AddRange(GetLocalFrameProperties(obj.LocalFrame, groups));

            return rows;

        }
    }
}
