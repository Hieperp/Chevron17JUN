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
using TotalSmartCoding.Libraries;
using TotalSmartCoding.Libraries.Helpers;

using TotalSmartCoding.Controllers.Inventories;
using TotalCore.Repositories.Inventories;
using TotalSmartCoding.Controllers.APIs.Inventories;
using TotalCore.Services.Inventories;
using TotalSmartCoding.ViewModels.Inventories;

namespace TotalSmartCoding.Views.Inventories.GoodsReceipts
{
    public partial class GoodsReceipts : BaseView
    {
        private GoodsReceiptAPIs goodsReceiptAPIs;
        private GoodsReceiptController goodsReceiptController;

        public GoodsReceipts()
            : base()
        {
            InitializeComponent();


            this.toolstripChild = this.toolStripChildForm;
            this.fastListIndex = this.fastGoodsReceiptIndex;

            this.goodsReceiptAPIs = new GoodsReceiptAPIs(CommonNinject.Kernel.Get<IGoodsReceiptAPIRepository>());

            this.goodsReceiptController = new GoodsReceiptController(CommonNinject.Kernel.Get<IGoodsReceiptService>(), CommonNinject.Kernel.Get<GoodsReceiptViewModel>());
            this.goodsReceiptController.PropertyChanged += new PropertyChangedEventHandler(baseController_PropertyChanged);

            this.baseController = this.goodsReceiptController;
        }

        Binding bindingEntryDate;
        Binding bindingReference;

        protected override void InitializeCommonControlBinding()
        {
            base.InitializeCommonControlBinding();

            this.bindingReference = this.textexReference.DataBindings.Add("Text", this.goodsReceiptController.GoodsReceiptViewModel, "Reference", true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingEntryDate = this.dateTimexEntryDate.DataBindings.Add("Value", this.goodsReceiptController.GoodsReceiptViewModel, "EntryDate", true);


            this.bindingReference.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.bindingEntryDate.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);






            this.naviGroupDetails.DataBindings.Add("ExpandedHeight", this.numericUpDownSizingDetail, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            this.numericUpDownSizingDetail.Minimum = this.naviGroupDetails.HeaderHeight * 2;
            this.numericUpDownSizingDetail.Maximum = this.naviGroupDetails.Height + this.fastGoodsReceiptIndex.Height;

            this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].Width = 10;
            this.tableLayoutPanelExtend.ColumnStyles[this.tableLayoutPanelExtend.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelExtend.ColumnStyles[this.tableLayoutPanelExtend.ColumnCount - 1].Width = 10;
        }



        //private void InitializeDataGridBinding()
        //{
        //    this.dataGridViewDetails.AutoGenerateColumns = false;
        //    //marketingIncentiveDetailListView = new BindingListView<GoodsReceiptViewDetail>(this.goodsReceiptsController.ViewModel.GoodsReceiptViewDetails);
        //    //this.dataGridViewDetails.DataSource = marketingIncentiveDetailListView;

        //    //StackedHeaderDecorator stackedHeaderDecorator = new StackedHeaderDecorator(this.dataGridViewDetails);
        //}


        protected override void InitializeDataGridBinding()
        {
            this.gridexViewDetails.AutoGenerateColumns = false;
            this.gridexViewDetails.DataSource = this.goodsReceiptController.GoodsReceiptViewModel.ViewDetails;
            this.dataGridexView1.DataSource = this.goodsReceiptController.GoodsReceiptViewModel.PalletDetails;

            //StackedHeaderDecorator stackedHeaderDecorator = new StackedHeaderDecorator(this.dataGridViewDetails);
        }

        public override void Loading()
        {
            this.fastGoodsReceiptIndex.SetObjects(this.goodsReceiptAPIs.GetGoodsReceiptIndexes());
            base.Loading();
        }


        protected override DialogResult NewWizard()
        {
            Wizard wizard = new Wizard(this.goodsReceiptAPIs, this.goodsReceiptController.GoodsReceiptViewModel);
            return wizard.ShowDialog();
        }





    }
}
