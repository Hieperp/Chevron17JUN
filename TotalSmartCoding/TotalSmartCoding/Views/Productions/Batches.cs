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

using TotalCore.Repositories.Productions;
using TotalSmartCoding.Controllers.APIs.Productions;
using TotalCore.Services.Productions;
using TotalSmartCoding.Builders.Productions;
using TotalSmartCoding.ViewModels.Productions;
using TotalSmartCoding.Controllers.Productions;

namespace TotalSmartCoding.Views.Productions
{
    public partial class Batches : BaseView
    {
        private BatchController Controller { get; set; }

        public Batches()
            : base()
        {
            InitializeComponent();


            this.ChildToolStrip = this.toolStripChildForm;
            this.FastObjectListView = this.fastObjectListViewIndex;

            var batchAPIRepository = CommonNinject.Kernel.Get<IBatchAPIRepository>();
            BatchAPIController batchAPIController = new BatchAPIController(batchAPIRepository);

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

        protected override void InitializeCommonControlBinding()
        {
            base.InitializeCommonControlBinding();

            this.bindingReference = this.textBoxReference.DataBindings.Add("Text", this.Controller.BatchViewModel, "Reference", true);

            this.bindingEntryDate = this.datePickerEntryDate.DataBindings.Add("Value", this.Controller.BatchViewModel, "EntryDate", true);


            this.bindingReference.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.bindingEntryDate.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);



            this.naviGroupDetails.DataBindings.Add("ExpandedHeight", this.numericUpDownSizingDetail, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            this.numericUpDownSizingDetail.Minimum = this.naviGroupDetails.HeaderHeight * 2;
            this.numericUpDownSizingDetail.Maximum = this.naviGroupDetails.Height + this.fastObjectListViewIndex.Height;

            this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].Width = 10;
            this.tableLayoutPanelExtend.ColumnStyles[this.tableLayoutPanelExtend.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelExtend.ColumnStyles[this.tableLayoutPanelExtend.ColumnCount - 1].Width = 10;
        }


        












    }
}
