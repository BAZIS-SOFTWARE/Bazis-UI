using Geometry;
using Model.Interfaces;
using Newtonsoft.Json;
using Project.Interfaces.Tasks;
using Project.TaskParameters;
using Project.Tasks.Functions.FrameFunctions;
using Project.Tasks.LocalFrames;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    // Из GUI/Methods/Navigator/NavigatorMethods.cs. EditTSFFile (помечен Obsolete, свойства
    // редактируются через панель свойств) не переносится.
    internal partial class MainWindow
    {
        private void DisplayMRF(float time, ICondData data)
        {
            if (data.LocalFrame is MovedFrame mf)
            {
                mf.Time = time - data.StartTime;
                var trajPoints = mf.BaseLine.Select(x => x.CalcCentr()).ToArray();
                DisplayPath(trajPoints);
            }

            data.LocalFrame.CalcPosition();
            DisplayLocalFrame(data.LocalFrame.Frame);

            if (data.Function is SPH sphear)
            {
                DisplaySphere((float)sphear.Width, data.LocalFrame.Frame);
            }
            else if (data.Function is CIL cilinder)
            {
                DisplayConus((float)cilinder.UpperDiam, (float)cilinder.BottomDiam, (float)cilinder.Length, data.LocalFrame.Frame);
            }
        }

        /// <summary>
        /// Добавляет направления условия к обозначениям текущего кадра без очистки предыдущих условий.
        /// </summary>
        public void DisplayDirection(float time, ICondData data, IEnumerable<IModelObject> modelObjs)
        {
            Point3D vector;
            Color color;

            if (data.Direction == Direction.X)
            {
                vector = new Point3D(1, 0, 0);
                color = Color.FromArgb(255, 0, 0);
            }
            else if (data.Direction == Direction.Y)
            {
                vector = new Point3D(0, 1, 0);
                color = Color.FromArgb(0, 255, 0);
            }
            else
            {
                vector = new Point3D(0, 0, 1);
                color = Color.FromArgb(0, 0, 255);
            }

            foreach (var obj in modelObjs)
            {
                foreach (var point in obj.GetCoordinates())
                {
                    var temp = vector.Mult(0.01f);
                    DisplayVector(temp, point, color);
                }
            }
        }

        public GeneralParameters ReadTaskParametersFromFile(string filePath)
        {
            var settingsSerializer = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };

            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var taskName = fileName.Split('_')[0];

            Enum.TryParse(taskName, out TaskKind taskKind);

            if (taskKind == TaskKind.термическая)
                return JsonConvert.DeserializeObject<TermalParameters>(File.ReadAllText(filePath), settingsSerializer);
            else if (taskKind == TaskKind.механическая)
                return JsonConvert.DeserializeObject<MechanicalParameters>(File.ReadAllText(filePath), settingsSerializer);
            else
                return JsonConvert.DeserializeObject<ChemicalParameters>(File.ReadAllText(filePath), settingsSerializer);
        }
    }
}
