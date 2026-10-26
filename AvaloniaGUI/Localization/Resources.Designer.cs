// Строковые ресурсы перенесены из GUI/Properties/Resources.resx; имена свойств — как в GUI/Properties/Resources.Designer.cs.
namespace AvaloniaGUI.Localization;

internal class Resources
{
    protected Resources() { }
    private static System.Resources.ResourceManager resourceMan;

    internal static System.Resources.ResourceManager ResourceManager =>
        resourceMan ??= new System.Resources.ResourceManager(typeof(Resources));

    internal static System.Globalization.CultureInfo Culture { get; set; }

    internal static string About => ResourceManager.GetString("About", Culture);

    internal static string AddBranch_ExceptionMessage => ResourceManager.GetString("AddBranch.ExceptionMessage", Culture);

    internal static string AddReaction => ResourceManager.GetString("AddReaction", Culture);

    internal static string AdvanceSelection2NodesWarning => ResourceManager.GetString("AdvanceSelection2NodesWarning", Culture);

    internal static string AdvanceSelection3NodesWarning => ResourceManager.GetString("AdvanceSelection3NodesWarning", Culture);

    internal static string AdvanceSelectionElemntsSelectionWarning => ResourceManager.GetString("AdvanceSelectionElemntsSelectionWarning", Culture);

    internal static string AdvanceSelectionForm_Text => ResourceManager.GetString("AdvanceSelectionForm.Text", Culture);

    internal static string AdvanceSelectionNoObjectSelectedWarning => ResourceManager.GetString("AdvanceSelectionNoObjectSelectedWarning", Culture);

    internal static string AdvaneSelectionSelectedCaption => ResourceManager.GetString("AdvaneSelectionSelectedCaption", Culture);

    internal static string AlreadyExistsNRenameTheMaterial => ResourceManager.GetString("AlreadyExistsNRenameTheMaterial", Culture);

    internal static string AnimationForm_Text => ResourceManager.GetString("AnimationForm.Text", Culture);

    internal static string AttentionCaption => ResourceManager.GetString("AttentionCaption", Culture);

    internal static string AvailableCommands => ResourceManager.GetString("AvailableCommands", Culture);

    internal static string BaseForm_BaseForm_Load_Navigator => ResourceManager.GetString("BaseForm_BaseForm_Load_Navigator", Culture);

    internal static string BaseForm_материалыMenuItem_Click_Materials => ResourceManager.GetString("BaseForm_материалыMenuItem_Click_Materials", Culture);

    internal static string BaseForm_настройкиToolStripMenuItem_Click_Settings => ResourceManager.GetString("BaseForm_настройкиToolStripMenuItem_Click_Settings", Culture);

    internal static string BaseForm_функцииMenuItem_Click_Functions => ResourceManager.GetString("BaseForm_функцииMenuItem_Click_Functions", Culture);

    internal static string BasePage_CreateSurfaceAsync_AsyncContainer_Message => ResourceManager.GetString("BasePage.CreateSurfaceAsync.AsyncContainer.Message", Culture);

    internal static string BasePage_CreateSurfaceAsync_SelectNodeType_Message => ResourceManager.GetString("BasePage.CreateSurfaceAsync.SelectNodeType.Message", Culture);

    internal static string BasePage_CreateSurfaceAsync_SelectThreeNodes_Message => ResourceManager.GetString("BasePage.CreateSurfaceAsync.SelectThreeNodes.Message", Culture);

    internal static string BasePage_CreateSurfaceAsync_SurfaceSet_Message => ResourceManager.GetString("BasePage.CreateSurfaceAsync.SurfaceSet.Message", Culture);

    internal static string BasePage_CreateSurfaceAync_OperationCanceled_Message => ResourceManager.GetString("BasePage.CreateSurfaceAync.OperationCanceled.Message", Culture);

    internal static string BazisServerPathMissingMessage => ResourceManager.GetString("BazisServerPathMissingMessage", Culture);

    internal static string BeamConnection_BeamConnection_ObjectsCreated_Message => ResourceManager.GetString("BeamConnection.BeamConnection.ObjectsCreated.Message", Culture);

    internal static string BeamConnectionEventArgsArgNumExc => ResourceManager.GetString("BeamConnectionEventArgsArgNumExc", Culture);

    internal static string btnCalc_Text => ResourceManager.GetString("btnCalc.Text", Culture);

    internal static string btnCreateCopy_Text => ResourceManager.GetString("btnCreateCopy.Text", Culture);

    internal static string btnCreateCopy_ToolTip => ResourceManager.GetString("btnCreateCopy.ToolTip", Culture);

    internal static string btnMeasure_Text => ResourceManager.GetString("btnMeasure.Text", Culture);

    internal static string btnSelect_AccessibleName => ResourceManager.GetString("btnSelect.AccessibleName", Culture);

    internal static string btnSelect_Text_Curves => ResourceManager.GetString("btnSelect.Text.Curves", Culture);

    internal static string btnSelect_Text_Elements1D => ResourceManager.GetString("btnSelect.Text.Elements1D", Culture);

    internal static string btnSelect_Text_Elements2D => ResourceManager.GetString("btnSelect.Text.Elements2D", Culture);

    internal static string btnSelect_Text_Elements3D => ResourceManager.GetString("btnSelect.Text.Elements3D", Culture);

    internal static string btnSelect_Text_Nodes => ResourceManager.GetString("btnSelect.Text.Nodes", Culture);

    internal static string btnSelect_Text_Objects => ResourceManager.GetString("btnSelect.Text.Objects", Culture);

    internal static string btnSelect_Text_Points => ResourceManager.GetString("btnSelect.Text.Points", Culture);

    internal static string btnSelect_Text_Select => ResourceManager.GetString("btnSelect_Text_Select", Culture);

    internal static string btnSelect_Text_Surfaces => ResourceManager.GetString("btnSelect.Text.Surfaces", Culture);

    internal static string button1_Text => ResourceManager.GetString("button1.Text", Culture);

    internal static string button1_ToolTip => ResourceManager.GetString("button1.ToolTip", Culture);

    internal static string button2_Text => ResourceManager.GetString("button2.Text", Culture);

    internal static string CalculateCreep => ResourceManager.GetString("CalculateCreep", Culture);

    internal static string CalculateDiagram => ResourceManager.GetString("CalculateDiagram", Culture);

    internal static string CalculateHardening => ResourceManager.GetString("CalculateHardening", Culture);

    internal static string CallBackFormName => ResourceManager.GetString("CallBackFormName", Culture);

    internal static string ChamferPreview_AngleMustBeLessThan => ResourceManager.GetString("ChamferPreview_AngleMustBeLessThan", Culture);

    internal static string ChamferPreview_AngleOutOfRange => ResourceManager.GetString("ChamferPreview_AngleOutOfRange", Culture);

    internal static string ChamferPreview_CalculatedLengthMustBePositive => ResourceManager.GetString("ChamferPreview_CalculatedLengthMustBePositive", Culture);

    internal static string ChamferPreview_CurveHasZeroLength => ResourceManager.GetString("ChamferPreview_CurveHasZeroLength", Culture);

    internal static string ChamferPreview_CurveMustBelongToTwoSurfaces => ResourceManager.GetString("ChamferPreview_CurveMustBelongToTwoSurfaces", Culture);

    internal static string ChamferPreview_CurveParametrizationUnavailable => ResourceManager.GetString("ChamferPreview_CurveParametrizationUnavailable", Culture);

    internal static string ChamferPreview_InvalidGeometryCoordinates => ResourceManager.GetString("ChamferPreview_InvalidGeometryCoordinates", Culture);

    internal static string ChamferPreview_InvalidSurfaceAngle => ResourceManager.GetString("ChamferPreview_InvalidSurfaceAngle", Culture);

    internal static string ChamferPreview_OffsetDirectionUnavailable => ResourceManager.GetString("ChamferPreview_OffsetDirectionUnavailable", Culture);

    internal static string ChamferPreview_OnlyStraightCurvesSupported => ResourceManager.GetString("ChamferPreview_OnlyStraightCurvesSupported", Culture);

    internal static string ChamferPreview_SurfaceHasInvalidNormal => ResourceManager.GetString("ChamferPreview_SurfaceHasInvalidNormal", Culture);

    internal static string ChamferPreview_SurfaceNormalUnavailable => ResourceManager.GetString("ChamferPreview_SurfaceNormalUnavailable", Culture);

    internal static string ChamferPreview_SurfaceNotOnVolumeBoundary => ResourceManager.GetString("ChamferPreview_SurfaceNotOnVolumeBoundary", Culture);

    internal static string ChamferPreview_SurfacesMustBelongToOneVolume => ResourceManager.GetString("ChamferPreview_SurfacesMustBelongToOneVolume", Culture);

    internal static string ChamferWindow_AddButton => ResourceManager.GetString("ChamferWindow_AddButton", Culture);

    internal static string ChamferWindow_Angle => ResourceManager.GetString("ChamferWindow_Angle", Culture);

    internal static string ChamferWindow_AngleModeTab => ResourceManager.GetString("ChamferWindow_AngleModeTab", Culture);

    internal static string ChamferWindow_FirstLength => ResourceManager.GetString("ChamferWindow_FirstLength", Culture);

    internal static string ChamferWindow_LengthsModeTab => ResourceManager.GetString("ChamferWindow_LengthsModeTab", Culture);

    internal static string ChamferWindow_ReflectTooltip => ResourceManager.GetString("ChamferWindow_ReflectTooltip", Culture);

    internal static string ChamferWindow_SecondLength => ResourceManager.GetString("ChamferWindow_SecondLength", Culture);

    internal static string ChamferWindow_Title => ResourceManager.GetString("ChamferWindow_Title", Culture);

    internal static string ChangeCondProperties_ChangeGeneralProperties_FileNotSelected_Message => ResourceManager.GetString("ChangeCondProperties.ChangeGeneralProperties.FileNotSelected.Message", Culture);

    internal static string ChangeTaskInvalidExc => ResourceManager.GetString("ChangeTaskInvalidExc", Culture);

    internal static string ChangeTaskTypeWithoutProjectExc => ResourceManager.GetString("ChangeTaskTypeWithoutProjectExc", Culture);

    internal static string chbChangeDirection_Text => ResourceManager.GetString("chbChangeDirection.Text", Culture);

    internal static string chbTemp_Text => ResourceManager.GetString("chbTemp.Text", Culture);

    internal static string chbTimeDependent_Text => ResourceManager.GetString("chbTimeDependent.Text", Culture);

    internal static string Checking_StartCheckingEvent_SelectedDataIsNotCheckable_Message => ResourceManager.GetString("Checking.StartCheckingEvent.SelectedDataIsNotCheckable.Message", Culture);

    internal static string clslLigthingIntensity_Text => ResourceManager.GetString("clslLigthingIntensity.Text", Culture);

    internal static string clslTransparency_Text => ResourceManager.GetString("clslTransparency.Text", Culture);

    internal static string cmbFinalPhase_AccessibleName => ResourceManager.GetString("cmbFinalPhase.AccessibleName", Culture);

    internal static string cmbInitialPhase_AccessibleName => ResourceManager.GetString("cmbInitialPhase.AccessibleName", Culture);

    internal static string cmbMeasureObjects_Items => ResourceManager.GetString("cmbMeasureObjects.Items", Culture);

    internal static string cmbMeasureObjects_Items1 => ResourceManager.GetString("cmbMeasureObjects.Items1", Culture);

    internal static string cmbPhaseName_AccessibleName => ResourceManager.GetString("cmbPhaseName.AccessibleName", Culture);

    internal static string cmbPhaseName_Items => ResourceManager.GetString("cmbPhaseName.Items", Culture);

    internal static string cmbPhaseName_Items1 => ResourceManager.GetString("cmbPhaseName.Items1", Culture);

    internal static string cmbPhaseName_Items2 => ResourceManager.GetString("cmbPhaseName.Items2", Culture);

    internal static string cmbPhases_AccessibleName => ResourceManager.GetString("cmbPhases.AccessibleName", Culture);

    internal static string cntrГант_AddConds_UndefinedCondExc => ResourceManager.GetString("cntrГант_AddConds_UndefinedCondExc", Culture);

    internal static string cntrГант_AddConds_Закрепление => ResourceManager.GetString("cntrГант_AddConds_Закрепление", Culture);

    internal static string cntrГант_AddConds_Материал => ResourceManager.GetString("cntrГант_AddConds_Материал", Culture);

    internal static string cntrГант_AddConds_Нагрев => ResourceManager.GetString("cntrГант_AddConds_Нагрев", Culture);

    internal static string cntrГант_AddConds_Нагрузка => ResourceManager.GetString("cntrГант_AddConds_Нагрузка", Culture);

    internal static string cntrГант_AddConds_Среда => ResourceManager.GetString("cntrГант_AddConds_Среда", Culture);

    internal static string cntrГант_headerName_text => ResourceManager.GetString("cntrГант_headerName_text", Culture);

    internal static string ColorObjects_ObjectConversion_Exception => ResourceManager.GetString("ColorObjects.ObjectConversion.Exception", Culture);

    internal static string colorSlider1_Text => ResourceManager.GetString("colorSlider1.Text", Culture);

    internal static string colorSlider2_Text => ResourceManager.GetString("colorSlider2.Text", Culture);

    internal static string colorSlider3_Text => ResourceManager.GetString("colorSlider3.Text", Culture);

    internal static string comboBox1_ToolTip => ResourceManager.GetString("comboBox1.ToolTip", Culture);

    internal static string Comp_Chem_Header_InitialConcentration => ResourceManager.GetString("Comp.Chem.Header.InitialConcentration", Culture);

    internal static string Comp_Chem_Header_MaxConcentration => ResourceManager.GetString("Comp.Chem.Header.MaxConcentration", Culture);

    internal static string Comp_Chem_Header_MaxConcentrationValue => ResourceManager.GetString("Comp.Chem.Header.MaxConcentrationValue", Culture);

    internal static string Comp_General_Header_Accuracy => ResourceManager.GetString("Comp.General.Header.Accuracy", Culture);

    internal static string Comp_General_Header_Execute => ResourceManager.GetString("Comp.General.Header.Execute", Culture);

    internal static string Comp_General_Header_InitialStep => ResourceManager.GetString("Comp.General.Header.InitialStep", Culture);

    internal static string Comp_General_Header_InitialTemp => ResourceManager.GetString("Comp.General.Header.InitialTemp", Culture);

    internal static string Comp_General_Header_IterationQuantity => ResourceManager.GetString("Comp.General.Header.IterationQuantity", Culture);

    internal static string Comp_General_Header_IterationsPerStep => ResourceManager.GetString("Comp.General.Header.IterationsPerStep", Culture);

    internal static string Comp_General_Header_MaxCalcStep => ResourceManager.GetString("Comp.General.Header.MaxCalcStep", Culture);

    internal static string Comp_General_Header_MinCalcStep => ResourceManager.GetString("Comp.General.Header.MinCalcStep", Culture);

    internal static string Comp_General_Header_Priority => ResourceManager.GetString("Comp.General.Header.Priority", Culture);

    internal static string Comp_General_Header_RelaxCoef => ResourceManager.GetString("Comp.General.Header.RelaxCoef", Culture);

    internal static string Comp_General_Header_SaveFrequency => ResourceManager.GetString("Comp.General.Header.SaveFrequency", Culture);

    internal static string Comp_General_Header_StartTime => ResourceManager.GetString("Comp.General.Header.StartTime", Culture);

    internal static string Comp_General_Header_StopTime => ResourceManager.GetString("Comp.General.Header.StopTime", Culture);

    internal static string Comp_General_Header_Type => ResourceManager.GetString("Comp.General.Header.Type", Culture);

    internal static string Comp_General_HeaderMaxRelaxCoef => ResourceManager.GetString("Comp.General.HeaderMaxRelaxCoef", Culture);

    internal static string Comp_GeneralHeader_Algorithm => ResourceManager.GetString("Comp.GeneralHeader.Algorithm", Culture);

    internal static string Comp_Mech_Header_MaxDifference => ResourceManager.GetString("Comp.Mech.Header.MaxDifference", Culture);

    internal static string Comp_Mech_Header_MaxDisplacement => ResourceManager.GetString("Comp.Mech.Header.MaxDisplacement", Culture);

    internal static string Comp_Mech_Header_MaxDisplacementValue => ResourceManager.GetString("Comp.Mech.Header.MaxDisplacementValue", Culture);

    internal static string Comp_Mech_Header_PlasticDeformation => ResourceManager.GetString("Comp.Mech.Header.PlasticDeformation", Culture);

    internal static string Comp_Mech_Header_PlasticDeformationValue => ResourceManager.GetString("Comp.Mech.Header.PlasticDeformationValue", Culture);

    internal static string Comp_Term_Header_MaxTemperture => ResourceManager.GetString("Comp.Term.Header.MaxTemperture", Culture);

    internal static string Comp_Term_Header_MaxTempertureValue => ResourceManager.GetString("Comp.Term.Header.MaxTempertureValue", Culture);

    internal static string Comp_Value_Execute => ResourceManager.GetString("Comp.Value.Execute", Culture);

    internal static string Comp_Value_Skip => ResourceManager.GetString("Comp.Value.Skip", Culture);

    internal static string Cond_Clamp_Header_Direction => ResourceManager.GetString("Cond.Clamp.Header.Direction", Culture);

    internal static string Cond_Clamp_Header_Type => ResourceManager.GetString("Cond.Clamp.Header.Type", Culture);

    internal static string Cond_General_Header_CoordinateSystem => ResourceManager.GetString("Cond.General.Header.CoordinateSystem", Culture);

    internal static string Cond_General_Header_File => ResourceManager.GetString("Cond.General.Header.File", Culture);

    internal static string Cond_General_Header_Function => ResourceManager.GetString("Cond.General.Header.Function", Culture);

    internal static string Cond_General_Header_Group => ResourceManager.GetString("Cond.General.Header.Group", Culture);

    internal static string Cond_General_Header_Parameter => ResourceManager.GetString("Cond.General.Header.Parameter", Culture);

    internal static string Cond_General_Header_Plane => ResourceManager.GetString("Cond.General.Header.Plane", Culture);

    internal static string Cond_General_Header_ReferenceLine => ResourceManager.GetString("Cond.General.Header.ReferenceLine", Culture);

    internal static string Cond_General_Header_RotationX => ResourceManager.GetString("Cond.General.Header.RotationX", Culture);

    internal static string Cond_General_Header_RotationY => ResourceManager.GetString("Cond.General.Header.RotationY", Culture);

    internal static string Cond_General_Header_RotationZ => ResourceManager.GetString("Cond.General.Header.RotationZ", Culture);

    internal static string Cond_General_Header_ShiftingX => ResourceManager.GetString("Cond.General.Header.ShiftingX", Culture);

    internal static string Cond_General_Header_ShiftingY => ResourceManager.GetString("Cond.General.Header.ShiftingY", Culture);

    internal static string Cond_General_Header_ShiftingZ => ResourceManager.GetString("Cond.General.Header.ShiftingZ", Culture);

    internal static string Cond_General_Header_Speed => ResourceManager.GetString("Cond.General.Header.Speed", Culture);

    internal static string Cond_General_Header_StartTime => ResourceManager.GetString("Cond.General.Header.StartTime", Culture);

    internal static string Cond_General_Header_StopTime => ResourceManager.GetString("Cond.General.Header.StopTime", Culture);

    internal static string Cond_General_Header_Table => ResourceManager.GetString("Cond.General.Header.Table", Culture);

    internal static string Cond_General_Header_Trajectory => ResourceManager.GetString("Cond.General.Header.Trajectory", Culture);

    internal static string Cond_General_Header_Value => ResourceManager.GetString("Cond.General.Header.Value", Culture);

    internal static string Cond_Heat_Header_Power => ResourceManager.GetString("Cond.Heat.Header.Power", Culture);

    internal static string Cond_Load_Header_Direction => ResourceManager.GetString("Cond.Load.Header.Direction", Culture);

    internal static string Cond_Load_Header_Type => ResourceManager.GetString("Cond.Load.Header.Type", Culture);

    internal static string Cond_Load_Header_Value => ResourceManager.GetString("Cond.Load.Header.Value", Culture);

    internal static string Cond_Material_Header_Diameter => ResourceManager.GetString("Cond.Material.Header.Diameter", Culture);

    internal static string Cond_Material_Header_Material => ResourceManager.GetString("Cond.Material.Header.Material", Culture);

    internal static string Cond_Material_Header_Thickness => ResourceManager.GetString("Cond.Material.Header.Thickness", Culture);

    internal static string ConnectionEstablishedAnswer => ResourceManager.GetString("ConnectionEstablishedAnswer", Culture);

    internal static string ConsoleControl_headerName_text => ResourceManager.GetString("ConsoleControl_headerName_text", Culture);

    internal static string ConsoleEvents_ConsoleInEvent_ObjectFound_Message => ResourceManager.GetString("ConsoleEvents.ConsoleInEvent.ObjectFound.Message", Culture);

    internal static string ConsoleEvents_ConsoleInEvent_ObjectNotFound_Message => ResourceManager.GetString("ConsoleEvents.ConsoleInEvent.ObjectNotFound.Message", Culture);

    internal static string ConsoleEvents_ConsoleInEvents_VolumeElements_Message => ResourceManager.GetString("ConsoleEvents.ConsoleInEvents.VolumeElements.Message", Culture);

    internal static string Converters_ConvertClampKindKeysToClampKind_CastExc => ResourceManager.GetString("Converters_ConvertClampKindKeysToClampKind_CastExc", Culture);

    internal static string ConvertFailCaption => ResourceManager.GetString("ConvertFailCaption", Culture);

    internal static string CopySuffics => ResourceManager.GetString("CopySuffics", Culture);

    internal static string CreateDiagram_LackOfCalcDataWarning => ResourceManager.GetString("CreateDiagram.LackOfCalcDataWarning", Culture);

    internal static string CreatePlot_BuildGraph_BuildingGraph_Text_Part1 => ResourceManager.GetString("CreatePlot.BuildGraph.BuildingGraph.Text_Part1", Culture);

    internal static string CreatePlot_BuildGraph_BuildingGraph_Text_Part2 => ResourceManager.GetString("CreatePlot.BuildGraph.BuildingGraph.Text_Part2", Culture);

    internal static string CreatePlot_BuildGraph_NoNodesSelected_Exception => ResourceManager.GetString("CreatePlot.BuildGraph.NoNodesSelected.Exception", Culture);

    internal static string CreatePlot_BuildGraph_SelectContainerAsync_SelectNodes_Message => ResourceManager.GetString("CreatePlot.BuildGraph.SelectContainerAsync.SelectNodes.Message", Culture);

    internal static string CreatePlot_BuildGraph_SelectContainerAsync_SelectResult_Message => ResourceManager.GetString("CreatePlot.BuildGraph.SelectContainerAsync.SelectResult.Message", Culture);

    internal static string CreatePlot_BuildGraph_SelectResult_Exception => ResourceManager.GetString("CreatePlot.BuildGraph.SelectResult.Exception", Culture);

    internal static string CreatePlot_CreateGraphData_Header => ResourceManager.GetString("CreatePlot.CreateGraphData.Header", Culture);

    internal static string CreatePlot_DisplayText3D_Text => ResourceManager.GetString("CreatePlot.DisplayText3D.Text", Culture);

    internal static string CreatePlot_GraphData_Header_Part1 => ResourceManager.GetString("CreatePlot.GraphData.Header_Part1", Culture);

    internal static string CreatePlot_GraphData_XUnit => ResourceManager.GetString("CreatePlot.GraphData.XUnit", Culture);

    internal static string CreatePlot_GraphForm_Text_Part1 => ResourceManager.GetString("CreatePlot.GraphForm.Text_Part1", Culture);

    internal static string CreatePlot_GraphForm_Text_Part2 => ResourceManager.GetString("CreatePlot.GraphForm.Text_Part2", Culture);

    internal static string CreatePlot_SelectContainerAsync_CancelOperation_Message => ResourceManager.GetString("CreatePlot.SelectContainerAsync.CancelOperation.Message", Culture);

    internal static string CreateTaskWithoutProjectExc => ResourceManager.GetString("CreateTaskWithoutProjectExc", Culture);

    internal static string CrossSectionControl_InvalidSurfaceCoordsSetException => ResourceManager.GetString("CrossSectionControl.InvalidSurfaceCoordsSetException", Culture);

    internal static string CurrentSession => ResourceManager.GetString("CurrentSession", Culture);

    internal static string Data => ResourceManager.GetString("Data", Culture);

    internal static string DataBase_Functions_MissingException => ResourceManager.GetString("DataBase.Functions.MissingException", Culture);

    internal static string DataBase_Functions_NoDataException => ResourceManager.GetString("DataBase.Functions.NoDataException", Culture);

    internal static string DataBase_Materials_MissingException => ResourceManager.GetString("DataBase.Materials.MissingException", Culture);

    internal static string DataBase_Materials_NoDataException => ResourceManager.GetString("DataBase.Materials.NoDataException", Culture);

    internal static string DataBaseMainMenuEvents_OpenDB_Message => ResourceManager.GetString("DataBaseMainMenuEvents.OpenDB.Message", Culture);

    internal static string DataBaseMainMenuEvents_OpenDB_SuccessfullyAdded_Message => ResourceManager.GetString("DataBaseMainMenuEvents.OpenDB.SuccessfullyAdded.Message", Culture);

    internal static string DataBaseMainMenuEvents_OpenFuncDB_DBNotLoaded_Message => ResourceManager.GetString("DataBaseMainMenuEvents.OpenFuncDB.DBNotLoaded.Message", Culture);

    internal static string DataBaseMainMenuEvents_OpenMatDB_DBNotLoaded_Message => ResourceManager.GetString("DataBaseMainMenuEvents.OpenMatDB.DBNotLoaded.Message", Culture);

    internal static string DataRemovedSuccessfully => ResourceManager.GetString("DataRemovedSuccessfully", Culture);

    internal static string DelBranchException => ResourceManager.GetString("DelBranchException", Culture);

    internal static string DeletingError => ResourceManager.GetString("DeletingError", Culture);

    internal static string DiagramCalculator => ResourceManager.GetString("DiagramCalculator", Culture);

    internal static string domainUpDown1_Items => ResourceManager.GetString("domainUpDown1.Items", Culture);

    internal static string domainUpDown1_Items1 => ResourceManager.GetString("domainUpDown1.Items1", Culture);

    internal static string domainUpDown1_Items2 => ResourceManager.GetString("domainUpDown1.Items2", Culture);

    internal static string domainUpDown1_Items3 => ResourceManager.GetString("domainUpDown1.Items3", Culture);

    internal static string domainUpDown1_Text => ResourceManager.GetString("domainUpDown1.Text", Culture);

    internal static string domainUpDown1_ToolTip => ResourceManager.GetString("domainUpDown1.ToolTip", Culture);

    internal static string dToolStripMenuItem_Text => ResourceManager.GetString("dToolStripMenuItem.Text", Culture);

    internal static string dToolStripMenuItem1_Text => ResourceManager.GetString("dToolStripMenuItem1.Text", Culture);

    internal static string dToolStripMenuItem2_Text => ResourceManager.GetString("dToolStripMenuItem2.Text", Culture);

    internal static string Edit => ResourceManager.GetString("Edit", Culture);

    internal static string EditGroup_EditGroupAsync_GroupChanged_Message => ResourceManager.GetString("EditGroup.EditGroupAsync.GroupChanged.Message", Culture);

    internal static string EditGroup_EditGroupAsync_NoObjectsSelected_Message => ResourceManager.GetString("EditGroup.EditGroupAsync.NoObjectsSelected.Message", Culture);

    internal static string EditGroup_EditGroupAsync_OperationCanceled_Message => ResourceManager.GetString("EditGroup.EditGroupAsync.OperationCanceled.Message", Culture);

    internal static string EditGroup_EditGroupAsync_Preamble_Message => ResourceManager.GetString("EditGroup.EditGroupAsync.Preamble.Message", Culture);

    internal static string EditReaction => ResourceManager.GetString("EditReaction", Culture);

    internal static string EnteredDataDeletingWarning => ResourceManager.GetString("EnteredDataDeletingWarning", Culture);

    internal static string Error => ResourceManager.GetString("Error", Culture);

    internal static string ErrorCaption => ResourceManager.GetString("ErrorCaption", Culture);

    internal static string Execute => ResourceManager.GetString("Execute", Culture);

    internal static string ExecuteCMDFileMissing => ResourceManager.GetString("ExecuteCMDFileMissing", Culture);

    internal static string FailedCreateGeometry => ResourceManager.GetString("FailedCreateGeometry", Culture);

    internal static string FileAbsenceCaption => ResourceManager.GetString("FileAbsenceCaption", Culture);

    internal static string FindCoincidentNodes_Action_Found_Message => ResourceManager.GetString("FindCoincidentNodes.Action.Found.Message", Culture);

    internal static string FindCoincidentNodes_Action_Matches_Message => ResourceManager.GetString("FindCoincidentNodes.Action.Matches.Message", Culture);

    internal static string FindCoincidentNodes_Action_Message => ResourceManager.GetString("FindCoincidentNodes.Action.Message", Culture);

    internal static string FindCoincidentNodes_Action_OperationCanceled_Message => ResourceManager.GetString("FindCoincidentNodes.Action.OperationCanceled.Message", Culture);

    internal static string FindCoincidentNodes_ActionConfirm_MergeNodes_Message => ResourceManager.GetString("FindCoincidentNodes.ActionConfirm.MergeNodes.Message", Culture);

    internal static string FindCoincidentNodes_AsyncContainer_Message => ResourceManager.GetString("FindCoincidentNodes.AsyncContainer.Message", Culture);

    internal static string FindCoincidentOption => ResourceManager.GetString("FindCoincidentOption", Culture);

    internal static string FindFreeNodesEvent_Found_Message => ResourceManager.GetString("FindFreeNodesEvent.Found.Message", Culture);

    internal static string FindFreeNodesEvent_FreeNodes_Message => ResourceManager.GetString("FindFreeNodesEvent.FreeNodes.Message", Culture);

    internal static string FindVolElemsEventArgsArgExc => ResourceManager.GetString("FindVolElemsEventArgsArgExc", Culture);

    internal static string Found => ResourceManager.GetString("Found", Culture);

    internal static string Function => ResourceManager.GetString("Function", Culture);

    internal static string FunctionDataBasePage_headerName_text => ResourceManager.GetString("FunctionDataBasePage_headerName_text", Culture);

    internal static string GenBeamConnection => ResourceManager.GetString("GenBeamConnection", Culture);

    internal static string GenCreateCurve => ResourceManager.GetString("GenCreateCurve", Culture);

    internal static string GenCreateMesh2DPoligon => ResourceManager.GetString("GenCreateMesh2DPoligon", Culture);

    internal static string GenCreatePoint => ResourceManager.GetString("GenCreatePoint", Culture);

    internal static string GenCreateSurface => ResourceManager.GetString("GenCreateSurface", Culture);

    internal static string GenerateBoundaryMesh_CreateBoundaryMeh_CheckRecommendation_Message => ResourceManager.GetString("GenerateBoundaryMesh.CreateBoundaryMeh.CheckRecommendation.Message", Culture);

    internal static string GenerateBoundaryMesh_CreateBoundaryMeh_ObjectsGenerated_Message => ResourceManager.GetString("GenerateBoundaryMesh.CreateBoundaryMeh.ObjectsGenerated.Message", Culture);

    internal static string GenerateBoundaryMesh_Generate2DOn3DSurfaces_Elements_Message => ResourceManager.GetString("GenerateBoundaryMesh.Generate2DOn3DSurfaces.Elements.Message", Culture);

    internal static string GenerateBoundaryMesh_Generate2DOn3DSurfaces_NoObjects_Message => ResourceManager.GetString("GenerateBoundaryMesh.Generate2DOn3DSurfaces.NoObjects.Message", Culture);

    internal static string GenerateMesh2DEvent_GenerateOnGeometry_GenElements_Message => ResourceManager.GetString("GenerateMesh2DEvent.GenerateOnGeometry.GenElements.Message", Culture);

    internal static string GenerateMesh2DEvent_GenerateOnGeometry_Recomendation_Message => ResourceManager.GetString("GenerateMesh2DEvent.GenerateOnGeometry.Recomendation.Message", Culture);

    internal static string GenerateMesh3DEvents_Generate3D_CheckRecommendation_Message => ResourceManager.GetString("GenerateMesh3DEvents.Generate3D.CheckRecommendation.Message", Culture);

    internal static string GenerateMesh3DEvents_Generate3D_GeneratedElements_Message => ResourceManager.GetString("GenerateMesh3DEvents.Generate3D.GeneratedElements.Message", Culture);

    internal static string GenerateMesh3DEvents_Generate3D_GMSHNull_Exception => ResourceManager.GetString("GenerateMesh3DEvents.Generate3D.GMSHNull.Exception", Culture);

    internal static string GenerateTSF_InstructionsFormed_Message => ResourceManager.GetString("GenerateTSF.InstructionsFormed.Message", Culture);

    internal static string GenExit => ResourceManager.GetString("GenExit", Culture);

    internal static string GenExtrudeByRotation => ResourceManager.GetString("GenExtrudeByRotation", Culture);

    internal static string GenExtrudeCurve => ResourceManager.GetString("GenExtrudeCurve", Culture);

    internal static string GenFindCoincident => ResourceManager.GetString("GenFindCoincident", Culture);

    internal static string GenFindFreeNodes => ResourceManager.GetString("GenFindFreeNodes", Culture);

    internal static string GenFindObject => ResourceManager.GetString("GenFindObject", Culture);

    internal static string GenFindVolElems => ResourceManager.GetString("GenFindVolElems", Culture);

    internal static string GenLoadProject => ResourceManager.GetString("GenLoadProject", Culture);

    internal static string GenMergeElementSets => ResourceManager.GetString("GenMergeElementSets", Culture);

    internal static string GenMoveMesh => ResourceManager.GetString("GenMoveMesh", Culture);

    internal static string GenMoveNodes => ResourceManager.GetString("GenMoveNodes", Culture);

    internal static string GenRenumberMesh => ResourceManager.GetString("GenRenumberMesh", Culture);

    internal static string GenRotateMesh => ResourceManager.GetString("GenRotateMesh", Culture);

    internal static string GenSaveProject => ResourceManager.GetString("GenSaveProject", Culture);

    internal static string GenSetLevel => ResourceManager.GetString("GenSetLevel", Culture);

    internal static string GenSolveProject => ResourceManager.GetString("GenSolveProject", Culture);

    internal static string Geo_Curve_Header_Algorithm => ResourceManager.GetString("Geo.Curve.Header.Algorithm", Culture);

    internal static string Geo_Curve_Header_Coefficient => ResourceManager.GetString("Geo.Curve.Header.Coefficient", Culture);

    internal static string Geo_Curve_Header_PointQuantity => ResourceManager.GetString("Geo.Curve.Header.PointQuantity", Culture);

    internal static string Geo_General_Header_Algorithm2D => ResourceManager.GetString("Geo.General.Header.Algorithm2D", Culture);

    internal static string Geo_General_Header_Algorithm3D => ResourceManager.GetString("Geo.General.Header.Algorithm3D", Culture);

    internal static string Geo_General_Header_MaxSize => ResourceManager.GetString("Geo.General.Header.MaxSize", Culture);

    internal static string Geo_General_Header_MeshSizeFactor => ResourceManager.GetString("Geo.General.Header.MeshSizeFactor", Culture);

    internal static string Geo_General_Header_MinSize => ResourceManager.GetString("Geo.General.Header.MinSize", Culture);

    internal static string Geo_General_Header_ShowPointsOnCurves => ResourceManager.GetString("Geo.General.Header.ShowPointsOnCurves", Culture);

    internal static string Geo_General_ShowAllMeshOnGeneration => ResourceManager.GetString("Geo.General.ShowAllMeshOnGeneration", Culture);

    internal static string Geo_Point_Header_ElementsSize => ResourceManager.GetString("Geo.Point.Header.ElementsSize", Culture);

    internal static string Geo_Surface_Header_AddedCurves => ResourceManager.GetString("Geo.Surface.Header.AddedCurves", Culture);

    internal static string Geo_Surface_Header_CornerPoints => ResourceManager.GetString("Geo.Surface.Header.CornerPoints", Culture);

    internal static string Geo_Surface_Header_MeshType => ResourceManager.GetString("Geo.Surface.Header.MeshType", Culture);

    internal static string Geo_Surface_Header_MeshType_Regular => ResourceManager.GetString("Geo.Surface.Header.MeshType.Regular", Culture);

    internal static string Geo_Surface_Header_RibbersOrientation => ResourceManager.GetString("Geo.Surface.Header.RibbersOrientation", Culture);

    internal static string Geo_Surface_Header_Squaring => ResourceManager.GetString("Geo.Surface.Header.Squaring", Culture);

    internal static string Geo_Volume_Header_LayerThickness => ResourceManager.GetString("Geo.Volume.Header.LayerThickness", Culture);

    internal static string Geo_Volume_Header_MeshType => ResourceManager.GetString("Geo.Volume.Header.MeshType", Culture);

    internal static string Geo_Volume_Header_MeshType_Gradient => ResourceManager.GetString("Geo.Volume.Header.MeshType.Gradient", Culture);

    internal static string Geo_Volume_Header_MeshType_Regulare => ResourceManager.GetString("Geo.Volume.Header.MeshType.Regulare", Culture);

    internal static string Geo_Volume_Header_SizeOfElementsInCenter => ResourceManager.GetString("Geo.Volume.Header.SizeOfElementsInCenter", Culture);

    internal static string Geo_Volume_Header_SurfaceElementsSize => ResourceManager.GetString("Geo.Volume.Header.SurfaceElementsSize", Culture);

    internal static string Geo_Volume_Header_TransitionGradientDegree => ResourceManager.GetString("Geo.Volume.Header.TransitionGradientDegree", Culture);

    internal static string GetDAtaBase_FindFileByPath_DBNotFound_Message_Part1 => ResourceManager.GetString("GetDAtaBase.FindFileByPath.DBNotFound.Message_Part1", Culture);

    internal static string GetDAtaBase_FindFileByPath_DBNotFound_Message_Part2 => ResourceManager.GetString("GetDAtaBase.FindFileByPath.DBNotFound.Message_Part2", Culture);

    internal static string GetLicenseInfoException => ResourceManager.GetString("GetLicenseInfoException", Culture);

    internal static string Graph => ResourceManager.GetString("Graph", Culture);

    internal static string Groups_Header_CreateCondition => ResourceManager.GetString("Groups.Header.CreateCondition", Culture);

    internal static string Groups_Header_Name => ResourceManager.GetString("Groups.Header.Name", Culture);

    internal static string HandleArgsCADAbsenceException => ResourceManager.GetString("HandleArgsCADAbsenceException", Culture);

    internal static string HandleArgsProjectAbsenceException => ResourceManager.GetString("HandleArgsProjectAbsenceException", Culture);

    internal static string HandleArgsResultsAbsenceException => ResourceManager.GetString("HandleArgsResultsAbsenceException", Culture);

    internal static string HandleArgsResultsLoadingWithoutProjectException => ResourceManager.GetString("HandleArgsResultsLoadingWithoutProjectException", Culture);

    internal static string HandleBaseMaster_Handle_CondCreation_Message => ResourceManager.GetString("HandleBaseMaster.Handle.CondCreation.Message", Culture);

    internal static string HandleBaseMaster_Success_Message => ResourceManager.GetString("HandleBaseMaster.Success.Message", Culture);

    internal static string HardeningCalculator => ResourceManager.GetString("HardeningCalculator", Culture);

    internal static string Header_chemical_InitialConcentration => ResourceManager.GetString("Header_chemical_InitialConcentration", Culture);

    internal static string Header_chemical_MaxConcentration => ResourceManager.GetString("Header_chemical_MaxConcentration", Culture);

    internal static string Header_chemical_MaxConcentrationValue => ResourceManager.GetString("Header_chemical_MaxConcentrationValue", Culture);

    internal static string Header_clamp_type => ResourceManager.GetString("Header_clamp_type", Culture);

    internal static string Header_Color => ResourceManager.GetString("Header.Color", Culture);

    internal static string Header_comp_Algorithm => ResourceManager.GetString("Header_comp_Algorithm", Culture);

    internal static string Header_comp_ApplyForAll => ResourceManager.GetString("Header_comp_ApplyForAll", Culture);

    internal static string Header_comp_Execute => ResourceManager.GetString("Header_comp_Execute", Culture);

    internal static string Header_comp_FieldConcentration => ResourceManager.GetString("Header_comp_FieldConcentration", Culture);

    internal static string Header_comp_FieldDisplacement => ResourceManager.GetString("Header_comp_FieldDisplacement", Culture);

    internal static string Header_comp_FieldPhaseComposition => ResourceManager.GetString("Header_comp_FieldPhaseComposition", Culture);

    internal static string Header_comp_FieldPressure => ResourceManager.GetString("Header_comp_FieldPressure", Culture);

    internal static string Header_comp_FieldStrain => ResourceManager.GetString("Header_comp_FieldStrain", Culture);

    internal static string Header_comp_FieldStress => ResourceManager.GetString("Header_comp_FieldStress", Culture);

    internal static string Header_comp_FieldTemperature => ResourceManager.GetString("Header_comp_FieldTemperature", Culture);

    internal static string Header_comp_FieldVelocity => ResourceManager.GetString("Header_comp_FieldVelocity", Culture);

    internal static string Header_comp_FileName => ResourceManager.GetString("Header_comp_FileName", Culture);

    internal static string Header_comp_ForeignSets => ResourceManager.GetString("Header_comp_ForeignSets", Culture);

    internal static string Header_comp_InitialSolveStep => ResourceManager.GetString("Header_comp_InitialSolveStep", Culture);

    internal static string Header_comp_InitialStateSource => ResourceManager.GetString("Header_comp_InitialStateSource", Culture);

    internal static string Header_comp_InitTemp => ResourceManager.GetString("Header_comp_InitTemp", Culture);

    internal static string Header_comp_InputSource => ResourceManager.GetString("Header_comp_InputSource", Culture);

    internal static string Header_comp_IterationsOnStep => ResourceManager.GetString("Header_comp_IterationsOnStep", Culture);

    internal static string Header_comp_MatrixStorage => ResourceManager.GetString("Header_comp_MatrixStorage", Culture);

    internal static string Header_comp_MaxRelaxationCoef => ResourceManager.GetString("Header_comp_MaxRelaxationCoef", Culture);

    internal static string Header_comp_MaxSolveStep => ResourceManager.GetString("Header_comp_MaxSolveStep", Culture);

    internal static string Header_comp_MinSolveStep => ResourceManager.GetString("Header_comp_MinSolveStep", Culture);

    internal static string Header_comp_NativeSet => ResourceManager.GetString("Header_comp_NativeSet", Culture);

    internal static string Header_comp_Priority => ResourceManager.GetString("Header_comp_Priority", Culture);

    internal static string Header_comp_RelaxationCoef => ResourceManager.GetString("Header_comp_RelaxationCoef", Culture);

    internal static string Header_comp_SaveRate => ResourceManager.GetString("Header_comp_SaveRate", Culture);

    internal static string Header_comp_SetChemical => ResourceManager.GetString("Header_comp_SetChemical", Culture);

    internal static string Header_comp_SetHydrodynamic => ResourceManager.GetString("Header_comp_SetHydrodynamic", Culture);

    internal static string Header_comp_SetMechanical => ResourceManager.GetString("Header_comp_SetMechanical", Culture);

    internal static string Header_comp_SetThermal => ResourceManager.GetString("Header_comp_SetThermal", Culture);

    internal static string Header_comp_SolveAccuracy => ResourceManager.GetString("Header_comp_SolveAccuracy", Culture);

    internal static string Header_comp_SolveIterations => ResourceManager.GetString("Header_comp_SolveIterations", Culture);

    internal static string Header_comp_SourceConstant => ResourceManager.GetString("Header_comp_SourceConstant", Culture);

    internal static string Header_comp_SourceFile => ResourceManager.GetString("Header_comp_SourceFile", Culture);

    internal static string Header_comp_StartTime => ResourceManager.GetString("Header_comp_StartTime", Culture);

    internal static string Header_comp_StopTime => ResourceManager.GetString("Header_comp_StopTime", Culture);

    internal static string Header_comp_Type => ResourceManager.GetString("Header_comp_Type", Culture);

    internal static string Header_comp_Value => ResourceManager.GetString("Header_comp_Value", Culture);

    internal static string Header_cond_coordinateSystem => ResourceManager.GetString("Header_cond_coordinateSystem", Culture);

    internal static string Header_cond_direction => ResourceManager.GetString("Header_cond_direction", Culture);

    internal static string Header_cond_function => ResourceManager.GetString("Header_cond_function", Culture);

    internal static string Header_cond_material_diametr => ResourceManager.GetString("Header_cond_material_diametr", Culture);

    internal static string Header_cond_material_material => ResourceManager.GetString("Header_cond_material_material", Culture);

    internal static string Header_cond_material_thickness => ResourceManager.GetString("Header_cond_material_thickness", Culture);

    internal static string Header_cond_media_condType => ResourceManager.GetString("Header_cond_media_condType", Culture);

    internal static string Header_cond_objectsGroup => ResourceManager.GetString("Header_cond_objectsGroup", Culture);

    internal static string Header_cond_parameter_placeHolder => ResourceManager.GetString("Header_cond_parameter_placeHolder", Culture);

    internal static string Header_cond_parameterValue_placeHolder => ResourceManager.GetString("Header_cond_parameterValue_placeHolder", Culture);

    internal static string Header_cond_plane => ResourceManager.GetString("Header_cond_plane", Culture);

    internal static string Header_cond_rotX => ResourceManager.GetString("Header_cond_rotX", Culture);

    internal static string Header_cond_rotY => ResourceManager.GetString("Header_cond_rotY", Culture);

    internal static string Header_cond_rotZ => ResourceManager.GetString("Header_cond_rotZ", Culture);

    internal static string Header_cond_shiftingX => ResourceManager.GetString("Header_cond_shiftingX", Culture);

    internal static string Header_cond_shiftingY => ResourceManager.GetString("Header_cond_shiftingY", Culture);

    internal static string Header_cond_ShiftingZ => ResourceManager.GetString("Header_cond_ShiftingZ", Culture);

    internal static string Header_cond_start => ResourceManager.GetString("Header_cond_start", Culture);

    internal static string Header_cond_stop => ResourceManager.GetString("Header_cond_stop", Culture);

    internal static string Header_cond_table_placeHolder => ResourceManager.GetString("Header_cond_table_placeHolder", Culture);

    internal static string Header_cond_value => ResourceManager.GetString("Header_cond_value", Culture);

    internal static string Header_curve_algorithm => ResourceManager.GetString("Header_curve_algorithm", Culture);

    internal static string Header_curve_coefficient => ResourceManager.GetString("Header_curve_coefficient", Culture);

    internal static string Header_curve_number => ResourceManager.GetString("Header_curve_number", Culture);

    internal static string Header_curve_PointsNumber => ResourceManager.GetString("Header_curve_PointsNumber", Culture);

    internal static string Header_element_elementLevel => ResourceManager.GetString("Header_element_elementLevel", Culture);

    internal static string Header_element_includedNodes => ResourceManager.GetString("Header_element_includedNodes", Culture);

    internal static string Header_element_number => ResourceManager.GetString("Header_element_number", Culture);

    internal static string Header_File => ResourceManager.GetString("Header.File", Culture);

    internal static string Header_frameFunction_CFF_file => ResourceManager.GetString("Header_frameFunction_CFF_file", Culture);

    internal static string Header_frameFunction_CIL_bottomDiam => ResourceManager.GetString("Header_frameFunction_CIL_bottomDiam", Culture);

    internal static string Header_frameFunction_CIL_length => ResourceManager.GetString("Header_frameFunction_CIL_length", Culture);

    internal static string Header_frameFunction_CIL_upperDiam => ResourceManager.GetString("Header_frameFunction_CIL_upperDiam", Culture);

    internal static string Header_frameFunction_SPH_width => ResourceManager.GetString("Header_frameFunction_SPH_width", Culture);

    internal static string Header_geo_algorithm2D => ResourceManager.GetString("Header_geo_algorithm2D", Culture);

    internal static string Header_geo_algorithm3D => ResourceManager.GetString("Header_geo_algorithm3D", Culture);

    internal static string Header_geo_maxSize => ResourceManager.GetString("Header_geo_maxSize", Culture);

    internal static string Header_geo_minSize => ResourceManager.GetString("Header_geo_minSize", Culture);

    internal static string Header_geo_scaleCoef => ResourceManager.GetString("Header_geo_scaleCoef", Culture);

    internal static string Header_geo_showMeshOnGeneration => ResourceManager.GetString("Header_geo_showMeshOnGeneration", Culture);

    internal static string Header_geo_showPointsNumbers => ResourceManager.GetString("Header_geo_showPointsNumbers", Culture);

    internal static string Header_geo_showPointsNumbersOnCurves => ResourceManager.GetString("Header_geo_showPointsNumbersOnCurves", Culture);

    internal static string Header_geo_showPointsOnCurves => ResourceManager.GetString("Header_geo_showPointsOnCurves", Culture);

    internal static string Header_geo_showSurfacesNumbers => ResourceManager.GetString("Header_geo_showSurfacesNumbers", Culture);

    internal static string Header_geo_showVolumesNumbers => ResourceManager.GetString("Header_geo_showVolumesNumbers", Culture);

    internal static string Header_group_show => ResourceManager.GetString("Header_group_show", Culture);

    internal static string Header_groups_createCond => ResourceManager.GetString("Header_groups_createCond", Culture);

    internal static string Header_groups_direction => ResourceManager.GetString("Header_groups_direction", Culture);

    internal static string Header_groups_elementsNodes => ResourceManager.GetString("Header_groups_elementsNodes", Culture);

    internal static string Header_groups_name => ResourceManager.GetString("Header_groups_name", Culture);

    internal static string Header_groups_sort => ResourceManager.GetString("Header_groups_sort", Culture);

    internal static string Header_load_type => ResourceManager.GetString("Header_load_type", Culture);

    internal static string Header_localFrame_referenceLine => ResourceManager.GetString("Header_localFrame_referenceLine", Culture);

    internal static string Header_localFrame_speed => ResourceManager.GetString("Header_localFrame_speed", Culture);

    internal static string Header_localFrame_trajectory => ResourceManager.GetString("Header_localFrame_trajectory", Culture);

    internal static string Header_mechanicalTask_maxDiference => ResourceManager.GetString("Header_mechanicalTask_maxDiference", Culture);

    internal static string Header_mechanicalTask_maxMove => ResourceManager.GetString("Header_mechanicalTask_maxMove", Culture);

    internal static string Header_mechanicalTask_maxMoveValue => ResourceManager.GetString("Header_mechanicalTask_maxMoveValue", Culture);

    internal static string Header_mechanicalTask_maxPlasticDeformation => ResourceManager.GetString("Header_mechanicalTask_maxPlasticDeformation", Culture);

    internal static string Header_mechanicalTask_plasticDeformationValue => ResourceManager.GetString("Header_mechanicalTask_plasticDeformationValue", Culture);

    internal static string Header_node_cordX => ResourceManager.GetString("Header_node_cordX", Culture);

    internal static string Header_node_cordY => ResourceManager.GetString("Header_node_cordY", Culture);

    internal static string Header_node_cordZ => ResourceManager.GetString("Header_node_cordZ", Culture);

    internal static string Header_node_linkedElements => ResourceManager.GetString("Header_node_linkedElements", Culture);

    internal static string Header_node_number => ResourceManager.GetString("Header_node_number", Culture);

    internal static string Header_object_object => ResourceManager.GetString("Header_object_object", Culture);

    internal static string Header_point_elementsSize => ResourceManager.GetString("Header_point_elementsSize", Culture);

    internal static string Header_point_number => ResourceManager.GetString("Header_point_number", Culture);

    internal static string Header_projectMesh_delete => ResourceManager.GetString("Header_projectMesh_delete", Culture);

    internal static string Header_projectMesh_elements1D => ResourceManager.GetString("Header_projectMesh_elements1D", Culture);

    internal static string Header_projectMesh_elements2D => ResourceManager.GetString("Header_projectMesh_elements2D", Culture);

    internal static string Header_projectMesh_elements3D => ResourceManager.GetString("Header_projectMesh_elements3D", Culture);

    internal static string Header_projectMesh_hide => ResourceManager.GetString("Header_projectMesh_hide", Culture);

    internal static string Header_projectMesh_nodes => ResourceManager.GetString("Header_projectMesh_nodes", Culture);

    internal static string Header_projectMesh_show => ResourceManager.GetString("Header_projectMesh_show", Culture);

    internal static string Header_result_averageValues => ResourceManager.GetString("Header_result_averageValues", Culture);

    internal static string Header_result_clarifyValues => ResourceManager.GetString("Header_result_clarifyValues", Culture);

    internal static string Header_result_intervals => ResourceManager.GetString("Header_result_intervals", Culture);

    internal static string Header_result_maxScaleValue => ResourceManager.GetString("Header_result_maxScaleValue", Culture);

    internal static string Header_result_minScaleValue => ResourceManager.GetString("Header_result_minScaleValue", Culture);

    internal static string Header_result_precision => ResourceManager.GetString("Header_result_precision", Culture);

    internal static string Header_result_result => ResourceManager.GetString("Header_result_result", Culture);

    internal static string Header_result_resultScale => ResourceManager.GetString("Header_result_resultScale", Culture);

    internal static string Header_result_showElementsValues => ResourceManager.GetString("Header_result_showElementsValues", Culture);

    internal static string Header_result_showFields => ResourceManager.GetString("Header_result_showFields", Culture);

    internal static string Header_result_showNodesValues => ResourceManager.GetString("Header_result_showNodesValues", Culture);

    internal static string Header_result_showScale => ResourceManager.GetString("Header_result_showScale", Culture);

    internal static string Header_result_xPos => ResourceManager.GetString("Header_result_xPos", Culture);

    internal static string Header_result_yPos => ResourceManager.GetString("Header_result_yPos", Culture);

    internal static string Header_set_adjacentNodes => ResourceManager.GetString("Header_set_adjacentNodes", Culture);

    internal static string Header_set_color => ResourceManager.GetString("Header_set_color", Culture);

    internal static string Header_set_create => ResourceManager.GetString("Header_set_create", Culture);

    internal static string Header_set_group => ResourceManager.GetString("Header_set_group", Culture);

    internal static string Header_set_name => ResourceManager.GetString("Header_set_name", Culture);

    internal static string Header_set_precisionOrder => ResourceManager.GetString("Header_set_precisionOrder", Culture);

    internal static string Header_set_show => ResourceManager.GetString("Header_set_show", Culture);

    internal static string Header_set_view => ResourceManager.GetString("Header_set_view", Culture);

    internal static string Header_surface_addedCurves => ResourceManager.GetString("Header_surface_addedCurves", Culture);

    internal static string Header_surface_cornerPoints => ResourceManager.GetString("Header_surface_cornerPoints", Culture);

    internal static string Header_surface_meshKind => ResourceManager.GetString("Header_surface_meshKind", Culture);

    internal static string Header_surface_meshType => ResourceManager.GetString("Header_surface_meshType", Culture);

    internal static string Header_surface_number => ResourceManager.GetString("Header_surface_number", Culture);

    internal static string Header_surface_pointsNumbers => ResourceManager.GetString("Header_surface_pointsNumbers", Culture);

    internal static string Header_surface_quadratization => ResourceManager.GetString("Header_surface_quadratization", Culture);

    internal static string Header_surface_ribersOrientation => ResourceManager.GetString("Header_surface_ribersOrientation", Culture);

    internal static string Header_task_checkCondValues => ResourceManager.GetString("Header_task_checkCondValues", Culture);

    internal static string Header_task_functions => ResourceManager.GetString("Header_task_functions", Culture);

    internal static string Header_task_materials => ResourceManager.GetString("Header_task_materials", Culture);

    internal static string Header_task_type => ResourceManager.GetString("Header_task_type", Culture);

    internal static string Header_termalTask_maxTemperture => ResourceManager.GetString("Header_termalTask_maxTemperture", Culture);

    internal static string Header_termalTask_maxTempertureValue => ResourceManager.GetString("Header_termalTask_maxTempertureValue", Culture);

    internal static string Header_volume_centerElementsSize => ResourceManager.GetString("Header_volume_centerElementsSize", Culture);

    internal static string Header_volume_layerThickness => ResourceManager.GetString("Header_volume_layerThickness", Culture);

    internal static string Header_volume_meshType => ResourceManager.GetString("Header_volume_meshType", Culture);

    internal static string Header_volume_number => ResourceManager.GetString("Header_volume_number", Culture);

    internal static string Header_volume_surfaceElementsSize => ResourceManager.GetString("Header_volume_surfaceElementsSize", Culture);

    internal static string Header_volume_TransitionGradientDegree => ResourceManager.GetString("Header_volume_TransitionGradientDegree", Culture);

    internal static string Header_volume_volume => ResourceManager.GetString("Header_volume_volume", Culture);

    internal static string HeaderName => ResourceManager.GetString("HeaderName", Culture);

    internal static string Headers_task_kind => ResourceManager.GetString("Headers_task_kind", Culture);

    internal static string ImportMaster_CreateImportedMasters_MasterNotFoundOrAlreadyLoaded_Message => ResourceManager.GetString("ImportMaster.CreateImportedMasters.MasterNotFoundOrAlreadyLoaded.Message", Culture);

    internal static string ImportMaster_CreateImportedMasters_MasterOpened_Message => ResourceManager.GetString("ImportMaster.CreateImportedMasters.MasterOpened.Message", Culture);

    internal static string ImportMeshCaption => ResourceManager.GetString("ImportMeshCaption", Culture);

    internal static string InvalidArgumentsNumberException => ResourceManager.GetString("InvalidArgumentsNumberException", Culture);

    internal static string InvalidCommandException => ResourceManager.GetString("InvalidCommandException", Culture);

    internal static string InvalidCoordinatesException => ResourceManager.GetString("InvalidCoordinatesException", Culture);

    internal static string InvalidRegexPhaseMatchWarning => ResourceManager.GetString("InvalidRegexPhaseMatchWarning", Culture);

    internal static string label1_Text => ResourceManager.GetString("label1.Text", Culture);

    internal static string label1_ToolTip => ResourceManager.GetString("label1.ToolTip", Culture);

    internal static string label2_Text => ResourceManager.GetString("label2.Text", Culture);

    internal static string label2_ToolTip => ResourceManager.GetString("label2.ToolTip", Culture);

    internal static string label3_Text => ResourceManager.GetString("label3.Text", Culture);

    internal static string label3_ToolTip => ResourceManager.GetString("label3.ToolTip", Culture);

    internal static string label4_Text => ResourceManager.GetString("label4.Text", Culture);

    internal static string label4_ToolTip => ResourceManager.GetString("label4.ToolTip", Culture);

    internal static string label5_Text => ResourceManager.GetString("label5.Text", Culture);

    internal static string label5_ToolTip => ResourceManager.GetString("label5.ToolTip", Culture);

    internal static string label6_Text => ResourceManager.GetString("label6.Text", Culture);

    internal static string label7_Text => ResourceManager.GetString("label7.Text", Culture);

    internal static string label8_ToolTip => ResourceManager.GetString("label8.ToolTip", Culture);

    internal static string lblAngle_Text => ResourceManager.GetString("lblAngle.Text", Culture);

    internal static string LicenseAllowedAnswer => ResourceManager.GetString("LicenseAllowedAnswer", Culture);

    internal static string LicenseInfo => ResourceManager.GetString("LicenseInfo", Culture);

    internal static string Licensing => ResourceManager.GetString("Licensing", Culture);

    internal static string List => ResourceManager.GetString("List", Culture);

    internal static string LoadDBAddIntoMissingDBException => ResourceManager.GetString("LoadDBAddIntoMissingDBException", Culture);

    internal static string LoadDBCorruptedException => ResourceManager.GetString("LoadDBCorruptedException", Culture);

    internal static string LoadingForm_Text => ResourceManager.GetString("LoadingForm.Text", Culture);

    internal static string LoadPythonFile_ВыберитеPythonФайл => ResourceManager.GetString("LoadPythonFile_ВыберитеPythonФайл", Culture);

    internal static string MainMenuEvents_ChangeLanguage_Message => ResourceManager.GetString("MainMenuEvents.ChangeLanguage.Message", Culture);

    internal static string Results_InvalidDatabase_Message => ResourceManager.GetString("Results.InvalidDatabase.Message", Culture);

    internal static string MakeScreenShot_Message => ResourceManager.GetString("MakeScreenShot.Message", Culture);

    internal static string MakeScreenShot_ScreenShotTaken_Message => ResourceManager.GetString("MakeScreenShot.ScreenShotTaken.Message", Culture);

    internal static string Material => ResourceManager.GetString("Material", Culture);

    internal static string MaterialsDataBasePage_headerName_text => ResourceManager.GetString("MaterialsDataBasePage_headerName_text", Culture);

    internal static string menuStrip_Text => ResourceManager.GetString("menuStrip.Text", Culture);

    internal static string MeshObj_General_Header_CoordinateX => ResourceManager.GetString("MeshObj.General.Header.CoordinateX", Culture);

    internal static string MeshObj_General_Header_CoordinateY => ResourceManager.GetString("MeshObj.General.Header.CoordinateY", Culture);

    internal static string MeshObj_General_Header_CoordinateZ => ResourceManager.GetString("MeshObj.General.Header.CoordinateZ", Culture);

    internal static string MeshSet_General_Header_AccuracyLevel => ResourceManager.GetString("MeshSet.General.Header.AccuracyLevel", Culture);

    internal static string MeshSet_General_Header_Color => ResourceManager.GetString("MeshSet.General.Header.Color", Culture);

    internal static string MeshSet_General_Header_Name => ResourceManager.GetString("MeshSet.General.Header.Name", Culture);

    internal static string MeshSet_General_Header_View => ResourceManager.GetString("MeshSet.General.Header.View", Culture);

    internal static string message_Text => ResourceManager.GetString("message.Text", Culture);

    internal static string MethodIsNotImplementedException => ResourceManager.GetString("MethodIsNotImplementedException", Culture);

    internal static string ModelShiftCoordinateEventArgsVectorExc => ResourceManager.GetString("ModelShiftCoordinateEventArgsVectorExc", Culture);

    internal static string MoveRotNodesOption => ResourceManager.GetString("MoveRotNodesOption", Culture);

    internal static string Navigator_TreeView_Node_Text_Calculation => ResourceManager.GetString("Navigator.TreeView.Node.Text.Calculation", Culture);

    internal static string Navigator_TreeView_Node_Text_Calculations => ResourceManager.GetString("Navigator.TreeView.Node.Text.Calculations", Culture);

    internal static string Navigator_TreeView_Node_Text_Clamp => ResourceManager.GetString("Navigator.TreeView.Node.Text.Clamp", Culture);

    internal static string Navigator_TreeView_Node_Text_ElementsGroup => ResourceManager.GetString("Navigator.TreeView.Node.Text.ElementsGroup", Culture);

    internal static string Navigator_TreeView_Node_Text_Geometry => ResourceManager.GetString("Navigator.TreeView.Node.Text.Geometry", Culture);

    internal static string Navigator_TreeView_Node_Text_Groups => ResourceManager.GetString("Navigator.TreeView.Node.Text.Groups", Culture);

    internal static string Navigator_TreeView_Node_Text_Heat => ResourceManager.GetString("Navigator.TreeView.Node.Text.Heat", Culture);

    internal static string Navigator_TreeView_Node_Text_Load => ResourceManager.GetString("Navigator.TreeView.Node.Text.Load", Culture);

    internal static string Navigator_TreeView_Node_Text_Material => ResourceManager.GetString("Navigator.TreeView.Node.Text.Material", Culture);

    internal static string Navigator_TreeView_Node_Text_Media => ResourceManager.GetString("Navigator.TreeView.Node.Text.Media", Culture);

    internal static string Navigator_TreeView_Node_Text_Mesh => ResourceManager.GetString("Navigator.TreeView.Node.Text.Mesh", Culture);

    internal static string Navigator_TreeView_Node_Text_NodesGroup => ResourceManager.GetString("Navigator.TreeView.Node.Text.NodesGroup", Culture);

    internal static string Navigator_TreeView_Node_Text_Objects => ResourceManager.GetString("Navigator.TreeView.Node.Text.Objects", Culture);

    internal static string Navigator_TreeView_Node_Text_Project => ResourceManager.GetString("Navigator.TreeView.Node.Text.Project", Culture);

    internal static string Navigator_TreeView_Node_Text_Result => ResourceManager.GetString("Navigator.TreeView.Node.Text.Result", Culture);

    internal static string Navigator_TreeView_Node_Text_Results => ResourceManager.GetString("Navigator.TreeView.Node.Text.Results", Culture);

    internal static string Navigator_TreeView_Node_Text_Sets => ResourceManager.GetString("Navigator.TreeView.Node.Text.Sets", Culture);

    internal static string Navigator_TreeView_Node_Text_Task => ResourceManager.GetString("Navigator.TreeView.Node.Text.Task", Culture);

    internal static string Navigator_TreeView_Node_Text_Time => ResourceManager.GetString("Navigator.TreeView.Node.Text.Time", Culture);

    internal static string NavigatorControl_headerName_text => ResourceManager.GetString("NavigatorControl_headerName_text", Culture);

    internal static string New_function_ => ResourceManager.GetString("New_function_", Culture);

    internal static string New_material_ => ResourceManager.GetString("New_material_", Culture);

    internal static string NewProjectNameTemplate => ResourceManager.GetString("NewProjectNameTemplate", Culture);

    internal static string NotACommandException => ResourceManager.GetString("NotACommandException", Culture);

    internal static string OK => ResourceManager.GetString("OK", Culture);

    internal static string OpenTSF_OpenInstructions_Message => ResourceManager.GetString("OpenTSF.OpenInstructions.Message", Culture);

    internal static string PanelConverter_UndefinedConverterTypeException => ResourceManager.GetString("PanelConverter.UndefinedConverterTypeException", Culture);

    internal static string PositiveCellingNumberException => ResourceManager.GetString("PositiveCellingNumberException", Culture);

    internal static string ProjectSavedCaption => ResourceManager.GetString("ProjectSavedCaption", Culture);

    internal static string PropertiesPanelControl_headerName_text => ResourceManager.GetString("PropertiesPanelControl_headerName_text", Culture);

    internal static string PropertyTableIsMissing => ResourceManager.GetString("PropertyTableIsMissing", Culture);

    internal static string radioButton1_Text => ResourceManager.GetString("radioButton1.Text", Culture);

    internal static string radioButton1_ToolTip => ResourceManager.GetString("radioButton1.ToolTip", Culture);

    internal static string radioButton2_Text => ResourceManager.GetString("radioButton2.Text", Culture);

    internal static string radioButton2_ToolTip => ResourceManager.GetString("radioButton2.ToolTip", Culture);

    internal static string radioButton3_Text => ResourceManager.GetString("radioButton3.Text", Culture);

    internal static string radioButton3_ToolTip => ResourceManager.GetString("radioButton3.ToolTip", Culture);

    internal static string radioButton4_Text => ResourceManager.GetString("radioButton4.Text", Culture);

    internal static string radioButton4_ToolTip => ResourceManager.GetString("radioButton4.ToolTip", Culture);

    internal static string radioButton5_Text => ResourceManager.GetString("radioButton5.Text", Culture);

    internal static string radioButton5_ToolTip => ResourceManager.GetString("radioButton5.ToolTip", Culture);

    internal static string radioButton6_Text => ResourceManager.GetString("radioButton6.Text", Culture);

    internal static string radioButton6_ToolTip => ResourceManager.GetString("radioButton6.ToolTip", Culture);

    internal static string rbtCCT_Text => ResourceManager.GetString("rbtCCT.Text", Culture);

    internal static string rbtCurve_AccessibleName => ResourceManager.GetString("rbtCurve.AccessibleName", Culture);

    internal static string rbtCurve_Text => ResourceManager.GetString("rbtCurve.Text", Culture);

    internal static string rbtDirection_Text => ResourceManager.GetString("rbtDirection.Text", Culture);

    internal static string rbtnDistance_Text => ResourceManager.GetString("rbtnDistance.Text", Culture);

    internal static string rbtnPath_Text => ResourceManager.GetString("rbtnPath.Text", Culture);

    internal static string rbtSet_Text => ResourceManager.GetString("rbtSet.Text", Culture);

    internal static string rbtSquare_Text => ResourceManager.GetString("rbtSquare.Text", Culture);

    internal static string rbtSurface_AccessibleName => ResourceManager.GetString("rbtSurface.AccessibleName", Culture);

    internal static string rbtSurface_Text => ResourceManager.GetString("rbtSurface.Text", Culture);

    internal static string rbtTTT_AccessibleName => ResourceManager.GetString("rbtTTT.AccessibleName", Culture);

    internal static string rbtTTT_Text => ResourceManager.GetString("rbtTTT.Text", Culture);

    internal static string rbtVolume_AccessibleName => ResourceManager.GetString("rbtVolume.AccessibleName", Culture);

    internal static string rbtVolume_Text => ResourceManager.GetString("rbtVolume.Text", Culture);

    internal static string rbtXY_Text => ResourceManager.GetString("rbtXY.Text", Culture);

    internal static string rbtXZ_Text => ResourceManager.GetString("rbtXZ.Text", Culture);

    internal static string rbtYZ_Text => ResourceManager.GetString("rbtYZ.Text", Culture);

    internal static string ReactionWithASuchNameIsAlreadyExistChooseAnotherName => ResourceManager.GetString("ReactionWithASuchNameIsAlreadyExistChooseAnotherName", Culture);

    internal static string Reference => ResourceManager.GetString("Reference", Culture);

    internal static string Reflect_CreateReflectedVBObject_Exception_Part1 => ResourceManager.GetString("Reflect.CreateReflectedVBObject.Exception_Part1", Culture);

    internal static string Reflect_CreateReflectedVBObject_Exception_Part2 => ResourceManager.GetString("Reflect.CreateReflectedVBObject.Exception_Part2", Culture);

    internal static string Reflect_CreateReflectedVBObject_Exception_Part3 => ResourceManager.GetString("Reflect.CreateReflectedVBObject.Exception_Part3", Culture);

    internal static string Reflect_Form_Text => ResourceManager.GetString("Reflect.Form.Text", Culture);

    internal static string releaseNoteslinkLabel_Text => ResourceManager.GetString("releaseNoteslinkLabel.Text", Culture);

    internal static string Remove => ResourceManager.GetString("Remove", Culture);

    internal static string RemoveReaction => ResourceManager.GetString("RemoveReaction", Culture);

    internal static string Rename => ResourceManager.GetString("Rename", Culture);

    internal static string Result_BuildDiagram_DistanceResultSet_Header => ResourceManager.GetString("Result.BuildDiagram.DistanceResultSet.Header", Culture);

    internal static string Result_BuildDiagram_GraphData_XUnit => ResourceManager.GetString("Result.BuildDiagram.GraphData.XUnit", Culture);

    internal static string Result_BuildDiagram_NoNodesSelectedException => ResourceManager.GetString("Result.BuildDiagram.NoNodesSelectedException", Culture);

    internal static string Result_BuildDiagram_SelectContaimerAsync_SelectTime_Message => ResourceManager.GetString("Result.BuildDiagram.SelectContaimerAsync.SelectTime.Message", Culture);

    internal static string Result_BuildDiagram_SelectTime_Exception => ResourceManager.GetString("Result.BuildDiagram.SelectTime.Exception", Culture);

    internal static string Result_BuildDiagram_Text_Part1 => ResourceManager.GetString("Result.BuildDiagram.Text_Part1", Culture);

    internal static string Result_BuildDiagram_Text_Part2 => ResourceManager.GetString("Result.BuildDiagram.Text_Part2", Culture);

    internal static string Result_CreateGIFAnimation_AnimationCreated => ResourceManager.GetString("Result.CreateGIFAnimation.AnimationCreated", Culture);

    internal static string Result_CreateGIFAnimation_CreateGIFAnimationInfo => ResourceManager.GetString("Result.CreateGIFAnimation.CreateGIFAnimationInfo", Culture);

    internal static string Result_CreateGIFAnimation_Exception => ResourceManager.GetString("Result.CreateGIFAnimation.Exception", Culture);

    internal static string Result_CreateGIFAnimation_SelectContainerAsync_SelectResult_Message => ResourceManager.GetString("Result.CreateGIFAnimation.SelectContainerAsync.SelectResult.Message", Culture);

    internal static string Result_General_Header_Accuracy => ResourceManager.GetString("Result.General.Header.Accuracy", Culture);

    internal static string Result_General_Header_AverageValues => ResourceManager.GetString("Result.General.Header.AverageValues", Culture);

    internal static string Result_General_Header_Intervals => ResourceManager.GetString("Result.General.Header.Intervals", Culture);

    internal static string Result_General_Header_MaxValue => ResourceManager.GetString("Result.General.Header.MaxValue", Culture);

    internal static string Result_General_Header_MinValue => ResourceManager.GetString("Result.General.Header.MinValue", Culture);

    internal static string Result_General_Header_RefineValues => ResourceManager.GetString("Result.General.Header.RefineValues", Culture);

    internal static string Result_General_Header_Scale => ResourceManager.GetString("Result.General.Header.Scale", Culture);

    internal static string Result_General_Header_ScaleScreenPositionX => ResourceManager.GetString("Result.General.Header.ScaleScreenPositionX", Culture);

    internal static string Result_General_Header_ScaleScreenPositionY => ResourceManager.GetString("Result.General.Header.ScaleScreenPositionY", Culture);

    internal static string Result_General_Header_ShowFields => ResourceManager.GetString("Result.General.Header.ShowFields", Culture);

    internal static string Result_General_Header_ShowScale => ResourceManager.GetString("Result.General.Header.ShowScale", Culture);

    internal static string Result_General_Header_ShowValuesInElements => ResourceManager.GetString("Result.General.Header.ShowValuesInElements", Culture);

    internal static string Result_General_Header_ShowValuesInNodes => ResourceManager.GetString("Result.General.Header.ShowValuesInNodes", Culture);

    internal static string ResultsMainMenuEvents_MergeResults_Recalculated_Message => ResourceManager.GetString("ResultsMainMenuEvents.MergeResults.Recalculated.Message", Culture);

    internal static string ResultsMainMenuEvents_MergeResults_RecalculationOnNodes_Message => ResourceManager.GetString("ResultsMainMenuEvents.MergeResults.RecalculationOnNodes.Message", Culture);

    internal static string ResultsMainMenuEvents_MergeResults_RecalculationOnNodesResNames_Message => ResourceManager.GetString("ResultsMainMenuEvents.MergeResults.RecalculationOnNodesResNames.Message", Culture);

    internal static string rtxbField_Text => ResourceManager.GetString("rtxbField.Text", Culture);

    internal static string SaveLoadScript_GeoScript_Executed_Message => ResourceManager.GetString("SaveLoadScript.GeoScript.Executed.Message", Culture);

    internal static string SaveLoadScript_GeoScript_Formed_Message => ResourceManager.GetString("SaveLoadScript.GeoScript.Formed.Message", Culture);

    internal static string SaveLoadScript_GeoScript_Message => ResourceManager.GetString("SaveLoadScript.GeoScript.Message", Culture);

    internal static string SaveWithoutProjectMessage => ResourceManager.GetString("SaveWithoutProjectMessage", Culture);

    internal static string SceneEvents_CreateGroup_InvalidGroupTypeWarning => ResourceManager.GetString("SceneEvents.CreateGroup.InvalidGroupTypeWarning", Culture);

    internal static string SceneEvents_CreateGroup_SuccessCaption => ResourceManager.GetString("SceneEvents.CreateGroup.SuccessCaption", Culture);

    internal static string SceneEvents_Info_Selected => ResourceManager.GetString("SceneEvents.Info.Selected", Culture);

    internal static string SceneView_CreateGroup_NothingSelected_Message => ResourceManager.GetString("SceneView.CreateGroup.NothingSelected.Message", Culture);

    internal static string SceneView_RemoveSelected_Message => ResourceManager.GetString("SceneView.RemoveSelected.Message", Culture);

    internal static string SceneView_ToolTip_AdvSelect => ResourceManager.GetString("SceneView.ToolTip.AdvSelect", Culture);

    internal static string SceneView_ToolTip_ShowSidesRibs => ResourceManager.GetString("SceneView.ToolTip.ShowSidesRibs", Culture);

    internal static string SceneView_ToolTip_ShowRibs => ResourceManager.GetString("SceneView.ToolTip.ShowRibs", Culture);

    internal static string SceneView_ToolTip_ShowSides => ResourceManager.GetString("SceneView.ToolTip.ShowSides", Culture);

    internal static string SceneView_ToolTip_Basis => ResourceManager.GetString("SceneView.ToolTip.Basis", Culture);

    internal static string SceneView_ToolTip_Contours => ResourceManager.GetString("SceneView.ToolTip.Contours", Culture);

    internal static string SceneView_ToolTip_DisplayStates => ResourceManager.GetString("SceneView.ToolTip.DisplayStates", Culture);

    internal static string SceneView_ToolTip_ScreenShot => ResourceManager.GetString("SceneView.ToolTip.ScreenShot", Culture);

    internal static string SceneView_ToolTip_FitToScreen => ResourceManager.GetString("SceneView.ToolTip.FitToScreen", Culture);

    internal static string SceneView_ToolTip_InsideObjects => ResourceManager.GetString("SceneView.ToolTip.InsideObjects", Culture);

    internal static string SceneView_ToolTip_PlaneXY => ResourceManager.GetString("SceneView.ToolTip.PlaneXY", Culture);

    internal static string SceneView_ToolTip_PlaneZX => ResourceManager.GetString("SceneView.ToolTip.PlaneZX", Culture);

    internal static string SceneView_ToolTip_PlaneZY => ResourceManager.GetString("SceneView.ToolTip.PlaneZY", Culture);

    internal static string SceneView_ToolTip_RotateX => ResourceManager.GetString("SceneView.ToolTip.RotateX", Culture);

    internal static string SceneView_ToolTip_RotateY => ResourceManager.GetString("SceneView.ToolTip.RotateY", Culture);

    internal static string SceneView_ToolTip_RotateZ => ResourceManager.GetString("SceneView.ToolTip.RotateZ", Culture);

    internal static string SceneView_ToolTip_RotateHorizontal90 => ResourceManager.GetString("SceneView.ToolTip.RotateHorizontal90", Culture);

    internal static string SceneView_ToolTip_RotateVertical90 => ResourceManager.GetString("SceneView.ToolTip.RotateVertical90", Culture);

    internal static string SelectAFunction => ResourceManager.GetString("SelectAFunction", Culture);

    internal static string SelectAPropertyOrReactionToRemove => ResourceManager.GetString("SelectAPropertyOrReactionToRemove", Culture);

    internal static string SelectByPoint_ObjectSelected_Message => ResourceManager.GetString("SelectByPoint.ObjectSelected.Message", Culture);

    internal static string SelectByRect_Declination_Type1 => ResourceManager.GetString("SelectByRect.Declination.Type1", Culture);

    internal static string SelectByRect_Declination_Type2 => ResourceManager.GetString("SelectByRect.Declination.Type2", Culture);

    internal static string SelectByRect_Declination_Type3 => ResourceManager.GetString("SelectByRect.Declination.Type3", Culture);

    internal static string SelectByRect_Hidden_Message => ResourceManager.GetString("SelectByRect.Hidden.Message", Culture);

    internal static string SelectByRect_Selected_Message => ResourceManager.GetString("SelectByRect.Selected.Message", Culture);

    internal static string SelectMaterial => ResourceManager.GetString("SelectMaterial", Culture);

    internal static string SelectReaction => ResourceManager.GetString("SelectReaction", Culture);

    internal static string SelectSetEvent_CreateGroupBySet_Message => ResourceManager.GetString("SelectSetEvent.CreateGroupBySet.Message", Culture);

    internal static string SettingsControl_headerName_text => ResourceManager.GetString("SettingsControl_headerName_text", Culture);

    internal static string ShowHideDelObjects_DelObjectEvent_TryToDelGeom_Message => ResourceManager.GetString("ShowHideDelObjects.DelObjectEvent.TryToDelGeom.Message", Culture);

    internal static string ShowInsideObjects_HideInnerObjects_Message => ResourceManager.GetString("ShowInsideObjects.HideInnerObjects.Message", Culture);

    internal static string ShowInsideObjects_ShowAllObjects_Message => ResourceManager.GetString("ShowInsideObjects.ShowAllObjects.Message", Culture);

    internal static string Source => ResourceManager.GetString("Source", Culture);

    internal static string StackTrace => ResourceManager.GetString("StackTrace", Culture);

    internal static string StartCaption => ResourceManager.GetString("StartCaption", Culture);

    internal static string StartStopConp_FormCommandFile_Message => ResourceManager.GetString("StartStopConp.FormCommandFile.Message", Culture);

    internal static string StartStopConp_ProjectDataCheck_LackOfFunctions_Message => ResourceManager.GetString("StartStopConp.ProjectDataCheck.LackOfFunctions.Message", Culture);

    internal static string StartStopConp_ProjectDataCheck_LackOfMaterials_Message => ResourceManager.GetString("StartStopConp.ProjectDataCheck.LackOfMaterials.Message", Culture);

    internal static string StartStopConp_ProjectDataCheck_LackOfProjFile_Message => ResourceManager.GetString("StartStopConp.ProjectDataCheck.LackOfProjFile.Message", Culture);

    internal static string StartStopConp_ProjectDirectoryCheck_Message_Part1 => ResourceManager.GetString("StartStopConp.ProjectDirectoryCheck.Message_Part1", Culture);

    internal static string StartStopConp_SaveProjectInto_Message => ResourceManager.GetString("StartStopConp.SaveProjectInto.Message", Culture);

    internal static string StopCaption => ResourceManager.GetString("StopCaption", Culture);

    internal static string StringEx_ToEnum_ArgumentException_Part1 => ResourceManager.GetString("StringEx.ToEnum.ArgumentException.Part1", Culture);

    internal static string StringEx_ToEnum_ArgumentException_Part2 => ResourceManager.GetString("StringEx.ToEnum.ArgumentException.Part2", Culture);

    internal static string SubBeamConnection => ResourceManager.GetString("SubBeamConnection", Culture);

    internal static string SubCreateCurve => ResourceManager.GetString("SubCreateCurve", Culture);

    internal static string SubCreateMesh2DPoligon => ResourceManager.GetString("SubCreateMesh2DPoligon", Culture);

    internal static string SubCreatePoint => ResourceManager.GetString("SubCreatePoint", Culture);

    internal static string SubCreateSurface => ResourceManager.GetString("SubCreateSurface", Culture);

    internal static string SubExtrudeCurve => ResourceManager.GetString("SubExtrudeCurve", Culture);

    internal static string SubExtrudeRotation => ResourceManager.GetString("SubExtrudeRotation", Culture);

    internal static string SubFindCoincident => ResourceManager.GetString("SubFindCoincident", Culture);

    internal static string SubFindObject => ResourceManager.GetString("SubFindObject", Culture);

    internal static string SubFindVolElems => ResourceManager.GetString("SubFindVolElems", Culture);

    internal static string SubLoadProject => ResourceManager.GetString("SubLoadProject", Culture);

    internal static string SubMergeElementSets => ResourceManager.GetString("SubMergeElementSets", Culture);

    internal static string SubMoveMesh => ResourceManager.GetString("SubMoveMesh", Culture);

    internal static string SubMoveNodes => ResourceManager.GetString("SubMoveNodes", Culture);

    internal static string SubRenumberMesh => ResourceManager.GetString("SubRenumberMesh", Culture);

    internal static string SubRotateMesh => ResourceManager.GetString("SubRotateMesh", Culture);

    internal static string SubSaveProject => ResourceManager.GetString("SubSaveProject", Culture);

    internal static string SubSetLevel => ResourceManager.GetString("SubSetLevel", Culture);

    internal static string tableLayoutPanel1_ToolTip => ResourceManager.GetString("tableLayoutPanel1.ToolTip", Culture);

    internal static string Task_General_Header_CheckConditionValues => ResourceManager.GetString("Task.General.Header.CheckConditionValues", Culture);

    internal static string Task_General_Header_Kind => ResourceManager.GetString("Task.General.Header.Kind", Culture);

    internal static string Task_General_Header_Type => ResourceManager.GetString("Task.General.Header.Type", Culture);

    internal static string TaskKind_Chemical => ResourceManager.GetString("TaskKind_Chemical", Culture);

    internal static string TaskKind_Hydrodynamic => ResourceManager.GetString("TaskKind_Hydrodynamic", Culture);

    internal static string TaskKind_Mechanical => ResourceManager.GetString("TaskKind_Mechanical", Culture);

    internal static string TaskKind_Termal => ResourceManager.GetString("TaskKind_Termal", Culture);

    internal static string textBox1_Text => ResourceManager.GetString("textBox1.Text", Culture);

    internal static string textBox1_ToolTip => ResourceManager.GetString("textBox1.ToolTip", Culture);

    internal static string textBox2_Text => ResourceManager.GetString("textBox2.Text", Culture);

    internal static string ThePhaseNameCanOnlyConsistOfLettersNumbersAndTheUnderscore => ResourceManager.GetString("ThePhaseNameCanOnlyConsistOfLettersNumbersAndTheUnderscore", Culture);

    internal static string ThePhaseNameCanOnlyConsistOfLettersNumbersAndTheUnderscore_ => ResourceManager.GetString("ThePhaseNameCanOnlyConsistOfLettersNumbersAndTheUnderscore_", Culture);

    internal static string TheReactionRequiresAtLeastTwoPhases => ResourceManager.GetString("TheReactionRequiresAtLeastTwoPhases", Culture);

    internal static string title_Text => ResourceManager.GetString("title.Text", Culture);

    internal static string toolStrip2_Text => ResourceManager.GetString("toolStrip2.Text", Culture);

    internal static string toolStripContainer_Text => ResourceManager.GetString("toolStripContainer.Text", Culture);

    internal static string toolStripContainer1_Text => ResourceManager.GetString("toolStripContainer1.Text", Culture);

    internal static string toolStripContainer2_Text => ResourceManager.GetString("toolStripContainer2.Text", Culture);

    internal static string trackBar1_ToolTip => ResourceManager.GetString("trackBar1.ToolTip", Culture);

    internal static string trackBar2_ToolTip => ResourceManager.GetString("trackBar2.ToolTip", Culture);

    internal static string trackBar3_ToolTip => ResourceManager.GetString("trackBar3.ToolTip", Culture);

    internal static string txbAngle_Text => ResourceManager.GetString("txbAngle.Text", Culture);

    internal static string txbFinTemp_Text => ResourceManager.GetString("txbFinTemp.Text", Culture);

    internal static string txbIniTemp_Text => ResourceManager.GetString("txbIniTemp.Text", Culture);

    internal static string txbMaxPhase_Text => ResourceManager.GetString("txbMaxPhase.Text", Culture);

    internal static string txbMaxTime_AccessibleName => ResourceManager.GetString("txbMaxTime.AccessibleName", Culture);

    internal static string txbMaxTime_Text => ResourceManager.GetString("txbMaxTime.Text", Culture);

    internal static string txbMaxVel_Text => ResourceManager.GetString("txbMaxVel.Text", Culture);

    internal static string txbMinPhase_Text => ResourceManager.GetString("txbMinPhase.Text", Culture);

    internal static string txbMinVel_Text => ResourceManager.GetString("txbMinVel.Text", Culture);

    internal static string txbPhaseValue_Text => ResourceManager.GetString("txbPhaseValue.Text", Culture);

    internal static string txbPoint1_Text => ResourceManager.GetString("txbPoint1.Text", Culture);

    internal static string txbPoint2_Text => ResourceManager.GetString("txbPoint2.Text", Culture);

    internal static string txbPoint3_Text => ResourceManager.GetString("txbPoint3.Text", Culture);

    internal static string txbTemp_AccessibleName => ResourceManager.GetString("txbTemp.AccessibleName", Culture);

    internal static string UndefinedConditionTypeExc => ResourceManager.GetString("UndefinedConditionTypeExc", Culture);

    internal static string UnknownTypeException => ResourceManager.GetString("UnknownTypeException", Culture);

    internal static string UtilityTiilStrip_SelectObjectAsync_NoObjectSelected_Message => ResourceManager.GetString("UtilityTiilStrip.SelectObjectAsync.NoObjectSelected.Message", Culture);

    internal static string UtilityTiilStrip_SelectObjectAsync_Selected_Message => ResourceManager.GetString("UtilityTiilStrip.SelectObjectAsync.Selected.Message", Culture);

    internal static string UtilityTiilStrip_SelectObjectAsync_SelectOne_Message => ResourceManager.GetString("UtilityTiilStrip.SelectObjectAsync.SelectOne.Message", Culture);

    internal static string UtilityTiilStrip_SelectObjectAsync_WithNumber_Message => ResourceManager.GetString("UtilityTiilStrip.SelectObjectAsync.WithNumber.Message", Culture);

    internal static string UtilityToolStip_ClipForm_Text => ResourceManager.GetString("UtilityToolStip.ClipForm.Text", Culture);

    internal static string UtilityToolStrip_CalcSquare_Output => ResourceManager.GetString("UtilityToolStrip.CalcSquare.Output", Culture);

    internal static string UtilityToolStrip_CalcVolume_Output => ResourceManager.GetString("UtilityToolStrip.CalcVolume.Output", Culture);

    internal static string UtilityToolStrip_CreateCrossSection_InvalidNodeNumerErrorMessage => ResourceManager.GetString("UtilityToolStrip.CreateCrossSection.InvalidNodeNumerErrorMessage", Culture);

    internal static string UtilityToolStrip_CrossSection_Text => ResourceManager.GetString("UtilityToolStrip.CrossSection.Text", Culture);

    internal static string UtilityToolStrip_Distance_Output => ResourceManager.GetString("UtilityToolStrip.Distance.Output", Culture);

    internal static string UtilityToolStrip_DistancePointToPlane_InstructionPart1 => ResourceManager.GetString("UtilityToolStrip.DistancePointToPlane.InstructionPart1", Culture);

    internal static string UtilityToolStrip_DistancePointToPlane_InstructionPart2 => ResourceManager.GetString("UtilityToolStrip.DistancePointToPlane.InstructionPart2", Culture);

    internal static string UtilityToolStrip_DistancePointToPoint_EmptySelectionErrorMessage => ResourceManager.GetString("UtilityToolStrip.DistancePointToPoint.EmptySelectionErrorMessage", Culture);

    internal static string UtilityToolStrip_Measuring_InvalidSeletedTypeError => ResourceManager.GetString("UtilityToolStrip.Measuring.InvalidSeletedTypeError", Culture);

    internal static string UtilityToolStrip_SelectObjectAsync_OperationCanceled_Message => ResourceManager.GetString("UtilityToolStrip.SelectObjectAsync.OperationCanceled.Message", Culture);

    internal static string VBObjectController_ChangeViewMode_Message => ResourceManager.GetString("VBObjectController.ChangeViewMode.Message", Culture);

    internal static string VBObjectCtor_EmptyArrayArgumentException => ResourceManager.GetString("VBObjectCtor.EmptyArrayArgumentException", Culture);

    internal static string VersionNews => ResourceManager.GetString("VersionNews", Culture);

    internal static string versionWordPrefix => ResourceManager.GetString("versionWordPrefix", Culture);

    internal static string webPageLabel_Text => ResourceManager.GetString("webPageLabel.Text", Culture);

    internal static string выбратьСопряженныеToolStripMenuItem_Text => ResourceManager.GetString("выбратьСопряженныеToolStripMenuItem.Text", Culture);

    internal static string Выполнить => ResourceManager.GetString("Выполнить", Culture);

    internal static string Отсортировать => ResourceManager.GetString("Отсортировать", Culture);

    internal static string Показать => ResourceManager.GetString("Показать", Culture);

    internal static string Пропустить => ResourceManager.GetString("Пропустить", Culture);

    internal static string Реверс => ResourceManager.GetString("Реверс", Culture);
}
