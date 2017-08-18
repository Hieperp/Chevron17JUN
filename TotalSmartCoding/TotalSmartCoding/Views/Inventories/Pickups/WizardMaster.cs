using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

using Ninject;

using TotalBase;
using TotalModel.Models;
using TotalCore.Repositories.Commons;
using TotalSmartCoding.Controllers.APIs.Commons;
using TotalSmartCoding.Controllers.APIs.Inventories;
using TotalSmartCoding.Libraries;
using TotalSmartCoding.Libraries.Helpers;
using TotalSmartCoding.ViewModels.Inventories;


namespace TotalSmartCoding.Views.Inventories.Pickups
{
    public partial class WizardMaster : Form
    {
        private PickupAPIs pickupAPIs;
        private PickupViewModel pickupViewModel;

        Binding bindingWarehouseID;
        Binding bindingFillingLineID;
        Binding bindingForkliftDriverID;
        Binding bindingStorekeeperID;

        Binding bindingRemarks;

        public WizardMaster(PickupAPIs pickupAPIs, PickupViewModel pickupViewModel)
        {
            InitializeComponent();

            this.pickupAPIs = pickupAPIs;
            this.pickupViewModel = pickupViewModel;
        }

        private void WizardMaster_Load(object sender, EventArgs e)
        {
            try
            {
                this.pickupViewModel.PropertyChanged += pickupDetailDTO_PropertyChanged;

                WarehouseAPIs warehouseAPIs = new WarehouseAPIs(CommonNinject.Kernel.Get<IWarehouseAPIRepository>());

                this.combexWarehouseID.DataSource = warehouseAPIs.GetWarehouseBases();
                this.combexWarehouseID.DisplayMember = CommonExpressions.PropertyName<WarehouseBase>(p => p.Name);
                this.combexWarehouseID.ValueMember = CommonExpressions.PropertyName<WarehouseBase>(p => p.WarehouseID);
                this.bindingWarehouseID = this.combexWarehouseID.DataBindings.Add("SelectedValue", this.pickupViewModel, CommonExpressions.PropertyName<PickupViewModel>(p => p.WarehouseID), true, DataSourceUpdateMode.OnPropertyChanged);

                FillingLineAPIs fillingLineAPIs = new FillingLineAPIs(CommonNinject.Kernel.Get<IFillingLineAPIRepository>());

                this.combexFillingLineID.DataSource = fillingLineAPIs.GetFillingLineBases();
                this.combexFillingLineID.DisplayMember = CommonExpressions.PropertyName<FillingLineBase>(p => p.Name);
                this.combexFillingLineID.ValueMember = CommonExpressions.PropertyName<FillingLineBase>(p => p.FillingLineID);
                this.bindingFillingLineID = this.combexFillingLineID.DataBindings.Add("SelectedValue", this.pickupViewModel, CommonExpressions.PropertyName<PickupViewModel>(p => p.FillingLineID), true, DataSourceUpdateMode.OnPropertyChanged);

                EmployeeAPIs employeeAPIs = new EmployeeAPIs(CommonNinject.Kernel.Get<IEmployeeAPIRepository>());

                this.combexForkliftDriverID.DataSource = employeeAPIs.GetEmployeeBases();
                this.combexForkliftDriverID.DisplayMember = CommonExpressions.PropertyName<EmployeeBase>(p => p.Name);
                this.combexForkliftDriverID.ValueMember = CommonExpressions.PropertyName<EmployeeBase>(p => p.EmployeeID);
                this.bindingForkliftDriverID = this.combexForkliftDriverID.DataBindings.Add("SelectedValue", this.pickupViewModel, CommonExpressions.PropertyName<PickupViewModel>(p => p.ForkliftDriverID), true, DataSourceUpdateMode.OnPropertyChanged);


                this.combexStorekeeperID.DataSource = employeeAPIs.GetEmployeeBases();
                this.combexStorekeeperID.DisplayMember = CommonExpressions.PropertyName<EmployeeBase>(p => p.Name);
                this.combexStorekeeperID.ValueMember = CommonExpressions.PropertyName<EmployeeBase>(p => p.EmployeeID);
                this.bindingStorekeeperID = this.combexStorekeeperID.DataBindings.Add("SelectedValue", this.pickupViewModel, CommonExpressions.PropertyName<PickupViewModel>(p => p.StorekeeperID), true, DataSourceUpdateMode.OnPropertyChanged);

                this.bindingRemarks = this.textexRemarks.DataBindings.Add("Text", this.pickupViewModel, "Remarks", true, DataSourceUpdateMode.OnPropertyChanged);

                this.bindingWarehouseID.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
                this.bindingFillingLineID.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
                this.bindingForkliftDriverID.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
                this.bindingStorekeeperID.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

                this.bindingRemarks.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

                this.errorProviderMaster.DataSource = this.pickupViewModel;
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void pickupDetailDTO_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.buttonOK.Enabled = this.pickupViewModel.IsValid;
        }

        private void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState == BindingCompleteState.Exception) { ExceptionHandlers.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
            if (sender.Equals(this.bindingWarehouseID))
            {
                if (this.combexWarehouseID.SelectedItem != null)
                {
                    WarehouseBase warehouseBase = (WarehouseBase)this.combexWarehouseID.SelectedItem;
                    this.pickupViewModel.WarehouseName = warehouseBase.Name;
                }
            }
            else if (sender.Equals(this.bindingFillingLineID))
            {
                if (this.combexFillingLineID.SelectedItem != null)
                {
                    FillingLineBase fillingLineBase = (FillingLineBase)this.combexFillingLineID.SelectedItem;
                    this.pickupViewModel.FillingLineName = fillingLineBase.Name;
                }
            }
        }

        private void buttonOKESC_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender.Equals(this.buttonOK))
                {
                    if (this.pickupViewModel.WarehouseID != null && this.pickupViewModel.ForkliftDriverID != null && this.pickupViewModel.StorekeeperID != null)
                    this.DialogResult = DialogResult.OK;
                    else
                        CustomMsgBox.Show(this, "Vui lòng chọn kho, tài xế và nhân viên kho.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
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
