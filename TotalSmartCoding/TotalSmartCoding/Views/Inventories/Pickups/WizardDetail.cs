using System;
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
        private PickupViewModel pickupViewModel;

        private PendingPallet pendingPallet;
        private PickupDetailDTO pickupDetailDTO;

        Binding bindingCodeID;
        Binding bindingCommodityCode;
        Binding bindingCommodityName;
        Binding bindingQuantity;
        Binding bindingRemarks;
        Binding bindingBinLocationID;

        public WizardDetail(PickupViewModel pickupViewModel, PendingPallet pendingPallet)
        {
            InitializeComponent();

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
                this.bindingRemarks = this.textexRemarks.DataBindings.Add("Text", this.pickupDetailDTO, CommonExpressions.PropertyName<PickupDetailDTO>(p => p.Remarks), true, DataSourceUpdateMode.OnPropertyChanged);


                BinLocationAPIs binLocationAPIs = new BinLocationAPIs(CommonNinject.Kernel.Get<IBinLocationAPIRepository>());

                this.combexBinLocationID.DataSource = binLocationAPIs.GetBinLocationBases();
                this.combexBinLocationID.DisplayMember = CommonExpressions.PropertyName<BinLocationBase>(p => p.Name);
                this.combexBinLocationID.ValueMember = CommonExpressions.PropertyName<BinLocationBase>(p => p.BinLocationID);
                this.bindingBinLocationID = this.combexBinLocationID.DataBindings.Add("SelectedValue", this.pickupDetailDTO, CommonExpressions.PropertyName<PickupDetailDTO>(p => p.BinLocationID), true, DataSourceUpdateMode.OnPropertyChanged);


                this.bindingCodeID.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
                this.bindingCommodityCode.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
                this.bindingCommodityName.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
                this.bindingQuantity.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
                this.bindingRemarks.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
                this.bindingBinLocationID.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

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
