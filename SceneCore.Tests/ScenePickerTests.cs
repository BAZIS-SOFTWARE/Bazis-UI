using SceneCore.Picking;
using Geometry;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using System.Collections.Generic;

namespace SceneCore.Tests
{
    public class ScenePickerTests
    {
        [Test]
        public void SelectByPoint_IgnoresHiddenObject()
        {
            var picker = new ScenePicker(null);
            var setInfo = new TestSetInfo(ObjType.Узел, 1);
            var hits = new List<int>();
            picker.ObjectsHit += (name, numbers) => hits.AddRange(numbers);

            var isSelected = picker.SelectByPoint([setInfo], new Point2D(0, 0), true, (objType, number) => false);

            Assert.That(isSelected, Is.False);
            Assert.That(hits, Is.Empty);
        }

        [Test]
        public void SelectByRect_IgnoresHiddenObject()
        {
            var picker = new ScenePicker(null);
            var setInfo = new TestSetInfo(ObjType.Узел, 1);
            var hits = new List<int>();
            picker.ObjectsHit += (name, numbers) => hits.AddRange(numbers);
            var selectionBox = new RectangleBox(0, 1, 0, 1);

            picker.SelectByRect([setInfo], selectionBox, true, (objType, number) => false);

            Assert.That(hits, Is.Empty);
        }

        private class TestSetInfo : ISetInfo
        {
            readonly int number;

            public TestSetInfo(ObjType objType, int number)
            {
                ObjType = objType;
                this.number = number;
            }

            public string Name => "test";
            public int Number { get; set; }
            public ObjType ObjType { get; }
            public int NumberOfObjects => 1;

            public string GetObjectInfo(int number) => string.Empty;
            public IEnumerable<string> GetObjectsInfo() => [];
            public IEnumerable<Point3D> GetCoords(int number) => throw new AssertionException("Hidden object must not be processed.");
            public Point3D GetCentr(int number) => throw new AssertionException("Hidden object must not be processed.");
            public IEnumerable<int> GetNumbers() => [number];
        }
    }
}
