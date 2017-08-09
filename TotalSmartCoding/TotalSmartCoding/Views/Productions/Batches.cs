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
            this.buttonApply.Enabled = allQueueEmpty;

            this.batchAPIs = new BatchAPIs(CommonNinject.Kernel.Get<IBatchAPIRepository>());

            this.batchController = new BatchController(CommonNinject.Kernel.Get<IBatchService>(), CommonNinject.Kernel.Get<IBatchViewModelSelectListBuilder>(), CommonNinject.Kernel.Get<BatchViewModel>());
            this.batchController.PropertyChanged += new PropertyChangedEventHandler(batchController_PropertyChanged);

            this.baseController = this.batchController;
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

            this.bindingEntryDate = this.dateTimexEntryDate.DataBindings.Add("Value", this.batchController.BatchViewModel, "EntryDate", true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingCode = this.textexCode.DataBindings.Add("Text", this.batchController.BatchViewModel, "Code", true, DataSourceUpdateMode.OnPropertyChanged);

            this.bindingNextPackNo = this.textexNextPackNo.DataBindings.Add("Text", this.batchController.BatchViewModel, "NextPackNo", true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingNextCartonNo = this.textexNextCartonNo.DataBindings.Add("Text", this.batchController.BatchViewModel, "NextCartonNo", true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingNextPalletNo = this.textexNextPalletNo.DataBindings.Add("Text", this.batchController.BatchViewModel, "NextPalletNo", true, DataSourceUpdateMode.OnPropertyChanged);

            this.bindingRemarks = this.textexRemarks.DataBindings.Add("Text", this.batchController.BatchViewModel, "Remarks", true, DataSourceUpdateMode.OnPropertyChanged);

            this.textexCommodityName.DataBindings.Add("Text", this.batchController.BatchViewModel, CommonExpressions.PropertyName<BatchViewModel>(p => p.CommodityName), true);

            CommodityAPIs commodityAPIs = new CommodityAPIs(CommonNinject.Kernel.Get<ICommodityAPIRepository>());

            this.combexCommodityID.DataSource = commodityAPIs.GetCommodityBases();
            this.combexCommodityID.DisplayMember = CommonExpressions.PropertyName<CommodityBase>(p => p.Code);
            this.combexCommodityID.ValueMember = CommonExpressions.PropertyName<CommodityBase>(p => p.CommodityID);
            this.bindingCommodityID = this.combexCommodityID.DataBindings.Add("SelectedValue", this.batchController.BatchViewModel, CommonExpressions.PropertyName<BatchViewModel>(p => p.CommodityID), true, DataSourceUpdateMode.OnPropertyChanged);

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
                if (this.combexCommodityID.SelectedItem != null && this.batchController.BatchViewModel.TrackChanges)
                {
                    CommodityBase commodityBase = (CommodityBase)this.combexCommodityID.SelectedItem;
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
                    if (MessageBox.Show(this, "Are you sure you want to delete " + this.baseController.BaseDTO.Reference + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                        if (this.baseController.Delete(this.baseController.BaseDTO.GetID()))
                            this.Loading();
                    BatchIndex batchIndex = (BatchIndex)fastBatchIndex.SelectedObject;
                    if (batchIndex != null) { Mapper.Map<BatchIndex, FillingData>(batchIndex, this.fillingData); this.MdiParent.DialogResult = System.Windows.Forms.DialogResult.OK; }
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void buttonDiscontinued_Click(object sender, EventArgs e)
        {

        }
    }
}
