using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;

using CustomControls;
using BrightIdeasSoftware;

using TotalDTO;
using TotalBase.Enums;
using TotalModel.Interfaces;
using TotalSmartCoding.Libraries.Helpers;
using TotalSmartCoding.Controllers;

namespace TotalSmartCoding.Views.Mains
{
    public partial class BaseView : Form, IToolstripMerge, IToolstripChild
    {
        #region CONTRUCTOR
        protected BaseDTO baseDTO { get; set; }

        public BaseView()
        {
            InitializeComponent();

            this.baseDTO = new BaseDTO(); //JUST FOR CREATE AN EMPTY BaseDTO IN BaseView AT DESIGN TIME ONLY (NOT FUNCTIONAL AT RUN TIME) 

            this.fastListIndex = new FastObjectListView();
        }

        private void BaseView_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeTabControl();

                this.fastListIndex.CheckBoxes = false;
                this.fastListIndex.SelectedIndexChanged += new EventHandler(this.fastListIndex_SelectedIndexChanged);
                this.fastListIndex.MouseClick += new MouseEventHandler(fastListIndex_MouseClick);
                this.fastListIndex.KeyDown += new KeyEventHandler(fastListIndex_KeyDown);

                this.baseDTO.PropertyChanged += new PropertyChangedEventHandler(ModelDTO_PropertyChanged);

                this.InitializeCommonControlBinding();
                this.InitializeDataGridBinding();
                this.InitializeReadOnlyModeBinding();

                this.Loading();
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        Binding bindingIsDirty;

        protected virtual void InitializeTabControl() { }

        protected virtual void InitializeCommonControlBinding()
        {
            //IMPORTANT: SHOULD BINDING IsDirty TO CONTROL, BECAUSE: THE PropertyChanged EVENT NEED THE BINDING TARGET IN ORDER TO RAISE: SEE HERE: if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName))
            this.bindingIsDirty = this.checkBoxIsDirty.DataBindings.Add("Checked", this.baseDTO, "IsDirty", true);

            this.bindingIsDirty.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

            this.errorProviderMaster.DataSource = this.baseDTO; //JUST SET this.errorProviderMaster.DataSource HERE, IT WILL PROVIDE ERROR BINDING TO EVERY VIEW
        }

        protected virtual void InitializeDataGridBinding() { }

        protected virtual void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState == BindingCompleteState.Exception) { ExceptionHandlers.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
        }

        protected virtual void InitializeReadOnlyModeBinding()
        {
            List<Control> controlList = ViewHelpers.GetAllControls(this);

            foreach (Control control in controlList)
            {
                IControlExtension controlExtension = control as IControlExtension;
                if (controlExtension != null)
                {
                    if (controlExtension.Editable)
                    {
                        if (control is DataGridexView)
                        {
                            DataGridexView dataGridexView = control as DataGridexView;
                            if (dataGridexView != null && dataGridexView.AllowAddRow)
                                control.DataBindings.Add("AllowUserToAddRows", this, "EditableMode");
                            else
                                ((DataGridView)control).AllowUserToAddRows = false;

                            if (dataGridexView != null && dataGridexView.AllowDeleteRow)
                                control.DataBindings.Add("AllowUserToDeleteRows", this, "EditableMode");
                            else
                                ((DataGridView)control).AllowUserToDeleteRows = false;
                        }

                        control.DataBindings.Add("ReadOnly", this, "ReadonlyMode");
                    }
                    else
                        controlExtension.ReadOnly = true;
                }
            }

            //this.fastListIndex.DataBindings.Add("Enabled", this, "ReadonlyMode"); //HERE: WE DON'T LOCK fastListIndex.Enabled TO ReadonlyMode, INSTEAD: WE HANDLE fastListIndex.MouseClick AND fastListIndex.KeyDown TO KEEP THE CURRENT ROW OF fastListIndex WHEN EditableMode
        }
        #endregion CONTRUCTOR

        #region EventHandlers
        private void fastListIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ReadonlyMode)
                {
                    int? selectedIndexID = this.getSelectedIndexID();
                    if (selectedIndexID != null && selectedIndexID != this.baseDTO.GetID()) this.invokeEdit(selectedIndexID);
                }
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void fastListIndex_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.EditableMode)
            {
                int? selectedIndexID = this.getSelectedIndexID();
                if (selectedIndexID != null && selectedIndexID != this.baseDTO.LastID)
                    this.setSelectedIndexID(this.baseDTO.LastID);
            }
        }

        private void fastListIndex_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.EditableMode) e.Handled = true;
        }

        private int? getSelectedIndexID()
        {
            if (this.fastListIndex.SelectedObject != null)
            {
                IBaseIndex baseIndex = (IBaseIndex)this.fastListIndex.SelectedObject;
                if (baseIndex != null) return baseIndex.Id; else return null;
            }
            else return null;
        }

        private void setSelectedIndexID(int? baseIndexID)
        {
            //if (baseIndexID != null && baseIndexID > 0)
            //{
                IBaseIndex baseIndex = this.fastListIndex.Objects.Cast<IBaseIndex>().FirstOrDefault(w => w.Id == baseIndexID);
                if (baseIndex == null && (IBaseIndex)this.fastListIndex.SelectedObject != null)
                    fastListIndex_SelectedIndexChanged(this.fastListIndex, new EventArgs());
                else
                {
                    if (baseIndex == null) baseIndex = this.fastListIndex.Objects.Cast<IBaseIndex>().FirstOrDefault();
                    if (baseIndex != null)
                    {
                        this.fastListIndex.SelectObject(baseIndex);
                        this.fastListIndex.EnsureModelVisible(baseIndex);
                    }
                }
            //}
            //else
            //    if (this.ReadonlyMode && this.fastListIndex.GetItemCount() > 0) this.fastListIndex.SelectedIndex = 0;
        }

        protected virtual bool checkSelectedIndexID()
        {
            if (this.baseDTO.GetID() > 0)
            {
                this.setSelectedIndexID(this.baseDTO.GetID());
                if (this.baseDTO.GetID() == this.getSelectedIndexID())
                    return true;
            }

            throw new System.ArgumentException("Lỗi", "Vui lòng chọn dữ liệu.");
        }

        #endregion EventHandlers

        #region <Implement Interface>

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void ModelDTO_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged(e.PropertyName);
        }


        public GlobalEnums.NmvnTaskID NMVNTaskID
        {
            get { return this.baseDTO.NMVNTaskID; }
        }

        public virtual ToolStrip toolstripChild { get; protected set; }
        protected virtual FastObjectListView fastListIndex { get; set; }

        #region Context toolbar

        public bool IsDirty
        {
            get { return this.baseDTO.IsDirty; }
        }

        public bool IsValid
        {
            get { return this.baseDTO.IsValid; }
        }

        public virtual bool Closable { get { return true; } }
        public virtual bool Loadable { get { return true; } }

        public virtual bool Newable { get { return this.baseDTO.Newable; } }
        public virtual bool Editable { get { return this.baseDTO.Editable; } }
        public virtual bool Deletable { get { return this.baseDTO.Deletable; } }

        public virtual bool Importable { get { return false; } }
        public virtual bool Exportable { get { return false; } }

        public virtual bool Approvable { get { return false; } }
        public virtual bool Unapprovable { get { return false; } }

        public virtual bool Printable { get { return false; } }
        public virtual bool Filterable { get { return true; } }


        private bool editableMode;
        public bool EditableMode
        {
            get { return this.editableMode; }
            set
            {
                if (this.editableMode != value)
                {
                    this.editableMode = value && this.Editable;
                    this.NotifyPropertyChanged("EditableMode");
                    this.NotifyPropertyChanged("ReadonlyMode");
                }
            }
        }
        public bool ReadonlyMode { get { return !this.editableMode; } }

        #endregion Context toolbar


        #region IToolstripChild

        protected virtual BaseController myController { get { return new BaseController(); } }

        public virtual void Loading()
        {
            this.setSelectedIndexID(this.baseDTO.GetID());

            if (this.ReadonlyMode) this.invokeEdit(this.baseDTO.GetID()); //THIS LINE IS FOR REFRESH THE STATE OF THE CURRENT ENTITY (Editable/ Deletable/ ...)=> THIS MAY BE NOT NECCESSARY IN SOME CASE => LATER: WE SHOULD TRY TO REFRESH BY A BETTER WAY: TO REFRESH WHEN NECCESSARY ONLY
        }

        public void ApplyFilter(string filterTexts)
        {
            OLVHelpers.ApplyFilters(this.fastListIndex, filterTexts.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
        }

        public void Escape()
        {
            if (this.EditableMode)
            {
                if (this.IsDirty)
                {
                    DialogResult dialogResult = CustomMsgBox.Show(this, "Dữ liệu chưa lưu. Bạn có muốn lưu lại không?", "Warning", MessageBoxButtons.YesNoCancel);
                    if (dialogResult == DialogResult.Yes)
                    {
                        this.Save();
                        if (!this.IsDirty) this.CancelDirty(false);
                    }
                    else if (dialogResult == DialogResult.No)
                        this.CancelDirty(true);
                    else
                        return;
                }
                else
                    this.CancelDirty(false);
            }
            else
                this.Close(); //Unload this module
        }

        private void CancelDirty(bool withRestore)
        {
            this.myController.CancelDirty(withRestore);
            this.EditableMode = false;
        }

        public void New()
        {
            //this.ControlBox = false;

            //string plainText = "nguyễnđạtphú";
            //////////// Convert the plain string pwd into bytes
            ////////////byte[] plainTextBytes = UnicodeEncoding.Unicode.GetBytes(plainText);
            ////////////System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            ////////////byte[] hash = hashAlgo.ComputeHash(plainTextBytes);

            //byte[] data = UnicodeEncoding.Unicode.GetBytes(plainText);
            //data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            //String hash = UnicodeEncoding.Unicode.GetString(data);
            //CustomMsgBox.Show(this, hash);


            this.myController.Create();
            if (this.wizardMaster() == DialogResult.OK)
            {
                this.EditableMode = true;
                this.wizardDetail();
            }
            else
                this.CancelDirty(true);
        }

        protected virtual DialogResult wizardMaster()
        {
            return DialogResult.OK;
        }

        protected virtual void wizardDetail() { }

        public void Edit()
        {
            if (this.checkSelectedIndexID())
            {
                this.invokeEdit(this.baseDTO.GetID());
                this.EditableMode = true;
            }
        }

        private void invokeEdit(int? id)
        {
            this.myController.Edit(id);
        }

        public void Save()
        {
            try
            {
                if (this.myController.Save())
                {
                    this.Escape();
                    this.Loading();
                }
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        public void Delete()
        {
            try
            {
                if (this.checkSelectedIndexID())
                {
                    if (CustomMsgBox.Show(this, "Are you sure you want to delete " + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                        if (this.myController.Delete(this.baseDTO.GetID()))
                            this.Loading();
                }
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


        public void Approve()
        {
            try
            {
                if (this.checkSelectedIndexID())
                {
                    this.myController.Approve(this.baseDTO.GetID());

                    if (this.ApproveCheck(this.baseDTO.GetID()) && CustomMsgBox.Show(this, "Cài đặt batch này cho sản xuất " + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                        if (this.myController.ApproveConfirmed())
                        {
                            this.ApproveMore(this.baseDTO.GetID());
                            this.Loading();
                        }
                }
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        protected virtual bool ApproveCheck(int id) { return true; }
        protected virtual void ApproveMore(int id) { }

        public void Void()
        {
            try
            {
                if (this.checkSelectedIndexID())
                {
                    this.myController.Void(this.baseDTO.GetID());

                    if (this.VoidCheck(this.baseDTO.GetID()) && CustomMsgBox.Show(this, "Dừng sản xuất batch này" + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                        if (this.myController.VoidConfirmed())
                        {
                            this.VoidMore(this.baseDTO.GetID());
                            this.Loading();
                        }
                }
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        protected virtual bool VoidCheck(int id) { return true; }
        protected virtual void VoidMore(int id) { }


        public void Print(GlobalEnums.PrintDestination printDestination)
        {
            CustomMsgBox.Show(Form.ActiveForm, "Print");
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



        #endregion


        #endregion <Implement Interface>
    }
}
