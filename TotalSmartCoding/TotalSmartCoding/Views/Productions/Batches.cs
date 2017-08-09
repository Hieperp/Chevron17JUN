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
using TotalSmartCoding.CommonLibraries;

using TotalCore.Repositories.Productions;
using TotalSmartCoding.Controllers.APIs.Productions;
using TotalCore.Services.Productions;
using TotalSmartCoding.Builders.Productions;
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
        private FillingData fillingData;
        private bool allQueueEmpty;

        private BatchAPIs batchAPIs;
        private BatchController batchController;

        public Batches(FillingData fillingData, bool allQueueEmpty)
            : base()
        {
            InitializeComponent();
            this.comboDiscontinued.SelectedIndex = 0;

            this.fillingData = fillingData;
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


            this.batchAPIs = new BatchAPIs(CommonNinject.Kernel.Get<IBatchAPIRepository>());

            this.batchController = new BatchController(CommonNinject.Kernel.Get<IBatchService>(), CommonNinject.Kernel.Get<IBatchViewModelSelectListBuilder>(), CommonNinject.Kernel.Get<BatchViewModel>());
            this.batchController.PropertyChanged += new PropertyChangedEventHandler(batchController_PropertyChanged);

            this.baseController = this.batchController;
        }
        
        private void batchController_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }


        protected override void InitializeTabControl()
        {
            try
            {
                CustomTabControl customTabBatch = new CustomTabControl();
                //customTabControlCustomerChannel.ImageList = this.imageListTabControl;

                customTabBatch.Font = this.textBoxCode.Font;
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
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
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

            this.bindingEntryDate = this.datePickerEntryDate.DataBindings.Add("Value", this.batchController.BatchViewModel, "EntryDate", true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingCode = this.textBoxCode.DataBindings.Add("Text", this.batchController.BatchViewModel, "Code", true, DataSourceUpdateMode.OnPropertyChanged);

            this.bindingNextPackNo = this.textNextPackNo.DataBindings.Add("Text", this.batchController.BatchViewModel, "NextPackNo", true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingNextCartonNo = this.textNextCartonNo.DataBindings.Add("Text", this.batchController.BatchViewModel, "NextCartonNo", true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingNextPalletNo = this.textNextPalletNo.DataBindings.Add("Text", this.batchController.BatchViewModel, "NextPalletNo", true, DataSourceUpdateMode.OnPropertyChanged);

            this.bindingRemarks = this.textRemarks.DataBindings.Add("Text", this.batchController.BatchViewModel, "Remarks", true, DataSourceUpdateMode.OnPropertyChanged);

            this.textCommodityName.DataBindings.Add("Text", this.batchController.BatchViewModel, CommonExpressions.PropertyName<BatchViewModel>(p => p.CommodityName), true);

            CommodityAPIs commodityAPIs = new CommodityAPIs(CommonNinject.Kernel.Get<ICommodityAPIRepository>());

            this.comboCommodityID.DataSource = commodityAPIs.GetCommodityBases();
            this.comboCommodityID.DisplayMember = CommonExpressions.PropertyName<CommodityBase>(p => p.Code);
            this.comboCommodityID.ValueMember = CommonExpressions.PropertyName<CommodityBase>(p => p.CommodityID);
            this.bindingCommodityID = this.comboCommodityID.DataBindings.Add("SelectedValue", this.batchController.BatchViewModel, CommonExpressions.PropertyName<BatchViewModel>(p => p.CommodityID), true, DataSourceUpdateMode.OnPropertyChanged);

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
                if (this.comboCommodityID.SelectedItem != null && this.batchController.BatchViewModel.TrackChanges)
                {
                    CommodityBase commodityBase = (CommodityBase)this.comboCommodityID.SelectedItem;
                    this.batchController.BatchViewModel.CommodityName = commodityBase.Name;
                }
            }
        }


        public override void Loading()
        {            
            this.fastBatchIndex.SetObjects(this.batchAPIs.GetBatchIndexes());
            base.Loading();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.allQueueEmpty && this.fastBatchIndex.SelectedObject != null)
                {
                    BatchIndex batchIndex = (BatchIndex)fastBatchIndex.SelectedObject;
                    if (batchIndex != null) { Mapper.Map<BatchIndex, FillingData>(batchIndex, this.fillingData); this.MdiParent.DialogResult = System.Windows.Forms.DialogResult.OK; }
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }
    }
}
