﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using BrightIdeasSoftware;

using Ninject;

using TotalModel.Models;
using TotalDTO.Inventories;
using TotalSmartCoding.Controllers.APIs.Commons;
using TotalSmartCoding.Controllers.APIs.Inventories;
using TotalSmartCoding.Libraries;
using TotalSmartCoding.Libraries.Helpers;
using TotalSmartCoding.ViewModels.Inventories;
using TotalBase;
using TotalCore.Repositories.Commons;



namespace TotalSmartCoding.Views.Inventories.Pickups
{
    public partial class WizardDetail : Form
    {
        private CustomTabControl tabBinLocation;

        private PickupViewModel pickupViewModel;

        private PendingPallet pendingPallet;
        private PickupDetailDTO pickupDetailDTO;

        Binding bindingCodeID;
        Binding bindingCommodityCode;
        Binding bindingCommodityName;
        Binding bindingQuantity;

        public WizardDetail(PickupViewModel pickupViewModel, PendingPallet pendingPallet)
        {
            InitializeComponent();

            this.tabBinLocation = new CustomTabControl();

            this.tabBinLocation.Font = new Font("Niagara Engraved", 16);
            this.tabBinLocation.DisplayStyle = TabStyle.VisualStudio;

            this.tabBinLocation.TabPages.Add("tabBinLocations", "Available Bin Location   ");
            this.tabBinLocation.TabPages[0].Controls.Add(this.fastBinLocations);

            this.tabBinLocation.Dock = DockStyle.Fill;
            this.fastBinLocations.Dock = DockStyle.Fill;
            this.splitContainer2.Panel2.Controls.Add(this.tabBinLocation);

            this.splitContainer2.SplitterDistance = this.textexCode.Height + this.textexCommodityCode.Height + this.textexCommodityName.Height + this.textexQuantity.Height + this.textexBinLocationFilters.Height + 30;
            this.splitContainer1.SplitterDistance = this.Width - this.button001.Width - this.button002.Width - this.button003.Width - this.button004.Width - 22;
            this.ActiveControl = this.textexBinLocationFilters;

            this.pickupViewModel = pickupViewModel;
            this.pendingPallet = pendingPallet;
        }


        private void WizardDetail_Load(object sender, EventArgs e)
        {
            try
            {
                this.pickupDetailDTO = new PickupDetailDTO()
                {
                    PickupID = this.pickupViewModel.PickupID,

                    PalletID = this.pendingPallet.PalletID,
                    PalletCode = this.pendingPallet.Code,
                    PalletEntryDate = this.pendingPallet.EntryDate,

                    CommodityID = this.pendingPallet.CommodityID,
                    CommodityCode = this.pendingPallet.CommodityCode,
                    CommodityName = this.pendingPallet.CommodityName,

                    Quantity = (decimal)this.pendingPallet.QuantityRemains
                };

                this.bindingCodeID = this.textexCode.DataBindings.Add("Text", this.pickupDetailDTO, CommonExpressions.PropertyName<PickupDetailDTO>(p => p.PalletCode));
                this.bindingCommodityCode = this.textexCommodityCode.DataBindings.Add("Text", this.pickupDetailDTO, CommonExpressions.PropertyName<PickupDetailDTO>(p => p.CommodityCode));
                this.bindingCommodityName = this.textexCommodityName.DataBindings.Add("Text", this.pickupDetailDTO, CommonExpressions.PropertyName<PickupDetailDTO>(p => p.CommodityName));
                this.bindingQuantity = this.textexQuantity.DataBindings.Add("Text", this.pickupDetailDTO, CommonExpressions.PropertyName<PickupDetailDTO>(p => p.Quantity));

                this.fastBinLocations.SetObjects((new BinLocationAPIs(CommonNinject.Kernel.Get<IBinLocationAPIRepository>())).GetBinLocationBases());

                this.tabBinLocation.TabPages[0].Text = this.fastBinLocations.GetItemCount().ToString("N0") + " Bin" + (this.fastBinLocations.GetItemCount() > 1 ? "s" : "") + " Available       ";

                

                this.bindingCodeID.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
                this.bindingCommodityCode.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
                this.bindingCommodityName.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
                this.bindingQuantity.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

                this.errorProviderMaster.DataSource = this.pickupDetailDTO; 
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState == BindingCompleteState.Exception) { ExceptionHandlers.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }

        private void buttonAddESC_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender.Equals(this.buttonAdd) && this.pickupDetailDTO.IsValid)
                {
                    this.pickupViewModel.ViewDetails.Add(pickupDetailDTO);
                    this.DialogResult = DialogResult.OK;
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
