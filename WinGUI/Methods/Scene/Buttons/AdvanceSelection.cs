using WinGUI.AdvanceSelection;
using WinGUI.AdvanceSelection.ControlsForSelect;
using WinGUI.Extensions;
using WinGUI.Properties;
using WinGUI.Utilities;
using Model.Interfaces;
using Model.MeshObjects;
using Model.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Community.CsharpSqlite.Sqlite3;
using static IronPython.Modules.PythonIterTools;

namespace WinGUI
{
    public partial class BaseForm
    {
        private void btnSelection_Paint(object sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            var button = sender as Button;
            var rectangle = new Rectangle(0, 0, button.Width - 1, button.Height - 1);


            if (bool.Parse(button.Tag.ToString()))
                e.Graphics.DrawRectangle(new Pen(Color.Black, 3.0f), rectangle);
        }

        private void btnAdvSelection_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;

            if (!bool.Parse(btn.Tag.ToString()))
            {
                if(SelectedObjects == SelectionType.Select || SelectedObjects == SelectionType.Objects)
                    return;
                btn.Tag = true;
                var form = new Form()
                {
                    Name = "selectForm",
                    Text = Resources.AdvanceSelectionForm_Text,
                    AutoSize = false,
                    ShowIcon = false,
                    MinimizeBox = false,
                    MaximizeBox = false,
                    TopMost = true,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Owner = Application.OpenForms[0]
                };
                form.FormClosing += (s1, s2) => 
                {
                    CleanupSelectionControl(form);
                    btn.Tag = false;
                    btn.Invalidate();
                };

                if (IsMesh())
                {
                    var selectionControl = new MeshSelect(SelectedObjects);
                    OnChangeSelectedObjectsEvent += selectionControl.SetAvailableModes;
                    //selectionControl.SelectInDirection += OnReverseChanged;
                    selectionControl.CloseForm += RefreshForm;

                    form.ClientSize = selectionControl.Size;
                    form.Controls.Add(selectionControl);
                }

                else if (IsGeometry())
                {
                    var selectionControl = new GeomSelect(SelectedObjects);
                    OnChangeSelectedObjectsEvent += selectionControl.SetAvailableModes;
                    selectionControl.CloseForm += RefreshForm;

                    form.ClientSize = selectionControl.Size;
                    form.Controls.Add(selectionControl);
                }

                form.Show();
                var location = GetPosition(form.Height);
                form.Location = location;
            }
            else
            {
                CloseAdvancedSelectionForm();
            }
        }

        private void DispatchSelection(List<int> numbers, bool isSelected)
        {
            var forms = Application.OpenForms.Cast<Form>().ToList();
            var form = forms.Find(x => x.Name == "selectForm");
            if(form != null)
            {
                if (IsMesh())
                {
                    var mesh = form.Controls.OfType<MeshSelect>().FirstOrDefault();
                    var additionalMode = mesh.GetSelectedAdditionalMode();
                    if (additionalMode is SelectInDirectionEventArgs sdArgs)
                    {
                        sdArgs.SelectedNumbers.AddRange(numbers);
                        SelectInDirection(sdArgs);
                    }
                    else if (additionalMode is SelectInPlainEventArgs spArgs)
                    {
                        spArgs.SelectedNumbers.AddRange(numbers);
                        SelectInPlane(spArgs);
                    }
                    else if (additionalMode is ObjType setType)
                        SelectionControl_SelectInSet(setType, numbers, isSelected);

                }
                else if (IsGeometry())
                {       
                    var geom = form.Controls.OfType<GeomSelect>().FirstOrDefault();
                    SelectionControl_SelectInGeom(geom.GetSelectDimension(), numbers, isSelected);
                }
            }
        }

        private bool SelectInPlane(SelectInPlainEventArgs spArgs)
        {
            try
            {
                var objType = Converters.ConvertSelectionTypeToObjType(SelectedObjects);

                if (objType == ObjType.Узел)
                {
                    if (spArgs.SelectedNumbers.Count > 2 && Converters.ConvertSelectionTypeToObjType(SelectedObjects) == ObjType.Узел)
                    {

                        if (SelectNodeInPlane(spArgs.SelectedNumbers).Count > 0)
                        {
                            // отчищаем список
                            spArgs.SelectedNumbers.Clear();
                            return true;
                        }

                    }
                    else
                        console.PrintInfo(Resources.AdvanceSelection3NodesWarning, Color.Orange);
                }

                else if(objType == ObjType.Элемент2D)
                {
                    if (spArgs.SelectedNumbers.Count > 0 && Converters.ConvertSelectionTypeToObjType(SelectedObjects) == ObjType.Элемент2D)
                    {
                        if (SelectE2DInPlane(spArgs.SelectedNumbers, spArgs.Angle).Count > 0)
                        {
                            // отчищаем список
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
                spArgs.SelectedNumbers.RemoveAt(spArgs.SelectedNumbers.Count - 1);
                return false;
            }
            
        }

        private bool SelectInDirection(SelectInDirectionEventArgs sdArgs)
        {
            try
            {
               
                if (sdArgs.SelectedNumbers.Count() > 1)
                {
                    if (!sdArgs.Reverse)
                    {
                        if (project.SelectNodeInDirection(sdArgs.Angle, sdArgs.SelectedNumbers[0],
                            sdArgs.SelectedNumbers[1], settingsConfig.SelectObjectColor).Count > 0)
                        {
                            sdArgs.SelectedNumbers.Clear();
                            return true;
                        }
                            
                    }

                    else
                    {
                        if (project.SelectNodeInDirection(sdArgs.Angle, sdArgs.SelectedNumbers[1],
                            sdArgs.SelectedNumbers[0], settingsConfig.SelectObjectColor).Count > 0)
                        {
                            sdArgs.SelectedNumbers.Clear();
                            return true;
                        }
                    }
  
                }
                else
                    console.PrintInfo(Resources.AdvanceSelection2NodesWarning, Color.Orange);
                return false;
            }
            catch (Exception)
            {
                sdArgs.SelectedNumbers.RemoveAt(sdArgs.SelectedNumbers.Count - 1);
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
            foreach (var setInfo in uniqueSets)
            {
                var setNumbers = setInfo.GetNumbers();
                if (isSelected)
                    project.Select(selectType, setNumbers);
                else
                    project.Deselect(selectType, setNumbers);
            }

            var selectedCount = project.GetSelected(selectType).ToList();

            console.PrintInfo($"{selectType}, {Resources.AdvaneSelectionSelectedCaption}: {selectedCount}", Color.Black);
            return selectedCount;
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
                return project.SelectE2DInPlane(
                    angle, selectedE2D.Last(), settingsConfig.SelectObjectColor);           
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
            btnAdvSelection_Click(btnAdvSelection, EventArgs.Empty);
        }

        private void BackColorToAllObjects()
        {
            SetBackColorToAllObjects();
        }

        private void CloseAdvancedSelectionForm()
        {
            var forms = Application.OpenForms.Cast<Form>().ToList();
            var form = forms.Find(x => x.Name == "selectForm");
            if (form != null)
            {
                CleanupSelectionControl(form);
                form.Close();
            }
        }

        private Point GetPosition(int hightForm)
        {
            var scenePosition = scene.PointToScreen(Point.Empty);
            var x = scenePosition.X;
            var y = scenePosition.Y + scene.Height - hightForm;
            return new Point(x, y);
        }
        private void CleanupSelectionControl(Form form)
        {
            var mesh = form.Controls.OfType<MeshSelect>().FirstOrDefault();
            if (mesh != null)
            {
                OnChangeSelectedObjectsEvent -= mesh.SetAvailableModes;
                //mesh.SelectInDirection -= OnReverseChanged;
                mesh.CloseForm -= RefreshForm;
                mesh.Dispose();
                return;
            }

            var geom = form.Controls.OfType<GeomSelect>().FirstOrDefault();
            if (geom != null)
            {
                OnChangeSelectedObjectsEvent -= geom.SetAvailableModes;
                geom.CloseForm -= RefreshForm;
                geom.Dispose();
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
            return SelectedObjects == SelectionType.Points||
                   SelectedObjects == SelectionType.Curves ||
                   SelectedObjects == SelectionType.Surfaces ||
                   SelectedObjects == SelectionType.Objects;
        }
    }
}
