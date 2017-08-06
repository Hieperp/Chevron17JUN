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



namespace TotalSmartCoding.Views.Productions
{
    public partial class Batches : BaseView
    {
        private BatchController batchController;
        private BatchAPIs batchAPIs;
        private FillingData fillingData;
        private bool isAllQueuesEmpty;

        public Batches(FillingData fillingData, bool isAllQueuesEmpty)
            : base()
        {
            InitializeComponent();

            this.fillingData = fillingData;
            this.isAllQueuesEmpty = isAllQueuesEmpty;

            this.ChildToolStrip = this.toolStripChildForm;
            this.fastListIndex = this.fastListBatchIndex;


            this.batchAPIs = new BatchAPIs(CommonNinject.Kernel.Get<IBatchAPIRepository>());

            this.batchController = new BatchController(CommonNinject.Kernel.Get<IBatchService>(), CommonNinject.Kernel.Get<IBatchViewModelSelectListBuilder>(), CommonNinject.Kernel.Get<BatchViewModel>());
            this.batchController.PropertyChanged += new PropertyChangedEventHandler(batchController_PropertyChanged);

            this.baseController = this.batchController;
        }

        private void Batches_Load(object sender, EventArgs e)
        {
            try
            {
                //InitializeCommonControlBinding();


                //InitializeReadOnlyModeBinding();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void batchController_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }


        Binding bindingEntryDate;
        Binding bindingCode;

        Binding bindingCommodityID;

        Binding bindingNextPackNo;
        Binding bindingNextCartonNo;
        Binding bindingNextPalletNo;

        protected override void InitializeCommonControlBinding()
        {
            base.InitializeCommonControlBinding();

            this.bindingCode = this.textBoxCode.DataBindings.Add("Text", this.batchController.BatchViewModel, "Code", true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingEntryDate = this.datePickerEntryDate.DataBindings.Add("Value", this.batchController.BatchViewModel, "EntryDate", true, DataSourceUpdateMode.OnPropertyChanged);

            this.bindingNextPackNo = this.textNextPackNo.DataBindings.Add("Text", this.batchController.BatchViewModel, "NextPackNo", true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingNextCartonNo = this.textNextCartonNo.DataBindings.Add("Text", this.batchController.BatchViewModel, "NextCartonNo", true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingNextPalletNo = this.textNextPalletNo.DataBindings.Add("Text", this.batchController.BatchViewModel, "NextPalletNo", true, DataSourceUpdateMode.OnPropertyChanged);

            this.textCommodityName.DataBindings.Add("Text", this.batchController.BatchViewModel, CommonExpressions.PropertyName<BatchViewModel>(p => p.CommodityName), true);

            CommodityAPIs commodityAPIs = new CommodityAPIs(CommonNinject.Kernel.Get<ICommodityAPIRepository>());

            this.comboCommodityID.DataSource = commodityAPIs.GetCommodityBases();
            this.comboCommodityID.DisplayMember = CommonExpressions.PropertyName<CommodityBase>(p => p.Code);
            this.comboCommodityID.ValueMember = CommonExpressions.PropertyName<CommodityBase>(p => p.CommodityID);
            this.bindingCommodityID = this.comboCommodityID.DataBindings.Add("SelectedValue", this.batchController.BatchViewModel, CommonExpressions.PropertyName<BatchViewModel>(p => p.CommodityID), true, DataSourceUpdateMode.OnPropertyChanged);




            this.bindingCode.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.bindingEntryDate.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.bindingCommodityID.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.bindingNextPackNo.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.bindingNextCartonNo.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.bindingNextPalletNo.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
        }


        protected override void InitializeTabControl()
        {
            try
            {
                CustomTabControl customTabControlCustomerChannel = new CustomTabControl();
                //customTabControlCustomerChannel.ImageList = this.imageListTabControl;


                customTabControlCustomerChannel.TabPages.Add("CustomerChannel", "Batch Information    ");
                customTabControlCustomerChannel.TabPages[0].Controls.Add(this.layoutMaster);
                customTabControlCustomerChannel.Font = this.label1.Font;

                this.layoutMaster.Dock = DockStyle.Fill;

                customTabControlCustomerChannel.DisplayStyle = TabStyle.VisualStudio;
                customTabControlCustomerChannel.DisplayStyleProvider.ImageAlign = ContentAlignment.MiddleLeft;
                this.naviBarMaster.Bands[0].ClientArea.Controls.Add(customTabControlCustomerChannel);
                customTabControlCustomerChannel.Dock = DockStyle.Fill;
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
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


                ////    int addressAreaID;
                ////    DataRow selectedDataRow = ((DataRowView)this.comboCommodityID.SelectedItem).Row;
                ////    if (selectedDataRow != null && int.TryParse(this.lfGetColumnValueMapFieldID(GlobalEnum.EIColumnID.EIAddressAreaID, (string)selectedDataRow["ColumnFilterValueOriginal"]), out addressAreaID))
                ////        if (this.AddressAreaID != addressAreaID && addressAreaID > 0)//DIEU KIEN addressAreaID > 0 NHAM MUC DICH: => KHONG LOAD [All Territory]: TUC this.AddressAreaID = 0: NHAM KHONG LAM CHAM QUA TRINH NAY
                ////            this.AddressAreaID = addressAreaID;
                ////        else
                ////            this.lShowMapping();
                ////    else
                ////        this.lShowMapping();

                //this.textCommodityName.ReadOnly = true;
                //this.comboCommodityID.Enabled = true;
                //this.datePickerEntryDate.Enabled = true;



                ////KeyValuePair<int, string> keyValuePair = (KeyValuePair<int, string>)this.toolStripComboBoxListingOptions.ComboBox.SelectedItem;

                ////this.toolStripComboBoxAddressAreaID.Visible = (ListingOptions)keyValuePair.Key == ListingOptions.AddressArea;
                ////this.toolStripComboBoxCustomerCategoryID.Visible = (ListingOptions)keyValuePair.Key == ListingOptions.CustomerCategory;
                ////this.toolStripComboBoxCustomerChannelID.Visible = (ListingOptions)keyValuePair.Key == ListingOptions.CustomerChannel;
                ////this.toolStripComboBoxCustomerTypeID.Visible = (ListingOptions)keyValuePair.Key == ListingOptions.CustomerType;

                ////this.toolStripLabelFilter.Visible = (ListingOptions)keyValuePair.Key == ListingOptions.AddressArea || (ListingOptions)keyValuePair.Key == ListingOptions.CustomerCategory || (ListingOptions)keyValuePair.Key == ListingOptions.CustomerChannel || (ListingOptions)keyValuePair.Key == ListingOptions.CustomerType;






                ////KeyValuePair<int, string> keyValuePair = (KeyValuePair<int, string>)this.toolStripComboBoxListingOptions.ComboBox.SelectedItem;

                ////switch ((ListingOptions)keyValuePair.Key)
                ////{
                ////    case ListingOptions.AddressArea:
                ////        if (this.FilterAddressAreaID < 0)
                ////            listCustomerNameListingDataTable = this.listCustomerNameBLL.CustomerNameEmptyListing();
                ////        else
                ////            listCustomerNameListingDataTable = this.listCustomerNameBLL.CustomerNameAddressAreaListing(this.FilterAddressAreaID, this.FilterSalesmenID);
                ////        break;
                ////    case ListingOptions.CustomerCategory:
                ////        if (this.FilterCustomerCategoryID < 0)
                ////            listCustomerNameListingDataTable = this.listCustomerNameBLL.CustomerNameEmptyListing();
                ////        else
                ////            listCustomerNameListingDataTable = this.listCustomerNameBLL.CustomerNameCustomerCategoryListing(this.FilterCustomerCategoryID, this.FilterSalesmenID);
                ////        break;
                ////    case ListingOptions.CustomerChannel:
                ////        if (this.FilterCustomerChannelID < 0)
                ////            listCustomerNameListingDataTable = this.listCustomerNameBLL.CustomerNameEmptyListing();
                ////        else
                ////            listCustomerNameListingDataTable = this.listCustomerNameBLL.CustomerNameCustomerChannelListing(this.FilterCustomerChannelID, this.FilterSalesmenID);
                ////        break;
                ////    case ListingOptions.CustomerType:
                ////        if (this.FilterCustomerTypeID < 0)
                ////            listCustomerNameListingDataTable = this.listCustomerNameBLL.CustomerNameEmptyListing();
                ////        else
                ////            listCustomerNameListingDataTable = this.listCustomerNameBLL.CustomerNameCustomerTypeListing(this.FilterCustomerTypeID, this.FilterSalesmenID);
                ////        break;
                ////    case ListingOptions.MasterCustomer:
                ////        listCustomerNameListingDataTable = this.listCustomerNameBLL.CustomerNameMasterCustomerListing(this.FilterSalesmenID);
                ////        break;
                ////    case ListingOptions.TenderCustomer:
                ////        listCustomerNameListingDataTable = this.listCustomerNameBLL.CustomerNameTenderCustomerListing(this.FilterSalesmenID);
                ////        break;
                ////    default:
                ////        listCustomerNameListingDataTable = this.listCustomerNameBLL.CustomerNameEmptyListing();
                ////        break;
                ////}

            }
        }

        public override void Loading()
        {
            base.Loading();
            this.fastListBatchIndex.SetObjects(this.batchAPIs.GetBatchIndexes());
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.isAllQueuesEmpty && this.fastListBatchIndex.SelectedObject != null)
                {
                    BatchIndex batchIndex = (BatchIndex)fastListBatchIndex.SelectedObject;
                    if (batchIndex != null) { Mapper.Map<BatchIndex, FillingData>(batchIndex, this.fillingData); this.MdiParent.DialogResult = System.Windows.Forms.DialogResult.OK; }
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            GlobalExceptionHandler.ShowExceptionMessageBox(this, this.batchController.BatchViewModel.BatchID.ToString());
            //if (this.comboBox1.AutoCompleteSource == AutoCompleteSource.None)
            //    this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            //else
            //    this.comboBox1.AutoCompleteSource = AutoCompleteSource.None;

            //if (this.comboBox1.AutoCompleteMode == AutoCompleteMode.None)
            //    this.comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //else
            //    this.comboBox1.AutoCompleteMode = AutoCompleteMode.None;
        }



















    }
}
