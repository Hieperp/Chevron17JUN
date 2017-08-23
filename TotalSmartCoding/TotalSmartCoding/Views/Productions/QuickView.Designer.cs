namespace TotalSmartCoding.Views.Productions
{
    partial class QuickView
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
            this.fastBarcodes = new BrightIdeasSoftware.FastObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvCode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.textFilter = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.fastBarcodes)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // fastBarcodes
            // 
            this.fastBarcodes.AllColumns.Add(this.olvColumn1);
            this.fastBarcodes.AllColumns.Add(this.olvCode);
            this.fastBarcodes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvCode});
            this.fastBarcodes.Cursor = System.Windows.Forms.Cursors.Default;
            this.fastBarcodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fastBarcodes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fastBarcodes.FullRowSelect = true;
            this.fastBarcodes.HideSelection = false;
            this.fastBarcodes.HighlightBackgroundColor = System.Drawing.SystemColors.Highlight;
            this.fastBarcodes.Location = new System.Drawing.Point(97, 27);
            this.fastBarcodes.Name = "fastBarcodes";
            this.fastBarcodes.OwnerDraw = true;
            this.fastBarcodes.ShowGroups = false;
            this.fastBarcodes.Size = new System.Drawing.Size(372, 623);
            this.fastBarcodes.TabIndex = 71;
            this.fastBarcodes.UnfocusedHighlightBackgroundColor = System.Drawing.SystemColors.Highlight;
            this.fastBarcodes.UseCompatibleStateImageBehavior = false;
            this.fastBarcodes.UseFiltering = true;
            this.fastBarcodes.View = System.Windows.Forms.View.Details;
            this.fastBarcodes.VirtualMode = true;
            this.fastBarcodes.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.fastBarcodes_FormatRow);
            // 
            // olvColumn1
            // 
            this.olvColumn1.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumn1.Text = "";
            this.olvColumn1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumn1.Width = 35;
            // 
            // olvCode
            // 
            this.olvCode.AspectName = "Code";
            this.olvCode.FillsFreeSpace = true;
            this.olvCode.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvCode.Text = "Barcode";
            this.olvCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvCode.Width = 263;
            // 
            // textFilter
            // 
            this.textFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.textFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textFilter.Location = new System.Drawing.Point(97, 0);
            this.textFilter.Name = "textFilter";
            this.textFilter.Size = new System.Drawing.Size(372, 27);
            this.textFilter.TabIndex = 72;
            this.textFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textFilter.TextChanged += new System.EventHandler(this.textFilter_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(97, 650);
            this.panel1.TabIndex = 73;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TotalSmartCoding.Properties.Resources.Barcode2D;
            this.pictureBox1.Location = new System.Drawing.Point(21, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // QuickView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 650);
            this.Controls.Add(this.fastBarcodes);
            this.Controls.Add(this.textFilter);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quick View";
            ((System.ComponentModel.ISupportInitialize)(this.fastBarcodes)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.FastObjectListView fastBarcodes;
        private BrightIdeasSoftware.OLVColumn olvCode;
        private System.Windows.Forms.TextBox textFilter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
    }
}