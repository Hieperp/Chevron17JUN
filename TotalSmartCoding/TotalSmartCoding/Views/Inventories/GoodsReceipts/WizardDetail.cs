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


namespace TotalSmartCoding.Views.Inventories.GoodsReceipts
{
    public partial class WizardDetail : Form
    {
        private GoodsReceiptAPIs goodsReceiptAPIs;
        private GoodsReceiptViewModel goodsReceiptViewModel;
        private CustomTabControl customTabBatch;
        public WizardDetail(GoodsReceiptAPIs goodsReceiptAPIs, GoodsReceiptViewModel goodsReceiptViewModel)
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


            this.goodsReceiptAPIs = goodsReceiptAPIs;
            this.goodsReceiptViewModel = goodsReceiptViewModel;
        }


        private void WizardDetail_Load(object sender, EventArgs e)
        {
            try
            {
                List<PendingPickupDetail> pendingPickupDetails = this.goodsReceiptAPIs.GetPendingPickupDetails(this.goodsReceiptViewModel.LocationID, this.goodsReceiptViewModel.GoodsReceiptID, this.goodsReceiptViewModel.PickupID, this.goodsReceiptViewModel.WarehouseID, string.Join(",", this.goodsReceiptViewModel.ViewDetails.Select(d => d.PickupDetailID)), false);

                this.fastPendingPallets.SetObjects(pendingPickupDetails.Where(w => w.PalletID != null));
                this.fastPendingCartons.SetObjects(pendingPickupDetails.Where(w => w.CartonID != null));
                this.fastPendingPacks.SetObjects(pendingPickupDetails.Where(w => w.PackID != null));

                this.customTabBatch.TabPages[0].Text = "Pending " + this.fastPendingPallets.GetItemCount().ToString("N0") + " pallet" + (this.fastPendingPacks.GetItemCount() > 1 ? "s      " : "      ");
                this.customTabBatch.TabPages[1].Text = "Pending " + this.fastPendingCartons.GetItemCount().ToString("N0") + " carton" + (this.fastPendingPacks.GetItemCount() > 1 ? "s      " : "      ");
                this.customTabBatch.TabPages[2].Text = "Pending " + this.fastPendingPacks.GetItemCount().ToString("N0") + " pack" + (this.fastPendingPacks.GetItemCount() > 1 ? "s      " : "      ");
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
                            PendingPickupDetail pendingPickupDetail = (PendingPickupDetail)checkedObjects;
                            GoodsReceiptDetailDTO goodsReceiptDetailDTO = new GoodsReceiptDetailDTO()
                            {
                                GoodsReceiptID = this.goodsReceiptViewModel.GoodsReceiptID,

                                PickupID = pendingPickupDetail.PickupID,
                                PickupDetailID = pendingPickupDetail.PickupDetailID,
                                PickupReference = pendingPickupDetail.PickupReference,
                                PickupEntryDate = pendingPickupDetail.PickupEntryDate,

                                BinLocationID = pendingPickupDetail.BinLocationID,
                                BinLocationCode = pendingPickupDetail.BinLocationCode,

                                CommodityID = pendingPickupDetail.CommodityID,
                                CommodityCode = pendingPickupDetail.CommodityCode,
                                CommodityName = pendingPickupDetail.CommodityName,

                                Quantity = (decimal)pendingPickupDetail.QuantityRemains,
                                Volume = pendingPickupDetail.Volume,
                                

                                PackID = pendingPickupDetail.PackID,
                                PackCode = pendingPickupDetail.PackCode,
                                CartonID = pendingPickupDetail.CartonID,
                                CartonCode = pendingPickupDetail.CartonCode,
                                PalletID = pendingPickupDetail.PalletID,
                                PalletCode = pendingPickupDetail.PalletCode,
                            };
                            this.goodsReceiptViewModel.ViewDetails.Add(goodsReceiptDetailDTO);
                        }
                    }


                    if (sender.Equals(this.buttonAddExit))
                        this.DialogResult = DialogResult.OK;
                    else
                        this.WizardDetail_Load(this, new EventArgs());
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
