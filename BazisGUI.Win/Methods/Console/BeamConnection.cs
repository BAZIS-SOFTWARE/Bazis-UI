using BazisGUI.Win.Console;
using BazisGUI.Win.Properties;
using IronPython.Compiler.Ast;
using Model.Interfaces;
using System;
using System.Drawing;
using System.Linq;
using static IronPython.SQLite.PythonSQLite;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void PrepareDataForConnectionBeam(string _radius, string _maxBeams, out double radius, out int maxBeams)
        {
            if (!double.TryParse(_radius, out radius))
                throw new ArgumentException(Resources.BeamConnectionEventArgsArgNumExc, nameof(_radius));

            if (!int.TryParse(_maxBeams, out maxBeams))
                throw new ArgumentException(Resources.BeamConnectionEventArgsArgNumExc, nameof(_maxBeams));
        }
        private string BeamConnection(double radius, int maxBeams, string master, string slave)
        {
            // TO DO это пока прототип метода сшивки. Далее добавить асинхронные операции 
            // для выбора групп узлов
            project.ConnectByBeams(master, slave, radius, maxBeams);

            var beams = project.GetModelSetsInfo(ObjType.Элемент1D).Last();
            Invoke(new Action(() => { console.PrintInfo($"{Resources.BeamConnection_BeamConnection_ObjectsCreated_Message} {beams.ObjType}", Color.Black); }));
            return beams.Name;
        }
    }
}
