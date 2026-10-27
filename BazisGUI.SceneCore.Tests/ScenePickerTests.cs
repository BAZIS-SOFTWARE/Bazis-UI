using BazisGUI.SceneCore.Picking;
using BazisGUI.SceneCore.Camera;
using BazisGUI.SceneCore.Interfaces;
using BazisGUI.SceneCore.Coordinates;
using System.Collections.Generic;
using NUnit.Framework;

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

        /// <summary>Проверяет все способы пересечения рамки с треугольником и отсутствие пересечения.</summary>
        [TestCase(-2, -2, 2, -2, 0, 2, -3, 3, -3, 3, true)]
        [TestCase(-40, -40, 40, -40, 0, 40, -3, 3, -3, 3, true)]
        [TestCase(-10, 0, 10, 0, 0, 10, -3, 3, -3, 3, true)]
        [TestCase(3, 0, 10, 0, 3, 10, -3, 3, -3, 3, true)]
        [TestCase(10, 10, 20, 10, 15, 20, -3, 3, -3, 3, false)]
        public void SelectByRect_ChecksTriangleIntersection(int x1, int y1, int x2, int y2, int x3, int y3, int left, int right, int bottom, int top, bool expected)
        {
            var camera = new SceneCamera { Viewport = new Viewport(100, 100) };
            camera.Initialize(0, 0, -5);
            var unitX = camera.GetScreenCoord(camera.GetSceneCoord(new ModelPoint3D(1, 0, 0))).X;
            var unitY = camera.GetScreenCoord(camera.GetSceneCoord(new ModelPoint3D(0, 1, 0))).Y;
            var points = new[]
            {
                new ModelPoint3D(x1 / unitX, y1 / unitY, 0),
                new ModelPoint3D(x2 / unitX, y2 / unitY, 0),
                new ModelPoint3D(x3 / unitX, y3 / unitY, 0)
            };
            var picker = new ScenePicker(camera);
            var hits = new List<int>();
            picker.ObjectsHit += (name, numbers) => hits.AddRange(numbers);
            var objects = new[] { new ScenePickObject(7, points) };
            var sets = new[] { new Set("surfaces", ScenePickKind.Surface, objects) };
            var selectionBox = new RectangleBox(left, right, bottom, top);

            picker.SelectByRect(sets, selectionBox, true);

            Assert.That(hits.Contains(7), Is.EqualTo(expected));
        }

        /// <summary>Проверяет пересечение рамки с промежуточным сегментом кривой.</summary>
        [Test]
        public void SelectByRect_ChecksEveryAdjacentCurveSegment()
        {
            var camera = new SceneCamera { Viewport = new Viewport(100, 100) };
            camera.Initialize(0, 0, -5);
            var unitX = camera.GetScreenCoord(camera.GetSceneCoord(new ModelPoint3D(1, 0, 0))).X;
            var unitY = camera.GetScreenCoord(camera.GetSceneCoord(new ModelPoint3D(0, 1, 0))).Y;
            var points = new[]
            {
                new ModelPoint3D(-20 / unitX, -20 / unitY, 0),
                new ModelPoint3D(-10 / unitX, -10 / unitY, 0),
                new ModelPoint3D(10 / unitX, 10 / unitY, 0)
            };
            var picker = new ScenePicker(camera);
            var hits = new List<int>();
            picker.ObjectsHit += (name, numbers) => hits.AddRange(numbers);
            var objects = new[] { new ScenePickObject(8, points) };
            var sets = new[] { new Set("curves", ScenePickKind.Curve, objects) };

            picker.SelectByRect(sets, new RectangleBox(-1, 1, -1, 1), true);

            Assert.That(hits, Is.EqualTo(new[] { 8 }));
        }

        /// <summary>Проверяет продолжение обхода треугольников одной поверхности после промаха.</summary>
        [Test]
        public void SelectByRect_ChecksFollowingSurfaceTriangles()
        {
            var camera = new SceneCamera { Viewport = new Viewport(100, 100) };
            camera.Initialize(0, 0, -5);
            var unitX = camera.GetScreenCoord(camera.GetSceneCoord(new ModelPoint3D(1, 0, 0))).X;
            var unitY = camera.GetScreenCoord(camera.GetSceneCoord(new ModelPoint3D(0, 1, 0))).Y;
            var points = new[]
            {
                new ModelPoint3D(20 / unitX, 20 / unitY, 0),
                new ModelPoint3D(30 / unitX, 20 / unitY, 0),
                new ModelPoint3D(25 / unitX, 30 / unitY, 0),
                new ModelPoint3D(-2 / unitX, -2 / unitY, 0),
                new ModelPoint3D(2 / unitX, -2 / unitY, 0),
                new ModelPoint3D(0, 2 / unitY, 0)
            };
            var picker = new ScenePicker(camera);
            var hits = new List<int>();
            picker.ObjectsHit += (name, numbers) => hits.AddRange(numbers);
            var objects = new[] { new ScenePickObject(10, points) };
            var sets = new[] { new Set("surfaces", ScenePickKind.Surface, objects) };

            picker.SelectByRect(sets, new RectangleBox(-3, 3, -3, 3), true);

            Assert.That(hits, Is.EqualTo(new[] { 10 }));
        }

        /// <summary>Проверяет выбор кликом второго сегмента кривой и второго треугольника поверхности.</summary>
        [TestCase(ScenePickKind.Curve)]
        [TestCase(ScenePickKind.Surface)]
        public void SelectByPoint_ChecksEveryPrimitive(ScenePickKind kind)
        {
            var camera = new SceneCamera { Viewport = new Viewport(100, 100) };
            camera.Initialize(0, 0, -5);
            var unitX = camera.GetScreenCoord(camera.GetSceneCoord(new ModelPoint3D(1, 0, 0))).X;
            var unitY = camera.GetScreenCoord(camera.GetSceneCoord(new ModelPoint3D(0, 1, 0))).Y;
            var curvePoints = new[]
            {
                new ModelPoint3D(-20 / unitX, -20 / unitY, 0),
                new ModelPoint3D(-10 / unitX, -10 / unitY, 0),
                new ModelPoint3D(10 / unitX, 10 / unitY, 0)
            };
            var surfacePoints = new[]
            {
                new ModelPoint3D(20 / unitX, 20 / unitY, 0),
                new ModelPoint3D(30 / unitX, 20 / unitY, 0),
                new ModelPoint3D(25 / unitX, 30 / unitY, 0),
                new ModelPoint3D(-10 / unitX, -10 / unitY, 0),
                new ModelPoint3D(10 / unitX, -10 / unitY, 0),
                new ModelPoint3D(0, 10 / unitY, 0)
            };
            var points = kind == ScenePickKind.Curve ? curvePoints : surfacePoints;
            var picker = new ScenePicker(camera);
            var hits = new List<int>();
            picker.ObjectsHit += (name, numbers) => hits.AddRange(numbers);
            var objects = new[] { new ScenePickObject(9, points) };
            var sets = new[] { new Set("objects", kind, objects) };

            var selected = picker.SelectByPoint(sets, new ViewportPoint(0, 0), true);

            Assert.That(selected, Is.True);
            Assert.That(hits, Is.EqualTo(new[] { 9 }));
        }
    }
}
