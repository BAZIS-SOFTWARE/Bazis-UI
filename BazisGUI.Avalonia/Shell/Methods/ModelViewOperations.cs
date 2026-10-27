using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using OperationalController;
using Project.Interfaces.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisGUI.Avalonia.Shell
{
    internal partial class MainWindow
    {
        // Обновление буферов по ModelView.Changed (SubscribeToModelView, ModelView_Changed, RefreshModelSetBuffer,
        // RecolorModelSetBuffer) выполняет SceneView; точки подключения — в Scene/SceneConnection.cs.

        /// <summary>
        /// Возвращает признак выделения объекта в текущем представлении.
        /// </summary>
        private bool IsSelected(IModelObject modelObject)
        {
            return project.IsSelected(modelObject.ObjType, modelObject.Number);
        }

        /// <summary>
        /// Возвращает видимые объекты заданного набора.
        /// </summary>
        private IEnumerable<int> GetVisibleNumbers(ISetInfo setInfo)
        {
            foreach (var number in setInfo.GetNumbers())
                if (project.GetVisible(setInfo.ObjType, number))
                    yield return number;
        }

        /// <summary>
        /// Применяет цвет выделения из настроек к текущему представлению.
        /// </summary>
        private void ApplySelectionColor()
        {
            if (project != null)
                project.SetSelectionColor(settingsConfig.SelectObjectColor);
        }

        /// <summary>
        /// Применяет настройки цвета выделения и прозрачности к текущему представлению.
        /// </summary>
        private void ApplyModelViewSettings()
        {
            if (project == null)
                return;

            using (project.BeginViewUpdate())
            {
                project.SetSelectionColor(settingsConfig.SelectObjectColor);
                project.SetTransparency(GetModelViewTransparency());
                // Цвет узлов из настроек хранится в конфиге, а новый проект получает цвет по умолчанию
                if (!settingsConfig.NodeColor.IsEmpty)
                    foreach (var set in project.GetModelSetsInfo(ObjType.Узел))
                        project.SetColor(set, settingsConfig.NodeColor);
            }
        }

        /// <summary>
        /// Преобразует процент прозрачности интерфейса в альфа-канал представления.
        /// Вне режима прозрачности модель непрозрачна.
        /// </summary>
        private byte GetModelViewTransparency()
        {
            if (!settingsConfig.Transparency)
                return byte.MaxValue;

            var transparency = settingsConfig.TransparencyValue * byte.MaxValue / 100;
            return (byte)Math.Clamp(transparency, byte.MinValue, byte.MaxValue);
        }

        /// <summary>
        /// Возвращает цвет визуализации условия расчётной задачи.
        /// </summary>
        private Color GetConditionColor(DataKind dataKind)
        {
            if (dataKind == DataKind.Материал)
                return Color.FromArgb(255, 255, 0);
            if (dataKind == DataKind.Среда)
                return Color.FromArgb(255, 155, 0);
            if (dataKind == DataKind.Закрепление || dataKind == DataKind.Нагрузка)
                return Color.FromArgb(255, 0, 0);
            if (dataKind == DataKind.Нагрев)
                return Color.FromArgb(125, 155, 255, 0);

            return settingsConfig.SelectObjectColor;
        }
    }
}
