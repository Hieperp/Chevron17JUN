namespace TotalSmartCoding.Views.Inventories.Pickups
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
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.buttonESC = new System.Windows.Forms.ToolStripButton();
            this.buttonOK = new System.Windows.Forms.ToolStripButton();
            this.combexWarehouseID = new CustomControls.CombexBox();
            this.combexForkliftDriverID = new CustomControls.CombexBox();
            this.combexStorekeeperID = new CustomControls.CombexBox();
            this.textexRemarks = new CustomControls.TextexBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.combexFillingLineID = new CustomControls.CombexBox();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProviderMaster = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderMaster)).BeginInit();
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
            this.toolStrip1.Location = new System.Drawing.Point(0, 305);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip1.Size = new System.Drawing.Size(856, 55);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // buttonESC
            // 
            this.buttonESC.Font = new System.Drawing.Font("Niagara Engraved", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonESC.Image = global::TotalSmartCoding.Properties.Resources.signout_icon_24;
            this.buttonESC.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.buttonESC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonESC.Name = "buttonESC";
            this.buttonESC.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonESC.Size = new System.Drawing.Size(85, 52);
            this.buttonESC.Text = "Cancel";
            this.buttonESC.Click += new System.EventHandler(this.buttonOKESC_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Enabled = false;
            this.buttonOK.Font = new System.Drawing.Font("Niagara Engraved", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.Image = global::TotalSmartCoding.Properties.Resources.Oxygen_Icons_org_Oxygen_Actions_go_next_view;
            this.buttonOK.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.buttonOK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonOK.Size = new System.Drawing.Size(93, 52);
            this.buttonOK.Text = "Next";
            this.buttonOK.Click += new System.EventHandler(this.buttonOKESC_Click);
            // 
            // combexWarehouseID
            // 
            this.combexWarehouseID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.combexWarehouseID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.combexWarehouseID.Editable = true;
            this.combexWarehouseID.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combexWarehouseID.FormattingEnabled = true;
            this.combexWarehouseID.Location = new System.Drawing.Point(316, 87);
            this.combexWarehouseID.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.combexWarehouseID.Name = "combexWarehouseID";
            this.combexWarehouseID.ReadOnly = false;
            this.combexWarehouseID.Size = new System.Drawing.Size(509, 47);
            this.combexWarehouseID.TabIndex = 86;
            // 
            // combexForkliftDriverID
            // 
            this.combexForkliftDriverID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.combexForkliftDriverID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.combexForkliftDriverID.Editable = true;
            this.combexForkliftDriverID.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combexForkliftDriverID.FormattingEnabled = true;
            this.combexForkliftDriverID.Location = new System.Drawing.Point(316, 139);
            this.combexForkliftDriverID.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.combexForkliftDriverID.Name = "combexForkliftDriverID";
            this.combexForkliftDriverID.ReadOnly = false;
            this.combexForkliftDriverID.Size = new System.Drawing.Size(509, 47);
            this.combexForkliftDriverID.TabIndex = 87;
            // 
            // combexStorekeeperID
            // 
            this.combexStorekeeperID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.combexStorekeeperID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.combexStorekeeperID.Editable = true;
            this.combexStorekeeperID.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combexStorekeeperID.FormattingEnabled = true;
            this.combexStorekeeperID.Location = new System.Drawing.Point(316, 191);
            this.combexStorekeeperID.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.combexStorekeeperID.Name = "combexStorekeeperID";
            this.combexStorekeeperID.ReadOnly = false;
            this.combexStorekeeperID.Size = new System.Drawing.Size(509, 47);
            this.combexStorekeeperID.TabIndex = 88;
            // 
            // textexRemarks
            // 
            this.textexRemarks.Editable = true;
            this.textexRemarks.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textexRemarks.Location = new System.Drawing.Point(316, 243);
            this.textexRemarks.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.textexRemarks.Name = "textexRemarks";
            this.textexRemarks.Size = new System.Drawing.Size(509, 41);
            this.textexRemarks.TabIndex = 92;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(176, 89);
            this.label10.Margin = new System.Windows.Forms.Padding(2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(136, 40);
            this.label10.TabIndex = 85;
            this.label10.Text = "Warehouse";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(144, 141);
            this.label3.Margin = new System.Windows.Forms.Padding(2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(168, 40);
            this.label3.TabIndex = 90;
            this.label3.Text = "Forklift Driver";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(157, 193);
            this.label9.Margin = new System.Windows.Forms.Padding(1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(155, 40);
            this.label9.TabIndex = 91;
            this.label9.Text = "Store Keeper";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(210, 245);
            this.label1.Margin = new System.Windows.Forms.Padding(2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 40);
            this.label1.TabIndex = 89;
            this.label1.Text = "Remark";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // combexFillingLineID
            // 
            this.combexFillingLineID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.combexFillingLineID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.combexFillingLineID.Editable = true;
            this.combexFillingLineID.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combexFillingLineID.FormattingEnabled = true;
            this.combexFillingLineID.Location = new System.Drawing.Point(316, 35);
            this.combexFillingLineID.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.combexFillingLineID.Name = "combexFillingLineID";
            this.combexFillingLineID.ReadOnly = false;
            this.combexFillingLineID.Size = new System.Drawing.Size(509, 47);
            this.combexFillingLineID.TabIndex = 94;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(124, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 40);
            this.label2.TabIndex = 93;
            this.label2.Text = "Production Line";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // errorProviderMaster
            // 
            this.errorProviderMaster.ContainerControl = this;
            // 
            // WizardMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 360);
            this.Controls.Add(this.combexFillingLineID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.combexWarehouseID);
            this.Controls.Add(this.combexForkliftDriverID);
            this.Controls.Add(this.combexStorekeeperID);
            this.Controls.Add(this.textexRemarks);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WizardMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Wizard";
            this.Load += new System.EventHandler(this.WizardMaster_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderMaster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton buttonESC;
        private System.Windows.Forms.ToolStripButton buttonOK;
        private CustomControls.CombexBox combexWarehouseID;
        private CustomControls.CombexBox combexForkliftDriverID;
        private CustomControls.CombexBox combexStorekeeperID;
        private CustomControls.TextexBox textexRemarks;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private CustomControls.CombexBox combexFillingLineID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProviderMaster;
    }
}