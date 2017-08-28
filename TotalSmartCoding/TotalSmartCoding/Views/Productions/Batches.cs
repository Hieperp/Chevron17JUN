using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Ninject;



using TotalSmartCoding.Views.Mains;


using TotalBase;
using TotalBase.Enums;
using TotalSmartCoding.Libraries;
using TotalSmartCoding.Libraries.Helpers;

using TotalCore.Repositories.Productions;
using TotalSmartCoding.Controllers.APIs.Productions;
using TotalCore.Services.Productions;
using TotalSmartCoding.ViewModels.Productions;
using TotalSmartCoding.Controllers.Productions;
using TotalDTO.Productions;
using AutoMapper;
using TotalModel.Models;

using TotalSmartCoding.Controllers.APIs.Commons;
using TotalCore.Repositories.Commons;
using TotalModel.Interfaces;
using BrightIdeasSoftware;
using TotalSmartCoding.Properties;



namespace TotalSmartCoding.Views.Productions
{
    public partial class Batches : BaseView
    {
        private SmartCoding smartCoding;
        private bool allQueueEmpty;

        private BatchAPIs batchAPIs;
        private BatchViewModel batchViewModel { get; set; }

        public Batches(SmartCoding smartCoding, bool allQueueEmpty)
            : base()
        {
            InitializeComponent();


            this.smartCoding = smartCoding;
            this.allQueueEmpty = allQueueEmpty;

            this.toolstripChild = this.toolStripChildForm;
            this.fastListIndex = this.fastBatchIndex;


            this.olvIsDefault.AspectGetter = delegate(object row)
            {// IsDefault indicator column
                if (((BatchIndex)row).IsDefault)
                    return "IsDefault";
                return "";
            };
            this.olvIsDefault.Renderer = new MappedImageRenderer(new Object[] { "IsDefault", Resources.Play_Normal_16 });
            this.buttonApply.Enabled = allQueueEmpty;

            this.batchAPIs = new BatchAPIs(CommonNinject.Kernel.Get<IBatchAPIRepository>());

            this.batchViewModel = CommonNinject.Kernel.Get<BatchViewModel>();
            this.batchViewModel.PropertyChanged += new PropertyChangedEventHandler(ModelDTO_PropertyChanged);
            this.baseDTO = this.batchViewModel;
        }

        protected override void NotifyPropertyChanged(string propertyName)
        {
            base.NotifyPropertyChanged(propertyName);

            if (propertyName == "ReadonlyMode")
            {
                this.buttonApply.Enabled = this.allQueueEmpty && this.ReadonlyMode;
                this.buttonDiscontinued.Enabled = this.Newable && this.ReadonlyMode;
            }
        }

        protected override void InitializeTabControl()
        {
            try
            {
                this.comboDiscontinued.SelectedIndex = 0;


                CustomTabControl customTabBatch = new CustomTabControl();
                //customTabControlCustomerChannel.ImageList = this.imageListTabControl;

                customTabBatch.Font = this.textexCode.Font;
                customTabBatch.DisplayStyle = TabStyle.VisualStudio;
                customTabBatch.DisplayStyleProvider.ImageAlign = ContentAlignment.MiddleLeft;

                customTabBatch.TabPages.Add("Batch", "Batch Information    ");
                customTabBatch.TabPages[0].Controls.Add(this.layoutMaster);

                this.naviBarMaster.Bands[0].ClientArea.Controls.Add(customTabBatch);

                customTabBatch.Dock = DockStyle.Fill;
                this.layoutMaster.Dock = DockStyle.Fill;
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        Binding bindingEntryDate;
        Binding bindingCode;

        Binding bindingNextPackNo;
        Binding bindingNextCartonNo;
        Binding bindingNextPalletNo;

        Binding bindingRemarks;

        Binding bindingCommodityID;

        protected override void InitializeCommonControlBinding()
        {
            base.InitializeCommonControlBinding();

            this.bindingEntryDate = this.dateTimexEntryDate.DataBindings.Add("Value", this.batchViewModel, "EntryDate", true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingCode = this.textexCode.DataBindings.Add("Text", this.batchViewModel, "Code", true, DataSourceUpdateMode.OnPropertyChanged);

            this.bindingNextPackNo = this.textexNextPackNo.DataBindings.Add("Text", this.batchViewModel, "NextPackNo", true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingNextCartonNo = this.textexNextCartonNo.DataBindings.Add("Text", this.batchViewModel, "NextCartonNo", true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingNextPalletNo = this.textexNextPalletNo.DataBindings.Add("Text", this.batchViewModel, "NextPalletNo", true, DataSourceUpdateMode.OnPropertyChanged);

            this.bindingRemarks = this.textexRemarks.DataBindings.Add("Text", this.batchViewModel, "Remarks", true, DataSourceUpdateMode.OnPropertyChanged);

            this.textexCommodityName.DataBindings.Add("Text", this.batchViewModel, CommonExpressions.PropertyName<BatchViewModel>(p => p.CommodityName), true);

            CommodityAPIs commodityAPIs = new CommodityAPIs(CommonNinject.Kernel.Get<ICommodityAPIRepository>());

            this.combexCommodityID.DataSource = commodityAPIs.GetCommodityBases();
            this.combexCommodityID.DisplayMember = CommonExpressions.PropertyName<CommodityBase>(p => p.Code);
            this.combexCommodityID.ValueMember = CommonExpressions.PropertyName<CommodityBase>(p => p.CommodityID);
            this.bindingCommodityID = this.combexCommodityID.DataBindings.Add("SelectedValue", this.batchViewModel, CommonExpressions.PropertyName<BatchViewModel>(p => p.CommodityID), true, DataSourceUpdateMode.OnPropertyChanged);

            this.bindingEntryDate.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.bindingCode.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.bindingNextPackNo.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.bindingNextCartonNo.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.bindingNextPalletNo.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.bindingRemarks.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.bindingCommodityID.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
        }

        protected override void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            base.CommonControl_BindingComplete(sender, e);
            if (sender.Equals(this.bindingCommodityID))
            {
                if (this.combexCommodityID.SelectedItem != null)
                {
                    CommodityBase commodityBase = (CommodityBase)this.combexCommodityID.SelectedItem;
                    this.batchViewModel.CommodityName = commodityBase.Name;
                }
            }
        }


        protected override Controllers.BaseController myController
        {
            get { return new BatchController(CommonNinject.Kernel.Get<IBatchService>(), this.batchViewModel); }
        }

        public override void Loading()
        {
            this.fastBatchIndex.SetObjects(this.batchAPIs.GetBatchIndexes(this.comboDiscontinued.SelectedIndex == 0 ? GlobalEnums.ActiveOption.Active : GlobalEnums.ActiveOption.Both));
            base.Loading();

            this.smartCoding.Initialize();
        }

        private void comboDiscontinued_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.batchAPIs != null) this.Loading();
        }

        private void fastBatchIndex_FormatRow(object sender, FormatRowEventArgs e)
        {
            BatchIndex batchIndex = (BatchIndex)e.Model;
            if (batchIndex.InActive) e.Item.ForeColor = Color.Gray;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (this.allQueueEmpty) this.Approve();
        }

        protected override bool ApproveCheck(int id)
        {
            return !this.batchViewModel.IsDefault && !this.batchViewModel.InActive;
        }

        private void buttonDiscontinued_Click(object sender, EventArgs e)
        {
            this.Void();
        }

        protected override bool VoidCheck(int id)
        {
            this.batchViewModel.VoidTypeID = 1;
            return !this.batchViewModel.IsDefault;
        }


    }
}
