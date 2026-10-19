using Avalonia.Threading;
using BazisAvaloniaGUI.Localization;
using Model.Interfaces;
using System;
using System.Drawing;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    internal partial class MainWindow
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

            if (beams.NumberOfObjects > 0)
            {
                var pre = project.CreateModelObjectsPresentor(beams);
                var vbo = CreateVBObject(pre);
                VBOController.AddVbo(vbo);
                RequestRedraw();

                PresentMeshData();
            }
            Dispatcher.UIThread.Invoke(new Action(() => { console.PrintInfo($"{Resources.BeamConnection_BeamConnection_ObjectsCreated_Message} {beams.ObjType}", Color.Black); }));
            return beams.Name;
        }
    }
}
