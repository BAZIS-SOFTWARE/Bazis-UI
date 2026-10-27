using Geometry;
using Model.Interfaces;
using Model.MeshObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace BazisGUI.Avalonia.Shell
{
    internal partial class MainWindow
    {
        // async Task без внутреннего перехвата: консоль дожидается завершения выбора,
        // показывает ошибку и останавливает макрос (OperationCanceledException — нейтрально).
        private async Task console_NodesShiftCoordinate()
        {
            // выбор объектов
            var message = $@"Выберите первый узел и нажмите на клавишу ""E"" для подтверждения или клавишу ""ESC"" для отмены";
            // Esc: SelectObjectAsync возвращает null и уже сообщил об отмене.
            if (await SelectObjectAsync(ObjType.Узел, message) is not Node fNode)
                return;

            project.ClearSelection();

            message = $@"Выберите второй узел и нажмите на клавишу ""E"" для подтверждения или клавишу ""ESC"" для отмены";
            if (await SelectObjectAsync(ObjType.Узел, message) is not Node sNode)
                return;

            project.ClearSelection();

            await SelectContainerAsync(@"Выберите узлы для перемещения и нажмите на клавишу ""E"" для подтверждения");

            var nodes = project.GetSelected(ObjType.Узел)
                .Select(number => project.GetModelObject(ObjType.Узел, number) as Node)
                .Where(node => node != null)
                .ToList();

            if (nodes.Count == 0)
                throw new Exception("Не выбран ни один узел");

            var vec = sNode.Position.Sub(fNode.Position);

            ChangeCoordinates(nodes, vec);

            ClearAllDataOnScene();
            CreateVBObjects("Объекты");

            RequestRedraw();
        }

        private static void ChangeCoordinates(IEnumerable<Node> nodes, Point3D vec)
        {
            foreach (var node in nodes)
            {
                var temp = node.Position.Sum(vec);
                node.Position._x = temp._x;
                node.Position._y = temp._y;
                node.Position._z = temp._z;
            }
        }
    }
}
