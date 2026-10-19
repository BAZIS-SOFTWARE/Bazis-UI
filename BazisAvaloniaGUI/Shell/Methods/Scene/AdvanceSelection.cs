using Avalonia;
using Avalonia.Controls;
using BazisAvaloniaGUI.AdvanceSelection;
using BazisAvaloniaGUI.Utilities;
using Model.Interfaces;
using Model.MeshObjects;
using System;
using System.Collections.Generic;
using Color = System.Drawing.Color;
using System.Linq;

namespace BazisAvaloniaGUI.Shell
{
    // Из GUI/Methods/Scene/Buttons/AdvanceSelection.cs: окно дополнительного выбора (btnAdvSelection).
    internal partial class MainWindow
    {
        private Window advancedSelectionForm;

        /// <summary>Окно дополнительного выбора открыто (в BaseForm — btnAdvSelection.Tag).</summary>
        private bool IsAdvancedSelectionOpened => advancedSelectionForm != null;

        private void scene_AdvancedSelectionRequested()
        {
            if (!IsAdvancedSelectionOpened)
            {
                if (SelectedObjects == SelectionType.Select || SelectedObjects == SelectionType.Objects)
                    return;

                var form = new Window()
                {
                    Name = "selectForm",
                    Title = Resources.AdvanceSelectionForm_Text,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    CanResize = false,
                    CanMinimize = false,
                    CanMaximize = false,
                    Topmost = true,
                    ShowInTaskbar = false,
                    FontFamily = FontFamily,
                    FontSize = FontSize
                };
                form.Closed += (s1, s2) =>
                {
                    CleanupSelectionControl(form);
                    if (ReferenceEquals(advancedSelectionForm, form))
                        advancedSelectionForm = null;
                };

                if (IsMesh())
                {
                    var selectionControl = new MeshSelect(SelectedObjects);
                    OnChangeSelectedObjectsEvent += selectionControl.SetAvailableModes;
                    selectionControl.CloseForm += RefreshForm;
                    form.Content = selectionControl;
                }
                else if (IsGeometry())
                {
                    var selectionControl = new GeomSelect(SelectedObjects);
                    OnChangeSelectedObjectsEvent += selectionControl.SetAvailableModes;
                    selectionControl.CloseForm += RefreshForm;
                    form.Content = selectionControl;
                }

                advancedSelectionForm = form;
                form.Show(this);
                PlaceAtSceneBottom(form);
            }
            else
            {
                CloseAdvancedSelectionForm();
            }
        }

        /// <summary>BaseForm.GetPosition: окно выбора — в левом нижнем углу сцены.</summary>
        private void PlaceAtSceneBottom(Window form)
        {
            var origin = scene.PointToScreen(new Avalonia.Point(0, scene.Bounds.Height));
            var height = form.Bounds.Height > 0 ? form.Bounds.Height : 150;
            form.Position = new PixelPoint(origin.X, origin.Y - (int)(height * form.RenderScaling));
        }

        /// <summary>BaseForm.OnChangeSelectedObjectsEvent: сменился тип объектов для выбора.</summary>
        public event Action<SelectionType> OnChangeSelectedObjectsEvent;

        private void scene_SelectedObjectTypeChanged(ObjType? type) =>
            OnChangeSelectedObjectsEvent?.Invoke(SelectedObjects);

        private void DispatchSelection(List<int> numbers, bool isSelected)
        {
            var form = advancedSelectionForm;
            if (form == null)
                return;

            if (IsMesh() && form.Content is MeshSelect mesh)
            {
                var additionalMode = mesh.GetSelectedAdditionalMode();
                if (additionalMode is SelectInDirectionEventArgs sdArgs)
                {
                    // Запоминаем размер до AddRange, чтобы при ошибке откатить всю добавленную порцию.
                    var addedFrom = sdArgs.SelectedNumbers.Count;
                    sdArgs.SelectedNumbers.AddRange(numbers);
                    SelectInDirection(sdArgs, addedFrom);
                }
                else if (additionalMode is SelectInPlainEventArgs spArgs)
                {
                    var addedFrom = spArgs.SelectedNumbers.Count;
                    spArgs.SelectedNumbers.AddRange(numbers);
                    SelectInPlane(spArgs, addedFrom);
                }
                else if (additionalMode is ObjType setType)
                    SelectionControl_SelectInSet(setType, numbers, isSelected);
            }
            else if (IsGeometry() && form.Content is GeomSelect geom)
            {
                SelectionControl_SelectInGeom(geom.GetSelectDimension(), numbers, isSelected);
            }
        }

        private bool SelectInPlane(SelectInPlainEventArgs spArgs, int addedFrom)
        {
            try
            {
                var objType = Converters.ConvertSelectionTypeToObjType(SelectedObjects);

                if (objType == ObjType.Узел)
                {
                    if (spArgs.SelectedNumbers.Count > 2)
                    {
                        if (SelectNodeInPlane(spArgs.SelectedNumbers).Count > 0)
                        {
                            spArgs.SelectedNumbers.Clear();
                            return true;
                        }
                    }
                    else
                        console.PrintInfo(Resources.AdvanceSelection3NodesWarning, Color.Orange);
                }
                else if (objType == ObjType.Элемент2D)
                {
                    if (spArgs.SelectedNumbers.Count > 0)
                    {
                        if (SelectE2DInPlane(spArgs.SelectedNumbers, spArgs.Angle).Count > 0)
                        {
                            spArgs.SelectedNumbers.Clear();
                            return true;
                        }
                    }
                    else
                        console.PrintInfo(Resources.AdvanceSelectionElemntsSelectionWarning, Color.Orange);
                }

                return false;
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
                RemoveAddedNumbers(spArgs.SelectedNumbers, addedFrom);
                return false;
            }
        }

        /// <summary>
        /// Откатывает номера, добавленные последней порцией выбора (с позиции <paramref name="addedFrom"/>).
        /// Раньше удалялся только последний номер, и при выборе нескольких объектов остаток «залипал».
        /// </summary>
        private static void RemoveAddedNumbers(List<int> numbers, int addedFrom)
        {
            if (addedFrom >= 0 && addedFrom < numbers.Count)
                numbers.RemoveRange(addedFrom, numbers.Count - addedFrom);
        }

        private bool SelectInDirection(SelectInDirectionEventArgs sdArgs, int addedFrom)
        {
            try
            {
                if (sdArgs.SelectedNumbers.Count() > 1)
                {
                    var first = sdArgs.Reverse ? sdArgs.SelectedNumbers[1] : sdArgs.SelectedNumbers[0];
                    var second = sdArgs.Reverse ? sdArgs.SelectedNumbers[0] : sdArgs.SelectedNumbers[1];
                    if (project.SelectNodeInDirection(sdArgs.Angle, first, second, settingsConfig.SelectObjectColor).Count > 0)
                    {
                        sdArgs.SelectedNumbers.Clear();
                        return true;
                    }
                }
                else
                    console.PrintInfo(Resources.AdvanceSelection2NodesWarning, Color.Orange);
                return false;
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
                RemoveAddedNumbers(sdArgs.SelectedNumbers, addedFrom);
                return false;
            }
        }

        private List<int> SelectionControl_SelectInSet(ObjType selectType, List<int> numbers, bool isSelected)
        {
            if (numbers == null || numbers.Count == 0)
            {
                console.PrintInfo(Resources.AdvanceSelectionNoObjectSelectedWarning, Color.Red);
                return null;
            }

            var uniqueSets = numbers.Select(number => project.GetModelSetInfo(selectType, number)).GroupBy(setInfo => setInfo.Name).Select(g => g.First()).ToList();

            ApplySelectionColor();
            using (project.BeginViewUpdate())
            {
                foreach (var setInfo in uniqueSets)
                {
                    var setNumbers = setInfo.GetNumbers();
                    if (isSelected)
                        project.Select(selectType, setNumbers);
                    else
                        project.Deselect(selectType, setNumbers);
                }
            }

            var selected = project.GetSelected(selectType).ToList();

            console.PrintInfo($"{selectType}, {Resources.AdvaneSelectionSelectedCaption}: {selected.Count}", Color.Black);
            return selected;
        }

        private void SelectionControl_SelectInGeom(int targetDim, List<int> numbers, bool isSelected)
        {
            if (numbers == null || numbers.Count == 0)
            {
                console.PrintInfo(Resources.AdvanceSelectionNoObjectSelectedWarning, Color.Red);
                return;
            }

            var startDim = GetModelObjects(SelectedObjects).Where(x => x.Number == numbers[0]).First().Dim;
            var objType = Converters.ConvertSelectionTypeToObjType(SelectedObjects);
            var scopedNumbers = project.SelectByScope(startDim, numbers, targetDim);

            ApplySelectionColor();
            if (isSelected)
                project.Select(objType, scopedNumbers);
            else
                project.Deselect(objType, scopedNumbers);

            var selectedCount = project.SelectedCount;

            console.PrintInfo($"{objType}, {Resources.AdvaneSelectionSelectedCaption}: {selectedCount}", Color.Black);
        }

        private List<int> SelectE2DInPlane(List<int> selectedE2D, float angle)
        {
            return project.SelectE2DInPlane(angle, selectedE2D.Last(), settingsConfig.SelectObjectColor);
        }

        private List<int> SelectNodeInPlane(List<int> selectedNodes)
        {
            var n1 = (Node)project.GetModelObject(ObjType.Узел, selectedNodes[0]);
            var n2 = (Node)project.GetModelObject(ObjType.Узел, selectedNodes[1]);
            var n3 = (Node)project.GetModelObject(ObjType.Узел, selectedNodes[2]);

            var plane = new Geometry.Plane(n1.Position, n2.Position, n3.Position);
            return project.SelectNodeInPlane(plane, settingsConfig.SelectObjectColor);
        }

        private void RefreshForm()
        {
            CloseAdvancedSelectionForm();
            scene_AdvancedSelectionRequested();
        }

        private void CloseAdvancedSelectionForm()
        {
            var form = advancedSelectionForm;
            if (form != null)
            {
                CleanupSelectionControl(form);
                advancedSelectionForm = null;
                form.Close();
            }
        }

        private void CleanupSelectionControl(Window form)
        {
            if (form.Content is MeshSelect mesh)
            {
                OnChangeSelectedObjectsEvent -= mesh.SetAvailableModes;
                mesh.CloseForm -= RefreshForm;
            }
            else if (form.Content is GeomSelect geom)
            {
                OnChangeSelectedObjectsEvent -= geom.SetAvailableModes;
                geom.CloseForm -= RefreshForm;
            }
        }

        private bool IsMesh()
        {
            return SelectedObjects == SelectionType.Elements1D ||
                   SelectedObjects == SelectionType.Elements2D ||
                   SelectedObjects == SelectionType.Elements3D ||
                   SelectedObjects == SelectionType.Nodes;
        }

        private bool IsGeometry()
        {
            return SelectedObjects == SelectionType.Points ||
                   SelectedObjects == SelectionType.Curves ||
                   SelectedObjects == SelectionType.Surfaces ||
                   SelectedObjects == SelectionType.Objects;
        }
    }
}
