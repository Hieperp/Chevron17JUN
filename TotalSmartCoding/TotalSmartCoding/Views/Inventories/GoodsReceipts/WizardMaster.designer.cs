namespace TotalSmartCoding.Views.Inventories.GoodsReceipts
{
    partial class WizardMaster
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.fastPendingPickups = new BrightIdeasSoftware.FastObjectListView();
            this.olvPickupEntryDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvPickupReference = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvWarehouseName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.fastPendingPickupWarehouses = new BrightIdeasSoftware.FastObjectListView();
            this.olvWarehouseID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvWarehouseName1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.panelMaster = new System.Windows.Forms.Panel();
            this.buttonESC = new System.Windows.Forms.ToolStripButton();
            this.buttonOK = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fastPendingPickups)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastPendingPickupWarehouses)).BeginInit();
            this.panelMaster.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonESC,
            this.buttonOK});
            this.toolStrip1.Location = new System.Drawing.Point(0, 548);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip1.Size = new System.Drawing.Size(1147, 55);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // fastPendingPickups
            // 
            this.fastPendingPickups.AllColumns.Add(this.olvPickupEntryDate);
            this.fastPendingPickups.AllColumns.Add(this.olvPickupReference);
            this.fastPendingPickups.AllColumns.Add(this.olvWarehouseName);
            this.fastPendingPickups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvPickupEntryDate,
            this.olvPickupReference,
            this.olvWarehouseName});
            this.fastPendingPickups.Cursor = System.Windows.Forms.Cursors.Default;
            this.fastPendingPickups.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fastPendingPickups.FullRowSelect = true;
            this.fastPendingPickups.HideSelection = false;
            this.fastPendingPickups.HighlightBackgroundColor = System.Drawing.SystemColors.Highlight;
            this.fastPendingPickups.Location = new System.Drawing.Point(-3, 313);
            this.fastPendingPickups.Name = "fastPendingPickups";
            this.fastPendingPickups.OwnerDraw = true;
            this.fastPendingPickups.ShowGroups = false;
            this.fastPendingPickups.Size = new System.Drawing.Size(1147, 232);
            this.fastPendingPickups.TabIndex = 69;
            this.fastPendingPickups.UnfocusedHighlightBackgroundColor = System.Drawing.SystemColors.Highlight;
            this.fastPendingPickups.UseCompatibleStateImageBehavior = false;
            this.fastPendingPickups.UseFiltering = true;
            this.fastPendingPickups.View = System.Windows.Forms.View.Details;
            this.fastPendingPickups.VirtualMode = true;
            // 
            // olvPickupEntryDate
            // 
            this.olvPickupEntryDate.AspectName = "PickupEntryDate";
            this.olvPickupEntryDate.Width = 170;
            // 
            // olvPickupReference
            // 
            this.olvPickupReference.AspectName = "PickupReference";
            this.olvPickupReference.Width = 137;
            // 
            // olvWarehouseName
            // 
            this.olvWarehouseName.AspectName = "WarehouseName";
            this.olvWarehouseName.Width = 192;
            // 
            // fastPendingPickupWarehouses
            // 
            this.fastPendingPickupWarehouses.AllColumns.Add(this.olvWarehouseID);
            this.fastPendingPickupWarehouses.AllColumns.Add(this.olvWarehouseName1);
            this.fastPendingPickupWarehouses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvWarehouseID,
            this.olvWarehouseName1});
            this.fastPendingPickupWarehouses.Cursor = System.Windows.Forms.Cursors.Default;
            this.fastPendingPickupWarehouses.Dock = System.Windows.Forms.DockStyle.Top;
            this.fastPendingPickupWarehouses.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fastPendingPickupWarehouses.FullRowSelect = true;
            this.fastPendingPickupWarehouses.HideSelection = false;
            this.fastPendingPickupWarehouses.HighlightBackgroundColor = System.Drawing.SystemColors.Highlight;
            this.fastPendingPickupWarehouses.Location = new System.Drawing.Point(0, 9);
            this.fastPendingPickupWarehouses.Name = "fastPendingPickupWarehouses";
            this.fastPendingPickupWarehouses.OwnerDraw = true;
            this.fastPendingPickupWarehouses.ShowGroups = false;
            this.fastPendingPickupWarehouses.Size = new System.Drawing.Size(1147, 447);
            this.fastPendingPickupWarehouses.TabIndex = 70;
            this.fastPendingPickupWarehouses.UnfocusedHighlightBackgroundColor = System.Drawing.SystemColors.Highlight;
            this.fastPendingPickupWarehouses.UseCompatibleStateImageBehavior = false;
            this.fastPendingPickupWarehouses.UseFiltering = true;
            this.fastPendingPickupWarehouses.View = System.Windows.Forms.View.Details;
            this.fastPendingPickupWarehouses.VirtualMode = true;
            // 
            // olvWarehouseID
            // 
            this.olvWarehouseID.AspectName = "WarehouseID";
            this.olvWarehouseID.Width = 161;
            // 
            // olvWarehouseName1
            // 
            this.olvWarehouseName1.AspectName = "WarehouseName";
            this.olvWarehouseName1.Width = 263;
            // 
            // panelMaster
            // 
            this.panelMaster.Controls.Add(this.fastPendingPickups);
            this.panelMaster.Controls.Add(this.fastPendingPickupWarehouses);
            this.panelMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMaster.Location = new System.Drawing.Point(0, 0);
            this.panelMaster.Name = "panelMaster";
            this.panelMaster.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.panelMaster.Size = new System.Drawing.Size(1147, 548);
            this.panelMaster.TabIndex = 71;
            // 
            // buttonESC
            // 
            this.buttonESC.Image = global::TotalSmartCoding.Properties.Resources.signout_icon_24;
            this.buttonESC.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.buttonESC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonESC.Name = "buttonESC";
            this.buttonESC.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonESC.Size = new System.Drawing.Size(81, 52);
            this.buttonESC.Text = "Cancel";
            this.buttonESC.Click += new System.EventHandler(this.buttonOKESC_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Image = global::TotalSmartCoding.Properties.Resources.Oxygen_Icons_org_Oxygen_Actions_go_next_view;
            this.buttonOK.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.buttonOK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonOK.Size = new System.Drawing.Size(92, 52);
            this.buttonOK.Text = "Next";
            this.buttonOK.Click += new System.EventHandler(this.buttonOKESC_Click);
            // 
            // WizardMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 603);
            this.Controls.Add(this.panelMaster);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WizardMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Wizard";
            this.Load += new System.EventHandler(this.Wizard_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fastPendingPickups)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastPendingPickupWarehouses)).EndInit();
            this.panelMaster.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton buttonESC;
        private System.Windows.Forms.ToolStripButton buttonOK;
        private BrightIdeasSoftware.FastObjectListView fastPendingPickups;
        private BrightIdeasSoftware.FastObjectListView fastPendingPickupWarehouses;
        private System.Windows.Forms.Panel panelMaster;
        private BrightIdeasSoftware.OLVColumn olvPickupEntryDate;
        private BrightIdeasSoftware.OLVColumn olvWarehouseName;
        private BrightIdeasSoftware.OLVColumn olvWarehouseID;
        private BrightIdeasSoftware.OLVColumn olvWarehouseName1;
        private BrightIdeasSoftware.OLVColumn olvPickupReference;
    }
}