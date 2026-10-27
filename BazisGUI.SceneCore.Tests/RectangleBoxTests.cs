using RectangleBox = BazisGUI.SceneCore.Picking.RectangleBox;

namespace BazisGUI.SceneCore.Tests
{
    public class RectangleBoxTests
    {
        [TestCase(-3, -2, 2, 3)]
        [TestCase(-3, -2, -3, -2)]
        [TestCase(2, 3, 2, 3)]
        [TestCase(2, 3, -3, -2)]
        public void IsIntersectWithOther_ShouldBeTrue_WhenIntersectsInPointInSide(int left, int right, int bottom, int top)
        {
            var rectangle = new RectangleBox(-2, 2, -2, 2);
            var otherRectangle = new RectangleBox(left, right, bottom, top);

            Assert.That(rectangle.IsIntersectWithOther(otherRectangle), Is.True);
        }

        [TestCase(-3, 3, 2, 3)]
        [TestCase(-3, 3, -3, -2)]
        [TestCase(-3, -2, -3, 3)]
        [TestCase(2, 3, -3, 3)]
        public void IsIntersectWithOther_ShouldBeTrue_WhenIntersectsInTwoPointsInSide(int left, int right, int bottom, int top)
        {
            var rectangle = new RectangleBox(-2, 2, -2, 2);
            var otherRectangle = new RectangleBox(left, right, bottom, top);

            Assert.That(rectangle.IsIntersectWithOther(otherRectangle), Is.True);
        }

        [TestCase(-3, -2, 2, -2)]
        [TestCase(2, 3, 2, -2)]
        [TestCase(-2, 2, 2, 3)]
        [TestCase(-2, 2, -2, -3)]
        public void IsIntersectWithOther_ShouldBeTrue_WhenIntersectsInSide(int left, int right, int bottom, int top)
        {
            var rectangle = new RectangleBox(-2, 2, -2, 2);
            var otherRectangle = new RectangleBox(left, right, bottom, top);

            Assert.That(rectangle.IsIntersectWithOther(otherRectangle), Is.True);
        }

        [TestCase(1, 3, 1, 3)]
        [TestCase(-3, -1, 1, 3)]
        [TestCase(1, 3, -3, -1)]
        [TestCase(-3, -1, -3, -1)]
        public void IsIntersectWithOther_ShouldBeTrue_WhenOthersCornerInsideRectangle(int left, int right, int bottom, int top)
        {
            var rectangle = new RectangleBox(-2, 2, -2, 2);
            var otherRectangle = new RectangleBox(left, right, bottom, top);

            Assert.That(rectangle.IsIntersectWithOther(otherRectangle), Is.True);
        }

        [TestCase(1, 4, -1, 1)]
        [TestCase(-1, -4, -1, 1)]
        [TestCase(-1, 1, 1, 4)]
        [TestCase(-1, 1, -4, -1)]
        public void IsIntersectWithOther_ShouldBeTrue_WhenOthersTwoCornersInsideRectangle(int left, int right, int bottom, int top)
        {
            var rectangle = new RectangleBox(-2, 2, -2, 2);
            var otherRectangle = new RectangleBox(left, right, bottom, top);

            Assert.That(rectangle.IsIntersectWithOther(otherRectangle), Is.True);
        }

        [TestCase(-1, 1, -1, 1)]
        [TestCase(-2, 0, 0, 2)]
        [TestCase(0, 2, 0, 2)]
        [TestCase(-2, 0, -2, 0)]
        [TestCase(0, 2, -2, 0)]
        public void IsIntersectWithOther_ShouldBeTrue_WhenOtherInsideRectangle(int left, int right, int bottom, int top)
        {
            var rectangle = new RectangleBox(-3, 3, -3, 3);
            var otherRectangle = new RectangleBox(left, right, bottom, top);

            Assert.That(rectangle.IsIntersectWithOther(otherRectangle), Is.True);
        }

        [TestCase(-3, 3, 8, 12)]
        [TestCase(-3, 3, -10, -6)]
        [TestCase(-15, -10, -3, 3)]
        [TestCase(12, 15, -3, 3)]
        [TestCase(6, 9, 6, 9)]
        [TestCase(-9, -6, 6, 9)]
        [TestCase(6, 9, -9, -6)]
        [TestCase(-9, -6, -9, -6)]
        public void IsIntersectWithOther_ShouldBeFalse_WhenNoIntersection(int left, int right, int bottom, int top)
        {
            var rectangle = new RectangleBox(-3, 3, -3, 3);
            var otherRectangle = new RectangleBox(left, right, bottom, top);

            Assert.That(rectangle.IsIntersectWithOther(otherRectangle), Is.False);
        }

        [TestCase(-2, 0, 0, 2)]
        [TestCase(0, 2, 0, 2)]
        [TestCase(-2, 0, -2, 0)]
        [TestCase(0, 2, -2, 0)]
        [TestCase(-3, 3, -3, 3)]
        [TestCase(-1, 1, -1, 1)]
        [TestCase(-10, 10, -10, 10)]
        public void IsInnerOther_ShouldBeTrue_WhenRectanglesEquals(int left, int right, int bottom, int top)
        {
            var rectangle = new RectangleBox(left, right, bottom, top);
            var otherRectangle = new RectangleBox(left, right, bottom, top);

            Assert.That(rectangle.IsInnerOther(otherRectangle), Is.True);
        }

        [TestCase(-2, 0, 0, 2)]
        [TestCase(0, 2, 0, 2)]
        [TestCase(-2, 0, -2, 0)]
        [TestCase(0, 2, -2, 0)]
        [TestCase(-1, 1, -1, 1)]
        public void IsInnerOther_ShouldBeFalse_WhenOtherInsideRectangle(int left, int right, int bottom, int top)
        {
            var rectangle = new RectangleBox(-3, 3, -3, 3);
            var otherRectangle = new RectangleBox(left, right, bottom, top);

            Assert.That(rectangle.IsInnerOther(otherRectangle), Is.False);
        }

        [TestCase(-3, 3, -2, 2)]
        [TestCase(-2, 2, -3, 3)]
        public void IsInnerOther_ShouldBeTrue_WhenRectanglesSideInsideOthers(int left, int right, int bottom, int top)
        {
            var otherRectangle = new RectangleBox(-3, 3, -3, 3);
            var rectangle = new RectangleBox(left, right, bottom, top);

            Assert.That(rectangle.IsInnerOther(otherRectangle), Is.True);
        }

        [TestCase(-3, 0, 0, 3)]
        [TestCase(0, 3, 0, 3)]
        [TestCase(-3, 0, -3, 0)]
        [TestCase(0, 3, -3, 0)]
        public void IsInnerOther_ShouldBeTrue_WhenRectanglesTwoSidesInsideOthers(int left, int right, int bottom, int top)
        {
            var otherRectangle = new RectangleBox(-3, 3, -3, 3);
            var rectangle = new RectangleBox(left, right, bottom, top);

            Assert.That(rectangle.IsInnerOther(otherRectangle), Is.True);
        }

        [TestCase(-3, 3, 0, 3)]
        [TestCase(-3, 3, -3, 0)]
        [TestCase(-3, 0, -3, 3)]
        [TestCase(0, 3, -3, 3)]
        public void IsInnerOther_ShouldBeTrue_WhenRectanglesThreeSidesInsideOthers(int left, int right, int bottom, int top)
        {
            var otherRectangle = new RectangleBox(-3, 3, -3, 3);
            var rectangle = new RectangleBox(left, right, bottom, top);

            Assert.That(rectangle.IsInnerOther(otherRectangle), Is.True);
        }

        [TestCase(-1, 1, -1, 1)]
        [TestCase(-2, 2, -2, 2)]
        [TestCase(-1, 0, 0, 1)]
        [TestCase(-1, 0, -1, 0)]
        [TestCase(0, 1, -1, 0)]
        [TestCase(0, 1, 0, 1)]
        [TestCase(-2, 0, 0, 2)]
        [TestCase(-2, 0, -2, 0)]
        [TestCase(0, 2, -2, 0)]
        [TestCase(0, 2, 0, 2)]
        public void IsInnerOther_ShouldBeTrue_WhenRectanglesInsideOtherWithoutIntersection(int left, int right, int bottom, int top)
        {
            var otherRectangle = new RectangleBox(-3, 3, -3, 3);
            var rectangle = new RectangleBox(left, right, bottom, top);

            Assert.That(rectangle.IsInnerOther(otherRectangle), Is.True);
        }

        [TestCase(-5, -4, 4, 5)]
        [TestCase(3, 4, 4, 5)]
        [TestCase(-3, -2, -3, -2)]
        [TestCase(5, 6, -5, -4)]
        [TestCase(-1, 1, -5, -4)]
        [TestCase(-1, 1, 4, 5)]
        [TestCase(-5, -3, -1, 1)]
        [TestCase(3, 5, -1, 1)]
        public void IsInnerOther_ShouldBeFalse_WhenOtherOutside(int left, int right, int bottom, int top)
        {
            var otherRectangle = new RectangleBox(-1, 1, -1, 1);
            var rectangle = new RectangleBox(left, right, bottom, top);

            Assert.That(rectangle.IsInnerOther(otherRectangle), Is.False);
        }
    }
}
