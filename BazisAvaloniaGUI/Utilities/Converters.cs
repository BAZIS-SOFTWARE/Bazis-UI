using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Shell;
using Model.Interfaces;
using Project.Interfaces.Tasks;
using Project.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using static BazisAvaloniaGUI.Shell.MainWindow;

namespace BazisAvaloniaGUI.Utilities
{
    internal static class Converters
    {
        public static DataKind ConvertToDataKind(string dataKind)
        {
            DataKind objType;
            return Enum.TryParse(dataKind, out objType) ? objType :
                throw new Exception($"{Resources.ConvertFailCaption}:{dataKind} -> DataKind");
        }

        /// <summary>
        /// Метод для получения всех строковых значений для комбобокса
        /// </summary>
        public static List<string> GetEnumNames<T>() where T : Enum => Enum.GetNames(typeof(T)).ToList();

        public static ClampKind ConvertClampKindKeysToClampKind(ClampKindKeys key)
        {
            switch (key)
            {
                case ClampKindKeys.Hard: return ClampKind.Жесткое;
                case ClampKindKeys.Flexable: return ClampKind.Упругое;
                case ClampKindKeys.Symmetric: return ClampKind.Симметрия;
                case ClampKindKeys.Contact: return ClampKind.Контакт;
                default: throw new InvalidCastException(string.Format(Resources.Converters_ConvertClampKindKeysToClampKind_CastExc, "ClampKindKeys", "ClampKind"));
            }
        }

        public static LoadKind ConvertLoadKindKeysToLoadKind(LoadKindKeys key)
        {
            switch (key)
            {
                case LoadKindKeys.Force: return LoadKind.Сила;
                case LoadKindKeys.Pressure: return LoadKind.Давление;
                default: throw new InvalidCastException(string.Format(Resources.Converters_ConvertClampKindKeysToClampKind_CastExc, "LoadKindKeys", "LoadKind"));
            }
        }

        public static string GetDisplayName(TaskKind type)
        {
            return type switch
            {
                TaskKind.химическая => Resources.TaskKind_Chemical,
                TaskKind.термическая => Resources.TaskKind_Termal,
                TaskKind.механическая => Resources.TaskKind_Mechanical,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public static TaskKind GetTaskKind(string displayName)
        {
            if (displayName == Resources.TaskKind_Chemical)
                return TaskKind.химическая;

            if (displayName == Resources.TaskKind_Termal)
                return TaskKind.термическая;

            if (displayName == Resources.TaskKind_Mechanical)
                return TaskKind.механическая;

            throw new ArgumentOutOfRangeException(nameof(displayName), displayName, null);
        }

        public static ObjType ConvertSelectionTypeToObjType(SelectionType st)
        {
            switch (st)
            {
                case SelectionType.Points: return ObjType.Точка;
                case SelectionType.Curves: return ObjType.Кривая;
                case SelectionType.Surfaces: return ObjType.Поверхность;
                case SelectionType.Nodes: return ObjType.Узел;
                case SelectionType.Elements1D: return ObjType.Элемент1D;
                case SelectionType.Elements2D: return ObjType.Элемент2D;
                case SelectionType.Elements3D: return ObjType.Элемент3D;
                default:
                    throw new ArgumentException($"{Resources.ConvertFailCaption}:{st.ToString()} -> ObjType");
            }
        }

        public static bool TryConvertSelectionTypeToObjType(SelectionType st, out ObjType res)
        {
            try
            {
                res = ConvertSelectionTypeToObjType(st);
                return true;
            }
            catch (Exception ex)
            {
                res = ObjType.Узел;
                return false;
            }
        }

        public static SelectionType ConvertObjTypeToSelectionType(ObjType ot)
        {
            switch (ot)
            {
                case ObjType.Точка: return SelectionType.Points;
                case ObjType.Кривая: return SelectionType.Curves;
                case ObjType.Поверхность: return SelectionType.Surfaces;
                case ObjType.Узел: return SelectionType.Nodes;
                case ObjType.Элемент1D: return SelectionType.Elements1D;
                case ObjType.Элемент2D: return SelectionType.Elements2D;
                case ObjType.Элемент3D: return SelectionType.Elements3D;
                default: return SelectionType.Objects;
            }
        }
    }
}
