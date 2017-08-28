using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalBase.Enums;
using TotalSmartCoding.Libraries.Helpers;
using TotalSmartCoding.Views.Commons;

using TotalSmartCoding.Views.Productions;
using TotalSmartCoding.Views.Inventories.Pickups;
using TotalSmartCoding.Views.Inventories.GoodsReceipts;
using System.IO;
using System.Reflection;
using TotalBase;

namespace TotalSmartCoding.Views.Mains
{
    public partial class MasterMDI : Form
    {
        #region Contractor


        Binding beginingDateBinding;
        Binding endingDateBinding;

        Binding buttonNaviBarHeaderVisibleBinding;

        private GlobalEnums.NmvnTaskID nmvnTaskID;


        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, String pszSubAppName, String pszSubIdList);

        public MasterMDI()
            : this(GlobalEnums.NmvnTaskID.UnKnown)
        { }

        public MasterMDI(GlobalEnums.NmvnTaskID nmvnTaskID)
            : this(nmvnTaskID, null)
        { }

        public MasterMDI(Form childForm)
            : this(GlobalEnums.NmvnTaskID.UnKnown, childForm)
        { }

        public MasterMDI(GlobalEnums.NmvnTaskID nmvnTaskID, Form childForm)
        {
            InitializeComponent();


            try
            {
                this.nmvnTaskID = nmvnTaskID;
                if (this.nmvnTaskID == GlobalEnums.NmvnTaskID.Batch) { this.Size = new Size(1120, 630); this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog; this.MinimizeBox = false; this.MaximizeBox = false; this.WindowState = FormWindowState.Normal; }


                this.beginingDateBinding = this.textFillterLowerDate.TextBox.DataBindings.Add("Text", GlobalEnums.GlobalOptionSetting, "LowerFillterDate", true);
                this.endingDateBinding = this.textFillterUpperDate.TextBox.DataBindings.Add("Text", GlobalEnums.GlobalOptionSetting, "UpperFillterDate", true);


                this.beginingDateBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
                this.endingDateBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);



                this.buttonNaviBarHeaderVisibleBinding = this.buttonNaviBarHeader.DataBindings.Add("Visible", this.naviBarModuleMaster, "Collapsed", true, DataSourceUpdateMode.OnPropertyChanged);
                this.buttonNaviBarHeaderVisibleBinding.Parse += new ConvertEventHandler(buttonNaviBarHeaderVisibleBinding_Parse);
                this.buttonNaviBarHeaderVisibleBinding.Format += new ConvertEventHandler(buttonNaviBarHeaderVisibleBinding_Format);

                this.listViewTaskMaster.Dock = DockStyle.Fill;
                this.listViewTaskMaster.Columns.Add(new ColumnHeader() { Width = this.listViewTaskMaster.Width });




                //InitializeModuleMaster();

                if (childForm != null)
                {

                    childForm.MdiParent = this;
                    //childForm.Owner = this;
                    childForm.WindowState = FormWindowState.Maximized;
                    childForm.Show();
                }
                else
                    OpenTestView();

                DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
                this.statusVersion.Text = "Version 1.0i Date: " + buildDate.ToString("dd/MM/yyyy hh:mm:ss");

                this.statusFillingLine.Text = GlobalVariables.FillingLineName;
                this.statusUserDescription.Text = ContextAttributes.User.UserDescription;

            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


        void buttonNaviBarHeaderVisibleBinding_Parse(object sender, ConvertEventArgs e)
        {
            e.Value = !(bool)e.Value;
        }

        void buttonNaviBarHeaderVisibleBinding_Format(object sender, ConvertEventArgs e)
        {
            e.Value = !(bool)e.Value;
        }

        private void buttonNaviBarHeader_Click(object sender, EventArgs e)
        {
            this.naviBarModuleMaster.Collapsed = true;
        }

        private void naviBarModuleMaster_CollapsedChanged(object sender, EventArgs e)
        {
            this.listViewTaskMaster.Columns[0].Width = this.listViewTaskMaster.Columns[0].Width + (this.naviBarModuleMaster.Collapsed ? -4 : 4);
        }


        private void naviBarModuleMaster_ActiveBandChanged(object sender, EventArgs e)
        {
            try
            {
                this.buttonNaviBarHeader.Text = this.naviBarModuleMaster.ActiveBand.Text;

                this.listViewTaskMaster.Parent = null;
                this.naviBarModuleMaster.ActiveBand.ClientArea.Controls.Add(this.listViewTaskMaster);
                this.listViewTaskMaster.Visible = true;
                SetWindowTheme(listViewTaskMaster.Handle, "explorer", null);

                //int moduleID; if (int.TryParse(this.naviBarModuleMaster.ActiveBand.Tag.ToString(), out  moduleID)) InitializeTaskMaster(moduleID);
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }
        private void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState == BindingCompleteState.Exception) { ExceptionHandlers.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }

        #endregion Contractor


        #region Form Events: Merge toolstrip & Set toolbar context
        private void MasterMdi_MdiChildActivate(object sender, EventArgs e)
        {
            try
            {
                ToolStripManager.RevertMerge(this.toolstripMain);
                IToolstripMerge mergeToolstrip = ActiveMdiChild as IToolstripMerge;
                if (mergeToolstrip != null)
                {
                    ToolStripManager.Merge(mergeToolstrip.toolstripChild, toolstripMain);
                }

                IToolstripChild toolstripChild = ActiveMdiChild as IToolstripChild;
                if (toolstripChild != null)
                {
                    toolstripChild.PropertyChanged -= new PropertyChangedEventHandler(toolstripChild_PropertyChanged);
                    toolstripChild.PropertyChanged += new PropertyChangedEventHandler(toolstripChild_PropertyChanged);

                    toolstripChild_PropertyChanged(toolstripChild, new PropertyChangedEventArgs("IsDirty"));
                }
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolstripChild_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                IToolstripChild toolstripChild = sender as IToolstripChild;
                if (toolstripChild != null)
                {

                    bool closable = toolstripChild.Closable;
                    bool loadable = toolstripChild.Loadable;
                    bool newable = toolstripChild.Newable;
                    bool editable = toolstripChild.Editable;
                    bool isDirty = toolstripChild.IsDirty;
                    bool deletable = toolstripChild.Deletable;
                    bool importable = toolstripChild.Importable;
                    bool exportable = toolstripChild.Exportable;
                    bool approvable = toolstripChild.Approvable;
                    bool unapprovable = toolstripChild.Unapprovable;
                    bool printable = toolstripChild.Printable;
                    bool readonlyMode = toolstripChild.ReadonlyMode;
                    bool editableMode = toolstripChild.EditableMode;
                    bool isValid = toolstripChild.IsValid;


                    this.buttonEscape.Enabled = closable;
                    this.buttonLoading.Enabled = loadable && readonlyMode;

                    this.buttonNew.Enabled = newable && readonlyMode;
                    this.buttonEdit.Enabled = editable && readonlyMode;
                    this.buttonSave.Enabled = isDirty && isValid && editableMode;
                    this.buttonDelete.Enabled = deletable && readonlyMode;

                    this.buttonImport.Visible = importable;
                    this.buttonImport.Enabled = importable && newable && readonlyMode;
                    this.buttonExport.Visible = exportable;
                    this.buttonExport.Enabled = exportable;//&& !isDirty && readonlyMode;
                    this.toolStripSeparatorImport.Visible = importable || exportable;

                    this.buttonApprove.Visible = approvable || unapprovable;
                    this.buttonApprove.Enabled = approvable || unapprovable;
                    this.buttonApprove.Text = approvable ? "Verify" : "Unverify";
                    this.toolStripSeparatorApprove.Visible = approvable || unapprovable;

                    this.buttonPrint.Visible = printable;
                    this.buttonPrint.Enabled = printable;
                    this.buttonPrintPreview.Visible = printable;
                    this.buttonPrintPreview.Enabled = printable;
                    this.toolStripSeparatorPrint.Visible = printable;
                }
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        #endregion Form Events


        #region <Call Tool Strip>
        private void buttonEscape_Click(object sender, EventArgs e)
        {
            try
            {
                IToolstripChild toolstripChild = ActiveMdiChild as IToolstripChild;
                if (toolstripChild != null) toolstripChild.Escape();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void buttonLoading_Click(object sender, EventArgs e)
        {
            try
            {
                IToolstripChild toolstripChild = ActiveMdiChild as IToolstripChild;
                if (toolstripChild != null) toolstripChild.Loading();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


        private void comboFilterTexts_TextChanged(object sender, EventArgs e)
        {
            try
            {
                IToolstripChild toolstripChild = ActiveMdiChild as IToolstripChild;
                if (toolstripChild != null) toolstripChild.ApplyFilter(this.comboFilterTexts.Text);
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void buttonClearFilters_Click(object sender, EventArgs e)
        {
            this.comboFilterTexts.Text = "";
        }


        private void buttonNew_Click(object sender, EventArgs e)
        {
            try
            {
                IToolstripChild toolstripChild = ActiveMdiChild as IToolstripChild;
                if (toolstripChild != null) toolstripChild.New();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


        private void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                IToolstripChild toolstripChild = ActiveMdiChild as IToolstripChild;
                if (toolstripChild != null) toolstripChild.Edit();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                IToolstripChild toolstripChild = ActiveMdiChild as IToolstripChild;
                if (toolstripChild != null) toolstripChild.Save();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                IToolstripChild toolstripChild = ActiveMdiChild as IToolstripChild;
                if (toolstripChild != null) toolstripChild.Delete();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


        private void toolStripButtonImport_Click(object sender, EventArgs e)
        {
            try
            {
                IToolstripChild toolstripChild = ActiveMdiChild as IToolstripChild;
                if (toolstripChild != null) toolstripChild.Import();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonExport_Click(object sender, EventArgs e)
        {
            try
            {
                IToolstripChild toolstripChild = ActiveMdiChild as IToolstripChild;
                if (toolstripChild != null) toolstripChild.Export();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void buttonApprove_Click(object sender, EventArgs e)
        {
            try
            {                
                IToolstripChild toolstripChild = ActiveMdiChild as IToolstripChild;
                if (toolstripChild != null) toolstripChild.Approve();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {
            try
            {
                IToolstripChild toolstripChild = ActiveMdiChild as IToolstripChild;
                if (toolstripChild != null) toolstripChild.Print(GlobalEnums.PrintDestination.Print);
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonPrintPreview_Click(object sender, EventArgs e)
        {
            try
            {
                IToolstripChild toolstripChild = ActiveMdiChild as IToolstripChild;
                if (toolstripChild != null) toolstripChild.Print(GlobalEnums.PrintDestination.PrintPreview);
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        #endregion <Call Tool Strip>



        private void OpenTestView()
        {
            return;

            //Open new form
            Pickups grForm;  //
            //GoodsReceipts grForm;
            //childForm = new Batches();
            //childForm = new DeliveryAdvices();
            grForm = new Pickups(); //
            //grForm = new GoodsReceipts();

            if (grForm != null)
            {
                grForm.MdiParent = this;
                grForm.WindowState = FormWindowState.Maximized;
                //childForm.ControlBox = false;
                grForm.Show();
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenTestView();
        }












        private void MasterMdi_Load(object sender, EventArgs e)
        {
            //this.toolStripMDIMain.Visible = false;

            try
            {
                Form childForm;

                switch (this.nmvnTaskID)
                {

                    case GlobalEnums.NmvnTaskID.SmartCoding:

                        this.buttonEscape.Visible = false;
                        this.buttonLoading.Visible = false;
                        this.buttonNew.Visible = false;
                        this.buttonEdit.Visible = false;
                        this.buttonSave.Visible = false;
                        this.buttonDelete.Visible = false;
                        this.buttonImport.Visible = false;
                        this.buttonExport.Visible = false;
                        this.toolStripSeparatorImport.Visible = false;
                        this.buttonApprove.Visible = false;
                        this.toolStripSeparatorApprove.Visible = false;
                        this.buttonPrint.Visible = false;
                        this.buttonPrintPreview.Visible = false;
                        this.toolStripSeparatorPrint.Visible = false;

                        this.separatorESC.Visible = false;
                        this.toolStrip1.Visible = false;
                        this.naviBarModuleMaster.Visible = false;

                        childForm = new SmartCoding();
                        break;
                    default:
                        childForm = new BaseView();
                        break;
                }

                if (this.nmvnTaskID != GlobalEnums.NmvnTaskID.UnKnown && this.nmvnTaskID != GlobalEnums.NmvnTaskID.Batch)
                {
                    childForm.MdiParent = this;
                    //childForm.WindowState = FormWindowState.Maximized;
                    childForm.Show();
                }
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }

        }

        private void MasterMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                for (int i = 0; i < this.MdiChildren.Length; i++)
                {
                    IToolstripChild mdiChildCallToolStrip = this.MdiChildren[i] as IToolstripChild;
                    if (mdiChildCallToolStrip != null)
                    {
                        if (mdiChildCallToolStrip.ReadonlyMode) ((Form)mdiChildCallToolStrip).Close();
                    }
                    else
                        this.MdiChildren[i].Close();
                }

                if (this.MdiChildren.Length > 0)
                    e.Cancel = true;
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }



    }
}
