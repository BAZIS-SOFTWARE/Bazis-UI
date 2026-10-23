using WinGUI.Extensions;
using WinGUI.Properties;
using WinGUI.Scene.VBO;
using Geometry;
using Model.Interfaces;
using Model.Utilities;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinGUI
{
    public partial class BaseForm
    {
        private void создатьГруппуItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedObjects == SelectionType.Select |
                    SelectedObjects == SelectionType.Objects |
                    SelectedObjects == SelectionType.Figures |
                    SelectedObjects == SelectionType.Elements)
                {

                    console.PrintInfo($"{Resources.SceneEvents_CreateGroup_InvalidGroupTypeWarning}: {SelectedObjects.ToString()}", Color.Orange);
                }
                else
                {
                    //CreatedMeshGroupEvent?.Invoke(this, spbSelectObject.ToolTipText);
                    var selObjs = project.GetSelection().ToList();

                    if (selObjs.Count() > 0)
                    {
                        //var objType = objTypeStr.ToEnum<ObjType>();
                        //var name = $"newGroup{objType}";

                        // TODO перейти на другой метод создания группы.
                        // Вынести на уровень контроллера
                        project.CreateGroup(selObjs);
                        var gr = project.GetAllModelGroups().Last();

                        console.PrintInfo($"{Resources.SceneEvents_CreateGroup_SuccessCaption}: {gr.Name}", Color.Black);
                    }
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void скрытьВыбранноеItem_Click(object sender, EventArgs e)
        {
            try
            {
                project.HideSelected();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void показатьСкрытыеItem_Click(object sender, EventArgs e)
        {
            try
            {
                project.ShowAll();
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void menuItem_InfoSelectedObjects_Click(object sender, EventArgs e)
        {
            try
            {
                var selObjs = project.GetSelection().ToList();

                var message = $"{Resources.SceneEvents_Info_Selected} {SelectedObjects}: {selObjs.Count()}";

                var numbers = string.Join("\n", selObjs.Select(x => x.ToString()).ToArray());

                message += "\n" + numbers;

                console.PrintInfo(message, Color.Black);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void menuItem_SetRotPoint_Click(object sender, EventArgs e)
        {
            var left = ScreenMousePosition.X;
            var rigth = ScreenMousePosition.X + 10;
            var top = ScreenMousePosition.Y;
            var bottom = ScreenMousePosition.Y - 10;

            var selectionBox = new RectangleBox(left, rigth, bottom, top);

            var selection = new List<Point3D>();



            foreach (var glObj in VBOController.GetVBObjs())
            {
                var coords = glObj.PointsCoords;

                var length = coords.Length / 3;

                for (int i = 0; i < length; i++)
                {
                    var x = coords[3 * i + 0];
                    var y = coords[3 * i + 1];
                    var z = coords[3 * i + 2];

                    var scnCoord = GetSceenCoord(x, y, z);
                    var scrCoord = GetScreenCoord(scnCoord);

                    if (selectionBox.IsPointInside(scrCoord))
                        selection.Add(scnCoord);
                }
            }

            selection = selection.OrderByDescending(x => x._z).ToList();
            if (selection.Count > 0)
                SetRotationCentre(selection.First());

            RequestRedraw();
        }

        private void menuItem_DeleteSelectedObjects_Click(object sender, EventArgs e)
        {
            try
            {
                // Пока нельзя удалить геометрию рамкой с экрана. Пока только через дерево.
                if (SelectedObjects == SelectionType.Points |
                    SelectedObjects == SelectionType.Curves |
                    SelectedObjects == SelectionType.Surfaces)
                    return;

                var selObjs = project.GetSelection().ToList();

                foreach (var item in selObjs)
                    item.ExistState = false;

                project.ClearNotExistedModelData();
                //project.ClearEmptySet();
                //project.ClearNotExistedGroupData();
                //project.ClearNotExistedCondData();

                //project.ModelData.ObjectData.ClearNotExisted();
                //project.ModelData.ObjectData.ClearEmptySet();
                //project.ModelData.GroupData.ClearNotExisted();
                //project.TaskData.ClearNotExisted(project.ModelData.GroupData);

            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void scene_SceneInfoEvent(object arg1, string arg2, Color arg3)
        {
            console.PrintInfo(arg2, arg3);
        }

        internal void SetBackColorToAllObjects()
        {
            if (project == null)
                return;

            // Как в исходном BaseForm: сбрасываются и выделение, и подсветка групп/условий
            using (project.BeginViewUpdate())
            {
                project.ClearSelection();
                project.ClearColor();
            }
        }

        private void GlControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DisplayGeometryObjectEvent = null;
                // все таки может выключать весь текст по эскайп?
                DisplayText2DEvent = null;
                DisplayText3DEvent = null;

                SelectedObjects = SelectionType.Select;
                CloseAdvancedSelectionForm();
                SetBackColorToAllObjects();
                RequestRedraw();
            }
            else if (e.KeyCode == Keys.C)
            {
                var left = ScreenMousePosition.X;
                var rigth = ScreenMousePosition.X + 10;
                var top = ScreenMousePosition.Y;
                var bottom = ScreenMousePosition.Y - 10;

                var selectionBox = new RectangleBox(left, rigth, bottom, top);

                var selection = new List<Point3D>();


                foreach (var glObj in VBOController.GetVBObjs())
                {
                    // добавление прямоугольника выбора по параллелограмму объекта
                    var scrPoints = glObj.BoundingBox.GetCornerPoints().
                        Select(c => GetSceenCoord(c._x, c._y, c._z)).
                        Select(s => GetScreenCoord(s));

                    var rect = new RectangleBox(scrPoints);

                    if (selectionBox.IsInnerOther(rect) |
                        selectionBox.IsIntersectWithOther(rect))
                    {
                        var coords = glObj.PointsCoords;

                        var length = coords.Length / 3;

                        for (int i = 0; i < length; i++)
                        {
                            var x = coords[3 * i + 0];
                            var y = coords[3 * i + 1];
                            var z = coords[3 * i + 2];

                            var scnPoint = GetSceenCoord(x, y, z);
                            var scrPoint = GetScreenCoord(scnPoint);

                            if (selectionBox.IsPointInside(scrPoint))
                                selection.Add(new Point3D(x, y, z));
                        }
                        var distSelection = selection.Distinct();
                        var sortedSelection = distSelection.OrderByDescending(x => x._z);
                        if (sortedSelection.Count() > 0)
                        {
                            SetRotationCentre(sortedSelection.First());
                            break;
                        }
                    }
                }
                RequestRedraw();
            }
            else if (e.KeyCode == Keys.F)
            {
                FitObjectsToScreen();
                RequestRedraw();
            }
        }

        /// <inheritdoc/>
        // Перенесено в SceneCamera.SetRotationCentre — см. GUI/Documents/scene.avalonia.md, раздел 6.
        public void SetRotationCentre(Point3D modelPoint) => sceneController.SetRotationCentre(modelPoint);

        private void GlControl_Resize(object sender, EventArgs e) => sceneController.Resize(scene.Width, scene.Height);

        private void SelectObjects(Point2D point, bool isSelected)
        {
            try
            {
                if (project != null)
                {
                    var sets = GetModelSetsInfo(SelectedObjects);
                    //var sets = project.GetModelSetsInfo(spbSelectObject.ToolTipText);
                    if (SelectByPoint(sets, point, isSelected))
                    {
                        sceneSelectionChangedAction?.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }

        private void SelectObjects(RectangleBox rectangleBox, bool isSelected)
        {
            try
            {
                if(project != null)
                {
                    var sets = GetModelSetsInfo(SelectedObjects);
                    SelectByRect(sets, rectangleBox, isSelected);

                    sceneSelectionChangedAction?.Invoke();
                }
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
            }
        }      
    }
}
