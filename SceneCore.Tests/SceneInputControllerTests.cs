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
    }
}
