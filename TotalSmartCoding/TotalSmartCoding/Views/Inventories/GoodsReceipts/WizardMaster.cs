using System;
using System.Drawing;
using System.Windows.Forms;

using TotalModel.Models;
using TotalSmartCoding.Controllers.APIs.Inventories;
using TotalSmartCoding.Libraries.Helpers;
using TotalSmartCoding.ViewModels.Inventories;

namespace TotalSmartCoding.Views.Inventories.GoodsReceipts
{
    public partial class WizardMaster : Form
    {
        private GoodsReceiptAPIs goodsReceiptAPIs;
        private GoodsReceiptViewModel goodsReceiptViewModel;
        private CustomTabControl customTabBatch;
        public WizardMaster(GoodsReceiptAPIs goodsReceiptAPIs, GoodsReceiptViewModel goodsReceiptViewModel)
        {
            InitializeComponent();

            this.customTabBatch = new CustomTabControl();
            //this.customTabBatch.ImageList = this.imageListTabControl;

            this.customTabBatch.Font = this.fastPendingPickups.Font;
            this.customTabBatch.DisplayStyle = TabStyle.VisualStudio;
            this.customTabBatch.DisplayStyleProvider.ImageAlign = ContentAlignment.MiddleLeft;

            this.customTabBatch.TabPages.Add("tabPendingPickups", "Receipt by pickup                  ");
            this.customTabBatch.TabPages.Add("tabPendingPickupWarehouses", "Receipt by warehouse          ");
            this.customTabBatch.TabPages.Add("tabPendingPickups", "Transfer Receipt                    ");
            this.customTabBatch.TabPages[0].Controls.Add(this.fastPendingPickups);
            this.customTabBatch.TabPages[1].Controls.Add(this.fastPendingPickupWarehouses);


            this.customTabBatch.Dock = DockStyle.Fill;
            this.fastPendingPickups.Dock = DockStyle.Fill;
            this.fastPendingPickupWarehouses.Dock = DockStyle.Fill;
            this.panelMaster.Controls.Add(this.customTabBatch);


            this.goodsReceiptAPIs = goodsReceiptAPIs;
            this.goodsReceiptViewModel = goodsReceiptViewModel;
        }


        private void Wizard_Load(object sender, EventArgs e)
        {
            try
            {
                this.fastPendingPickups.SetObjects(this.goodsReceiptAPIs.GetPendingPickups(this.goodsReceiptViewModel.LocationID));
                this.fastPendingPickupWarehouses.SetObjects(this.goodsReceiptAPIs.GetPendingPickupWarehouses(this.goodsReceiptViewModel.LocationID));

            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


        private void buttonOKESC_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender.Equals(this.buttonOK))
                {
                    this.goodsReceiptViewModel.GoodsReceiptTypeID = 1; //GoodsReceiptTypeID = 1-FROM PRODUCTION/ LATER: WE SHOULD IMPLEMENT FOR EXPORT: GoodsReceiptTypeID = 2

                    bool nextOK = false;
                    if (this.customTabBatch.SelectedIndex == 0)
                    {
                        PendingPickup pendingPickup = (PendingPickup)this.fastPendingPickups.SelectedObject;
                        if (pendingPickup != null) {                            
                            this.goodsReceiptViewModel.PickupID = pendingPickup.PickupID;
                            this.goodsReceiptViewModel.PickupReferences = pendingPickup.PickupReference;
                            this.goodsReceiptViewModel.WarehouseID = pendingPickup.WarehouseID;
                            this.goodsReceiptViewModel.WarehouseName = pendingPickup.WarehouseName;
                            nextOK = true;
                        }
                    }
                    if (this.customTabBatch.SelectedIndex == 1)
                    {
                        PendingPickupWarehouse pendingPickupWarehouse = (PendingPickupWarehouse)this.fastPendingPickupWarehouses.SelectedObject;
                        if (pendingPickupWarehouse != null)
                        {
                            this.goodsReceiptViewModel.WarehouseID = pendingPickupWarehouse.WarehouseID;
                            this.goodsReceiptViewModel.WarehouseName = pendingPickupWarehouse.WarehouseName;
                            nextOK = true;
                        }
                    }

                    if (nextOK)
                        this.DialogResult = DialogResult.OK;
                    else
                        CustomMsgBox.Show(this, "Vui lòng chọn phiếu giao thành phẩm sau đóng gói, hoặc kho nhận hàng.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);                    
                }

                if (sender.Equals(this.buttonESC))
                    this.DialogResult = DialogResult.Cancel;


            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }
    }
}
