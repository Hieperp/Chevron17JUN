using System;
using System.Drawing;
using System.Windows.Forms;

using TotalModel.Models;
using TotalSmartCoding.Controllers.APIs.Inventories;
using TotalSmartCoding.Libraries.Helpers;
using TotalSmartCoding.ViewModels.Inventories;

namespace TotalSmartCoding.Views.Inventories.Pickups
{
    public partial class WizardMaster : Form
    {
        private PickupAPIs pickupAPIs;
        private PickupViewModel pickupViewModel;
        private CustomTabControl customTabBatch;
        public WizardMaster(PickupAPIs pickupAPIs, PickupViewModel pickupViewModel)
        {
            InitializeComponent();

            this.customTabBatch = new CustomTabControl();
            //this.customTabBatch.ImageList = this.imageListTabControl;

            //this.customTabBatch.Font = this.fastPendingPallets.Font;
            //this.customTabBatch.DisplayStyle = TabStyle.VisualStudio;
            //this.customTabBatch.DisplayStyleProvider.ImageAlign = ContentAlignment.MiddleLeft;

            //this.customTabBatch.TabPages.Add("tabPendingPallets", "Receipt by pallet                  ");
            //this.customTabBatch.TabPages.Add("tabPendingPalletWarehouses", "Receipt by warehouse          ");
            //this.customTabBatch.TabPages.Add("tabPendingPallets", "Transfer Receipt                    ");
            //this.customTabBatch.TabPages[0].Controls.Add(this.fastPendingPallets);
            //this.customTabBatch.TabPages[1].Controls.Add(this.fastPendingPalletWarehouses);


            //this.customTabBatch.Dock = DockStyle.Fill;
            //this.fastPendingPallets.Dock = DockStyle.Fill;
            //this.fastPendingPalletWarehouses.Dock = DockStyle.Fill;
            //this.panelMaster.Controls.Add(this.customTabBatch);


            this.pickupAPIs = pickupAPIs;
            this.pickupViewModel = pickupViewModel;
        }


        private void Wizard_Load(object sender, EventArgs e)
        {
            try
            {
                //this.fastPendingPallets.SetObjects(this.pickupAPIs.GetPendingPallets(this.pickupViewModel.LocationID));
                //this.fastPendingPalletWarehouses.SetObjects(this.pickupAPIs.GetPendingPalletWarehouses(this.pickupViewModel.LocationID));

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
                    bool nextOK = false;
                    //if (this.customTabBatch.SelectedIndex == 0)
                    //{
                    //    PendingPallet pendingPallet = (PendingPallet)this.fastPendingPallets.SelectedObject;
                    //    if (pendingPallet != null) {                            
                    //        this.pickupViewModel.PalletID = pendingPallet.PalletID;
                    //        this.pickupViewModel.PalletReferences = pendingPallet.PalletReference;
                    //        this.pickupViewModel.WarehouseID = pendingPallet.WarehouseID;
                    //        this.pickupViewModel.WarehouseName = pendingPallet.WarehouseName;
                    //        nextOK = true;
                    //    }
                    //}
                    //if (this.customTabBatch.SelectedIndex == 1)
                    //{
                    //    PendingPalletWarehouse pendingPalletWarehouse = (PendingPalletWarehouse)this.fastPendingPalletWarehouses.SelectedObject;
                    //    if (pendingPalletWarehouse != null)
                    //    {
                    //        this.pickupViewModel.WarehouseID = pendingPalletWarehouse.WarehouseID;
                    //        this.pickupViewModel.WarehouseName = pendingPalletWarehouse.WarehouseName;
                    //        nextOK = true;
                    //    }
                    //}

                    if (nextOK)
                        this.DialogResult = DialogResult.OK;
                    else
                        MessageBox.Show(this, "Vui lòng chọn phiếu giao thành phẩm sau đóng gói, hoặc kho nhận hàng.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);                    
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
