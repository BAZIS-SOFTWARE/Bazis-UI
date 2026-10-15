using System;
using System.Drawing;
using System.Data;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using PropertiesCalculator;
using MaterialDB.FunctionData;
using MaterialDB.MaterialData;
using BazisAvaloniaGUI.Localization;


namespace BazisAvaloniaGUI.Databases
{
    internal sealed class FunctionDataBasePage : DataBasePage//, ILocalizableHeaderControl
    {
        public event Action OnMutationEvent;
        public FunctionDataBasePage()
        {
            SaveEvent += SafeDBEventHandler;
            Loader = new LoadFunctionDataBaseFromTextFormat();
            Saver = new SaveFunctionDataBaseToTextFormat();
        }

        public string GetLocalizableHeaderText() => Resources.FunctionDataBasePage_headerName_text;

        public FunctionDBData Functions { get; set; }
        = new FunctionDBData() { Name = "newFuncDataBase.jsf" };

        public override void DataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {

        }

        public override void DelBrachButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Functions.Remove(TreeView.SelectedNode.Name.Split(',')[0]))
                {
                    //MessageBox.Show(this, "Данные удалены успешно");
                    RemoveNode(TreeView.SelectedNode);
                    OnMutationEvent?.Invoke();
                }

                else throw new Exception(Resources.DelBranchException);
            }
            catch (Exception ex) { MessageBox.Show(this, ex.Message); }

        }

        public override void AddNewRowButton_Click(object sender, EventArgs e)
        {
            if (TreeView.SelectedNode == null) return;
            // Исключение (нет таблицы/функции, иное число столбцов) в обработчике кнопки ронял приложение
            try
            {
                var fun = TreeView.SelectedNode.Name.Split(',')[0];
                var table = Functions[fun].DataTable ?? throw new Exception(Resources.PropertyTableIsMissing);
                var row = table.NewRow();
                for (var i = 0; i < table.Columns.Count; i++)
                    if (table.Columns[i].DataType != typeof(string)) row[i] = 0f;
                table.Rows.Add(row);
            }
            catch (Exception ex) { MessageBox.Show(this, ex.Message); }
        }

        public void AddTreeNode(Property function)
        {
            var matMenu = new ContextMenu();
            var name = $"{function.Name},{function.Units}";
            var funNode = new TreeNode(name) { Name = name };

            var renameFunItem = new MenuItem { Header = Resources.Rename };
            var deleteFunItem = new MenuItem { Header = Resources.Remove };
            renameFunItem.Click += RenameMatItem_Click;
            deleteFunItem.Click += DeleteMaterialItem_Click;
            matMenu.Items.Add(renameFunItem);
            matMenu.Items.Add(deleteFunItem);
            funNode.ContextMenu = matMenu;


            TreeView.Nodes.Add(funNode);
        }

        public override void Resort_Click(object sender, EventArgs e)
        {
            try
            {
                if (TreeView.SelectedNode != null)
                {
                    var dataAr = TreeView.SelectedNode.FullPath.Split('\\', ',');

                    var fun = dataAr[0];

                    var property = Functions[fun];
                    if (property.DataTable == null)
                        throw new Exception(Resources.PropertyTableIsMissing);

                    var dt = Resort(property.DataTable, "X", "ASC");
                    property.DataTable = dt;

                    DataGridView.DataSource = property.DataTable;

                    var header = property.Name;
                    var xUnit = property.X_unit;
                    var yUnit = property.Y_unit;

                    var grDataRange = SetGraphData(property.DataTable, header, Color.Orange, xUnit, yUnit);
                    if (grDataRange.Count != 0)
                        GraphContainer.CreateGraphData(header, grDataRange, new AxisFormat(), new AxisFormat());

                    //TreeView.SelectedNode = e.Node;
                }
            }
            catch (Exception ex) { MessageBox.Show(this, ex.Message); }
        }

        public override void DelAllRowsButton_Click(object sender, EventArgs e)
        {
            //TO DO реализовать метод очистки столбца
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="addFlag"></param>

        public void Load(string fileName, bool addFlag) => TryLoad(fileName, addFlag);

        private bool TryLoad(string fileName, bool addFlag)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new IOException(Resources.LoadDBCorruptedException);
                var ext = Path.GetExtension(fileName).ToLowerInvariant();
                DataExtension = ext;

                FunctionDBData functions = new FunctionDBData();

                switch (ext)
                {
                    case ".txt":
                        var dataSet = Loader.LoadDataBase(fileName);
                        functions = ConvertToFunctions(dataSet);
                        break;
                    case ".jsf":
                        var settingsSerializer = new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto,
                            Formatting = Formatting.Indented,
                        };
                        functions = JsonConvert.DeserializeObject<FunctionDBData>
                            (File.ReadAllText(fileName), settingsSerializer);
                        break;
                    default:
                        throw new InvalidDataException(Resources.LoadDBCorruptedException);
                }

                if (functions == null)
                    throw new InvalidDataException(Resources.LoadDBCorruptedException);

                foreach (var function in functions)
                    if (function.Value == null || string.IsNullOrEmpty(function.Value.Name) || function.Key != function.Value.Name)
                        throw new InvalidDataException(Resources.LoadDBCorruptedException);

                if (addFlag)
                {
                    if (Functions == null)
                        throw new Exception(Resources.LoadDBAddIntoMissingDBException);

                    foreach (var function in functions)
                    {
                        if (!Functions.ContainsKey(function.Key))
                            Functions.Add(function.Key, function.Value);
                    }
                }
                else
                {
                    Functions = functions;
                    var name = Path.GetFileName(fileName);
                    Functions.Name = name;
                }

                PresentFunctions();
                if (addFlag) OnMutationEvent?.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"{fileName}: {ex.Message}");
                return false;
            }
        }

        public void PresentFunctions()
        {
            TreeView.Nodes.Clear();
            DataGridView.DataSource = null;
            GraphContainer.ClearData();
            if (Functions == null) return;
            foreach (var function in Functions)
                AddTreeNode(function.Value);
        }

        private void RenameMatItem_Click(object sender, EventArgs e)
        {
            LabelEditFlag = true;
            // OnMutationEvent поднимается в TreeView_AfterLabelEdit — только после успешного переименования.
            BeginLabelEdit(TreeView.SelectedNode);
        }

        private void DeleteMaterialItem_Click(object sender, EventArgs e)
        {
            if (TreeView.SelectedNode == null)
            {
                MessageBox.Show(this, Resources.SelectAFunction);
                return;
            }

            Functions.Remove(TreeView.SelectedNode.Text.Split(',')[0]);
            RemoveNode(TreeView.SelectedNode);
            OnMutationEvent?.Invoke();
        }
/// <inheritdoc/>

        public void SafeDBEventHandler(string dbFullPath)
        {

                var settingsSerializer = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented
                };
                var propertyString = JsonConvert.SerializeObject(Functions, settingsSerializer);
                File.WriteAllText(dbFullPath, propertyString);
        }
/// <inheritdoc/>

        public override void AddBranchButton_Click(object sender, EventArgs e)
        {
            try
            {
                var number = TreeView.Nodes.Count;

                var dbPath = string.Empty;
                var name = string.Empty;
                dbPath = Directory.GetFiles(AppContext.BaseDirectory, "functions_draft.txt", SearchOption.AllDirectories)[0];
                name = GetNextName(Resources.New_function_, Functions.Keys);

                var dataSet = Loader.LoadDataBase(dbPath);
                var function = ConvertToFunctions(dataSet);

                var oldName = function.Last().Key;
                var values = function.Last().Value;

                values.Name = name;
                Functions.Add(name, values);

                AddTreeNode(values);
                OnMutationEvent?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"{Resources.AddBranch_ExceptionMessage} : {ex.Message}");
            }
        }
/// <inheritdoc/>

        public override void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                var funName = e.Node.Name.Split(',')[0];

                var table = Functions[funName].DataTable;
                if (table != null)
                {
                    DataGridView.DataSource = table;

                    var header = Functions[funName].Name;

                    var xUnit = Functions[funName].X_unit;
                    var yUnit = Functions[funName].Y_unit;
                    var units = Functions[funName].Units;

                    var grDataRange = SetGraphData(table, header, Color.Orange, xUnit, yUnit);
                    if (grDataRange.Count != 0)
                        GraphContainer.CreateGraphData(Functions[funName].Name, grDataRange, new AxisFormat(), new AxisFormat());
                    // Без точек график предыдущей функции оставался на экране под заголовком новой
                    else GraphContainer.ClearData();
                }
            }
            catch (Exception ex) { MessageBox.Show(this, ex.Message); }
        }

        public override void TreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            try
            {
                // || : при Esc Label == null, а ContainsKey(null) бросил бы ArgumentNullException.
                // Ключ словаря — имя без единиц ("Имя,K-s" → "Имя"): дубликат проверяется по нему,
                // и все проверки выполняются до изменения данных, чтобы ошибка не оставила словарь неполным
                var oldName = e.Node.Text.Split(',')[0];
                var newKey = e.Label?.Split(',')[0];
                if (string.IsNullOrEmpty(newKey) || (newKey != oldName && Functions.ContainsKey(newKey))
                    || !Functions.ContainsKey(oldName)
                    || e.Label.Split(',').Count() < 2 || e.Label.Split(',')[1].Split('-').Count() < 2)
                    e.CancelEdit = true;
                else
                {
                    var newName = e.Label;
                    var fun = Functions[oldName];
                    Functions.Remove(oldName);
                    Functions.Add(newKey, fun);
                    fun.Name = newKey;
                    e.Node.Name = newName;
                    OnMutationEvent?.Invoke();
                }
                LabelEditFlag = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        public override async void AddDB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var files = await FileStorage.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    AllowMultiple = false,
                    FileTypeFilter =
                    [
                        new FilePickerFileType("jsf files (*.jsf)") { Patterns = ["*.jsf"] },
                        new FilePickerFileType("txt files (*.txt)") { Patterns = ["*.txt"] },
                        FilePickerFileTypes.All
                    ]
                });

                if (files.Count == 0)
                    return;

                if (TryLoad(files[0].TryGetLocalPath() ?? throw new IOException($"{files[0].Path}: {Resources.LoadDBCorruptedException}"), true)) base.AddDB_Click(sender, e);
            }
            catch (Exception ex) { await MessageBox.Show(this, ex.Message); }
        }

        public override async void OpenFileDB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var files = await FileStorage.OpenFilePickerAsync(new FilePickerOpenOptions { AllowMultiple = false });

                if (files.Count == 0)
                    return;

                if (TryLoad(files[0].TryGetLocalPath() ?? throw new IOException($"{files[0].Path}: {Resources.LoadDBCorruptedException}"), false)) base.OpenFileDB_Click(sender, e);
            }
            catch (Exception ex) { await MessageBox.Show(this, ex.Message); }
        }

        private FunctionDBData ConvertToFunctions(DataSet dataSet)
        {
            var functions = new FunctionDBData();

            foreach (DataTable table in dataSet.Tables)
            {
                var tableAr = table.TableName.Split(',');
                if (!functions.ContainsKey(tableAr[0]))
                {
                    var unit = tableAr[1];
                    var propName = tableAr[0];
                    var yunit = unit.Split('-')[0];
                    var xunit = unit.Split('-').Count() == 2 ? unit.Split('-')[1] : unit.Split('-')[0];
                    var prop = new Property(propName, xunit, yunit, unit, table);

                    functions.Add(tableAr[0], prop);
                }
            }

            return functions;
        }
        /// <summary>
        /// CreateCopy_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void CreateCopy_Click(object sender, EventArgs e)
        {
            if (TreeView.SelectedNode != null && TreeView.SelectedNode.Level == 0)
            {
                var functionName = TreeView.SelectedNode.Name.Split(',')[0];
                var copyName = functionName + Resources.CopySuffics;
                if (Functions.ContainsKey(copyName))
                {
                    MessageBox.Show(this,
                        Resources.Function +
                        $" \"{copyName}\" " +
                        Resources.Function,
                        BazisAvaloniaGUI.Localization.Localization.GetAttentionCaption(),
                        MessageBoxButtons.OK);
                    return;
                }
                var newFunction = Functions[functionName].Copy(copyName);
                Functions.Add(newFunction.Name, newFunction);

                var newNod = (TreeNode)TreeView.SelectedNode.Clone();
                newNod.Name = newFunction.Name + "," + TreeView.SelectedNode.Name.Split(',')[1];
                newNod.Text = newFunction.Name + "," + TreeView.SelectedNode.Name.Split(',')[1];
                TreeView.Nodes.Add(newNod);
                OnMutationEvent?.Invoke();
            }
            else MessageBox.Show(this, Resources.SelectAFunction,
                BazisAvaloniaGUI.Localization.Localization.GetAttentionCaption(),
                MessageBoxButtons.OK);
        }


    }
}
