using AvaloniaGUI.Navigator;
using AvaloniaGUI.Shell;
using System.Resources;

namespace AvaloniaGUI.Localization;

internal static class Localization
{
    // Аналог ComponentResourceManager(typeof(BaseForm)): ресурсы формы MainWindow.resx
    private static readonly ResourceManager formResources = new(typeof(MainWindow));

    /// <summary>
    /// Получить подпись об отсутствии искомого файла на текущей языковой культуре
    /// </summary>
    // Подписи FileAbsenceCaption, ErrorCaption и AttentionCaption есть только в общих ресурсах (Resources.resx);
    // в ресурсах формы их нет, и GetString возвращал null — окна сообщений были без заголовка.
    public static string GetFileMissingCaption()
    {
        return Resources.FileAbsenceCaption;
    }

    /// <summary>
    /// Получение подписи элемента формы BaseForm (MainWindow.resx) на текущей языковой культуре
    /// </summary>
    public static string GetFormText(string name)
    {
        return formResources.GetString(name);
    }

    /// <summary>
    /// Получение подписи об ошибке на текущей языковой культуре
    /// </summary>
    public static string GetErrorCaption()
    {
        return Resources.ErrorCaption;
    }

    /// <summary>
    /// Получение подписи "Внимание"
    /// </summary>
    public static string GetAttentionCaption()
    {
        return Resources.AttentionCaption;
    }

    /// <summary>
    /// Получение стека сообщения об ошибке вместе со стеком на текущей языковой культуре
    /// </summary>
    public static string GetErrorWithStackMessage(Exception ex)
    {
        return $"{ex.Message} {Resources.StackTrace}:{ex.StackTrace}";
    }

    public static string GetErrorWithSource(Exception ex)
    {
        return $"{Resources.Error}: {ex.Message}.\n{Resources.Source}: {ex.Source}";
    }

    public static string GetMethodIsNotImplementedExceptionCaption()
    {
        return Resources.MethodIsNotImplementedException;
    }

    public static string GetStartCaption()
    {
        return Resources.StartCaption;
    }

    public static string GetStopCaption()
    {
        return Resources.StopCaption;
    }

    /// <summary>
    /// Получение выбранного типа объектов на текущей языковой культуре
    /// </summary>
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
