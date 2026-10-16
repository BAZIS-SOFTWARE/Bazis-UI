using System;
using System.Threading;

namespace BazisAvaloniaGUI.Chamfer.Services
{
    /// <summary>
    /// Сервис выполнения операций создания фаски, гарантирующий выполнение действий в указанном
    /// <see cref="SynchronizationContext"/> (обычно UI-поток).
    /// </summary>
    internal sealed class SynchronizationContextChamferOperationService : IChamferOperationService
    {
        private readonly SynchronizationContext synchronizationContext;
        private readonly Action<double, double, bool> addByAngle;
        private readonly Action<double, double, bool> addByLengths;
        private readonly Action<double, double, bool, bool> preview;
        private readonly Action clearPreview;
  
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="SynchronizationContextChamferOperationService"/>.
        /// </summary>
        /// <param name="synchronizationContext">
        /// Контекст синхронизации, в котором должны выполняться переданные операции (обычно контекст UI).
        /// </param>
        /// <param name="addByAngle">
        /// Делегат, реализующий добавление фаски по длине и углу. Вызывается в контексте <paramref name="synchronizationContext"/>.
        /// </param>
        /// <param name="addByLengths">
        /// Делегат, реализующий добавление фаски по двум длинам. Вызывается в контексте <paramref name="synchronizationContext"/>.
        /// </param>
        /// <param name="prewiew">
        /// Делегат, реализующий предварительный просмотр фаски. Вызывается в контексте <paramref name="synchronizationContext"/>.
        /// </param>
        /// <param name="clearPreview">
        /// Делегат, реализующий очистку предварительного просмотра фаски. Вызывается в контексте <paramref name="synchronizationContext"/>.
        /// </param>
        public SynchronizationContextChamferOperationService(SynchronizationContext synchronizationContext, Action<double, double, bool> addByAngle, Action<double, double, bool> addByLengths, Action<double, double, bool, bool> preview, Action clearPreview)
        {
            this.synchronizationContext = synchronizationContext
                ?? throw new ArgumentNullException(nameof(synchronizationContext));
            this.addByAngle = addByAngle ?? throw new ArgumentNullException(nameof(addByAngle));
            this.addByLengths = addByLengths ?? throw new ArgumentNullException(nameof(addByLengths));
            this.preview = preview ?? throw new ArgumentNullException(nameof(preview));
            this.clearPreview = clearPreview ?? throw new ArgumentNullException(nameof(clearPreview));
        }

        /// <summary>
        /// Запрашивает выполнение операции добавления фаски по длине и углу в контексте синхронизации.
        /// </summary>
        /// <param name="length">Длина фаски. Единицы измерения определяются вызывающим кодом.</param>
        /// <param name="angle">Угол фаски. Формат (градусы/радианы) определяется вызывающим кодом.</param>
        /// <param name="reflected">Указывает, является ли фаска отраженной.</param>
        public void AddByAngle(double length, double angle, bool reflected) => synchronizationContext.Post(_ => addByAngle(length, angle, reflected), null);

        /// <summary>
        /// Запрашивает выполнение операции добавления фаски по двум длинам в контексте синхронизации.
        /// </summary>
        /// <param name="length1">Первая длина фаски. Единицы измерения определяются вызывающим кодом.</param>
        /// <param name="length2">Вторая длина фаски. Единицы измерения определяются вызывающим кодом.</param>
        /// <param name="reflected">Указывает, является ли фаска отраженной.</param>
        public void AddByLengths(double length1, double length2, bool reflected) => synchronizationContext.Post(_ => addByLengths(length1, length2, reflected), null);
        /// <summary>
        /// Запрашивает выполнение предварительного просмотра фаски в контексте синхронизации.
        /// </summary>
        /// <param name="length">
        /// Первая величина, используемая для построения превью — обычно первая длина или основной параметр фаски.
        /// </param>
        /// <param name="valueSecond">
        /// Вторая величина: если <paramref name="isAngle"/> == true, интерпретируется как угол или вспомогательное значение;
        /// иначе — как вторая длина фаски.
        /// </param>
        /// <param name="isAngle">Флаг, указывающий, интерпретируется ли <paramref name="valueSecond"/> как угол.</param>
        /// <param name="isReflected">Флаг, указывающий, должен ли предварительный просмотр отображать отражённую фаску.</param>
        public void Prewiew(double length, double valueSecond, bool isAngle, bool isReflected) => synchronizationContext.Post(_ => preview(length, valueSecond, isAngle, isReflected), null);
        /// <summary>
        /// Запрашивает очистку предварительного просмотра фаски в контексте синхронизации.
        /// </summary>
        public void ClearPreview() => synchronizationContext.Post(_ => clearPreview(), null);
    }
}
