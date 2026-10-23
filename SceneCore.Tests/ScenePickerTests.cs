using SceneCore.Picking;
using SceneCore.Camera;
using SceneCore.Interfaces;
using Geometry;
using System.Collections.Generic;

namespace SceneCore.Tests
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
                new ScenePickObject(1, [new Point3D(0, 0, 0)]),
                new ScenePickObject(2, [new Point3D(0, 0, 1)])
            };
            var sets = new[] { new Set("nodes", ScenePickKind.Other, objects) };

            var isSelected = picker.SelectByPoint(sets, new Point2D(0, 0), true);

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
                new ScenePickObject(1, [new Point3D(0, 0, 0)]),
                new ScenePickObject(2, [new Point3D(100, 0, 0)])
            };
            var sets = new[] { new Set("nodes", ScenePickKind.Other, objects) };
            var selectionBox = new RectangleBox(-1, 1, -1, 1);

            picker.SelectByRect(sets, selectionBox, true);

            Assert.That(hits, Is.EqualTo(new[] { 1 }));
        }
    }
}
