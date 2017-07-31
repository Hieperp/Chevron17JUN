using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalBase.Enums;
using TotalModel.Interfaces;
using TotalModel.Models;
using TotalSmartCoding.CommonLibraries;
using TotalSmartCoding.Controllers;

using TotalDAL.Repositories;
using TotalService;

namespace TotalSmartCoding.Views.Mains
{
    public partial class BaseView : Form, IMergeToolStrip, ICallToolStrip
    {
        public BaseView()
        {
            InitializeComponent();
            this.FastObjectListView = new FastObjectListView();
            this.baseController = new BaseController();
        }


        private void BaseView_Load(object sender, EventArgs e)
        {
            try
            {
                this.FastObjectListView.CheckBoxes = false;
                this.FastObjectListView.SelectedIndexChanged += new System.EventHandler(this.dataListViewMaster_SelectedIndexChanged);

                this.baseController.PropertyChanged += new PropertyChangedEventHandler(baseController_PropertyChanged);

                InitializeCommonControlBinding();
                InitializeReadOnlyModeBinding();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        public BaseController baseController { get; protected set; }


        #region <Implement Interface>

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void baseController_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }


        public GlobalEnums.TaskID TaskID
        {
            get { return GlobalEnums.TaskID.MarketingProgram; }
            //get { return this.marketingProgramBLL.TaskID; }
        }

        public virtual ToolStrip ChildToolStrip { get; set; }
        public virtual BrightIdeasSoftware.FastObjectListView FastObjectListView { get; set; }
        //{
        //    get
        //    {
        //        return this.toolStripChildForm;
        //    }
        //    set
        //    {
        //        this.toolStripChildForm = value;
        //    }
        //}



        #region Context toolbar

        public bool IsDirty
        {
            get { return this.baseController.IsDirty; }
        }

        public bool IsValid
        {
            get { return this.baseController.IsValid; }
        }

        public virtual bool Closable { get { return true; } }
        public virtual bool Loadable { get { return true; } }

        public virtual bool Newable { get { return this.baseController.BaseDTO.Editable; } }
        public virtual bool Editable { 
            get { return this.baseController.BaseDTO.Editable; } 
        }
        public virtual bool Deletable { get { return this.baseController.BaseDTO.Deletable; } }

        public virtual bool Importable { get { return true; } }
        public virtual bool Exportable { get { return true; } }

        public virtual bool Verifiable { get { return false; } }
        public virtual bool Unverifiable { get { return false; } }

        public virtual bool Printable { get { return false; } }
        public virtual bool Searchable { get { return true; } }




        /// <summary>
        /// Edit Mode: True, Read Mode: False
        /// </summary>
        private bool editableMode;
        private CheckBox checkBox1;
        private int lastMarketingProgramID;
        public bool EditableMode { get { return this.editableMode; } }
        /// <summary>
        /// This reverse of the EditableMode
        /// </summary>
        public bool ReadonlyMode { get { return !this.editableMode; } }

        /// <summary>
        /// Set the editableMode
        /// </summary>
        /// <param name="editableMode"></param>
        private void SetEditableMode(bool editableMode)
        {
            if (this.editableMode != editableMode)
            {
                //this.lastMarketingProgramID = this.marketingProgramBLL.MarketingProgramID;
                this.editableMode = editableMode;
                this.NotifyPropertyChanged("EditableMode");

                //this.toolStripMenuCustomerImport.Enabled = this.editableMode;
            }
        }


        private void CancelDirty(bool restoreSavedData)
        {
            try
            {
                //if (restoreSavedData || this.marketingProgramBLL.MarketingProgramID <= 0)
                //    this.marketingProgramBLL.Fill(this.lastMarketingProgramID);

                this.SetEditableMode(false);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }



        #endregion Context toolbar


        #region ICallToolStrip

        public void Escape()
        {
            if (this.EditableMode)
            {
                if (this.IsDirty)
                {
                    DialogResult dialogResult = MessageBox.Show(this, "Do you want to save your change?", "Warning", MessageBoxButtons.YesNoCancel);
                    if (dialogResult == DialogResult.Yes)
                    {
                        this.Save();
                        if (!this.IsDirty) this.CancelDirty(false);
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        this.CancelDirty(true);
                    }
                    else
                        return;
                }
                else
                    this.CancelDirty(false);
            }
            else
                this.Close(); //Unload this module
        }

        //Can phai xem lai trong VB de xem lai this.SetEditableMode () khi can thiet

        public void Loading()
        {
            //this.GetMasterList();
        }

        public void New()
        {
            this.ControlBox = false;
            //MessageBox.Show("New");

            string plainText = "nguyễnđạtphú";
            // Convert the plain string pwd into bytes
            //byte[] plainTextBytes = UnicodeEncoding.Unicode.GetBytes(plainText);
            //System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            //byte[] hash = hashAlgo.ComputeHash(plainTextBytes);

            byte[] data = UnicodeEncoding.Unicode.GetBytes(plainText);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = UnicodeEncoding.Unicode.GetString(data);
            MessageBox.Show(hash);

            //this.marketingProgramBLL.New();
            //this.SetEditableMode(true);

        }

        public void Edit()
        {
            this.SetEditableMode(true);
        }

        public void Save()
        {
            //this.marketingProgramBLL.Save();
            //this.GetMasterList();
        }

        public void Delete()
        {
            //DialogResult dialogResult = MessageBox.Show(this, "Are you sure you want to delete " + this.marketingProgramBLL.MarketingProgramMaster.Reference + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            //if (dialogResult == DialogResult.Yes)
            //{
            //    try
            //    {
            //        this.marketingProgramBLL.Delete();
            //        this.GetMasterList();
            //    }
            //    catch (Exception exception)
            //    {
            //        GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            //    }
            //}
        }

        public void Import()
        {
            //this.ImportExcel(OleDbDatabase.MappingTaskID.MarketingProgram);
        }

        public void Export()
        {
            //try
            //{
            //    if (this.ActiveControl.Equals(this.dataListViewMaster))
            //    {
            //        DataTable dataTableExport;
            //        dataTableExport = this.dataListViewMaster.DataSource as DataTable;
            //        if (dataTableExport != null) CommonFormAction.Export(dataTableExport);
            //    }
            //    else
            //    {
            //        List<MarketingProgramCustomerName> listExport;
            //        listExport = this.marketingProgramBLL.CustomerNameList.ToList();
            //        if (listExport != null) CommonFormAction.Export(listExport);
            //    }
            //}
            //catch (Exception exception)
            //{
            //    GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            //}
        }

        public void Verify()
        {
            MessageBox.Show("Verify");
        }

        public void Print(GlobalEnums.PrintDestination printDestination)
        {
            MessageBox.Show("Print");
        }

        public void SearchText(string searchText)
        {
            CommonFormAction.OLVFilter(this.FastObjectListView, searchText);
        }

        #endregion


        #endregion <Implement Interface>



        Binding bindingIsDirty;
        Binding bindingIsDirtyBLL;

        protected virtual void InitializeCommonControlBinding()
        {
            //IMPORTANT: SHOULD BINDING IsDirty TO CONTROL, BECAUSE: THE PropertyChanged EVENT NEED THE BINDING TARGET IN ORDER TO RAISE: SEE HERE: if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName))
            this.bindingIsDirty = this.checkBoxIsDirty.DataBindings.Add("Checked", this.baseController.BaseDTO, "IsDirty", true);
            this.bindingIsDirtyBLL = this.checkBoxIsDirtyBLL.DataBindings.Add("Checked", this.baseController, "IsDirty", true);

            this.bindingIsDirty.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);
            this.bindingIsDirtyBLL.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            //SHOULD MOVE errorProviderMaster TO EVERY VIEW
            this.errorProviderMaster.DataSource = this.baseController.BaseDTO;

        }

        protected virtual void InitializeReadOnlyModeBinding()
        {
            List<Control> controlList = CommonFormAction.GetAllControls(this);

            foreach (Control control in controlList)
            {
                //if (control is TextBox) control.DataBindings.Add("Readonly", this, "ReadonlyMode");
                if (control is TextBox) control.DataBindings.Add("Enabled", this, "EditableMode");
                else if (control is ComboBox || control is DateTimePicker) control.DataBindings.Add("Enabled", this, "EditableMode");
                else if (control is DataGridView)
                {
                    control.DataBindings.Add("Readonly", this, "ReadonlyMode");
                    control.DataBindings.Add("AllowUserToAddRows", this, "EditableMode");
                    control.DataBindings.Add("AllowUserToDeleteRows", this, "EditableMode");
                }
            }

            this.FastObjectListView.DataBindings.Add("Enabled", this, "ReadonlyMode");
        }


        protected virtual void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState == BindingCompleteState.Exception) { GlobalExceptionHandler.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }

        private void dataListViewMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView fastObjectListView = (FastObjectListView)sender;
                if (fastObjectListView.SelectedObject != null)
                {
                    IBaseIndex baseIndex = (IBaseIndex)fastObjectListView.SelectedObject;
                    if (baseIndex != null) this.baseController.Edit(baseIndex.Id);
                }
                //else this.BaseController.Open(0);
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


    }
}
