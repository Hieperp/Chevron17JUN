using System;
using System.Drawing;
using System.Windows.Forms;
using TotalCore.Repositories.Inventories;
using TotalModel.Models;
using TotalSmartCoding.Controllers.APIs.Inventories;
using TotalSmartCoding.Libraries;
using TotalSmartCoding.Libraries.Helpers;
using TotalSmartCoding.ViewModels.Inventories;

namespace TotalSmartCoding.Views.Inventories.Pickups
{
    public partial class WizardMaster : Form
    {
        private PickupAPIs pickupAPIs;
        private PickupViewModel pickupViewModel;

        public WizardMaster(PickupAPIs pickupAPIs, PickupViewModel pickupViewModel)
        {
            InitializeComponent();

            this.pickupAPIs = pickupAPIs;
            this.pickupViewModel = pickupViewModel;
        }

        private void WizardMaster_Load(object sender, EventArgs e)
        {
            try
            {
                CommodityAPIs commodityAPIs = new PickupAPIs(CommonNinject.Kernel.Get<IPickupAPIRepository>());

                this.combexCommodityID.DataSource = commodityAPIs.GetCommodityBases();
                this.combexCommodityID.DisplayMember = CommonExpressions.PropertyName<CommodityBase>(p => p.Code);
                this.combexCommodityID.ValueMember = CommonExpressions.PropertyName<CommodityBase>(p => p.CommodityID);
                this.bindingCommodityID = this.combexCommodityID.DataBindings.Add("SelectedValue", this.batchController.BatchViewModel, CommonExpressions.PropertyName<BatchViewModel>(p => p.CommodityID), true, DataSourceUpdateMode.OnPropertyChanged);



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
                //this.textexRemarks.Width = 542;
                this.textexRemarks.Height = 39;
                float i = this.textexRemarks.Font.Size;

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
