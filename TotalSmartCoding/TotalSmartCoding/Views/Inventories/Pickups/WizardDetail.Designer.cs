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
            this.buttonESC = new System.Windows.Forms.ToolStripButton();
            this.buttonAdd = new System.Windows.Forms.ToolStripButton();
            this.label2 = new System.Windows.Forms.Label();
            this.combexBinLocationID = new CustomControls.CombexBox();
            this.textexRemarks = new CustomControls.TextexBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textexCode = new CustomControls.TextexBox();
            this.textexCommodityCode = new CustomControls.TextexBox();
            this.textexCommodityName = new CustomControls.TextexBox();
            this.textexQuantity = new CustomControls.TextexBox();
            this.label4 = new System.Windows.Forms.Label();
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
            this.buttonAdd});
            this.toolStrip1.Location = new System.Drawing.Point(0, 340);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip1.Size = new System.Drawing.Size(920, 55);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
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
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(279, 42);
            this.label2.Margin = new System.Windows.Forms.Padding(2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 40);
            this.label2.TabIndex = 103;
            this.label2.Text = "Barcode";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // combexBinLocationID
            // 
            this.combexBinLocationID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.combexBinLocationID.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.combexBinLocationID.Editable = true;
            this.combexBinLocationID.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combexBinLocationID.FormattingEnabled = true;
            this.combexBinLocationID.Location = new System.Drawing.Point(387, 220);
            this.combexBinLocationID.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.combexBinLocationID.Name = "combexBinLocationID";
            this.combexBinLocationID.ReadOnly = false;
            this.combexBinLocationID.Size = new System.Drawing.Size(509, 47);
            this.combexBinLocationID.TabIndex = 98;
            // 
            // textexRemarks
            // 
            this.textexRemarks.Editable = true;
            this.textexRemarks.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textexRemarks.Location = new System.Drawing.Point(387, 271);
            this.textexRemarks.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.textexRemarks.Name = "textexRemarks";
            this.textexRemarks.Size = new System.Drawing.Size(509, 41);
            this.textexRemarks.TabIndex = 102;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(256, 87);
            this.label10.Margin = new System.Windows.Forms.Padding(2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(127, 40);
            this.label10.TabIndex = 95;
            this.label10.Text = "Item Code";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(243, 130);
            this.label3.Margin = new System.Windows.Forms.Padding(2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 40);
            this.label3.TabIndex = 100;
            this.label3.Text = "Description";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(232, 222);
            this.label9.Margin = new System.Windows.Forms.Padding(1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(151, 40);
            this.label9.TabIndex = 101;
            this.label9.Text = "Bin Location";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(281, 273);
            this.label1.Margin = new System.Windows.Forms.Padding(2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 40);
            this.label1.TabIndex = 99;
            this.label1.Text = "Remark";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textexCode
            // 
            this.textexCode.Editable = true;
            this.textexCode.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textexCode.Location = new System.Drawing.Point(387, 40);
            this.textexCode.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.textexCode.Name = "textexCode";
            this.textexCode.ReadOnly = true;
            this.textexCode.Size = new System.Drawing.Size(509, 41);
            this.textexCode.TabIndex = 104;
            // 
            // textexCommodityCode
            // 
            this.textexCommodityCode.Editable = true;
            this.textexCommodityCode.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textexCommodityCode.Location = new System.Drawing.Point(387, 85);
            this.textexCommodityCode.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.textexCommodityCode.Name = "textexCommodityCode";
            this.textexCommodityCode.ReadOnly = true;
            this.textexCommodityCode.Size = new System.Drawing.Size(509, 41);
            this.textexCommodityCode.TabIndex = 105;
            // 
            // textexCommodityName
            // 
            this.textexCommodityName.Editable = true;
            this.textexCommodityName.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textexCommodityName.Location = new System.Drawing.Point(387, 130);
            this.textexCommodityName.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.textexCommodityName.Name = "textexCommodityName";
            this.textexCommodityName.ReadOnly = true;
            this.textexCommodityName.Size = new System.Drawing.Size(509, 41);
            this.textexCommodityName.TabIndex = 106;
            // 
            // textexQuantity
            // 
            this.textexQuantity.Editable = true;
            this.textexQuantity.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textexQuantity.Location = new System.Drawing.Point(387, 175);
            this.textexQuantity.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.textexQuantity.Name = "textexQuantity";
            this.textexQuantity.ReadOnly = true;
            this.textexQuantity.Size = new System.Drawing.Size(509, 41);
            this.textexQuantity.TabIndex = 108;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Sitka Banner", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(274, 177);
            this.label4.Margin = new System.Windows.Forms.Padding(2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 40);
            this.label4.TabIndex = 107;
            this.label4.Text = "Quantity";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // errorProviderMaster
            // 
            this.errorProviderMaster.ContainerControl = this;
            // 
            // WizardDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 395);
            this.Controls.Add(this.textexQuantity);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textexCommodityName);
            this.Controls.Add(this.textexCommodityCode);
            this.Controls.Add(this.textexCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.combexBinLocationID);
            this.Controls.Add(this.textexRemarks);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton buttonESC;
        private System.Windows.Forms.ToolStripButton buttonAdd;
        private System.Windows.Forms.Label label2;
        private CustomControls.CombexBox combexBinLocationID;
        private CustomControls.TextexBox textexRemarks;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private CustomControls.TextexBox textexCode;
        private CustomControls.TextexBox textexCommodityCode;
        private CustomControls.TextexBox textexCommodityName;
        private CustomControls.TextexBox textexQuantity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider errorProviderMaster;
    }
}