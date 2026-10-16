using BazisAvaloniaGUI.Shell;
using BazisAvaloniaGUI.Utilities;
using Model.Interfaces;

namespace BazisAvaloniaGUI.Localization
{
    /// <summary>
    /// Локализованные подписи элементов сцены.
    /// </summary>
    /// <remarks>
    /// Класс является адаптером к ресурсам формы BaseForm (MainWindow.resx / MainWindow.ru.resx)
    /// и общим ресурсам приложения (Resources.resx — всплывающие подсказки кнопок, в WinForms их не было):
    /// подписи кнопок и пунктов контекстного меню сцены берутся по тем же ключам, что и в WinForms.
    /// Используется в разметке Avalonia через {x:Static}, так как класс Localization
    /// имеет модификатор internal и недоступен из XAML напрямую.
    /// Подписи читаются при создании сцены, поэтому она открывается на языке,
    /// выбранном в настройках приложения.
    /// </remarks>
    public static class SceneViewLocalization
    {
        /// <summary>
        /// Подпись списка выбора наборов (btnSelect)
        /// </summary>
        public static string Select => Localization.GetFormText("btnSelect.Text");

        /// <summary>
        /// Подпись кнопки раскрытия панели видов (btnDisplayViews)
        /// </summary>
        public static string View => Localization.GetFormText("btnDisplayViews.Text");

        /// <summary>
        /// Пункт контекстного меню «Создать новую группу»
        /// </summary>
        public static string CreateGroup => Localization.GetFormText("создатьГруппуItem.Text");

        /// <summary>
        /// Пункт контекстного меню «Скрыть выбранное»
        /// </summary>
        public static string HideSelected => Localization.GetFormText("скрытьВыбранноеItem.Text");

        /// <summary>
        /// Пункт контекстного меню «Показать все скрытые»
        /// </summary>
        public static string ShowAllHidden => Localization.GetFormText("показатьСкрытыеItem.Text");

        /// <summary>
        /// Пункт контекстного меню «Выбранные объекты»
        /// </summary>
        public static string SelectedObjects => Localization.GetFormText("menuItem_InfoSelectedObjects.Text");

        /// <summary>
        /// Пункт контекстного меню «Задать точку вращения»
        /// </summary>
        public static string SetRotationPoint => Localization.GetFormText("menuItem_SetRotPoint.Text");

        /// <summary>
        /// Пункт контекстного меню «Показать сопряженные»
        /// </summary>
        public static string ShowPaired => Localization.GetFormText("показатьСопряженныеItem.Text");

        /// <summary>
        /// Пункт контекстного меню «Удалить выбранное»
        /// </summary>
        public static string RemoveSelected => Localization.GetFormText("menuItem_DeleteSelectedObjects.Text");

        /// <summary>
        /// Всплывающая подсказка кнопки расширенного выбора
        /// </summary>
        public static string AdvSelectToolTip => Resources.SceneView_ToolTip_AdvSelect;

        /// <summary>
        /// Всплывающая подсказка кнопки режима «стороны и рёбра»
        /// </summary>
        public static string ShowSidesRibsToolTip => Resources.SceneView_ToolTip_ShowSidesRibs;

        /// <summary>
        /// Всплывающая подсказка кнопки режима «рёбра»
        /// </summary>
        public static string ShowRibsToolTip => Resources.SceneView_ToolTip_ShowRibs;

        /// <summary>
        /// Всплывающая подсказка кнопки режима «стороны»
        /// </summary>
        public static string ShowSidesToolTip => Resources.SceneView_ToolTip_ShowSides;

        /// <summary>
        /// Всплывающая подсказка кнопки показа базиса
        /// </summary>
        public static string BasisToolTip => Resources.SceneView_ToolTip_Basis;

        /// <summary>
        /// Всплывающая подсказка кнопки показа контуров
        /// </summary>
        public static string ContoursToolTip => Resources.SceneView_ToolTip_Contours;

        /// <summary>
        /// Всплывающая подсказка кнопки раскрытия режимов отображения
        /// </summary>
        public static string DisplayStatesToolTip => Resources.SceneView_ToolTip_DisplayStates;

        /// <summary>
        /// Всплывающая подсказка кнопки снимка экрана
        /// </summary>
        public static string ScreenShotToolTip => Resources.SceneView_ToolTip_ScreenShot;

        /// <summary>
        /// Всплывающая подсказка кнопки вписывания в экран
        /// </summary>
        public static string FitToScreenToolTip => Resources.SceneView_ToolTip_FitToScreen;

        /// <summary>
        /// Всплывающая подсказка кнопки внутренних объектов
        /// </summary>
        public static string InsideObjectsToolTip => Resources.SceneView_ToolTip_InsideObjects;

        /// <summary>
        /// Всплывающая подсказка кнопки вида XY
        /// </summary>
        public static string PlaneXYToolTip => Resources.SceneView_ToolTip_PlaneXY;

        /// <summary>
        /// Всплывающая подсказка кнопки вида ZX
        /// </summary>
        public static string PlaneZXToolTip => Resources.SceneView_ToolTip_PlaneZX;

        /// <summary>
        /// Всплывающая подсказка кнопки вида ZY
        /// </summary>
        public static string PlaneZYToolTip => Resources.SceneView_ToolTip_PlaneZY;

        /// <summary>
        /// Всплывающая подсказка кнопки вращения вокруг X
        /// </summary>
        public static string RotateXToolTip => Resources.SceneView_ToolTip_RotateX;

        /// <summary>
        /// Всплывающая подсказка кнопки вращения вокруг Y
        /// </summary>
        public static string RotateYToolTip => Resources.SceneView_ToolTip_RotateY;

        /// <summary>
        /// Всплывающая подсказка кнопки вращения вокруг Z
        /// </summary>
        public static string RotateZToolTip => Resources.SceneView_ToolTip_RotateZ;

        /// <summary>
        /// Всплывающая подсказка кнопки поворота на 90° по горизонтали
        /// </summary>
        public static string RotateHorizontal90ToolTip => Resources.SceneView_ToolTip_RotateHorizontal90;

        /// <summary>
        /// Всплывающая подсказка кнопки поворота на 90° по вертикали
        /// </summary>
        public static string RotateVertical90ToolTip => Resources.SceneView_ToolTip_RotateVertical90;

        /// <summary>
        /// Название типа объектов для списка наборов и сообщений сцены; null — «Объекты» (фильтр не задан)
        /// </summary>
        public static string ObjectType(ObjType? type)
        {
            var selection = type.HasValue ? Converters.ConvertObjTypeToSelectionType(type.Value) : SelectionType.Objects;
            return Localization.GetSelectionTypeLocalization(selection);
        }
    }
}
