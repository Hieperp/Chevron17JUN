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



using TotalBase.Enums;
using TotalSmartCoding.CommonLibraries;

using TotalSmartCoding.Controllers.Inventories;
using TotalCore.Repositories.Inventories;
using TotalSmartCoding.Controllers.APIs.Inventories;
using TotalCore.Services.Inventories;
using TotalSmartCoding.Builders.Inventories;

namespace TotalSmartCoding.Views.Inventories
{
    public partial class GoodsReceipts : BaseView
    {
        private GoodsReceiptsController goodsReceiptsController { get; set; }

        public GoodsReceipts()
            : base()
        {
            InitializeComponent();

            this.BaseController = new GoodsReceiptsController(CommonNinject.Kernel.Get<IGoodsReceiptService>(), CommonNinject.Kernel.Get<IGoodsReceiptViewModelSelectListBuilder>());

            this.ChildToolStrip = this.toolStripChildForm;
            this.FastObjectListView = this.fastObjectListViewIndex;

            var goodsReceiptAPIRepository = CommonNinject.Kernel.Get<IGoodsReceiptAPIRepository>();
            GoodsReceiptAPIsController goodsReceiptAPIsController = new GoodsReceiptAPIsController(goodsReceiptAPIRepository);

            this.fastObjectListViewIndex.SetObjects(goodsReceiptAPIsController.GetGoodsReceiptIndexes());

            this.goodsReceiptsController = new GoodsReceiptsController(CommonNinject.Kernel.Get<IGoodsReceiptService>(), CommonNinject.Kernel.Get<IGoodsReceiptViewModelSelectListBuilder>());
            this.goodsReceiptsController.PropertyChanged += new PropertyChangedEventHandler(goodsReceiptsController_PropertyChanged);
        }

        private void GoodsReceipts_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeCommonControlBinding();

                //InitializeDataGridBinding();

                //InitializeReadOnlyModeBinding();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void goodsReceiptsController_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }


        Binding bindingEntryDate;
        Binding bindingReference;

        Binding bindingIsDirty;
        Binding bindingIsDirtyBLL;

        private void InitializeCommonControlBinding()
        {


            this.bindingReference = this.textBoxReference.DataBindings.Add("Text", this.goodsReceiptsController.DtoViewModel, "Reference", true);

            this.bindingEntryDate = this.datePickerEntryDate.DataBindings.Add("Value", this.goodsReceiptsController.DtoViewModel, "EntryDate", true);

            this.bindingIsDirty = this.checkBoxIsDirty.DataBindings.Add("Checked", this.goodsReceiptsController.DtoViewModel, "IsDirty", true);
            this.bindingIsDirtyBLL = this.checkBoxIsDirtyBLL.DataBindings.Add("Checked", this.goodsReceiptsController, "IsDirty", true);



            this.bindingReference.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.bindingEntryDate.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.bindingIsDirty.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.bindingIsDirtyBLL.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);



            this.naviGroupDetails.DataBindings.Add("ExpandedHeight", this.numericUpDownSizingDetail, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            this.numericUpDownSizingDetail.Minimum = this.naviGroupDetails.HeaderHeight * 2;
            this.numericUpDownSizingDetail.Maximum = this.naviGroupDetails.Height + this.fastObjectListViewIndex.Height;

            this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].Width = 10;
            this.tableLayoutPanelExtend.ColumnStyles[this.tableLayoutPanelExtend.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelExtend.ColumnStyles[this.tableLayoutPanelExtend.ColumnCount - 1].Width = 10;

            this.errorProviderMaster.DataSource = this.goodsReceiptsController.DtoViewModel;

        }


        private void InitializeDataGridBinding()
        {
            this.dataGridViewDetails.AutoGenerateColumns = false;
            //marketingIncentiveDetailListView = new BindingListView<GoodsReceiptViewDetail>(this.goodsReceiptsController.ViewModel.GoodsReceiptViewDetails);
            //this.dataGridViewDetails.DataSource = marketingIncentiveDetailListView;

            //StackedHeaderDecorator stackedHeaderDecorator = new StackedHeaderDecorator(this.dataGridViewDetails);
        }


        ///lấy cái này!!!

        //////private void InitializeDataGridBinding()
        //////{
        //////    this.dataGridViewDetails.AutoGenerateColumns = false;
        //////    //marketingIncentiveDetailListView = new BindingListView<DeliveryAdviceDetailDTO>(this.deliveryAdvicesController.ViewDetailViewModel.DeliveryAdviceViewDetails);
        //////    this.dataGridViewDetails.DataSource = this.deliveryAdvicesController.ViewDetailViewModel.DeliveryAdviceViewDetails;

        //////    //StackedHeaderDecorator stackedHeaderDecorator = new StackedHeaderDecorator(this.dataGridViewDetails);
        //////}










        private void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState == BindingCompleteState.Exception) { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }


    }
}
