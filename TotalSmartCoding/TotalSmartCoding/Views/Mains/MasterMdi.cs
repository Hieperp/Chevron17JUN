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

using TotalSmartCoding.CommonLibraries;

using TotalSmartCoding.Views.Commons;

namespace TotalSmartCoding.Views.Mains
{
    public partial class MasterMdi : Form
    {
        public MasterMdi()
        {
            InitializeComponent();
        }

        private void MasterMdi_Load(object sender, EventArgs e)
        {

        }




        #region Form Events: Merge toolstrip & Set toolbar context
        private void MasterMdi_MdiChildActivate(object sender, EventArgs e)
        {       
            try
            {
                ToolStripManager.RevertMerge(this.toolStripMDIMain);
                IMergeToolStrip mdiChildMergeToolStrip = ActiveMdiChild as IMergeToolStrip;
                if (mdiChildMergeToolStrip != null)
                {
                    ToolStripManager.Merge(mdiChildMergeToolStrip.ChildToolStrip, toolStripMDIMain);
                }

                ICallToolStrip mdiChildCallToolStrip = ActiveMdiChild as ICallToolStrip;
                if (mdiChildCallToolStrip != null)
                {
                    mdiChildCallToolStrip.PropertyChanged -= new PropertyChangedEventHandler(mdiChildCallToolStrip_PropertyChanged);
                    mdiChildCallToolStrip.PropertyChanged += new PropertyChangedEventHandler(mdiChildCallToolStrip_PropertyChanged);

                    mdiChildCallToolStrip_PropertyChanged(mdiChildCallToolStrip, new PropertyChangedEventArgs("IsDirty"));
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        void mdiChildCallToolStrip_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                ICallToolStrip mdiChildCallToolStrip = sender as ICallToolStrip;
                if (mdiChildCallToolStrip != null)
                {

                    bool closable = mdiChildCallToolStrip.Closable;
                    bool loadable = mdiChildCallToolStrip.Loadable;
                    bool newable = mdiChildCallToolStrip.Newable;
                    bool editable = mdiChildCallToolStrip.Editable;
                    bool isDirty = mdiChildCallToolStrip.IsDirty;
                    bool deletable = mdiChildCallToolStrip.Deletable;
                    bool importable = mdiChildCallToolStrip.Importable;
                    bool exportable = mdiChildCallToolStrip.Exportable;
                    bool verifiable = mdiChildCallToolStrip.Verifiable;
                    bool unverifiable = mdiChildCallToolStrip.Unverifiable;
                    bool printable = mdiChildCallToolStrip.Printable;
                    bool readonlyMode = mdiChildCallToolStrip.ReadonlyMode;
                    bool editableMode = mdiChildCallToolStrip.EditableMode;
                    bool isValid = mdiChildCallToolStrip.IsValid;


                    this.toolStripButtonEscape.Enabled = closable;
                    this.toolStripButtonLoad.Enabled = loadable && readonlyMode;

                    this.toolStripButtonNew.Enabled = newable && readonlyMode;
                    this.toolStripButtonEdit.Enabled = editable && readonlyMode;
                    this.toolStripButtonSave.Enabled = isDirty && isValid && editableMode;
                    this.toolStripButtonDelete.Enabled = deletable && readonlyMode;

                    this.toolStripButtonImport.Visible = importable;
                    this.toolStripButtonImport.Enabled = importable && newable && readonlyMode;
                    this.toolStripButtonExport.Visible = exportable;
                    this.toolStripButtonExport.Enabled = exportable;//&& !isDirty && readonlyMode;
                    this.toolStripSeparatorImport.Visible = importable || exportable;

                    this.toolStripButtonVerify.Visible = verifiable || unverifiable;
                    this.toolStripButtonVerify.Enabled = verifiable || unverifiable;
                    this.toolStripButtonVerify.Text = verifiable ? "Verify" : "Unverify";
                    this.toolStripSeparatorVerify.Visible = verifiable || unverifiable;

                    this.toolStripButtonPrint.Visible = printable;
                    this.toolStripButtonPrint.Enabled = printable;
                    this.toolStripButtonPrintPreview.Visible = printable;
                    this.toolStripButtonPrintPreview.Enabled = printable;
                    this.toolStripSeparatorPrint.Visible = printable;
                }
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        #endregion Form Events


        #region <Call Tool Strip>
        private void toolStripButtonEscape_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Escape();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonLoad_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Loading();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.SearchText(this.toolStripComboBoxSearchText.Text);
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.New();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Edit();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Save();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Delete();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void toolStripButtonImport_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Import();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonExport_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Export();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonVerify_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Verify();
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }


        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Print(GlobalEnums.PrintDestination.Print);
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        private void toolStripButtonPrintPreview_Click(object sender, EventArgs e)
        {
            try
            {
                ICallToolStrip callToolStrip = ActiveMdiChild as ICallToolStrip;
                if (callToolStrip != null) callToolStrip.Print(GlobalEnums.PrintDestination.PrintPreview);
            }
            catch (Exception exception)
            {
                GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            }
        }

        #endregion <Call Tool Strip>

        

        


    }
}
