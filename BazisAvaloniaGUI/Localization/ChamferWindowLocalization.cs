namespace BazisAvaloniaGUI.Localization
{
    /// <summary>
    /// Локализованные подписи окна добавления фаски.
    /// </summary>
    /// <remarks>
    /// Класс является адаптером к общим ресурсам приложения <see cref="Resources"/>:
    /// собственного механизма локализации не содержит, значения берутся из
    /// Resources.resx / Resources.ru.resx / Resources.en.resx по текущей языковой культуре.
    /// Используется в разметке Avalonia через {x:Static}, так как класс ресурсов
    /// имеет модификатор internal и недоступен из XAML напрямую.
    /// Подписи читаются при создании окна, поэтому окно открывается на языке,
    /// выбранном в настройках приложения.
    /// </remarks>
    public static class ChamferWindowLocalization
    {
        /// <summary>
        /// Заголовок окна
        /// </summary>
        public static string Title => Resources.ChamferWindow_Title;

        /// <summary>
        /// Название режима построения фаски по длине и углу
        /// </summary>
        public static string AngleModeTab => Resources.ChamferWindow_AngleModeTab;

        /// <summary>
        /// Название режима построения фаски по двум длинам
        /// </summary>
        public static string LengthsModeTab => Resources.ChamferWindow_LengthsModeTab;

        /// <summary>
        /// Подпись поля первой длины фаски
        /// </summary>
        public static string FirstLength => Resources.ChamferWindow_FirstLength;

        /// <summary>
        /// Подпись поля второй длины фаски
        /// </summary>
        public static string SecondLength => Resources.ChamferWindow_SecondLength;

        /// <summary>
        /// Подпись поля угла фаски
        /// </summary>
        public static string Angle => Resources.ChamferWindow_Angle;

        /// <summary>
        /// Всплывающая подсказка кнопки отражения фаски
        /// </summary>
        public static string ReflectTooltip => Resources.ChamferWindow_ReflectTooltip;

        /// <summary>
        /// Подпись кнопки добавления фаски
        /// </summary>
        public static string AddButton => Resources.ChamferWindow_AddButton;
    }
}
