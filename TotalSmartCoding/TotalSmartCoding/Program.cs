using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using TotalBase;
using TotalSmartCoding.Views.Mains;

using TotalSmartCoding.Libraries;
using TotalBase.Enums;

namespace TotalSmartCoding
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Registries.ProductName = Application.ProductName.ToUpper();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            AutoMapperConfig.SetupMappings();

            Logon logon = new Logon();

            //if (logon.ShowDialog() == DialogResult.OK) Application.Run(new MasterMDI(GlobalEnums.NmvnTaskID.SmartCoding));
            if (logon.ShowDialog() == DialogResult.OK) Application.Run(new MasterMDI());

            logon.Dispose();


            
        }
    }
}
