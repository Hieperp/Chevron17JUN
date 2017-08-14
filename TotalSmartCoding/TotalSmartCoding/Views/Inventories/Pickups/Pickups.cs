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

namespace TotalSmartCoding.Views.Inventories.Pickups
{
    public partial class Pickups : BaseView
    {
        private CustomTabControl customTabBatch;

        private PickupAPIs pickupAPIs;
        private PickupController pickupController;

        private System.Timers.Timer timerLoadPending;
        private delegate void timerLoadCallback();

        public Pickups()
            : base()
        {
            InitializeComponent();

            this.toolstripChild = this.toolStripChildForm;
            this.fastListIndex = this.fastPickupIndex;

            this.pickupAPIs = new PickupAPIs(CommonNinject.Kernel.Get<IPickupAPIRepository>());

            this.pickupController = new PickupController(CommonNinject.Kernel.Get<IPickupService>(), CommonNinject.Kernel.Get<PickupViewModel>());
            this.pickupController.PropertyChanged += new PropertyChangedEventHandler(baseController_PropertyChanged);

            this.baseController = this.pickupController;

            this.timerLoadPending = new System.Timers.Timer(60000);
            this.timerLoadPending.Elapsed += new System.Timers.ElapsedEventHandler(timerLoadPending_Elapsed);
            this.timerLoadPending.Enabled = true;                       
        }

        protected override void InitializeTabControl()
        {
            try
            {
                this.naviIndex.Bands[0].ClientArea.Controls.Add(this.fastPickupIndex);

                this.customTabBatch = new CustomTabControl();
                this.setFont(new Font("Niagara Engraved", 16), new Font("Sitka Banner", 16), new Font("Niagara Engraved", 16));

                this.customTabBatch.DisplayStyle = TabStyle.VisualStudio;

                this.customTabBatch.TabPages.Add("tabDetailPallets", "Detail pallets          ");
                this.customTabBatch.TabPages.Add("tabDetailCartons", "Detail cartons          ");
                this.customTabBatch.TabPages.Add("tabDetailPacks", "Detail packs          ");
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

        protected override void InitializeCommonControlBinding()
        {
            base.InitializeCommonControlBinding();

            this.bindingReference = this.textexReference.DataBindings.Add("Text", this.pickupController.PickupViewModel, "Reference", true, DataSourceUpdateMode.OnPropertyChanged);
            this.bindingEntryDate = this.dateTimexEntryDate.DataBindings.Add("Value", this.pickupController.PickupViewModel, "EntryDate", true);


            this.bindingReference.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.bindingEntryDate.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);






            //this.naviGroupDetails.DataBindings.Add("ExpandedHeight", this.numericUpDownSizingDetail, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            //this.numericUpDownSizingDetail.Minimum = this.naviGroupDetails.HeaderHeight * 2;
            //this.numericUpDownSizingDetail.Maximum = this.naviGroupDetails.Height + this.fastPickupIndex.Height;

            this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelMaster.ColumnStyles[this.tableLayoutPanelMaster.ColumnCount - 1].Width = 10;
            //this.tableLayoutPanelExtend.ColumnStyles[this.tableLayoutPanelExtend.ColumnCount - 1].SizeType = SizeType.Absolute; this.tableLayoutPanelExtend.ColumnStyles[this.tableLayoutPanelExtend.ColumnCount - 1].Width = 10;
        }

        protected override void InitializeDataGridBinding()
        {
            this.gridexPalletDetails.AutoGenerateColumns = false;
            this.gridexPalletDetails.DataSource = this.pickupController.PickupViewModel.ViewDetails;
            this.gridexCartonDetails.DataSource = this.pickupController.PickupViewModel.PalletDetails;

            //StackedHeaderDecorator stackedHeaderDecorator = new StackedHeaderDecorator(this.dataGridViewDetails);
        }

        public override void Loading()
        {
            this.fastPickupIndex.SetObjects(this.pickupAPIs.GetPickupIndexes());
            base.Loading();

            this.LoadPendingItems(); //CALL AFTER LOAD
        }

        private void LoadPendingItems() //THIS MAY ALSO LOAD PENDING PALLET/ CARTON/ PACK
        {
            try
            {
                this.fastPendingPallets.SetObjects(this.pickupAPIs.GetPendingPallets(this.pickupController.PickupViewModel.LocationID, this.pickupController.PickupViewModel.PickupID, string.Join(",", this.pickupController.PickupViewModel.ViewDetails.Where(w => w.PalletID != null).Select(d => d.PalletID)), false));
                this.naviPendingItems.Text = "Pending " + this.fastPendingPallets.GetItemCount().ToString("N0") + " pallet" + (this.fastPendingPallets.GetItemCount() > 1 ? "s      " : "      ");
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        protected override DialogResult wizardMaster()
        {
            WizardMaster a = new WizardMaster(this.pickupAPIs, this.pickupController.PickupViewModel);
            a.ShowDialog();

            WizardMaster wizardMaster = new WizardMaster(this.pickupAPIs, this.pickupController.PickupViewModel);
            return wizardMaster.ShowDialog();
        }

        protected override void wizardDetail()
        {
            base.wizardDetail();
            WizardDetail wizardDetail = new WizardDetail(this.pickupAPIs, this.pickupController.PickupViewModel);
            wizardDetail.ShowDialog();
        }


        private void Pickups_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timerLoadPending.Enabled = false;
        }

        private void timerLoadPending_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timerLoadCallback loadPendingItemsCallback = new timerLoadCallback(LoadPendingItems);
                this.Invoke(loadPendingItemsCallback);
            }
            catch { }
        }

    }
}
