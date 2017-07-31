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
        private BatchController Controller { get; set; }
        private FillingData fillingData;
        private bool isAllQueuesEmpty;

        public Batches(FillingData fillingData, bool isAllQueuesEmpty)
            : base()
        {
            InitializeComponent();

            this.fillingData = fillingData;
            this.isAllQueuesEmpty = isAllQueuesEmpty;

            this.ChildToolStrip = this.toolStripChildForm;
            this.FastObjectListView = this.fastObjectListViewIndex;

            BatchAPIController batchAPIController = new BatchAPIController(CommonNinject.Kernel.Get<IBatchAPIRepository>());

            this.fastObjectListViewIndex.SetObjects(batchAPIController.GetBatchIndexes());

            this.Controller = new BatchController(CommonNinject.Kernel.Get<IBatchService>(), CommonNinject.Kernel.Get<IBatchViewModelSelectListBuilder>(), CommonNinject.Kernel.Get<BatchViewModel>());
            this.Controller.PropertyChanged += new PropertyChangedEventHandler(batchController_PropertyChanged);

            this.baseController = this.Controller;
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
        Binding bindingReference;

        Binding bindingCommodityID;

        protected override void InitializeCommonControlBinding()
        {
            base.InitializeCommonControlBinding();

            this.bindingReference = this.textBoxReference.DataBindings.Add("Text", this.Controller.BatchViewModel, "Reference", true);
            this.bindingEntryDate = this.datePickerEntryDate.DataBindings.Add("Value", this.Controller.BatchViewModel, "EntryDate", true);

            this.textCommodityName.DataBindings.Add("Text", this.Controller.BatchViewModel, CommonExpressions.PropertyName<BatchViewModel>(p => p.CommodityName), true);

            CommodityAPIs commodityAPIs = new CommodityAPIs(CommonNinject.Kernel.Get<ICommodityAPIRepository>());

            this.comboCommodityID.DataSource = commodityAPIs.GetCommodityBases();
            this.comboCommodityID.DisplayMember = CommonExpressions.PropertyName<CommodityBase>(p => p.Code);
            this.comboCommodityID.ValueMember = CommonExpressions.PropertyName<CommodityBase>(p => p.CommodityID);
            this.bindingCommodityID = this.comboCommodityID.DataBindings.Add("SelectedValue", this.Controller.BatchViewModel, CommonExpressions.PropertyName<BatchViewModel>(p => p.CommodityID), true, DataSourceUpdateMode.OnPropertyChanged);




            this.bindingReference.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.bindingEntryDate.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.bindingCommodityID.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);




            this.naviGroupDetails.DataBindings.Add("ExpandedHeight", this.numericUpDownSizingDetail, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            this.numericUpDownSizingDetail.Minimum = this.naviGroupDetails.HeaderHeight * 2;
            this.numericUpDownSizingDetail.Maximum = this.naviGroupDetails.Height + this.fastObjectListViewIndex.Height;

            this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].Width = 10;
            this.tableLayoutPanelExtend.ColumnStyles[this.tableLayoutPanelExtend.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelExtend.ColumnStyles[this.tableLayoutPanelExtend.ColumnCount - 1].Width = 10;
        }

        protected override void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            base.CommonControl_BindingComplete(sender, e);
            if (sender.Equals(this.bindingCommodityID) )
            {
                if (this.comboCommodityID.SelectedItem != null)
                {
                    CommodityBase a = (CommodityBase)this.comboCommodityID.SelectedItem;
                    this.Controller.BatchViewModel.CommodityName = a.Name;
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

        private void buttonApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.isAllQueuesEmpty && this.fastObjectListViewIndex.SelectedObject != null)
                {
                    BatchIndex batchIndex = (BatchIndex)fastObjectListViewIndex.SelectedObject;
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
