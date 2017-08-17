using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;

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
using TotalSmartCoding.Controllers.APIs.Commons;
using TotalCore.Repositories.Commons;
using TotalBase;
using TotalModel.Models;

namespace TotalSmartCoding.Views.Inventories.Pickups
{
    public partial class Pickups : BaseView
    {
        private CustomTabControl customTabBatch;

        private PickupAPIs pickupAPIs;
        private PickupViewModel pickupViewModel { get; set; }

        private System.Timers.Timer timerLoadPending;
        private delegate void timerLoadCallback();

        public Pickups()
            : base()
        {
            InitializeComponent();

            this.toolstripChild = this.toolStripChildForm;
            this.fastListIndex = this.fastPickupIndex;

            this.pickupAPIs = new PickupAPIs(CommonNinject.Kernel.Get<IPickupAPIRepository>());

            this.pickupViewModel = CommonNinject.Kernel.Get<PickupViewModel>();
            this.pickupViewModel.PropertyChanged += new PropertyChangedEventHandler(ModelDTO_PropertyChanged);
            this.baseDTO = this.pickupViewModel;

            this.timerLoadPending = new System.Timers.Timer(60000);
            this.timerLoadPending.Elapsed += new System.Timers.ElapsedEventHandler(timerLoadPending_Elapsed);
            this.timerLoadPending.Enabled = true;
        }

        private void Pickups_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timerLoadPending.Enabled = false;
        }

        protected override void InitializeTabControl()
        {
            try
            {
                this.naviIndex.Bands[0].ClientArea.Controls.Add(this.fastPickupIndex);

                this.customTabBatch = new CustomTabControl();
                this.setFont(new Font("Niagara Engraved", 16), new Font("Sitka Banner", 16), new Font("Niagara Engraved", 16));

                this.customTabBatch.DisplayStyle = TabStyle.VisualStudio;

                this.customTabBatch.TabPages.Add("tabDetailPallets", "Pallet details          ");
                this.customTabBatch.TabPages.Add("tabDetailCartons", "Carton details          ");
                this.customTabBatch.TabPages.Add("tabDetailPacks", "Pack details          ");
                this.customTabBatch.TabPages[0].Controls.Add(this.gridexPalletDetails);
                this.customTabBatch.TabPages[1].Controls.Add(this.gridexCartonDetails);
                this.customTabBatch.TabPages[2].Controls.Add(this.gridexPackDetails);


                this.customTabBatch.Dock = DockStyle.Fill;
                this.gridexPalletDetails.Dock = DockStyle.Fill;
                this.gridexCartonDetails.Dock = DockStyle.Fill;
                this.gridexPackDetails.Dock = DockStyle.Fill;
                this.panelMaster.Controls.Add(this.customTabBatch);

                this.naviDetails.ExpandedHeight = this.naviDetails.HeaderHeight + this.textexTotalPallet.Size.Height + this.textexTotalCarton.Size.Height + this.textexTotalPack.Size.Height + this.textexRemarks.Size.Height + 5 + 5 * 10 + 3;
                this.naviDetails.Expanded = false;
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


        private void setFont(Font titleFont, Font font, Font toolbarFont)
        {
            this.customTabBatch.Font = titleFont;
            this.naviDetails.Font = titleFont;
            this.labelFillingLineName.Font = titleFont;
            this.labelFillingLineName.Left = 78;
            this.labelFillingLineName.Top = 14;

            List<Control> controls = ViewHelpers.GetAllControls(this);
            foreach (Control control in controls)
            {
                if (control is TextBox || control is ComboBox || control is DateTimePicker || control is FastObjectListView) control.Font = font;
                else if (control is DataGridView)
                {
                    DataGridView dataGridView = control as DataGridView;
                    dataGridView.ColumnHeadersDefaultCellStyle.Font = font;
                    dataGridView.RowsDefaultCellStyle.Font = font;
                }
            }


            List<Control> parentControls = ViewHelpers.GetAllControls(this.MdiParent);
            foreach (Control parentControl in parentControls)
            {
                if (parentControl is ToolStrip)
                {
                    foreach (ToolStripItem item in ((ToolStrip)parentControl).Items)
                    {
                        if (item is ToolStripLabel || item is ToolStripTextBox || item is ToolStripComboBox || item is ToolStripButton)
                            item.Font = toolbarFont;
                    }
                }
            }



        }



        Binding bindingEntryDate;
        Binding bindingReference;
        Binding bindingFillingLineName;
        Binding bindingWarehouseCode;
        Binding bindingRemarks;

        Binding bindingForkliftDriverID;
        Binding bindingStorekeeperID;

        protected override void InitializeCommonControlBinding()
        {
            base.InitializeCommonControlBinding();

            this.bindingFillingLineName = this.labelFillingLineName.DataBindings.Add("Text", this.pickupViewModel, CommonExpressions.PropertyName<PickupViewModel>(p => p.FillingLineName));

            this.bindingEntryDate = this.dateTimexEntryDate.DataBindings.Add("Value", this.pickupViewModel, CommonExpressions.PropertyName<PickupViewModel>(p => p.EntryDate), true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingReference = this.textexReference.DataBindings.Add("Text", this.pickupViewModel, CommonExpressions.PropertyName<PickupViewModel>(p => p.Reference), true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingWarehouseCode = this.textexWarehouseCode.DataBindings.Add("Text", this.pickupViewModel, CommonExpressions.PropertyName<PickupViewModel>(p => p.WarehouseName), true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingRemarks = this.textexRemarks.DataBindings.Add("Text", this.pickupViewModel, CommonExpressions.PropertyName<PickupViewModel>(p => p.Remarks), true, DataSourceUpdateMode.OnPropertyChanged);


            EmployeeAPIs employeeAPIs = new EmployeeAPIs(CommonNinject.Kernel.Get<IEmployeeAPIRepository>());

            this.combexForkliftDriverID.DataSource = employeeAPIs.GetEmployeeBases();
            this.combexForkliftDriverID.DisplayMember = CommonExpressions.PropertyName<EmployeeBase>(p => p.Name);
            this.combexForkliftDriverID.ValueMember = CommonExpressions.PropertyName<EmployeeBase>(p => p.EmployeeID);
            this.bindingForkliftDriverID = this.combexForkliftDriverID.DataBindings.Add("SelectedValue", this.pickupViewModel, CommonExpressions.PropertyName<PickupViewModel>(p => p.ForkliftDriverID), true, DataSourceUpdateMode.OnPropertyChanged);


            this.combexStorekeeperID.DataSource = employeeAPIs.GetEmployeeBases();
            this.combexStorekeeperID.DisplayMember = CommonExpressions.PropertyName<EmployeeBase>(p => p.Name);
            this.combexStorekeeperID.ValueMember = CommonExpressions.PropertyName<EmployeeBase>(p => p.EmployeeID);
            this.bindingStorekeeperID = this.combexStorekeeperID.DataBindings.Add("SelectedValue", this.pickupViewModel, CommonExpressions.PropertyName<PickupViewModel>(p => p.StorekeeperID), true, DataSourceUpdateMode.OnPropertyChanged);


            this.bindingReference.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.bindingFillingLineName.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.bindingWarehouseCode.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.bindingRemarks.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.bindingEntryDate.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);


            this.bindingForkliftDriverID.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);


            this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].Width = 10;
        }

        protected override void InitializeDataGridBinding()
        {
            this.gridexPalletDetails.AutoGenerateColumns = false;
            this.gridexPalletDetails.DataSource = this.pickupViewModel.ViewDetails;
            this.gridexCartonDetails.DataSource = this.pickupViewModel.PalletDetails;

            //StackedHeaderDecorator stackedHeaderDecorator = new StackedHeaderDecorator(this.dataGridViewDetails);
        }

        protected override Controllers.BaseController myController
        {
            get { return new PickupController(CommonNinject.Kernel.Get<IPickupService>(), this.pickupViewModel); }
        }

        public override void Loading()
        {
            this.fastPickupIndex.SetObjects(this.pickupAPIs.GetPickupIndexes());
            base.Loading();

            this.getPendingItems(); //CALL AFTER LOAD
        }

        private void getPendingItems() //THIS MAY ALSO LOAD PENDING PALLET/ CARTON/ PACK
        {
            try
            {
                this.fastPendingPallets.SetObjects(this.pickupAPIs.GetPendingPallets(this.pickupViewModel.LocationID, this.pickupViewModel.PickupID, string.Join(",", this.pickupViewModel.ViewDetails.Where(w => w.PalletID != null).Select(d => d.PalletID)), false));
                this.naviPendingItems.Text = "Pending " + this.fastPendingPallets.GetItemCount().ToString("N0") + " pallet" + (this.fastPendingPallets.GetItemCount() > 1 ? "s      " : "      ");
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        protected override DialogResult wizardMaster()
        {
            WizardMaster wizardMaster = new WizardMaster(this.pickupAPIs, this.pickupViewModel);
            return wizardMaster.ShowDialog();
        }

        private void fastPendingPallets_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.EditableMode && this.pickupViewModel.Editable)
                {
                    PendingPallet pendingPallet = (PendingPallet)this.fastPendingPallets.SelectedObject;
                    if (pendingPallet != null)
                    {
                        WizardDetail wizardDetail = new WizardDetail(this.pickupViewModel, pendingPallet);
                        if (wizardDetail.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            getPendingItems();
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        
        private void timerLoadPending_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timerLoadCallback loadPendingItemsCallback = new timerLoadCallback(getPendingItems);
                this.Invoke(loadPendingItemsCallback);
            }
            catch { }
        }



    }
}
