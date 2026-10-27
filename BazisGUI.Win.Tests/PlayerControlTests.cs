using BazisGUI.Win.Player;
using System.Reflection;
using System.Threading;

namespace BazisGUI.Win.Tests
{
    /// <summary>
    /// Проверяет включение конечного кадра и сброс состояния плеера.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class PlayerControlTests
    {
        /// <summary>
        /// Проверяет сохранение конечной позиции и уведомление об очистке только по кнопке остановки.
        /// </summary>
        [TestCase(0, 0, new[] { 0 })]
        [TestCase(0, 2, new[] { 0, 1, 2 })]
        [TestCase(5, 6, new[] { 5, 6 })]
        public void TimerTick_KeepsLastFrameUntilExplicitStop(int start, int stop, int[] expected)
        {
            using var player = new PlayerControl();
            player.StopValue = stop;
            player.StartValue = start;
            player.CurrentValue = start;
            var frames = new List<int>();
            var stopCount = 0;
            player.CheckingEvent += (sender, value) => frames.Add(value);
            player.StopCheckingEvent += sender => stopCount++;
            var tick = typeof(PlayerControl).GetMethod("Timer_Tick", BindingFlags.Instance | BindingFlags.NonPublic);
            var arguments = new object[] { player, EventArgs.Empty };

            Assert.That(tick, Is.Not.Null);
            foreach (var value in expected)
                tick.Invoke(player, arguments);

            Assert.That(frames, Is.EqualTo(expected));
            Assert.That(stopCount, Is.Zero);
            Assert.That(player.CurrentValue, Is.EqualTo(stop));
            Assert.That(player.CheckState, Is.EqualTo(BazisGUI.Win.Player.CheckState.start));

            player.StopChecking_Click(player, EventArgs.Empty);

            Assert.That(stopCount, Is.EqualTo(1));
            Assert.That(player.CurrentValue, Is.EqualTo(start));
            Assert.That(player.CheckState, Is.EqualTo(BazisGUI.Win.Player.CheckState.start));
            Assert.That(frames, Has.Count.EqualTo(expected.Length));
        }

        /// <summary>
        /// Проверяет повторный запуск с начала после автоматического завершения.
        /// </summary>
        [TestCase(0)]
        [TestCase(2)]
        public void StartChecking_AfterCompletionRestartsFromBeginning(int stop)
        {
            using var player = new PlayerControl();
            player.StartValue = 0;
            player.StopValue = stop;
            player.CurrentValue = stop;
            var frames = new List<int>();
            var stopCount = 0;
            player.CheckingEvent += (sender, value) => frames.Add(value);
            player.StopCheckingEvent += sender => stopCount++;
            var tick = typeof(PlayerControl).GetMethod("Timer_Tick", BindingFlags.Instance | BindingFlags.NonPublic);
            var arguments = new object[] { player, EventArgs.Empty };

            Assert.That(tick, Is.Not.Null);
            tick.Invoke(player, arguments);
            Assert.That(stopCount, Is.Zero);

            player.StartChecking_Click(player, EventArgs.Empty);
            Assert.That(player.CurrentValue, Is.Zero);
            tick.Invoke(player, arguments);
            Assert.That(frames, Is.EqualTo(new[] { stop, 0 }));
            Assert.That(stopCount, Is.Zero);

            player.StopChecking_Click(player, EventArgs.Empty);
        }

        /// <summary>
        /// Проверяет отсутствие кадров при отмене и возможность проверки после сброса.
        /// </summary>
        [Test]
        public void Cancellation_StopsWithoutFrameAndResetsForNextRun()
        {
            using var player = new PlayerControl();
            player.StopValue = 1;
            player.Cancelation = true;
            var frameCount = 0;
            var stopCount = 0;
            player.CheckingEvent += (sender, value) => frameCount++;
            player.StopCheckingEvent += sender => stopCount++;
            var tick = typeof(PlayerControl).GetMethod("Timer_Tick", BindingFlags.Instance | BindingFlags.NonPublic);
            var arguments = new object[] { player, EventArgs.Empty };

            Assert.That(tick, Is.Not.Null);
            tick.Invoke(player, arguments);

            Assert.That(frameCount, Is.Zero);
            Assert.That(stopCount, Is.EqualTo(1));
            Assert.That(player.Cancelation, Is.False);

            tick.Invoke(player, arguments);

            Assert.That(frameCount, Is.EqualTo(1));
            Assert.That(stopCount, Is.EqualTo(1));
        }
    }
}
