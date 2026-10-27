using System;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using UserControlsEx.Graph;
using PropertiesCalculator;
using MaterialDB.FunctionData;
using MaterialDB.MaterialData;
using BazisGUI.Win.Properties;
using BazisGUI.Win.Localization;

namespace BazisGUI.Win.DataBases
{
    public partial class FunctionDataBasePage : DataBasePage//, ILocalizableHeaderControl
    {
        public event Action OnMutationEvent;
        public FunctionDataBasePage()
        {
            InitializeComponent();
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
                    //MessageBox.Show("Данные удалены успешно");
                    TreeView.Nodes.Remove(TreeView.SelectedNode);
                    OnMutationEvent?.Invoke();
                }

                else throw new Exception(Resources.DelBranchException);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        public override void AddNewRowButton_Click(object sender, EventArgs e)
        {
            if (TreeView.SelectedNode == null) return;
            var fun = TreeView.SelectedNode.Name.Split(',')[0];
            float number = 0;
            Functions[fun].DataTable.Rows.Add(number, number);
        }

        public void AddTreeNode(Property function)
        {
            var matMenu = new ContextMenuStrip();
            var name = $"{function.Name},{function.Units}";
            var funNode = new TreeNode(name) { Name = name };

            var renameFunItem = new ToolStripMenuItem(Resources.Rename);
            var deleteFunItem = new ToolStripMenuItem(Resources.Remove);
            renameFunItem.Click += RenameMatItem_Click;
            deleteFunItem.Click += DeleteMaterialItem_Click;
            matMenu.Items.Add(renameFunItem);
            matMenu.Items.Add(deleteFunItem);
            funNode.ContextMenuStrip = matMenu;


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
                        throw new Exception(BazisGUI.Win.Properties.Resources.PropertyTableIsMissing);

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
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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

        public void Load(string fileName, bool addFlag)
        {
            try
            {
                var ext = Path.GetExtension(fileName);
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
                }

                if (addFlag)
                {
                    if (Functions == null)
                        throw new Exception(Resources.LoadDBAddIntoMissingDBException);

                    foreach (var function in functions)
                    {
                        if (!Functions.ContainsKey(function.Key))
                            Functions.Add(function.Key, function.Value);
                    }
                    OnMutationEvent?.Invoke();
                }
                else
                {
                    if (functions == null)
                        throw new Exception(Resources.LoadDBCorruptedException);
                    Functions = functions;
                    var name = Path.GetFileName(fileName);
                    Functions.Name = name;
                }

                PresentFunctions();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void PresentFunctions()
        {
            TreeView.Nodes.Clear();
            foreach (var function in Functions)
                AddTreeNode(function.Value);
        }

        private void RenameMatItem_Click(object sender, EventArgs e)
        {
            LabelEditFlag = true;
            TreeView.SelectedNode.BeginEdit();
            OnMutationEvent?.Invoke();
        }

        private void DeleteMaterialItem_Click(object sender, EventArgs e)
        {
            if (TreeView.SelectedNode == null)
            {
                MessageBox.Show(Resources.SelectAFunction);
                return;
            }

            Functions.Remove(TreeView.SelectedNode.Text.Split(',')[0]);
            TreeView.Nodes.Remove(TreeView.SelectedNode);
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
                dbPath = Directory.GetFiles(Application.StartupPath, "functions_draft.txt", SearchOption.AllDirectories)[0];
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
                MessageBox.Show($"{Resources.AddBranch_ExceptionMessage} : {ex.Message}");
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
                    foreach (DataGridViewColumn column in DataGridView.Columns)
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;

                    var header = Functions[funName].Name;

                    var xUnit = Functions[funName].X_unit;
                    var yUnit = Functions[funName].Y_unit;
                    var units = Functions[funName].Units;

                    var grDataRange = SetGraphData(table, header, Color.Orange, xUnit, yUnit);
                    if (grDataRange.Count != 0)
                        GraphContainer.CreateGraphData(Functions[funName].Name, grDataRange, new AxisFormat(), new AxisFormat());
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public override void TreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            try
            {
                if (e.Label == null | e.Label == "" | Functions.ContainsKey(e.Label) 
                    | e.Label.Split(',').Count() < 2 || e.Label.Split(',')[1].Split('-').Count() < 2)
                    e.CancelEdit = true;
                else
                {
                    var oldName = e.Node.Text.Split(',')[0];
                    var newName = e.Label;
                    e.Node.Name = newName;
                    var fun = Functions[oldName];
                    fun.Name = newName.Split(',')[0];

                    Functions.Remove(oldName);
                    Functions.Add(newName.Split(',')[0], fun);
                }
                LabelEditFlag = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public override void AddDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.DefaultExt = ".jsf";
            dialog.Filter = "jsf files (*.jsf)|*.jsf|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.FilterIndex = 0;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            Load(dialog.FileName, true);

            base.AddDB_Click(sender, e);
        }

        public override void OpenFileDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.DefaultExt = ".jsf";

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            Load(dialog.FileName, false);

            base.OpenFileDB_Click(sender, e);
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
                    MessageBox.Show(
                        Resources.Function +
                        $" \"{copyName}\" " +
                        Resources.Function,
                        Localization.Localization.GetAttentionCaption(),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            else MessageBox.Show(Resources.SelectAFunction,
                Localization.Localization.GetAttentionCaption(),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public override void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            
        }
    }
}
