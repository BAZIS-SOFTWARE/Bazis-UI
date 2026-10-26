using Avalonia.Threading;
using AvaloniaGUI.Extensions;
using AvaloniaGUI.Localization;
using Geometry;
using Model.Interfaces;
using Model.Interfaces.MeshObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace AvaloniaGUI.Shell
{
    internal partial class MainWindow
    {
        private void ParsePolygonPoints(string v1, string v2, string v3, string v4, string numberOfElems, out Point2D p1, out Point2D p2, out Point2D p3, out Point2D p4, out int numberOfElemsInt)
        {
            var c1 = v1.Split(',').Select(x => float.Parse(x)).ToArray();
            p1 = new Point2D(c1[0], c1[1]);
            var c2 = v2.Split(',').Select(x => float.Parse(x)).ToArray();
            p2 = new Point2D(c2[0], c2[1]);
            var c3 = v3.Split(',').Select(x => float.Parse(x)).ToArray();
            p3 = new Point2D(c3[0], c3[1]);
            var c4 = v4.Split(',').Select(x => float.Parse(x)).ToArray();
            p4 = new Point2D(c4[0], c4[1]);
            numberOfElemsInt = int.Parse(numberOfElems);
        }
        private void CreateMesh2DPoligon(Point2D p1, Point2D p2, Point2D p3, Point2D p4, int numberOfElemsInt)
        {
            project.CreateQuadMeshOnPoligon(new List<Point2D>() { p1, p2, p3, p4 }, numberOfElemsInt);
            PresentMeshData();
            PresentModelObjectsForSelection();

            var set = project.GetModelSetsInfo(ObjType.Элемент2D).Last();
            var pres = project.CreateModelObjectsPresentor(set);
            var vbo = CreateVBObject(pres);

            VBOController.AddVbo(vbo);
            RequestRedraw();
        }

        private void FindObjectParserStr(string str, out ObjType objType, out uint number)
        {
            var strAr = str.Split(',');
            if (!Enum.TryParse(strAr[0].Replace(" ", ""), out objType))
                throw new Exception(Resources.UnknownTypeException);

            if (!uint.TryParse(strAr[1].Replace(" ", ""), out number))
                throw new Exception(Resources.PositiveCellingNumberException);
        }
        // Синхронно и в потоке вызова (команды консоли выполняются в UI-потоке):
        // исключение доходит до обработчика консоли и останавливает макрос.
        private void FindObject(ObjType objType, uint number)
        {
            if (project == null)
                throw new Exception(Resources.SaveWithoutProjectMessage);

            var obj = project.GetAllModelObjects().
                FirstOrDefault(x => x.ObjType == objType && x.Number == number);

            if (obj != null)
            {
                using (project.BeginViewUpdate())
                {
                    foreach (var item in project.GetModelSetsInfo(objType))
                        project.SetVisible(objType, item.GetNumbers(), false);

                    project.SetVisible(objType, [(int)number], true);
                }
            }
            else
                console.PrintInfo(Resources.ConsoleEvents_ConsoleInEvent_ObjectNotFound_Message, Color.Orange);
        }

        private void FindVolElems(string v)
        {
            if (!double.TryParse(v, out double volume))
                throw new ArgumentException(Resources.InvalidArgumentsNumberException, nameof(v));


            var e3ds = project.GetAllModelElements().
                        Where(x => x.ObjType == ObjType.Элемент3D).
                        Select(x => (IElement3D)x);

            var findElmems = e3ds.Where(
                e3d => e3d.CalcVolume() < volume);

            if (findElmems.Count() > 0)
            {
                // TO DO потом можно поискать способ более быстрый и
                // технологичный для отображения найденных элементов
                var numbers = findElmems.Select(x => x.Number).ToList();
                ApplySelectionColor();
                project.SetSelection(ObjType.Элемент3D, numbers);
            }
            Dispatcher.UIThread.Invoke(new Action(() => { console.PrintInfo($"{Resources.ConsoleEvents_ConsoleInEvent_ObjectFound_Message} {findElmems.Count()} {Resources.ConsoleEvents_ConsoleInEvents_VolumeElements_Message}", Color.Black); }));
        }

        private void SetLevel(string objTypeStr, string levelStr)
        {
            var objType = objTypeStr.ToEnum<ObjType>();
            var level = int.Parse(levelStr);
            foreach (var item in project.GetAllModelElements().Where(x => x.ObjType == objType))
                item.Level = level;
        }

        private void console_RenumberMeshEvent(string cmd)
        {
            if (!Enum.TryParse(cmd.Split(':')[0], out ObjType objType))
                throw new Exception(Resources.UnknownTypeException);

            if (!uint.TryParse(cmd.Split(':')[1], out uint number))
                throw new Exception(Resources.PositiveCellingNumberException);

            //project.Renumber(project.ModelData.ObjectData, objType);
        }

        private void ParseVector(string vector, out float x, out float y, out float z)
        {
            var strAr = vector.Split(',');
            if (strAr.Length < 3)
                throw new Exception(Resources.ModelShiftCoordinateEventArgsVectorExc);
            x = float.Parse(strAr[0], NumberStyles.Float, CultureInfo.InvariantCulture);
            y = float.Parse(strAr[1], NumberStyles.Float, CultureInfo.InvariantCulture);
            z = float.Parse(strAr[2], NumberStyles.Float, CultureInfo.InvariantCulture);
        }
        private void console_ModelShiftCoordinateEvent(float x, float y, float z)
        {
            project.MoveMesh(ObjType.Узел, new Point3D(x, y, z));

            HideAllGeometryObjects();
            HideAllText2D();

            foreach (var set in project.GetAllModelSetsInfo())
            {
                var pres = project.CreateModelObjectsPresentor(set);
                SetVBObjectAttribute(pres, "координаты");
            }
            RequestRedraw();
        }

        private void console_ModelRotateEvent(string v)
        {
            var strAr = v.Split(':');

            if (strAr.Length < 2)
                throw new Exception(Resources.InvalidCommandException);

            var coords = strAr[0].Split(',');

            if (coords.Length < 3)
                throw new Exception(Resources.InvalidCoordinatesException);

            var x = float.Parse(coords[0].Replace(" ", ""), NumberStyles.Float);
            var y = float.Parse(coords[1].Replace(" ", ""), NumberStyles.Float);
            var z = float.Parse(coords[2].Replace(" ", ""), NumberStyles.Float);

            var angle = float.Parse(strAr[1].Replace(" ", ""), NumberStyles.Float);
            var point = new Point3D(x, y, z);

            project.RotateMesh(ObjType.Узел, point, angle);

            HideAllGeometryObjects();
            HideAllText2D();
            HideAllText3D();

            foreach (ObjType item in Enum.GetValues(typeof(ObjType)))
            {
                foreach (var setInfo in project.GetModelSetsInfo(item))
                {
                    var pres = project.CreateModelObjectsPresentor(setInfo);
                    SetVBObjectAttribute(pres, "координаты");
                }
            }
            RequestRedraw();
        }
    }
}
