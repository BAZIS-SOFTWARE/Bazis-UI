using BazisGUI.Scene;
using Model.Interfaces;
using Model.Interfaces.ObjectsCollections;
using OperationalController;
using Project.Interfaces.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BazisGUI
{
    public partial class BaseForm
    {
        const ModelViewChange RebuildChanges = ModelViewChange.Visibility | ModelViewChange.InsideSurfaces | ModelViewChange.ViewMode;
        const ModelViewChange ColorChanges = ModelViewChange.Selection | ModelViewChange.SetColor | ModelViewChange.ObjectColor | ModelViewChange.SelectionColor | ModelViewChange.Transparency;

        /// <summary>
        /// Принимает пакет изменений контроллера и передаёт его в поток интерфейса.
        /// </summary>
        private void Project_Message(object sender, ProjectMessageEventArgs e)
        {
            if (IsDisposed || !ReferenceEquals(sender, project))
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => Project_Message(sender, e)));
                return;
            }

            // Изменения уже применены ядром: ошибка обновления интерфейса не должна выходить в вызывающую
            // команду (она сочтёт операцию неудачной) или в необработанное исключение потока UI
            try
            {
                ProcessProjectChanges(e.Changes);
            }
            catch (Exception ex)
            {
                console.PrintInfo(ex.Message, Color.Red);
                RequestRedraw();
            }
        }

        /// <summary>
        /// Применяет пакет изменений и один раз обновляет затронутые части интерфейса.
        /// </summary>
        private void ProcessProjectChanges(IReadOnlyList<ChangeMessage> changes)
        {
            var meshChanged = false;
            var groupsChanged = false;
            var conditionsChanged = false;
            var geometryChanged = false;
            var sceneChanged = false;

            foreach (var change in changes)
            {
                switch (change)
                {
                    case ProjectChange projectChange:
                        if (projectChange.ChangeKind != ProjectChange.ProjectChangeKind.PropertyChanged)
                        {
                            PresentReplacedProject();
                            return;
                        }
                        if (HandleProjectPropertyChange(projectChange))
                            conditionsChanged = true;
                        break;

                    case GeoChange:
                        geometryChanged = true;
                        sceneChanged = true;
                        break;

                    case MeshChange meshChange when meshChange.DataKind == MeshChange.MeshDataKind.Group:
                        NotifyGroupChange(meshChange);
                        groupsChanged = true;
                        conditionsChanged = true;
                        break;

                    case MeshChange meshChange:
                        ApplyMeshChange(meshChange);
                        meshChanged = true;
                        sceneChanged = true;
                        break;

                    case SetDelete setDelete:
                        ApplySetDelete(setDelete);
                        meshChanged = true;
                        sceneChanged = true;
                        break;

                    case SetRename setRename:
                        ApplySetRename(setRename);
                        meshChanged = true;
                        sceneChanged = true;
                        break;

                    case GroupRename groupRename:
                        NotifyGroupRename(groupRename);
                        groupsChanged = true;
                        conditionsChanged = true;
                        break;

                    case CondChange:
                        conditionsChanged = true;
                        break;

                    case ViewChange viewChange:
                        if (ApplyViewChange(viewChange))
                            sceneChanged = true;
                        break;
                }
            }

            if (geometryChanged)
                PresentGeoData();
            if (meshChanged)
            {
                PresentMeshData();
                PresentModelObjectsForSelection();
            }
            if (groupsChanged)
                PresentGroupDataOnTree();
            if (conditionsChanged)
                PresentCondDataOnTree();
            if (sceneChanged)
                RequestRedraw();
        }

        /// <summary>
        /// Полностью перестраивает представление после создания или загрузки проекта.
        /// </summary>
        private void PresentReplacedProject()
        {
            ResetProjectPresentation();

            if (project.HasProject)
            {
                PresentProject();
                UnblockInterface();
                FitObjectsToScreen();
            }
            else
                PresentModelObjectsForSelection();

            OnProjectLoaded?.Invoke();
            RequestRedraw();
        }

        /// <summary>
        /// Применяет изменение свойства проекта и возвращает признак обновления условий.
        /// </summary>
        private bool HandleProjectPropertyChange(ProjectChange change)
        {
            switch (change.Property)
            {
                case ProjectChange.ProjectProperty.Name:
                    if (string.IsNullOrWhiteSpace(project.FilePath))
                        return false;
                    var folder = System.IO.Path.GetDirectoryName(project.FilePath);
                    var projectPath = System.IO.Path.Combine(folder ?? string.Empty, project.Name);
                    lblStatus.Text = projectPath;
                    return false;

                case ProjectChange.ProjectProperty.FilePath:
                    lblStatus.Text = project.FilePath ?? string.Empty;
                    return false;

                case ProjectChange.ProjectProperty.MaterialsDB:
                    var materials = project.MaterialsDB?.Keys?.ToArray() ?? Array.Empty<string>();
                    var materialsArgs = new Args.ChangeMaterialsEventArgs(materials);
                    OnChangeMaterials?.Invoke(this, materialsArgs);
                    return false;

                case ProjectChange.ProjectProperty.FunctionsDB:
                    var functions = project.FunctionsDB?.Keys?.ToArray() ?? Array.Empty<string>();
                    var functionsArgs = new Args.ChangeFunctionsEventArgs(functions);
                    OnChangeFunctions?.Invoke(this, functionsArgs);
                    return false;

                case ProjectChange.ProjectProperty.TaskKind:
                case ProjectChange.ProjectProperty.TaskType:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Обновляет буферы наборов и возвращает признак изменения сцены.
        /// </summary>
        private bool ApplyViewChange(ViewChange change)
        {
            var sceneChanged = false;
            foreach (var setInfo in change.AffectedSets)
            {
                if (!IsCurrentModelSet(setInfo))
                    continue;
                var changeKind = change.GetChangeKind(setInfo);
                if ((changeKind & RebuildChanges) != 0)
                {
                    RefreshModelSetBuffer(setInfo);
                    sceneChanged = true;
                    continue;
                }
                if ((changeKind & ColorChanges) != 0)
                {
                    RecolorModelSetBuffer(setInfo);
                    sceneChanged = true;
                }
            }
            return sceneChanged;
        }

        /// <summary>
        /// Возвращает признак принадлежности набора текущей модели.
        /// </summary>
        private bool IsCurrentModelSet(ISetInfo setInfo)
        {
            foreach (var currentSet in project.GetModelSetsInfo(setInfo.ObjType))
                if (ReferenceEquals(currentSet, setInfo))
                    return true;
            return false;
        }

        /// <summary>
        /// Очищает сцену и навигатор прежнего проекта.
        /// </summary>
        private void ResetProjectPresentation()
        {
            ClearAllDataOnScene();
            if (navigator.TrySearchNodes(BazisGUI.Navigator.NodeName.Project, out var projectNodes))
                projectNodes.First().Nodes.Clear();
            lblStatus.Text = project.FilePath ?? string.Empty;
        }

        /// <summary>
        /// Применяет создание или обновление набора модели к VBO.
        /// </summary>
        private void ApplyMeshChange(MeshChange change)
        {
            var setInfo = project.GetModelSetsInfo(change.ObjectType).FirstOrDefault(item => item.Number == change.EntityId);
            if (setInfo == null)
                return;
            RefreshModelSetBuffer(setInfo);
        }

        /// <summary>
        /// Удаляет буфер удалённого набора.
        /// </summary>
        private void ApplySetDelete(SetDelete change)
        {
            VBOController.DeleteVBObjects(change.Name);
        }

        /// <summary>
        /// Применяет переименование набора к буферу сцены.
        /// </summary>
        private void ApplySetRename(SetRename change)
        {
            var oldBuffer = VBOController.FindVBObj(change.OldName);
            var drawingObject = oldBuffer?.ActiveDrawingObject;
            VBOController.DeleteVBObjects(change.OldName);
            var sets = project.GetModelSetsInfo(change.ObjectType);
            var setInfo = sets.FirstOrDefault(item => item.Number == change.EntityId);
            if (setInfo == null)
                return;

            RefreshModelSetBuffer(setInfo);
            var newBuffer = VBOController.FindVBObj(change.NewName);
            if (newBuffer != null && drawingObject != null)
                newBuffer.ActiveDrawingObject = drawingObject;
        }

        /// <summary>
        /// Передаёт переименование группы внутренним потребителям интерфейса.
        /// </summary>
        private void NotifyGroupRename(GroupRename change)
        {
            OnGroupRenamed?.Invoke(change.ObjectType, change.EntityId, change.NewName);
        }

        /// <summary>
        /// Передаёт изменение группы внутренним потребителям WinForms.
        /// </summary>
        private void NotifyGroupChange(MeshChange change)
        {
            if (change.Action == ChangeAction.Deleted)
            {
                OnGroupDeleted?.Invoke(change.ObjectType, change.EntityId);
                return;
            }
            if (change.Action != ChangeAction.Created)
                return;
            var group = project.GetAllModelGroups().FirstOrDefault(item => item.Number == change.EntityId);
            if (group != null)
                OnGroupCreated?.Invoke(group.ObjType, group.Number, group.Name);
        }

        /// <summary>
        /// Пересоздаёт буфер заданного набора с учётом состояния представления.
        /// </summary>
        private void RefreshModelSetBuffer(ISetInfo setInfo)
        {
            var oldBuffer = VBOController.FindVBObj(setInfo.Name);
            var drawingObject = oldBuffer?.ActiveDrawingObject;
            VBOController.DeleteVBObjects(setInfo.Name);
            if (setInfo.NumberOfObjects == 0)
                return;

            var presenter = project.CreateModelObjectsPresentor(setInfo);
            if (TryCreateVBObject(presenter, out var vbo))
            {
                if (drawingObject != null)
                    vbo.ActiveDrawingObject = drawingObject;
                else if (setInfo.ObjType == ObjType.Элемент3D && advanced3DClipper.ClipMode != ClipMode.None)
                    vbo.ActiveDrawingObject = advanced3DClipper;
                VBOController.AddVbo(vbo);
            }
        }

        /// <summary>
        /// Обновляет цвета буфера набора без пересборки геометрии.
        /// </summary>
        private void RecolorModelSetBuffer(ISetInfo setInfo)
        {
            var presenter = project.CreateModelObjectsPresentor(setInfo);
            SetVBObjectAttribute(presenter, "цвет");
        }

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

            project.SetSelectionColor(settingsConfig.SelectObjectColor);
            project.SetTransparency(GetModelViewTransparency());
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
