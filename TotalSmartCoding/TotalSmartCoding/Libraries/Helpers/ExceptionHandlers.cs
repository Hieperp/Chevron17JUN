using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SqlServer.MessageBox;
using System.Windows.Forms;
using TotalSmartCoding.Views.Commons;
using TotalModel.Helpers;

namespace TotalSmartCoding.Libraries.Helpers
{
    class ExceptionHandlers
    {

        public static ExceptionMessageBoxDialogResult ShowExceptionMessageBox(IWin32Window owner, string exceptionMessage)
        {
            return ExceptionHandlers.ShowExceptionMessageBox(owner, new Exception(exceptionMessage));
        }

        public static ExceptionMessageBoxDialogResult ShowExceptionMessageBox(IWin32Window owner, Exception ex)
        {
            //try
            //{

            ExceptionMessageBoxDialogResult exceptionMessageBoxDialogResult;

            CustomException customEx = ex as CustomException;
            if (customEx != null)
            {
                CustomExceptionMessageBox customExceptionMessageBox = new CustomExceptionMessageBox(customEx);
                exceptionMessageBoxDialogResult = (ExceptionMessageBoxDialogResult)customExceptionMessageBox.ShowDialog(owner);
            }
            else
            {
                ExceptionMessageBox exceptionMessageBox = new ExceptionMessageBox(ex); //exceptionMessageBox.ShowToolBar = false;
                exceptionMessageBoxDialogResult = (ExceptionMessageBoxDialogResult)exceptionMessageBox.Show(owner);
            }


            return exceptionMessageBoxDialogResult;
            //}
            //catch {}// Environment.Exit(0); return ExceptionMessageBoxDialogResult.None; }
        }

    }

}
