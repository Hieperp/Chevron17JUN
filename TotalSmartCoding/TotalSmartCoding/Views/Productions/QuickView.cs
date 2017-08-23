using System;
using System.Windows.Forms;
using System.Collections.Generic;

using TotalDTO.Productions;
using TotalSmartCoding.Libraries.Helpers;

namespace TotalSmartCoding.Views.Productions
{
    public partial class QuickView : Form
    {
        public QuickView(IList<BarcodeDTO> barcodeList)
        {
            InitializeComponent();

            this.fastBarcodes.SetObjects(barcodeList);
        }

        private void textFilter_TextChanged(object sender, EventArgs e)
        {
            OLVHelpers.ApplyFilters(this.fastBarcodes, textFilter.Text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
        }

        private void fastBarcodes_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            e.Item.SubItems[0].Text = (e.RowIndex + 1).ToString();
        }
    }
}
