using SceneCore.Camera;
using SceneCore.Input;
using SceneCore.EventsArgs;
using Geometry;

namespace SceneCore.Tests
{
    public class SceneInputControllerTests
    {
        private static (SceneCamera Camera, SceneInputController Input) Create()
        {
            var camera = new SceneCamera { Width = 400, Height = 300 };
            camera.Initialize(0, 0, -5);
            var input = new SceneInputController(camera) { Viewport = new Viewport(400, 300) };
            return (camera, input);
        }

        [Test]
        public void OnWheelChanged_FullStep_ScalesBy11()
        {
            var (camera, input) = Create();

            input.OnWheelChanged(new SceneMouseEventArgs { Delta = 120 });

            Assert.That(camera.ScaleFactor, Is.EqualTo(1.1f).Within(1e-5));
        }

        [Test]
        public void OnWheelChanged_FractionalDelta_ScalesProportionally()
        {
            var (camera, input) = Create();

            // Тачпад: десять событий по 12 дают тот же масштаб, что один полный шаг колеса.
            for (var i = 0; i < 10; i++)
                input.OnWheelChanged(new SceneMouseEventArgs { Delta = 12 });

            Assert.That(camera.ScaleFactor, Is.EqualTo(1.1f).Within(1e-4));
        }

        [Test]
        public void OnPointerReleased_LeftWithoutPress_DoesNotSelect()
        {
            var (_, input) = Create();
            SelectObjectsEventArgs selection = null;
            input.SelectionRequested += (_, e) => selection = e;

            input.OnPointerReleased(new SceneMouseEventArgs { Button = SceneMouseButton.Left, Position = new Point2D(10, 10) });

            Assert.That(selection, Is.Null);
        }

        [Test]
        public void CancelSelection_AfterPress_ReleaseDoesNotSelect()
        {
            var (_, input) = Create();
            SelectObjectsEventArgs selection = null;
            input.SelectionRequested += (_, e) => selection = e;

            input.OnPointerPressed(new SceneMouseEventArgs { Button = SceneMouseButton.Left, Position = new Point2D(10, 10) });
            input.CancelSelection();
            input.OnPointerReleased(new SceneMouseEventArgs { Button = SceneMouseButton.Left, Position = new Point2D(10, 10) });

            Assert.That(selection, Is.Null);
        }

        [Test]
        public void OnPointerReleased_AfterPress_Selects()
        {
            var (_, input) = Create();
            SelectObjectsEventArgs selection = null;
            input.SelectionRequested += (_, e) => selection = e;

            input.OnPointerPressed(new SceneMouseEventArgs { Button = SceneMouseButton.Left, Position = new Point2D(10, 10) });
            input.OnPointerReleased(new SceneMouseEventArgs { Button = SceneMouseButton.Left, Position = new Point2D(10, 10) });

            Assert.That(selection, Is.Not.Null);
        }

        /// <summary>Проверяет перевод позиции указателя в координаты выбора относительно центра окна.</summary>
        [TestCase(200, 150, -5, 5, -5, 5)]
        [TestCase(100, 100, -105, -95, 45, 55)]
        public void OnPointerReleased_ConvertsPointerPositionToViewport(int pointerX, int pointerY, int left, int right, int bottom, int top)
        {
            var (_, input) = Create();
            SelectObjectsEventArgs selection = null;
            input.SelectionRequested += (_, e) => selection = e;
            var position = new Point2D(pointerX, pointerY);

            input.OnPointerPressed(new SceneMouseEventArgs { Button = SceneMouseButton.Left, Position = position });
            input.OnPointerReleased(new SceneMouseEventArgs { Button = SceneMouseButton.Left, Position = position });

            Assert.That(selection, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(selection.SelectionBox.Left, Is.EqualTo(left));
                Assert.That(selection.SelectionBox.Right, Is.EqualTo(right));
                Assert.That(selection.SelectionBox.Bottom, Is.EqualTo(bottom));
                Assert.That(selection.SelectionBox.Top, Is.EqualTo(top));
            });
        }
    }
}
