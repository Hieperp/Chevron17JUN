using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using TotalBase.Enums;
using TotalSmartCoding.CommonLibraries;


namespace TotalSmartCoding.Views.Mains
{
    public class BasicView : Form, IMergeToolStrip, ICallToolStrip
    {



        #region <Implement Interface>

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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

        public virtual bool Verifiable { get { return false; } }
        public virtual bool Unverifiable { get { return false; } }

        public virtual bool Printable { get { return false; } }
        public virtual bool Searchable { get { return true; } }




        /// <summary>
        /// Edit Mode: True, Read Mode: False
        /// </summary>
        private bool editableMode;
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

            string plainText ="nguyễnđạtphú";
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
            //this.SetEditableMode(true);
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BasicView
            // 
            this.ClientSize = new System.Drawing.Size(861, 375);
            this.Name = "BasicView";
            this.ResumeLayout(false);

        }



        #endregion <Implement Interface>




    }
}
