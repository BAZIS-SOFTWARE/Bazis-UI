using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using PropertiesCalculator;
using UserControlsEx.Graph;
using BazisGUI.Win.DataBases.MechanicalGUI;
using BazisGUI.Win.DataBases.MetallurgyGUI;
using MaterialDB.MaterialData;
using BazisGUI.Win.Properties;
using BazisGUI.Win.Localization;

namespace BazisGUI.Win.DataBases
{
    public partial class MaterialsDataBasePage : DataBasePage//, ILocalizableHeaderControl
    {
        public event Action OnMutationEvent;

        public MaterialsDataBasePage()
        {
            InitializeComponent();
            Loader = new LoadMaterialDataBaseFromTextFormat();
            Saver = new SaveMaterialDataBaseToTextFormat();
        }

        public MaterialDBData Materials { get; set; }
        = new MaterialDBData() { Name = "newMatDataBase.jsf" };
/// <inheritdoc/>

        public void SafeDBEventHandler(string dbFullPath)
        {
                var settingsSerializer = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented
                };
                var propertyString = JsonConvert.SerializeObject(Materials, settingsSerializer);
                File.WriteAllText(dbFullPath, propertyString);         
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
/// <inheritdoc/>


        public override void OpenFileDB_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.DefaultExt = ".jsf";
            dialog.Filter = "jsf files (*.jsf)|*.jsf|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.FilterIndex = 0;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            Load(dialog.FileName, false);

            base.OpenFileDB_Click(sender, e);
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

                MaterialDBData materials = new MaterialDBData();

                switch (ext)
                {
                    case ".txt":
                        var dataSet = Loader.LoadDataBase(fileName);
                        materials = ConvertToMaterials(dataSet);
                        break;
                    case ".jsf":
                        var settingsSerializer = new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto,
                            Formatting = Formatting.Indented,
                        };
                        materials = JsonConvert.DeserializeObject<MaterialDBData>
                            (File.ReadAllText(fileName), settingsSerializer);
                        break;
                }

                if (addFlag)
                {
                    if (Materials == null)
                        throw new Exception(Resources.LoadDBAddIntoMissingDBException);

                    foreach (var material in materials)
                    {
                        if (!Materials.ContainsKey(material.Key))
                            Materials.Add(material.Key, material.Value);
                    }
                    OnMutationEvent?.Invoke();
                }
                else
                {
                    if (materials == null)
                        throw new Exception(Resources.LoadDBCorruptedException);
                    Materials = materials;
                    var name = Path.GetFileName(fileName);
                    Materials.Name = name;
                }

                PresentMaterials();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void PresentMaterials()
        {
            TreeView.Nodes.Clear();
            foreach (var material in Materials)
                AddTreeNode(material.Value);
        }

        public void AddTreeNode(MaterialDBItem material)
        {
                var categories = material.CategoryData;

                var matNode = new TreeNode(material.Name) { Name = material.Name };

                var matMenu = new ContextMenuStrip();

                var renameMatItem = new ToolStripMenuItem(Resources.Rename);
                renameMatItem.Click += RenameMatItem_Click;
                matMenu.Items.Add(renameMatItem);
                var deleteMatItem = new ToolStripMenuItem(Resources.Remove);
                deleteMatItem.Click += DeleteMaterialItem_Click;
                matMenu.Items.Add(deleteMatItem);

                matNode.ContextMenuStrip = matMenu;

            foreach (var category in categories.Values)
            {
                var catNode = new TreeNode(category.Name) { Name = category.Name };
                matNode.Nodes.Add(catNode);

                if (category.Name == "Металлургия")
                {
                    var catMenu = new ContextMenuStrip();
                    var addReacItem = new ToolStripMenuItem(Resources.AddReaction);
                    addReacItem.Click += AddReacItem_Click;
                    catMenu.Items.Add(addReacItem);
                    var diagramCalcItem = new ToolStripMenuItem(Resources.CalculateDiagram);
                    diagramCalcItem.Click += DiagramCalcItem_Click;
                    catMenu.Items.Add(diagramCalcItem);
                    catNode.ContextMenuStrip = catMenu;
                }
                else if (category.Name == "Механические свойства")
                {
                    var catMenu = new ContextMenuStrip();
                    var hardeningCalcItem = new ToolStripMenuItem(BazisGUI.Win.Properties.Resources.CalculateHardening);
                    hardeningCalcItem.Click += HardeningCalcItem_Click;
                    var creepCalcItem = new ToolStripMenuItem(BazisGUI.Win.Properties.Resources.CalculateCreep);
                    creepCalcItem.Click += CreepCalcItem_Click;
                    catMenu.Items.Add(hardeningCalcItem);
                    catMenu.Items.Add(creepCalcItem);
                    catNode.ContextMenuStrip = catMenu;
                }

                foreach (var prop in category.PropertyData.Values)
                {
                    var propNode = new TreeNode(prop.ToString()) { Name = prop.ToString() };

                    if (category.Name == "Металлургия")
                        propNode.ContextMenuStrip = CreateMetallurgyToolsStripMenu();

                    catNode.Nodes.Add(propNode);
                }
            }
            TreeView.Nodes.Add(matNode);
        }

        private void DiffCalcItem_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void CreepCalcItem_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void HardeningCalcItem_Click(object sender, EventArgs e)
        {
            if (TreeView.SelectedNode == null)
                return;
            var dataAr = TreeView.SelectedNode.FullPath.Split('\\', ',');

            var matName = dataAr[0];
            var generalProp = Materials[matName]["Общие сведения"].PropertyData;
            var mechProp = Materials[matName]["Механические свойства"].PropertyData;

            var hardCalc = new HardeningControl(mechProp, generalProp) { Dock = DockStyle.Fill };            
            var diagForm = new Form()
            {
                Name = "hardCalc",
                Text = BazisGUI.Win.Properties.Resources.HardeningCalculator,
                ShowIcon = false,
                Size = new Size(500, 500),
                TopMost = true
            };
            diagForm.Controls.Add(hardCalc);
            diagForm.Owner = ParentForm;
            diagForm.Show();

        }

        private void DiagramCalcItem_Click(object sender, EventArgs e)
        {
            if(TreeView.SelectedNode == null)
                return;
            var dataAr = TreeView.SelectedNode.FullPath.Split('\\', ',');

            var matName = dataAr[0];
            var metProps = Materials[matName]["Металлургия"].PropertyData;
            var genProp = Materials[matName]["Общие сведения"].PropertyData;
            var diagCalc = new DiagramControl(matName,metProps, genProp["Структура"].DataTable) { Dock = DockStyle.Fill };

            var diagForm = new Form() {
                Name = "diagCalc",
                Text = Resources.DiagramCalculator,
                ShowIcon = false,
                Size = new Size(500, 500),
                TopMost = true
            };
            diagForm.Controls.Add(diagCalc);
            diagForm.Owner = ParentForm;
            diagForm.Show();
        }

        private ContextMenuStrip CreateMetallurgyToolsStripMenu()
        {
            var menu = new ContextMenuStrip();
            var deleteReacItem = new ToolStripMenuItem(Resources.RemoveReaction);
            deleteReacItem.Click += DeleteReactionItem_Click;
            menu.Items.Add(deleteReacItem);
            var editMenuItem = new ToolStripMenuItem(BazisGUI.Win.Properties.Resources.Edit);
            editMenuItem.Click += EditMenuItem_Click;
            menu.Items.Add(editMenuItem);
            return menu;
        }

        private void EditMenuItem_Click(object sender, EventArgs e)
        {
            if(TreeView.SelectedNode == null)
            {
                MessageBox.Show(BazisGUI.Win.Properties.Resources.SelectReaction);
                return;
            }
                var dataAr = TreeView.SelectedNode.FullPath.Split('\\', ',');

                var mat = dataAr[0];
                var cat = dataAr[1];
                var reac = dataAr[2];

                var phaseTable = Materials[mat]["Общие сведения"]["Структура"].DataTable;

                var phaseNames = phaseTable.AsEnumerable().Select(r => r.Field<string>(0)).ToArray();

                var reaction = Materials[mat]["Металлургия"][reac];

                var reacControl = new ReactionControl(phaseNames, reaction) { Dock = DockStyle.Fill };

                reacControl.ChangeReactionName += (oldReacName, newReacName) =>
                {
                    if (Materials[mat][cat].PropertyData.ContainsKey(newReacName))
                    {
                        MessageBox.Show(Resources.ReactionWithASuchNameIsAlreadyExistChooseAnotherName);
                        return;
                    }

                    reaction.Name = newReacName;

                    TreeView.SelectedNode.Name = $"{newReacName},{reaction.Y_unit}-{reaction.X_unit}";
                    TreeView.SelectedNode.Text = $"{newReacName},{reaction.Y_unit}-{reaction.X_unit}";


                    Materials[mat][cat].PropertyData.Remove(oldReacName);
                    Materials[mat][cat].PropertyData.Add(newReacName, reaction);
                };

                var reacForm = new Form() 
                { 
                    Name = "editForm", 
                    Text = Resources.EditReaction, 
                    ShowIcon = false,
                    TopMost = true
                    
                };
                
                reacForm.Controls.Add(reacControl);
                reacForm.Owner = ParentForm;
                reacForm.ShowDialog();
        }

        private void DeleteMaterialItem_Click(object sender, EventArgs e)
        {
            if(TreeView.SelectedNode == null)
            {
                MessageBox.Show(Resources.SelectMaterial);
                return;
            }
            Materials.Remove(TreeView.SelectedNode.Text);
            TreeView.Nodes.Remove(TreeView.SelectedNode);
            OnMutationEvent?.Invoke();
        }

        public MaterialDBData ConvertToMaterials(DataSet dataSet)
        {
            var materials  = new MaterialDBData();

            foreach (DataTable table in dataSet.Tables)
            {
                var tableAr = table.TableName.Split(',');
                if (!materials.ContainsKey(tableAr[0]))
                {
                    var matItem = new MaterialDBItem(tableAr[0]);
                    matItem.CategoryData.Add("Общие сведения", new Category() { Name = "Общие сведения" });
                    matItem.CategoryData.Add("Тепловые свойства", new Category() { Name = "Тепловые свойства" });
                    matItem.CategoryData.Add("Механические свойства", new Category() { Name = "Механические свойства" });
                    matItem.CategoryData.Add("Металлургия", new Category() { Name = "Металлургия" });
                    materials.Add(tableAr[0], matItem);
                }
 
                var unit = tableAr[3];
                var propName = tableAr[2];
                var yunit = unit.Split('-')[0];
                var xunit = unit.Split('-').Count() == 2 ? unit.Split('-')[1] : unit.Split('-')[0];
                var prop = new Property(propName, xunit, yunit, unit, table);

                materials[tableAr[0]][tableAr[1]].PropertyData.Add(tableAr[2], prop);
            }
            return materials;
        }

        private void AddReacItem_Click(object sender, EventArgs e)
        {
            try
            {                
                var mat = TreeView.SelectedNode.Parent.Name;

                var phaseTable = Materials[mat]["Общие сведения"]["Структура"].DataTable;

                if (phaseTable.Rows.Count < 2)
                    throw new Exception(BazisGUI.Win.Properties.Resources.TheReactionRequiresAtLeastTwoPhases);

                var phaseNames = phaseTable.AsEnumerable().Select(r => r.Field<string>(0)).ToArray();

                var reacName = $"Реакция R1-R2";
                var reacTable = new DataTable();
                var tempCol = new DataColumn("Температура", typeof(float)) { DefaultValue = 0 };

                reacTable.Columns.Add(tempCol);
                var phaseCol = new DataColumn("Масс.Доли", typeof(float)) { DefaultValue = 0 };
                reacTable.Columns.Add(phaseCol);

                var reac = new Property(reacName, "°C", "Масс.Доли", "Масс.Доли-°C", reacTable);

                Materials[mat]["Металлургия"].PropertyData.Add(reacName, reac);

                var reacNodeName = $"{reacName},{reac.Y_unit}-{reac.X_unit}";
                var reacNode = new TreeNode(reacNodeName) { Name = reacNodeName };

                reacNode.ContextMenuStrip = CreateMetallurgyToolsStripMenu();
                TreeView.Nodes[mat].Nodes["Металлургия"].Nodes.Add(reacNode);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void DeleteReactionItem_Click(object sender, EventArgs e)
        {
            if (TreeView.SelectedNode == null)
            {
                MessageBox.Show(Resources.SelectAPropertyOrReactionToRemove);
                return;
            }

            var reacAr = TreeView.SelectedNode.FullPath.Split('\\', ',');

            Materials[reacAr[0]]["Металлургия"].PropertyData.Remove(reacAr[2]);
            TreeView.Nodes.Remove(TreeView.SelectedNode);
        }

        private void RenameMatItem_Click(object sender, EventArgs e)
        {
            try
            {
                LabelEditFlag = true;
                TreeView.SelectedNode.BeginEdit();
                OnMutationEvent?.Invoke();
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.SelectMaterial);
                LabelEditFlag = false;
            }

        }

        public override void Resort_Click(object sender, EventArgs e)
        {
            try
            {
                if (TreeView.SelectedNode != null & TreeView.SelectedNode.Level == 2)
                {
                    var dataAr = TreeView.SelectedNode.FullPath.Split('\\', ',');

                    var mat = dataAr[0];
                    var cat = dataAr[1];
                    var prop = dataAr[2];

                    var property = Materials[mat][cat][prop];
                    if (property.DataTable == null) 
                        throw new Exception(Resources.PropertyTableIsMissing);

                    var dt = Resort(property.DataTable, "Температура", "ASC");
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

        public override void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Parent != null & e.Node.Level == 2)
                {
                    //base.TreeView_AfterSelect(sender, e);

                    var dataAr = TreeView.SelectedNode.FullPath.Split('\\', ',');

                    var mat = dataAr[0];
                    var cat = dataAr[1];
                    var prop = dataAr[2];

                    var property = Materials[mat][cat][prop];
                    if (property.DataTable == null)
                        throw new Exception(Resources.PropertyTableIsMissing);

                    DataGridView.DataSource = property.DataTable;

                    foreach (DataGridViewColumn column in DataGridView.Columns)
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;

                    var xUnit = property.X_unit;
                    var yUnit = property.Y_unit;

                    var grDataRange = SetGraphData(property.DataTable, property.Name, Color.Orange, xUnit, yUnit);
                    if (grDataRange.Count != 0)
                        GraphContainer.CreateGraphData(property.Name, grDataRange,new AxisFormat(), new AxisFormat());

                    //TreeView.SelectedNode = e.Node;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public override void AddBranchButton_Click(object sender, EventArgs e)
        {
            try
            {
                var number = TreeView.Nodes.Count;

                var dbPath = string.Empty;
                var name = string.Empty;
                dbPath = Directory.GetFiles(Application.StartupPath, "materials_draft.txt", SearchOption.AllDirectories)[0];
                name = GetNextName(Resources.New_material_, Materials.Keys);

                var dataSet = Loader.LoadDataBase(dbPath);
                var material = ConvertToMaterials(dataSet);

                var lastItem = material.Last();
                var oldName = lastItem.Key;
                var values = lastItem.Value;

                Materials.Remove(oldName);
                values.Name = name;
                Materials.Add(name, values);

                AddTreeNode(values);
                OnMutationEvent?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Resources.AddBranch_ExceptionMessage} : {ex.Message}");
            }
        }


        /// <inheritdoc/>

        public override void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            base.DataGridView_CellEndEdit(sender, e);

            var grView = (DataGridView)sender;
            var sourceTable = (DataTable)grView.DataSource;

            var cell = grView[e.ColumnIndex, e.RowIndex];
            var colName = cell.OwningColumn.Name;

            if (colName == "Фаза" & TreeView.SelectedNode.Level == 2)
            {
                var dataAr = TreeView.SelectedNode.FullPath.Split('\\', ',');

                var mat = dataAr[0];
                var cat = dataAr[1];
                var prop = dataAr[2];

                var phaseNames = sourceTable.AsEnumerable().Select(r => r.Field<string>(0)).ToArray();

                var regex = new Regex(@"\W", RegexOptions.CultureInvariant);                
                if (regex.IsMatch(NewCellValue.ToString()) | NewCellValue.ToString().Count() == 0)
                {
                    MessageBox.Show(
                        BazisGUI.Win.Properties.Resources.ThePhaseNameCanOnlyConsistOfLettersNumbersAndTheUnderscore_,
                        Localization.Localization.GetAttentionCaption(),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    EditCell.Value = OldCellValue;
                    return;
                }

                var termTables = Materials[mat]["Тепловые свойства"].PropertyData.Values.Select(x => x.DataTable);
                UpdatePhaseColumns(termTables, phaseNames);
                var mechTables = Materials[mat]["Механические свойства"].PropertyData.Values.Select(x => x.DataTable);
                UpdatePhaseColumns(mechTables, phaseNames);

                var reactions = Materials[mat]["Металлургия"].PropertyData.Values.ToArray();

                foreach (var reaction in reactions)
                {
                    if (reaction.Name.Contains(OldCellValue.ToString()))
                    {
                        var newName = reaction.Name.Replace(OldCellValue.ToString(), NewCellValue.ToString());
                        Materials[mat]["Металлургия"].PropertyData.Remove(reaction.Name);
                        reaction.Name = newName;
                        Materials[mat]["Металлургия"].PropertyData.Add(newName, reaction);
                    }
                }
                var metallurgicalNode = TreeView.Nodes.Find(mat, true)[0].Nodes.Find("Металлургия", true)[0];

                foreach (TreeNode node in metallurgicalNode.Nodes)
                {
                    var name = node.Text.Replace(OldCellValue.ToString(), NewCellValue.ToString());
                    node.Text = name;
                    node.Name = name;
                }
 
            }
        }

        private void UpdatePhaseColumns(IEnumerable<DataTable> tables, string [] phaseNames)
        {
            foreach (var table in tables)
            {
                for (int i = 0; i < phaseNames.Length; i++)
                    table.Columns[i + 1].ColumnName = phaseNames[i];
            }
        }

        public override void TreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            try
            {
                if (e.Label == null | e.Label == "" | Materials.ContainsKey(e.Label))
                    e.CancelEdit = true;
                else
                {
                    var oldText = e.Node.Text;
                    var newName = e.Label;
                    e.Node.Name = newName;

                    var mat = Materials[oldText];
                    mat.Name = newName;

                    Materials.Remove(oldText);
                    Materials.Add(newName, mat);
                }
                LabelEditFlag = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        public override void DelBrachButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Materials.Remove(TreeView.SelectedNode.Name))
                {
                    MessageBox.Show(BazisGUI.Win.Properties.Resources.DataRemovedSuccessfully);
                    TreeView.Nodes.Remove(TreeView.SelectedNode);
                    OnMutationEvent?.Invoke();
                }

                else throw
                        new Exception(Resources.DelBranchException);
            }
            catch (Exception ex) { MessageBox.Show($"{Resources.DeletingError} : {ex.Message}"); }
        }

        public override void DelAllRowsButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (TreeView.SelectedNode.Level == 2)
                {
                    var dataAr = TreeView.SelectedNode.FullPath.Split('\\', ',');

                    var mat = dataAr[0];
                    var cat = dataAr[1];
                    var prop = dataAr[2];

                    var table = Materials[mat][cat][prop].DataTable;

                    if (prop == "Структура")
                    {
                        var phaseTable = Materials[mat]["Общие сведения"]["Структура"].DataTable;
                        var phaseNames = phaseTable.AsEnumerable().Select(r => r.Field<string>(0)).ToArray();

                        foreach (var phaseName in phaseNames)
                        {
                            var termTables = Materials[mat]["Тепловые свойства"].PropertyData.Values.Select(x => x.DataTable);
                            DelColumn(termTables, phaseName);
                            var mechTables = Materials[mat]["Механические свойства"].PropertyData.Values.Select(x => x.DataTable);
                            DelColumn(mechTables, phaseName);
                        }
                        Materials[mat]["Металлургия"].PropertyData.Clear();
                        TreeView.Nodes.Find(mat, true)[0].Nodes.Find("Металлургия", true)[0].Nodes.Clear();
                    }

                    table.Clear();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


            //var grView = (DataGridView)sender;
            //grView.DataSource = table;
        }

        public override void AddNewRowButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (TreeView.SelectedNode == null) return;
                var nodeName = TreeView.SelectedNode.Name;
                var dataAr = TreeView.SelectedNode.FullPath.Split('\\', ',');

                var mat = dataAr[0];
                var cat = dataAr[1];
                var prop = dataAr[2];
                var tableName = string.Join(",", dataAr);

                var table = Materials[mat][cat][prop].DataTable;
                var newRow = table.NewRow();

                if (prop == "Структура")
                {
                    var phaseName = $"newPhase{table.Rows.Count + 1}";
                    newRow[0] = phaseName;
                    newRow[1] = 0;

                    var termTables = Materials[mat]["Тепловые свойства"].PropertyData.Values.Select(x => x.DataTable);
                    AddNewColumn(termTables, phaseName, typeof(float));
                    var mechTables = Materials[mat]["Механические свойства"].PropertyData.Values.Select(x => x.DataTable);
                    AddNewColumn(mechTables, phaseName, typeof(float));
                }

                table.Rows.Add(newRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void AddNewColumn(IEnumerable<DataTable> tables, string colName, Type type)
        {
            foreach (var table in tables)
            {
                var column = new DataColumn(colName, type) { DefaultValue = 0 };

                table.Columns.Add(column);
            }
        }

        public override void DataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var sourceTable = (DataTable)e.Row.DataGridView.DataSource;
            
            if (TreeView.SelectedNode.Level == 2)
            {
                var dataAr = TreeView.SelectedNode.FullPath.Split('\\', ',');

                var matName = dataAr[0];
                var genProp = dataAr[1];
                var subProp = dataAr[2];

                if(subProp == "Структура")
                {
                    var phaseName = (string)e.Row.Cells[0].Value;
                    var termTables = Materials[matName]["Тепловые свойства"].PropertyData.Values.Select(x => x.DataTable);
                    DelColumn(termTables, phaseName);
                    var mechTables = Materials[matName]["Механические свойства"].PropertyData.Values.Select(x => x.DataTable);
                    DelColumn(mechTables, phaseName);

                    var reactions = Materials[matName]["Металлургия"].PropertyData.Values.ToArray();
                    var metallurgicalNode = TreeView.Nodes.Find(matName, true)[0].Nodes.Find("Металлургия", true)[0];
                    foreach (var reaction in reactions)
                    {
                        if (reaction.Name.Contains(phaseName))
                        {
                            Materials[matName]["Металлургия"].PropertyData.Remove(reaction.Name);
                            metallurgicalNode.Nodes.RemoveByKey(reaction.Name + "," + reaction.Units);
                        }
                    }
                }     
            }
        }

        private void DelColumn(IEnumerable<DataTable> tables, string name)
        {
            foreach (var table in tables)
            {
                table.Columns.Remove(name);
            }
        }

        public override void CreateCopy_Click(object sender, EventArgs e)
        {
            if (TreeView.SelectedNode != null && TreeView.SelectedNode.Level == 0)
            {
                var copyName = TreeView.SelectedNode.Name + Resources.CopySuffics;
                if (Materials.ContainsKey(copyName)) 
                {
                    MessageBox.Show(
                        Resources.Material +
                        " \"" + copyName + "\" " +
                        Resources.AlreadyExistsNRenameTheMaterial,
                        Localization.Localization.GetAttentionCaption(),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }                    

                var newMaterail = Materials[TreeView.SelectedNode.Name].Copy(copyName);                                               
                Materials.Add(copyName, newMaterail);               
               
                var newNod = (TreeNode)TreeView.SelectedNode.Clone();
                newNod.Name = copyName;
                newNod.Text = copyName;
                TreeView.Nodes.Add(newNod);
                OnMutationEvent?.Invoke();
            }
            else MessageBox.Show(
                Resources.SelectMaterial,
                Localization.Localization.GetAttentionCaption(),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public override void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Выбрать узел при нажатии правой кнопкой
                TreeView.SelectedNode = TreeView.GetNodeAt(e.X, e.Y);
            }
        }

    }
}