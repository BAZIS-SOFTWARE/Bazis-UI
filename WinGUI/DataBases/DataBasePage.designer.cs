using WinGUI.Properties;
using System.Windows.Forms;
using UserControlsEx;
using UserControlsEx.Graph;

namespace WinGUI.DataBases
{
    partial class DataBasePage
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataBasePage));
            toolStripContainer2 = new ToolStripContainer();
            treeView = new TreeView();
            toolStrip2 = new ToolStripEx();
            btnOpenDB = new ToolStripButton();
            btnAddDB = new ToolStripButton();
            btnSafeFile = new ToolStripButton();
            addBranchButton = new ToolStripButton();
            delBrachButton = new ToolStripButton();
            btnCreateCopy = new ToolStripButton();
            toolStripContainer1 = new ToolStripContainer();
            dataGridView = new DataGridViewEx(components);
            dgrToolStrip = new ToolStripEx();
            btnAddNewRow = new ToolStripButton();
            btnDelRow = new ToolStripButton();
            btnAscSort = new ToolStripButton();
            tableLayoutPanel = new TableLayoutPanel();
            treePanel = new Panel();
            graphPanel = new Panel();
            graphContainer = new GraphContainer();
            dataPanel = new Panel();
            menuLev0 = new ContextMenuStrip(components);
            itmRename = new ToolStripMenuItem();
            itmDelete = new ToolStripMenuItem();
            toolStripContainer2.ContentPanel.SuspendLayout();
            toolStripContainer2.TopToolStripPanel.SuspendLayout();
            toolStripContainer2.SuspendLayout();
            toolStrip2.SuspendLayout();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.TopToolStripPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            dgrToolStrip.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            treePanel.SuspendLayout();
            graphPanel.SuspendLayout();
            dataPanel.SuspendLayout();
            menuLev0.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripContainer2
            // 
            resources.ApplyResources(toolStripContainer2, "toolStripContainer2");
            // 
            // toolStripContainer2.BottomToolStripPanel
            // 
            resources.ApplyResources(toolStripContainer2.BottomToolStripPanel, "toolStripContainer2.BottomToolStripPanel");
            toolStripContainer2.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer2.ContentPanel
            // 
            resources.ApplyResources(toolStripContainer2.ContentPanel, "toolStripContainer2.ContentPanel");
            toolStripContainer2.ContentPanel.BackColor = System.Drawing.Color.Silver;
            toolStripContainer2.ContentPanel.Controls.Add(treeView);
            // 
            // toolStripContainer2.LeftToolStripPanel
            // 
            resources.ApplyResources(toolStripContainer2.LeftToolStripPanel, "toolStripContainer2.LeftToolStripPanel");
            toolStripContainer2.LeftToolStripPanelVisible = false;
            toolStripContainer2.Name = "toolStripContainer2";
            // 
            // toolStripContainer2.RightToolStripPanel
            // 
            resources.ApplyResources(toolStripContainer2.RightToolStripPanel, "toolStripContainer2.RightToolStripPanel");
            toolStripContainer2.RightToolStripPanelVisible = false;
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            resources.ApplyResources(toolStripContainer2.TopToolStripPanel, "toolStripContainer2.TopToolStripPanel");
            toolStripContainer2.TopToolStripPanel.BackColor = System.Drawing.Color.Gainsboro;
            toolStripContainer2.TopToolStripPanel.Controls.Add(toolStrip2);
            // 
            // treeView
            // 
            resources.ApplyResources(treeView, "treeView");
            treeView.BorderStyle = BorderStyle.None;
            treeView.LabelEdit = true;
            treeView.LineColor = System.Drawing.Color.Orange;
            treeView.Name = "treeView";
            treeView.BeforeLabelEdit += TreeView_BeforeLabelEdit;
            treeView.AfterLabelEdit += TreeView_AfterLabelEdit;
            treeView.AfterSelect += TreeView_AfterSelect;
            treeView.MouseDown += treeView_MouseDown;
            // 
            // toolStrip2
            // 
            resources.ApplyResources(toolStrip2, "toolStrip2");
            toolStrip2.BackColor = System.Drawing.Color.Transparent;
            toolStrip2.BackGroundColor = System.Drawing.Color.Gainsboro;
            toolStrip2.FrameColor = System.Drawing.Color.Gray;
            toolStrip2.GeneralFrame = false;
            toolStrip2.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip2.IconLocation = new System.Drawing.Point(1, 5);
            toolStrip2.ImageRectangleSize = new System.Drawing.Point(16, 16);
            toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip2.ItemBackGroundColor = System.Drawing.Color.White;
            toolStrip2.ItemFrame = true;
            toolStrip2.ItemLocation = new System.Drawing.Point(3, 3);
            toolStrip2.ItemPressColor = System.Drawing.Color.Black;
            toolStrip2.Items.AddRange(new ToolStripItem[] { btnOpenDB, btnAddDB, btnSafeFile, addBranchButton, delBrachButton, btnCreateCopy });
            toolStrip2.ItemSelectColor = System.Drawing.Color.Gray;
            toolStrip2.Name = "toolStrip2";
            toolStrip2.SplitButtonClickWidth = 16;
            toolStrip2.SplitButtonHeight = 34;
            toolStrip2.SplitButtonTriangleSize = 6;
            toolStrip2.Stretch = true;
            toolStrip2.TextBoxFrame = false;
            toolStrip2.TextBoxHeight = 0;
            // 
            // btnOpenDB
            // 
            resources.ApplyResources(btnOpenDB, "btnOpenDB");
            btnOpenDB.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnOpenDB.Name = "btnOpenDB";
            btnOpenDB.Click += OpenFileDB_Click;
            // 
            // btnAddDB
            // 
            resources.ApplyResources(btnAddDB, "btnAddDB");
            btnAddDB.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddDB.Name = "btnAddDB";
            btnAddDB.Click += AddDB_Click;
            // 
            // btnSafeFile
            // 
            resources.ApplyResources(btnSafeFile, "btnSafeFile");
            btnSafeFile.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnSafeFile.Name = "btnSafeFile";
            btnSafeFile.Click += SafeFileButton_Click;
            // 
            // addBranchButton
            // 
            resources.ApplyResources(addBranchButton, "addBranchButton");
            addBranchButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            addBranchButton.Name = "addBranchButton";
            addBranchButton.Click += AddBranchButton_Click;
            // 
            // delBrachButton
            // 
            resources.ApplyResources(delBrachButton, "delBrachButton");
            delBrachButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            delBrachButton.Name = "delBrachButton";
            delBrachButton.Click += DelBrachButton_Click;
            // 
            // btnCreateCopy
            // 
            resources.ApplyResources(btnCreateCopy, "btnCreateCopy");
            btnCreateCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnCreateCopy.Name = "btnCreateCopy";
            btnCreateCopy.Click += CreateCopy_Click;
            // 
            // toolStripContainer1
            // 
            resources.ApplyResources(toolStripContainer1, "toolStripContainer1");
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            resources.ApplyResources(toolStripContainer1.BottomToolStripPanel, "toolStripContainer1.BottomToolStripPanel");
            toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            resources.ApplyResources(toolStripContainer1.ContentPanel, "toolStripContainer1.ContentPanel");
            toolStripContainer1.ContentPanel.Controls.Add(dataGridView);
            // 
            // toolStripContainer1.LeftToolStripPanel
            // 
            resources.ApplyResources(toolStripContainer1.LeftToolStripPanel, "toolStripContainer1.LeftToolStripPanel");
            toolStripContainer1.LeftToolStripPanelVisible = false;
            toolStripContainer1.Name = "toolStripContainer1";
            // 
            // toolStripContainer1.RightToolStripPanel
            // 
            resources.ApplyResources(toolStripContainer1.RightToolStripPanel, "toolStripContainer1.RightToolStripPanel");
            toolStripContainer1.RightToolStripPanelVisible = false;
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            resources.ApplyResources(toolStripContainer1.TopToolStripPanel, "toolStripContainer1.TopToolStripPanel");
            toolStripContainer1.TopToolStripPanel.BackColor = System.Drawing.Color.Gainsboro;
            toolStripContainer1.TopToolStripPanel.Controls.Add(dgrToolStrip);
            // 
            // dataGridView
            // 
            resources.ApplyResources(dataGridView, "dataGridView");
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToOrderColumns = true;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Name = "dataGridView";
            dataGridView.CellBeginEdit += dataGridView_CellBeginEdit;
            dataGridView.CellEndEdit += DataGridView_CellEndEdit;
            dataGridView.UserDeletingRow += DataGridView_UserDeletingRow;
            // 
            // dgrToolStrip
            // 
            resources.ApplyResources(dgrToolStrip, "dgrToolStrip");
            dgrToolStrip.BackColor = System.Drawing.Color.Transparent;
            dgrToolStrip.BackGroundColor = System.Drawing.Color.Gainsboro;
            dgrToolStrip.FrameColor = System.Drawing.Color.Gray;
            dgrToolStrip.GeneralFrame = false;
            dgrToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            dgrToolStrip.IconLocation = new System.Drawing.Point(1, 5);
            dgrToolStrip.ImageRectangleSize = new System.Drawing.Point(16, 16);
            dgrToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            dgrToolStrip.ItemBackGroundColor = System.Drawing.Color.White;
            dgrToolStrip.ItemFrame = true;
            dgrToolStrip.ItemLocation = new System.Drawing.Point(3, 3);
            dgrToolStrip.ItemPressColor = System.Drawing.Color.Black;
            dgrToolStrip.Items.AddRange(new ToolStripItem[] { btnAddNewRow, btnDelRow, btnAscSort });
            dgrToolStrip.ItemSelectColor = System.Drawing.Color.Gray;
            dgrToolStrip.Name = "dgrToolStrip";
            dgrToolStrip.SplitButtonClickWidth = 16;
            dgrToolStrip.SplitButtonHeight = 34;
            dgrToolStrip.SplitButtonTriangleSize = 6;
            dgrToolStrip.Stretch = true;
            dgrToolStrip.TextBoxFrame = false;
            dgrToolStrip.TextBoxHeight = 16;
            // 
            // btnAddNewRow
            // 
            resources.ApplyResources(btnAddNewRow, "btnAddNewRow");
            btnAddNewRow.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddNewRow.Name = "btnAddNewRow";
            btnAddNewRow.Click += AddNewRowButton_Click;
            // 
            // btnDelRow
            // 
            resources.ApplyResources(btnDelRow, "btnDelRow");
            btnDelRow.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnDelRow.Name = "btnDelRow";
            btnDelRow.Click += DelAllRowsButton_Click;
            // 
            // btnAscSort
            // 
            resources.ApplyResources(btnAscSort, "btnAscSort");
            btnAscSort.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAscSort.Name = "btnAscSort";
            btnAscSort.Click += Resort_Click;
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(tableLayoutPanel, "tableLayoutPanel");
            tableLayoutPanel.Controls.Add(treePanel, 0, 0);
            tableLayoutPanel.Controls.Add(graphPanel, 1, 1);
            tableLayoutPanel.Controls.Add(dataPanel, 1, 0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // treePanel
            // 
            resources.ApplyResources(treePanel, "treePanel");
            treePanel.BackColor = System.Drawing.Color.Silver;
            treePanel.BorderStyle = BorderStyle.FixedSingle;
            treePanel.Controls.Add(toolStripContainer2);
            treePanel.Name = "treePanel";
            tableLayoutPanel.SetRowSpan(treePanel, 2);
            treePanel.Paint += treePanel_Paint;
            // 
            // graphPanel
            // 
            resources.ApplyResources(graphPanel, "graphPanel");
            graphPanel.BackColor = System.Drawing.Color.Silver;
            graphPanel.BorderStyle = BorderStyle.FixedSingle;
            graphPanel.Controls.Add(graphContainer);
            graphPanel.Name = "graphPanel";
            graphPanel.Paint += graphPanel_Paint;
            // 
            // graphContainer
            // 
            resources.ApplyResources(graphContainer, "graphContainer");
            graphContainer.Name = "graphContainer";
            // 
            // dataPanel
            // 
            resources.ApplyResources(dataPanel, "dataPanel");
            dataPanel.BackColor = System.Drawing.Color.Silver;
            dataPanel.BorderStyle = BorderStyle.FixedSingle;
            dataPanel.Controls.Add(toolStripContainer1);
            dataPanel.Name = "dataPanel";
            dataPanel.Paint += dataPanel_Paint;
            // 
            // menuLev0
            // 
            resources.ApplyResources(menuLev0, "menuLev0");
            menuLev0.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuLev0.Items.AddRange(new ToolStripItem[] { itmRename, itmDelete });
            menuLev0.Name = "menuLev0";
            // 
            // itmRename
            // 
            resources.ApplyResources(itmRename, "itmRename");
            itmRename.Name = "itmRename";
            // 
            // itmDelete
            // 
            resources.ApplyResources(itmDelete, "itmDelete");
            itmDelete.Name = "itmDelete";
            // 
            // DataBasePage
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel);
            Name = "DataBasePage";
            toolStripContainer2.ContentPanel.ResumeLayout(false);
            toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer2.TopToolStripPanel.PerformLayout();
            toolStripContainer2.ResumeLayout(false);
            toolStripContainer2.PerformLayout();
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            toolStripContainer1.ContentPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.PerformLayout();
            toolStripContainer1.ResumeLayout(false);
            toolStripContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            dgrToolStrip.ResumeLayout(false);
            dgrToolStrip.PerformLayout();
            tableLayoutPanel.ResumeLayout(false);
            treePanel.ResumeLayout(false);
            graphPanel.ResumeLayout(false);
            dataPanel.ResumeLayout(false);
            menuLev0.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TreeView treeView;
        private DataGridViewEx dataGridView;
        private System.Windows.Forms.Panel treePanel;
        private System.Windows.Forms.Panel graphPanel;
        private System.Windows.Forms.Panel dataPanel;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private ToolStripEx toolStrip2;
        private System.Windows.Forms.ToolStripButton btnOpenDB;
        private System.Windows.Forms.ToolStripButton btnSafeFile;
        private System.Windows.Forms.ToolStripButton addBranchButton;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private ToolStripEx dgrToolStrip;
        private System.Windows.Forms.ToolStripButton btnAddNewRow;
        private System.Windows.Forms.ToolStripButton btnDelRow;
        private System.Windows.Forms.ToolStripButton delBrachButton;

        private ContextMenuStrip menuLev0;
        private ToolStripMenuItem itmRename;
        private ToolStripMenuItem itmDelete;
        private ToolStripButton btnAscSort;
        private ToolStripButton btnAddDB;
        private ToolStripButton btnCreateCopy;
        private GraphContainer graphContainer;
    }
}
