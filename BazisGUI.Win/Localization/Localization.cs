using BazisGUI.Win.Navigator;
using BazisGUI.Win.Properties;
using BazisGUI.Win.PropertiesPanel;
using System;
using System.ComponentModel;

namespace BazisGUI.Win.Localization
{
    public static class Localization
    {
        /// <summary>
        /// Получить подпись об отсутствии искомого файла на текущей языковой культуре
        /// </summary>
        /// <returns>Строковая подпись на текущей языковой культуре</returns>
        public static string GetFileMissingCaption()
        {
            var resources = new ComponentResourceManager(typeof(BaseForm));
            return resources.GetString("FileAbsenceCaption");
        }

        /// <summary>
        /// Получение подписи об ошибке на текущей языковой культуре
        /// </summary>
        /// <returns>Строковая подпись на текущей языковой культуре</returns>
        public static string GetErrorCaption()
        {
            var resources = new ComponentResourceManager(typeof(BaseForm));
            return resources.GetString("ErrorCaption");
        }

        /// <summary>
        /// Получение подписи "Внимание"
        /// </summary>
        /// <returns>Строковая подпись на текущей языковой культуре</returns>
        public static string GetAttentionCaption()
        {
            var resources = new ComponentResourceManager(typeof(BaseForm));
            return resources.GetString("AttentionCaption");
        }

        /// <summary>
        /// Получение стека сообщения об ошибке вместе со стеком на текущей языковой культуре
        /// </summary>
        /// <param name="ex">Возникшая ошибка</param>
        /// <returns>Строка с ошибкой и стеком вызовов на текущей языковой культуре</returns>
        public static string GetErrorWithStackMessage(Exception ex)
        {
            return $"{ex.Message} {Resources.StackTrace}:{ex.StackTrace}";
        }

        public static string GetErrorWithSource(Exception ex)
        {
            return $"{Resources.Error}: {ex.Message}.\n{Resources.Source}: {ex.Source}";
        }

        /// <summary>
        /// Получение подписи-предупреждения об ошибке отсутствия реализации на текущей языковой культуре
        /// </summary>
        /// <returns>Сообщение об ошибке, вызванной отсутствием реализации на текущей языковой культуре</returns>
        public static string GetMethodIsNotImplementedExceptionCaption()
        {
            return Resources.MethodIsNotImplementedException;
        }

        /// <summary>
        /// Получение подписи "Старт" на текущей языковой культуре
        /// </summary>
        /// <returns>Строка с подписью "Старт" на текущей языковой культуре</returns>
        public static string GetStartCaption()
        {
            return Resources.StartCaption;
        }

        /// <summary>
        /// Получение подписи "Стоп" на текущей языковой культуре
        /// </summary>
        /// <returns>Строка с подписью "Стоп" на текущей языковой культуре</returns>
        public static string GetStopCaption()
        {
            return Resources.StopCaption;
        }

        /// <summary>
        /// Получение выбранного типа объектов на текущей языковой культуре
        /// </summary>
        /// <param name="select">Выбранный тип объектов</param>
        /// <returns>Строковое представление выбранного типа объектов на текущей языковой культуре</returns>
        public static string GetSelectionTypeLocalization(SelectionType select)
        {
            switch (select) 
            {
                case SelectionType.Select:
                    return Resources.btnSelect_Text_Select;
                case SelectionType.Points:
                    return Resources.btnSelect_Text_Points;
                case SelectionType.Curves:
                    return Resources.btnSelect_Text_Curves;
                case SelectionType.Surfaces:
                    return Resources.btnSelect_Text_Surfaces;
                case SelectionType.Nodes:
                    return Resources.btnSelect_Text_Nodes;
                case SelectionType.Elements1D:
                    return Resources.btnSelect_Text_Elements1D;
                case SelectionType.Elements2D:
                    return Resources.btnSelect_Text_Elements2D;
                case SelectionType.Elements3D:
                    return Resources.btnSelect_Text_Elements3D;
                default:
                    return Resources.btnSelect_Text_Objects;
            }
        }

        public static string GetNavigatorNodeNameLocalization(NodeName nodeName)
        {
            switch (nodeName)
            {
                case NodeName.Geometry:
                    return Resources.Navigator_TreeView_Node_Text_Geometry;
                case NodeName.Mesh:
                    return Resources.Navigator_TreeView_Node_Text_Mesh;
                case NodeName.Sets:
                    return Resources.Navigator_TreeView_Node_Text_Sets;
                case NodeName.Objects:
                    return Resources.Navigator_TreeView_Node_Text_Objects;
                case NodeName.Groups:
                    return Resources.Navigator_TreeView_Node_Text_Groups;
                case NodeName.NodesGroup:
                    return Resources.Navigator_TreeView_Node_Text_NodesGroup;
                case NodeName.ElementsGroup:
                    return Resources.Navigator_TreeView_Node_Text_ElementsGroup;
                case NodeName.Task:
                    return Resources.Navigator_TreeView_Node_Text_Task;
                case NodeName.Material:
                    return Resources.Navigator_TreeView_Node_Text_Material;
                case NodeName.Media:
                    return Resources.Navigator_TreeView_Node_Text_Media;
                case NodeName.Heat:
                    return Resources.Navigator_TreeView_Node_Text_Heat;
                case NodeName.Clamp:
                    return Resources.Navigator_TreeView_Node_Text_Clamp;
                case NodeName.Load:
                    return Resources.Navigator_TreeView_Node_Text_Load;
                case NodeName.Calculations:
                    return Resources.Navigator_TreeView_Node_Text_Calculations;
                case NodeName.Calculation:
                    return Resources.Navigator_TreeView_Node_Text_Calculation;
                case NodeName.Results:
                    return Resources.Navigator_TreeView_Node_Text_Results;
                case NodeName.Result:
                    return Resources.Navigator_TreeView_Node_Text_Result;
                case NodeName.Time:
                    return Resources.Navigator_TreeView_Node_Text_Time;
                default:
                    return Resources.Navigator_TreeView_Node_Text_Project;
            }
        }
    }
}
