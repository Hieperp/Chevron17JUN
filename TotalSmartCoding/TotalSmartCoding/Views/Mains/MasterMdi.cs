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
