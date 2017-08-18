using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalBase.Enums;
using TotalSmartCoding.Libraries.Helpers;


using TotalSmartCoding.Controllers;

namespace TotalSmartCoding.Views.Mains
{
    public class BasicView : Form, IToolstripMerge, IToolstripChild
    {

        protected BaseController baseController { get; private set; }

        #region <Implement Interface>

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public GlobalEnums.NmvnTaskID NMVNTaskID
        {
            get { return GlobalEnums.NmvnTaskID.UnKnown ; }
            //get { return this.marketingProgramBLL.TaskID; }
        }

        public virtual ToolStrip toolstripChild { get; set; }
        //public virtual BrightIdeasSoftware.FastObjectListView FastObjectListView { get; set; }
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

        public virtual bool IsDirty { get { return false; } }
        public virtual bool IsValid { get { return false; } }

        //public bool IsDirty
        //{
        //    get { return this.marketingProgramBLL.IsDirty; }
        //}

        //public bool IsValid
        //{
        //    get { return this.marketingProgramBLL.IsValid; }
        //}

        public virtual bool Closable { get { return true; } }
        public virtual bool Loadable { get { return true; } }

        public virtual bool Newable { get { return true; } }
        public virtual bool Editable { get { return false; } }
        public virtual bool Deletable { get { return false; } }

        public virtual bool Importable { get { return true; } }
        public virtual bool Exportable { get { return true; } }

        public virtual bool Approvable { get { return false; } }
        public virtual bool Unapprovable { get { return false; } }

        public virtual bool Printable { get { return false; } }
        public virtual bool Filterable { get { return true; } }




        /// <summary>
        /// Edit Mode: True, Read Mode: False
        /// </summary>
        private bool editableMode;
        private CheckBox checkBox1;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
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
            //if (this.editableMode != editableMode)
            //{
            //    this.lastMarketingProgramID = this.marketingProgramBLL.MarketingProgramID;
            //    this.editableMode = editableMode;
            //    this.NotifyPropertyChanged("EditableMode");

            //    this.toolStripMenuCustomerImport.Enabled = this.editableMode;
            //}
        }


        private void CancelDirty(bool restoreSavedData)
        {
            //try
            //{
            //    if (restoreSavedData || this.marketingProgramBLL.MarketingProgramID <= 0)
            //        this.marketingProgramBLL.Fill(this.lastMarketingProgramID);

            //    this.SetEditableMode(false);
            //}
            //catch (Exception exception)
            //{
            //    throw exception;
            //}
        }



        #endregion Context toolbar



        #region ICallToolStrip

        public void Escape()
        {
            if (this.EditableMode)
            {
                if (this.IsDirty)
                {
                    DialogResult dialogResult = CustomMsgBox.Show(this, "Do you want to save your change?", "Warning", MessageBoxButtons.YesNoCancel);
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
            //CustomMsgBox.Show(Form.ActiveForm, "New");

            string plainText ="nguyễnđạtphú";
            // Convert the plain string pwd into bytes
            //byte[] plainTextBytes = UnicodeEncoding.Unicode.GetBytes(plainText);
            //System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            //byte[] hash = hashAlgo.ComputeHash(plainTextBytes);

            byte[] data = UnicodeEncoding.Unicode.GetBytes(plainText);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = UnicodeEncoding.Unicode.GetString(data);
            CustomMsgBox.Show(this, hash);

            //this.marketingProgramBLL.New();
            //this.SetEditableMode(true);
        }

        public void Edit()
        {
            //this.SetEditableMode(true);
        }

        public void Save()
        {
            //this.marketingProgramBLL.Save();
            //this.GetMasterList();
        }

        public void Delete()
        {
            //DialogResult dialogResult = CustomMsgBox.Show(this, "Are you sure you want to delete " + this.marketingProgramBLL.MarketingProgramMaster.Reference + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
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

        public void Approve()
        {
            CustomMsgBox.Show(Form.ActiveForm, "Verify");
        }

        public void Print(GlobalEnums.PrintDestination printDestination)
        {
            CustomMsgBox.Show(Form.ActiveForm, "Print");
        }

        public void ApplyFilter(string searchText)
        {
            //CommonFormAction.OLVFilter(this.FastObjectListView, searchText);
        }

        #endregion

        private void InitializeComponent()
        {
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(629, 112);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(98, 21);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1107, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::TotalSmartCoding.Properties.Resources.Hopstarter_Warning;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // BasicView
            // 
            this.ClientSize = new System.Drawing.Size(1107, 395);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.checkBox1);
            this.Name = "BasicView";
            this.Load += new System.EventHandler(this.BasicView_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion <Implement Interface>

        private void BasicView_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeReadOnlyModeBinding();

                //this.FastObjectListView.SelectedIndexChanged += new System.EventHandler(this.dataListViewMaster_SelectedIndexChanged);
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }



        protected virtual void InitializeReadOnlyModeBinding()
        {
            List<Control> controlList = ViewHelpers.GetAllControls(this);

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

            //this.FastObjectListView.DataBindings.Add("Enabled", this, "ReadonlyMode");
        }




        private void dataListViewMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //ObjectListView objectListView = (ObjectListView)sender;
                //DataRowView row = (DataRowView)objectListView.SelectedObject;

                //if (row != null)
                //{
                //    int dtoID;

                //    if (int.TryParse(row.Row["MarketingIncentiveID"].ToString(), out dtoID)) this.marketingIncentiveBLL.Fill(dtoID);
                //    else this.marketingIncentiveBLL.Fill(0);
                //}
                //else this.marketingIncentiveBLL.Fill(0);
            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }


    }
}
