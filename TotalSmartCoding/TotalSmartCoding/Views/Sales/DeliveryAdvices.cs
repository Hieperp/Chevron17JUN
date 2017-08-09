using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TotalSmartCoding.Views.Mains;

using TotalSmartCoding.Controllers.Sales;
using TotalSmartCoding.Controllers.APIs.Sales;

using TotalSmartCoding.CommonLibraries;
using TotalCore.Repositories.Sales;
using TotalCore.Repositories;

using Ninject;
using TotalCore.Services.Sales;
using TotalSmartCoding.Builders.Sales;
using TotalSmartCoding.ViewModels.Sales; 

namespace TotalSmartCoding.Views.Sales
{
    public partial class DeliveryAdvices : BasicView
    {
        DeliveryAdvicesController deliveryAdvicesController;


        private void deliveryAdvicesController_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }

        public DeliveryAdvices()
        {
            InitializeComponent();

            this.toolstripChild = this.toolStripChildForm;
            //this.FastObjectListView = this.fastObjectListViewIndex;

            var deliveryAdviceAPIRepository = CommonNinject.Kernel.Get<IDeliveryAdviceAPIRepository>();
            DeliveryAdviceAPIsController deliveryAdviceAPIsController = new DeliveryAdviceAPIsController(deliveryAdviceAPIRepository);

            this.fastObjectListViewIndex.SetObjects(deliveryAdviceAPIsController.GetDeliveryAdviceIndexes());

            this.fastObjectListViewIndex.CheckBoxes = false;

            this.deliveryAdvicesController = new DeliveryAdvicesController(CommonNinject.Kernel.Get<IDeliveryAdviceService>(), CommonNinject.Kernel.Get<IDeliveryAdviceViewModelSelectListBuilder>(), CommonNinject.Kernel.Get<DeliveryAdviceViewModel>());
            //******************this.deliveryAdvicesController.PropertyChanged += new PropertyChangedEventHandler(deliveryAdvicesController_PropertyChanged);
        }

        private void DeliveryAdvices_Load(object sender, EventArgs e)
        {
            try
            {
                //InitializeCommonControlBinding();

                //InitializeDataGridBinding();

                //InitializeReadOnlyModeBinding();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        Binding requestedDateBinding;
        Binding paymentPeriodBinding;

        Binding isDirtyBinding;
        Binding isDirtyBLLBinding;

        private void InitializeCommonControlBinding()
        {


            //******************this.paymentPeriodBinding = this.textBoxReference.DataBindings.Add("Text", this.deliveryAdvicesController.ViewDetailViewModel, "Reference", true);


            //******************this.requestedDateBinding = this.datePickerEntryDate.DataBindings.Add("Value", this.deliveryAdvicesController.ViewDetailViewModel, "EntryDate", true);

            //******************this.isDirtyBinding = this.checkBoxIsDirty.DataBindings.Add("Checked", this.deliveryAdvicesController.ViewDetailViewModel, "IsDirty", true);
            this.isDirtyBLLBinding = this.checkBoxIsDirtyBLL.DataBindings.Add("Checked", this.deliveryAdvicesController, "IsDirty", true);



            this.paymentPeriodBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            
            this.requestedDateBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.isDirtyBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.isDirtyBLLBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);



            this.naviGroupDetails.DataBindings.Add("ExpandedHeight", this.numericUpDownSizingDetail, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            this.numericUpDownSizingDetail.Minimum = this.naviGroupDetails.HeaderHeight * 2;
            this.numericUpDownSizingDetail.Maximum = this.naviGroupDetails.Height + this.fastObjectListViewIndex.Height;

            this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].Width = 10;
            this.tableLayoutPanelExtend.ColumnStyles[this.tableLayoutPanelExtend.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelExtend.ColumnStyles[this.tableLayoutPanelExtend.ColumnCount - 1].Width = 10;

            //******************this.errorProviderMaster.DataSource = this.deliveryAdvicesController.ViewDetailViewModel;

        }


        private void InitializeDataGridBinding()
        {
            this.dataGridViewDetails.AutoGenerateColumns = false;
            //marketingIncentiveDetailListView = new BindingListView<DeliveryAdviceViewDetail>(this.deliveryAdvicesController.ViewDetailViewModel.DeliveryAdviceViewDetails);
            //this.dataGridViewDetails.DataSource = marketingIncentiveDetailListView;

            //StackedHeaderDecorator stackedHeaderDecorator = new StackedHeaderDecorator(this.dataGridViewDetails);
        }


        //private void InitializeReadOnlyModeBinding()
        //{
            //List<Control> controlList = GlobalStaticFunction.GetAllControls(this);

            //foreach (Control control in controlList)
            //{
            //    //if (control is TextBox) control.DataBindings.Add("Readonly", this, "ReadonlyMode");
            //    if (control is TextBox) control.DataBindings.Add("Enabled", this, "EditableMode");
            //    else if (control is ComboBox || control is DateTimePicker) control.DataBindings.Add("Enabled", this, "EditableMode");
            //    else if (control is DataGridView)
            //    {
            //        control.DataBindings.Add("Readonly", this, "ReadonlyMode");
            //        control.DataBindings.Add("AllowUserToAddRows", this, "EditableMode");
            //        control.DataBindings.Add("AllowUserToDeleteRows", this, "EditableMode");
            //    }
            //}

            //this.dataListViewMaster.DataBindings.Add("Enabled", this, "ReadonlyMode");
        //}






        private void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState == BindingCompleteState.Exception) { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }

        private void naviGroupDetails_HeaderMouseClick(object sender, MouseEventArgs e)
        {
            this.numericUpDownSizingDetail.Visible = this.naviGroupDetails.Expanded;
            this.toolStripNaviGroupDetails.Visible = this.naviGroupDetails.Expanded;
        }

        private void toolStripButtonShowDetailsExtend_Click(object sender, EventArgs e)
        {
            this.naviGroupDetailsExtend.Expanded = !this.naviGroupDetailsExtend.Expanded;
            //this.toolStripButtonShowDetailsExtend.Image = this.naviGroupDetailsExtend.Expanded ? ResourceIcon.Chevron_Collapse.ToBitmap() : ResourceIcon.Chevron_Expand.ToBitmap();
        }

        private void numericUpDownSizingDetail_ValueChanged(object sender, EventArgs e)
        {
            this.naviGroupDetails.Expand();
        }




    }
}
