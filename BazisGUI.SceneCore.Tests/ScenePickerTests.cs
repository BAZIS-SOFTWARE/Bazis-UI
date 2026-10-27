using BazisGUI.SceneCore.Picking;
using BazisGUI.SceneCore.Camera;
using BazisGUI.SceneCore.Interfaces;
using BazisGUI.SceneCore.Coordinates;
using System.Collections.Generic;

using RectangleBox = BazisGUI.SceneCore.Picking.RectangleBox;

namespace BazisGUI.SceneCore.Tests
{
    public class ScenePickerTests
    {
        /// <summary>Проверяет выбор ближайшего объекта при совпадении экранных координат.</summary>
        [Test]
        public void SelectByPoint_ChoosesClosestObject()
        {
            var camera = new SceneCamera { Viewport = new Viewport(100, 100) };
            camera.Initialize(0, 0, -5);
            var picker = new ScenePicker(camera);
            var hits = new List<int>();
            picker.ObjectsHit += (name, numbers) => hits.AddRange(numbers);
            var objects = new[]
            {
                new ScenePickObject(1, [new ModelPoint3D(0, 0, 0)]),
                new ScenePickObject(2, [new ModelPoint3D(0, 0, 1)])
            };
            var sets = new[] { new Set("nodes", ScenePickKind.Other, objects) };

            var isSelected = picker.SelectByPoint(sets, new ViewportPoint(0, 0), true);

            Assert.That(isSelected, Is.True);
            Assert.That(hits, Is.EqualTo(new[] { 2 }));
        }

        /// <summary>Проверяет, что выбор рамкой возвращает только попавшие в неё объекты.</summary>
        [Test]
        public void SelectByRect_ReturnsObjectsInsideRectangle()
        {
            var camera = new SceneCamera { Viewport = new Viewport(100, 100) };
            camera.Initialize(0, 0, -5);
            var picker = new ScenePicker(camera);
            var hits = new List<int>();
            picker.ObjectsHit += (name, numbers) => hits.AddRange(numbers);
            var objects = new[]
            {
                new ScenePickObject(1, [new ModelPoint3D(0, 0, 0)]),
                new ScenePickObject(2, [new ModelPoint3D(100, 0, 0)])
            };
            var sets = new[] { new Set("nodes", ScenePickKind.Other, objects) };
            var selectionBox = new RectangleBox(-1, 1, -1, 1);

            picker.SelectByRect(sets, selectionBox, true);

            Assert.That(hits, Is.EqualTo(new[] { 1 }));
        }
    }
}
