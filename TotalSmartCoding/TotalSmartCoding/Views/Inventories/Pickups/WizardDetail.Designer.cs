namespace TotalSmartCoding.Views.Inventories.Pickups
{
    partial class WizardDetail
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textexCode = new CustomControls.TextexBox();
            this.textexCommodityCode = new CustomControls.TextexBox();
            this.textexCommodityName = new CustomControls.TextexBox();
            this.textexQuantity = new CustomControls.TextexBox();
            this.label4 = new System.Windows.Forms.Label();
            this.errorProviderMaster = new System.Windows.Forms.ErrorProvider(this.components);
            this.fastBinLocations = new BrightIdeasSoftware.FastObjectListView();
            this.olvNo = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvCode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvQuantity = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.textexBinLocationFilters = new CustomControls.TextexBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.toolStrip5 = new System.Windows.Forms.ToolStrip();
            this.toolStrip6 = new System.Windows.Forms.ToolStrip();
            this.toolStrip7 = new System.Windows.Forms.ToolStrip();
            this.toolStrip8 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton16 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton14 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton15 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.buttonESC = new System.Windows.Forms.ToolStripButton();
            this.buttonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastBinLocations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip4.SuspendLayout();
            this.toolStrip5.SuspendLayout();
            this.toolStrip6.SuspendLayout();
            this.toolStrip7.SuspendLayout();
            this.toolStrip8.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonESC,
            this.buttonAdd});
            this.toolStrip1.Location = new System.Drawing.Point(0, 540);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip1.Size = new System.Drawing.Size(1405, 55);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(2, 22);
            this.label2.Margin = new System.Windows.Forms.Padding(2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(307, 40);
            this.label2.TabIndex = 103;
            this.label2.Text = "Barcode";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(2, 66);
            this.label10.Margin = new System.Windows.Forms.Padding(2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(307, 40);
            this.label10.TabIndex = 95;
            this.label10.Text = "Item Code";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(2, 110);
            this.label3.Margin = new System.Windows.Forms.Padding(2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(307, 40);
            this.label3.TabIndex = 100;
            this.label3.Text = "Description";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(1, 197);
            this.label9.Margin = new System.Windows.Forms.Padding(1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(309, 41);
            this.label9.TabIndex = 101;
            this.label9.Text = "Bin Location";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textexCode
            // 
            this.textexCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textexCode.Editable = true;
            this.textexCode.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textexCode.Location = new System.Drawing.Point(313, 21);
            this.textexCode.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.textexCode.Name = "textexCode";
            this.textexCode.ReadOnly = true;
            this.textexCode.Size = new System.Drawing.Size(696, 41);
            this.textexCode.TabIndex = 104;
            // 
            // textexCommodityCode
            // 
            this.textexCommodityCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textexCommodityCode.Editable = true;
            this.textexCommodityCode.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textexCommodityCode.Location = new System.Drawing.Point(313, 65);
            this.textexCommodityCode.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.textexCommodityCode.Name = "textexCommodityCode";
            this.textexCommodityCode.ReadOnly = true;
            this.textexCommodityCode.Size = new System.Drawing.Size(696, 41);
            this.textexCommodityCode.TabIndex = 105;
            // 
            // textexCommodityName
            // 
            this.textexCommodityName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textexCommodityName.Editable = true;
            this.textexCommodityName.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textexCommodityName.Location = new System.Drawing.Point(313, 109);
            this.textexCommodityName.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.textexCommodityName.Name = "textexCommodityName";
            this.textexCommodityName.ReadOnly = true;
            this.textexCommodityName.Size = new System.Drawing.Size(696, 41);
            this.textexCommodityName.TabIndex = 106;
            // 
            // textexQuantity
            // 
            this.textexQuantity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textexQuantity.Editable = true;
            this.textexQuantity.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textexQuantity.Location = new System.Drawing.Point(313, 153);
            this.textexQuantity.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.textexQuantity.Name = "textexQuantity";
            this.textexQuantity.ReadOnly = true;
            this.textexQuantity.Size = new System.Drawing.Size(696, 41);
            this.textexQuantity.TabIndex = 108;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(2, 154);
            this.label4.Margin = new System.Windows.Forms.Padding(2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(307, 40);
            this.label4.TabIndex = 107;
            this.label4.Text = "Quantity";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // errorProviderMaster
            // 
            this.errorProviderMaster.ContainerControl = this;
            // 
            // fastBinLocations
            // 
            this.fastBinLocations.AllColumns.Add(this.olvNo);
            this.fastBinLocations.AllColumns.Add(this.olvCode);
            this.fastBinLocations.AllColumns.Add(this.olvQuantity);
            this.fastBinLocations.AllColumns.Add(this.olvType);
            this.fastBinLocations.CheckedAspectName = "";
            this.fastBinLocations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvNo,
            this.olvCode,
            this.olvQuantity,
            this.olvType});
            this.fastBinLocations.Cursor = System.Windows.Forms.Cursors.Default;
            this.fastBinLocations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fastBinLocations.Font = new System.Drawing.Font("Sitka Banner", 16.2F);
            this.fastBinLocations.FullRowSelect = true;
            this.fastBinLocations.HideSelection = false;
            this.fastBinLocations.HighlightBackgroundColor = System.Drawing.SystemColors.Highlight;
            this.fastBinLocations.Location = new System.Drawing.Point(0, 0);
            this.fastBinLocations.Name = "fastBinLocations";
            this.fastBinLocations.OwnerDraw = true;
            this.fastBinLocations.ShowGroups = false;
            this.fastBinLocations.ShowImagesOnSubItems = true;
            this.fastBinLocations.Size = new System.Drawing.Size(1010, 287);
            this.fastBinLocations.TabIndex = 109;
            this.fastBinLocations.UnfocusedHighlightBackgroundColor = System.Drawing.SystemColors.Highlight;
            this.fastBinLocations.UseCompatibleStateImageBehavior = false;
            this.fastBinLocations.UseFiltering = true;
            this.fastBinLocations.View = System.Windows.Forms.View.Details;
            this.fastBinLocations.VirtualMode = true;
            // 
            // olvNo
            // 
            this.olvNo.Text = "";
            this.olvNo.Width = 15;
            // 
            // olvCode
            // 
            this.olvCode.AspectName = "Code";
            this.olvCode.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvCode.Text = "Code";
            this.olvCode.Width = 200;
            // 
            // olvQuantity
            // 
            this.olvQuantity.AspectName = "";
            this.olvQuantity.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvQuantity.Text = "No of Pallets";
            this.olvQuantity.Width = 160;
            // 
            // olvType
            // 
            this.olvType.AspectName = "";
            this.olvType.FillsFreeSpace = true;
            this.olvType.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvType.Text = "Type";
            this.olvType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvType.Width = 150;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip8);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip7);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip6);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip5);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip4);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer1.Size = new System.Drawing.Size(1405, 540);
            this.splitContainer1.SplitterDistance = 1010;
            this.splitContainer1.TabIndex = 110;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.fastBinLocations);
            this.splitContainer2.Size = new System.Drawing.Size(1010, 540);
            this.splitContainer2.SplitterDistance = 249;
            this.splitContainer2.TabIndex = 0;
            // 
            // textexBinLocationFilters
            // 
            this.textexBinLocationFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textexBinLocationFilters.Editable = true;
            this.textexBinLocationFilters.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textexBinLocationFilters.Location = new System.Drawing.Point(313, 197);
            this.textexBinLocationFilters.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.textexBinLocationFilters.Name = "textexBinLocationFilters";
            this.textexBinLocationFilters.Size = new System.Drawing.Size(696, 41);
            this.textexBinLocationFilters.TabIndex = 109;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.84455F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.15545F));
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.textexQuantity, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.textexBinLocationFilters, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textexCode, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textexCommodityCode, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textexCommodityName, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1010, 249);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripLabel1,
            this.toolStripButton5,
            this.toolStripButton6});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip2.Size = new System.Drawing.Size(391, 71);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStrip4
            // 
            this.toolStrip4.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton9});
            this.toolStrip4.Location = new System.Drawing.Point(0, 71);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip4.Size = new System.Drawing.Size(391, 71);
            this.toolStrip4.TabIndex = 3;
            this.toolStrip4.Text = "toolStrip4";
            // 
            // toolStrip5
            // 
            this.toolStrip5.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip5.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton11,
            this.toolStripButton12});
            this.toolStrip5.Location = new System.Drawing.Point(0, 142);
            this.toolStrip5.Name = "toolStrip5";
            this.toolStrip5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip5.Size = new System.Drawing.Size(391, 71);
            this.toolStrip5.TabIndex = 4;
            this.toolStrip5.Text = "toolStrip5";
            // 
            // toolStrip6
            // 
            this.toolStrip6.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip6.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton14,
            this.toolStripButton15});
            this.toolStrip6.Location = new System.Drawing.Point(0, 213);
            this.toolStrip6.Name = "toolStrip6";
            this.toolStrip6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip6.Size = new System.Drawing.Size(391, 71);
            this.toolStrip6.TabIndex = 5;
            this.toolStrip6.Text = "toolStrip6";
            // 
            // toolStrip7
            // 
            this.toolStrip7.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip7.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip7.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.toolStripButton10});
            this.toolStrip7.Location = new System.Drawing.Point(0, 284);
            this.toolStrip7.Name = "toolStrip7";
            this.toolStrip7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip7.Size = new System.Drawing.Size(391, 71);
            this.toolStrip7.TabIndex = 6;
            this.toolStrip7.Text = "toolStrip7";
            // 
            // toolStrip8
            // 
            this.toolStrip8.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip8.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip8.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton13,
            this.toolStripButton16});
            this.toolStrip8.Location = new System.Drawing.Point(0, 355);
            this.toolStrip8.Name = "toolStrip8";
            this.toolStrip8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip8.Size = new System.Drawing.Size(391, 71);
            this.toolStrip8.TabIndex = 7;
            this.toolStrip8.Text = "toolStrip8";
            // 
            // toolStripButton13
            // 
            this.toolStripButton13.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton13.Image = global::TotalSmartCoding.Properties.Resources.blank_key_64;
            this.toolStripButton13.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton13.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton13.Name = "toolStripButton13";
            this.toolStripButton13.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripButton13.Size = new System.Drawing.Size(68, 68);
            this.toolStripButton13.Text = "Add";
            // 
            // toolStripButton16
            // 
            this.toolStripButton16.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton16.Image = global::TotalSmartCoding.Properties.Resources.arrow_left_icon64;
            this.toolStripButton16.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton16.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton16.Name = "toolStripButton16";
            this.toolStripButton16.Size = new System.Drawing.Size(68, 68);
            this.toolStripButton16.Text = "toolStripButton3";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::TotalSmartCoding.Properties.Resources._9_icon64;
            this.toolStripButton2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripButton2.Size = new System.Drawing.Size(68, 68);
            this.toolStripButton2.Text = "Add";
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton10.Image = global::TotalSmartCoding.Properties.Resources._8_icon641;
            this.toolStripButton10.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Size = new System.Drawing.Size(68, 68);
            this.toolStripButton10.Text = "toolStripButton3";
            // 
            // toolStripButton14
            // 
            this.toolStripButton14.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton14.Image = global::TotalSmartCoding.Properties.Resources._7_icon64;
            this.toolStripButton14.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton14.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton14.Name = "toolStripButton14";
            this.toolStripButton14.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripButton14.Size = new System.Drawing.Size(68, 68);
            this.toolStripButton14.Text = "Add";
            // 
            // toolStripButton15
            // 
            this.toolStripButton15.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton15.Image = global::TotalSmartCoding.Properties.Resources._6_icon64;
            this.toolStripButton15.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton15.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton15.Name = "toolStripButton15";
            this.toolStripButton15.Size = new System.Drawing.Size(68, 68);
            this.toolStripButton15.Text = "toolStripButton3";
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton11.Image = global::TotalSmartCoding.Properties.Resources._5_icon64;
            this.toolStripButton11.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton11.Name = "toolStripButton11";
            this.toolStripButton11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripButton11.Size = new System.Drawing.Size(68, 68);
            this.toolStripButton11.Text = "Add";
            // 
            // toolStripButton12
            // 
            this.toolStripButton12.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton12.Image = global::TotalSmartCoding.Properties.Resources._4_icon64;
            this.toolStripButton12.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton12.Name = "toolStripButton12";
            this.toolStripButton12.Size = new System.Drawing.Size(68, 68);
            this.toolStripButton12.Text = "toolStripButton3";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::TotalSmartCoding.Properties.Resources._3_icon64;
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripButton1.Size = new System.Drawing.Size(68, 68);
            this.toolStripButton1.Text = "Add";
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton9.Image = global::TotalSmartCoding.Properties.Resources._2_icon64;
            this.toolStripButton9.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(68, 68);
            this.toolStripButton9.Text = "toolStripButton3";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::TotalSmartCoding.Properties.Resources._1_icon642;
            this.toolStripButton3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(68, 68);
            this.toolStripButton3.Text = "toolStripButton3";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::TotalSmartCoding.Properties.Resources._0_icon64;
            this.toolStripButton4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(68, 68);
            this.toolStripButton4.Text = "toolStripButton4";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::TotalSmartCoding.Properties.Resources.letter_b_icon;
            this.toolStripButton5.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(68, 68);
            this.toolStripButton5.Text = "toolStripButton5";
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::TotalSmartCoding.Properties.Resources.letter_a_icon;
            this.toolStripButton6.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(68, 68);
            this.toolStripButton6.Text = "toolStripButton6";
            // 
            // buttonESC
            // 
            this.buttonESC.Image = global::TotalSmartCoding.Properties.Resources.signout_icon_24;
            this.buttonESC.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.buttonESC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonESC.Name = "buttonESC";
            this.buttonESC.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonESC.Size = new System.Drawing.Size(73, 52);
            this.buttonESC.Text = "Close";
            this.buttonESC.Click += new System.EventHandler(this.buttonAddESC_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Image = global::TotalSmartCoding.Properties.Resources.Oxygen_Icons_org_Oxygen_Actions_go_next_view;
            this.buttonAdd.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.buttonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonAdd.Size = new System.Drawing.Size(89, 52);
            this.buttonAdd.Text = "Add";
            this.buttonAdd.Click += new System.EventHandler(this.buttonAddESC_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.AutoSize = false;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(35, 68);
            this.toolStripLabel1.Text = "  ";
            // 
            // WizardDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1405, 595);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WizardDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Please find a bin location for this pallet";
            this.Load += new System.EventHandler(this.WizardDetail_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastBinLocations)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            this.toolStrip5.ResumeLayout(false);
            this.toolStrip5.PerformLayout();
            this.toolStrip6.ResumeLayout(false);
            this.toolStrip6.PerformLayout();
            this.toolStrip7.ResumeLayout(false);
            this.toolStrip7.PerformLayout();
            this.toolStrip8.ResumeLayout(false);
            this.toolStrip8.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton buttonESC;
        private System.Windows.Forms.ToolStripButton buttonAdd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private CustomControls.TextexBox textexCode;
        private CustomControls.TextexBox textexCommodityCode;
        private CustomControls.TextexBox textexCommodityName;
        private CustomControls.TextexBox textexQuantity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider errorProviderMaster;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private CustomControls.TextexBox textexBinLocationFilters;
        private BrightIdeasSoftware.FastObjectListView fastBinLocations;
        private BrightIdeasSoftware.OLVColumn olvCode;
        private BrightIdeasSoftware.OLVColumn olvType;
        private BrightIdeasSoftware.OLVColumn olvQuantity;
        private BrightIdeasSoftware.OLVColumn olvNo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStrip toolStrip6;
        private System.Windows.Forms.ToolStripButton toolStripButton14;
        private System.Windows.Forms.ToolStripButton toolStripButton15;
        private System.Windows.Forms.ToolStrip toolStrip5;
        private System.Windows.Forms.ToolStripButton toolStripButton11;
        private System.Windows.Forms.ToolStripButton toolStripButton12;
        private System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStrip toolStrip7;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStrip toolStrip8;
        private System.Windows.Forms.ToolStripButton toolStripButton13;
        private System.Windows.Forms.ToolStripButton toolStripButton16;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}