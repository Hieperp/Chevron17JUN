using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using BrightIdeasSoftware;

using TotalModel.Models;
using TotalDTO.Inventories;
using TotalSmartCoding.Controllers.APIs.Inventories;
using TotalSmartCoding.Libraries.Helpers;
using TotalSmartCoding.ViewModels.Inventories;


namespace TotalSmartCoding.Views.Inventories.Pickups
{
    public partial class WizardDetail : Form
    {
        private PickupAPIs pickupAPIs;
        private PickupViewModel pickupViewModel;
        private CustomTabControl customTabBatch;
        public WizardDetail(PickupAPIs pickupAPIs, PickupViewModel pickupViewModel)
        {
            InitializeComponent();

            this.customTabBatch = new CustomTabControl();
            //this.customTabBatch.ImageList = this.imageListTabControl;

            this.customTabBatch.Font = this.fastPendingPallets.Font;
            this.customTabBatch.DisplayStyle = TabStyle.VisualStudio;
            this.customTabBatch.DisplayStyleProvider.ImageAlign = ContentAlignment.MiddleLeft;

            this.customTabBatch.TabPages.Add("tabPendingPallets", "Pending pallets");
            this.customTabBatch.TabPages.Add("tabPendingCartons", "Pending cartons");
            this.customTabBatch.TabPages.Add("tabPendingPacks", "Pending packs");
            this.customTabBatch.TabPages[0].Controls.Add(this.fastPendingPallets);
            this.customTabBatch.TabPages[1].Controls.Add(this.fastPendingCartons);
            this.customTabBatch.TabPages[2].Controls.Add(this.fastPendingPacks);


            this.customTabBatch.Dock = DockStyle.Fill;
            this.fastPendingPallets.Dock = DockStyle.Fill;
            this.fastPendingCartons.Dock = DockStyle.Fill;
            this.fastPendingPacks.Dock = DockStyle.Fill;
            this.panelMaster.Controls.Add(this.customTabBatch);


            this.pickupAPIs = pickupAPIs;
            this.pickupViewModel = pickupViewModel;
        }


        private void Wizard_Load(object sender, EventArgs e)
        {
            try
            {
                //List<PendingPalletDetail> pendingPalletDetails = this.pickupAPIs.GetPendingPalletDetails(this.pickupViewModel.LocationID, this.pickupViewModel.PickupID, this.pickupViewModel.PalletID, this.pickupViewModel.WarehouseID, string.Join(",", this.pickupViewModel.ViewDetails.Select(d => d.PalletDetailID)), false);

                //this.fastPendingPallets.SetObjects(pendingPalletDetails.Where(w => w.PalletID != null));
                //this.fastPendingCartons.SetObjects(pendingPalletDetails.Where(w => w.CartonID != null));
                //this.fastPendingPacks.SetObjects(pendingPalletDetails.Where(w => w.PackID != null));

                //this.customTabBatch.TabPages[0].Text = "Pending " + this.fastPendingPallets.GetItemCount().ToString("N0") + " pallet" + (this.fastPendingPacks.GetItemCount() > 1 ? "s      " : "      ");
                //this.customTabBatch.TabPages[1].Text = "Pending " + this.fastPendingCartons.GetItemCount().ToString("N0") + " carton" + (this.fastPendingPacks.GetItemCount() > 1 ? "s      " : "      ");
                //this.customTabBatch.TabPages[2].Text = "Pending " + this.fastPendingPacks.GetItemCount().ToString("N0") + " pack" + (this.fastPendingPacks.GetItemCount() > 1 ? "s      " : "      ");
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


        private void buttonAddESC_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender.Equals(this.buttonAdd) || sender.Equals(this.buttonAddExit))
                {
                    FastObjectListView fastPendingList = this.customTabBatch.SelectedIndex == 0 ? this.fastPendingPallets : (this.customTabBatch.SelectedIndex == 1 ? this.fastPendingCartons : this.customTabBatch.SelectedIndex == 2 ? this.fastPendingPacks : null);

                    if (fastPendingList != null)
                    {
                        foreach (var checkedObjects in fastPendingList.CheckedObjects)
                        {
                            //PendingPalletDetail pendingPalletDetail = (PendingPalletDetail)checkedObjects;
                            //PickupDetailDTO pickupDetailDTO = new PickupDetailDTO()
                            //{
                            //    PickupID = this.pickupViewModel.PickupID,

                            //    PalletID = pendingPalletDetail.PalletID,
                            //    PalletDetailID = pendingPalletDetail.PalletDetailID,
                            //    PalletReference = pendingPalletDetail.PalletReference,
                            //    PalletEntryDate = pendingPalletDetail.PalletEntryDate,

                            //    BinLocationID = pendingPalletDetail.BinLocationID,
                            //    BinLocationCode = pendingPalletDetail.BinLocationCode,

                            //    CommodityID = pendingPalletDetail.CommodityID,
                            //    CommodityCode = pendingPalletDetail.CommodityCode,
                            //    CommodityName = pendingPalletDetail.CommodityName,

                            //    Quantity = (decimal)pendingPalletDetail.QuantityRemains,
                            //    Volume = pendingPalletDetail.Volume,
                                

                            //    PackID = pendingPalletDetail.PackID,
                            //    PackCode = pendingPalletDetail.PackCode,
                            //    CartonID = pendingPalletDetail.CartonID,
                            //    CartonCode = pendingPalletDetail.CartonCode,
                            //    PalletID = pendingPalletDetail.PalletID,
                            //    PalletCode = pendingPalletDetail.PalletCode,
                            //};
                            //this.pickupViewModel.ViewDetails.Add(pickupDetailDTO);
                        }
                    }


                    if (sender.Equals(this.buttonAddExit))
                        this.DialogResult = DialogResult.OK;
                    else
                        this.Wizard_Load(this, new EventArgs());
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
