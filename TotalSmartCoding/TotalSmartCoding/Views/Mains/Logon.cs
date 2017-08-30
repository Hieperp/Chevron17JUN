using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Ninject;

using TotalDAL;
using TotalBase;
using TotalSmartCoding.Libraries.Helpers;
using TotalSmartCoding.Controllers.APIs.Commons;
using TotalSmartCoding.Libraries;
using TotalCore.Repositories.Commons;
using TotalModel.Models;

namespace TotalSmartCoding.Views.Mains
{
    public partial class Logon : Form
    {
        public Logon()
        {
            InitializeComponent();
        }


        public int EmployeeID { get; set; }
        private Binding employeeIDBinding;
        //private CommonMetaList commonMetaList;


        private void PublicApplicationLogon_Load(object sender, EventArgs e)
        {


            string plainText = "Lê Minh Hiệp";
            // Convert the plain string pwd into bytes
            //byte[] plainTextBytes = UnicodeEncoding.Unicode.GetBytes(plainText);
            //System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            //byte[] hash = hashAlgo.ComputeHash(plainTextBytes);

            byte[] data = UnicodeEncoding.Unicode.GetBytes(plainText);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = UnicodeEncoding.Unicode.GetString(data);
            //CustomMsgBox.Show(hash);



            try
            {
                FillingLineAPIs fillingLineAPIs = new FillingLineAPIs(CommonNinject.Kernel.Get<IFillingLineAPIRepository>());

                this.comboFillingLineID.DataSource = fillingLineAPIs.GetFillingLineBases();
                this.comboFillingLineID.DisplayMember = CommonExpressions.PropertyName<FillingLineBase>(p => p.Name);
                this.comboFillingLineID.ValueMember = CommonExpressions.PropertyName<FillingLineBase>(p => p.FillingLineID);

                int fillingLineID;
                if (int.TryParse(CommonConfigs.ReadSetting("FillingLineID"), out fillingLineID) && fillingLineID >= 1 && fillingLineID <= 3)
                    this.comboFillingLineID.SelectedValue = fillingLineID;

                //this.comboFillingLineID.SelectedValue = 2;

                //this.bindingFillingLineID = this.comboFillingLineID.DataBindings.Add("SelectedValue", GlobalVariables., CommonExpressions.PropertyName<PickupViewModel>(p => p.FillingLineID), true, DataSourceUpdateMode.OnPropertyChanged);

                //string a = CommonConfigs.ReadSetting("FillingLineID");
                //CommonConfigs.AddUpdateAppSettings("FillingLineID", "2");
                //a = CommonConfigs.ReadSetting("FillingLineID");

                //DataTable dataTable = ADODatabase.GetDataTable("SELECT * FROM ListProductionLine WHERE (ProductionLineID > 0 AND ProductionLineID <= 6) OR ProductionLineID = 99");
                //this.comboFillingLineID.DataSource = dataTable;
                //this.comboFillingLineID.ValueMember = "ProductionLineID";
                //this.comboFillingLineID.DisplayMember = "ProductionLineName";
                //if (dataTable.Rows.Count > 0) this.comboFillingLineID.SelectedValue = dataTable.Rows[0]["LastLogonProductionLineID"];


                this.comboBoxAutonicsPortName.DataSource = System.IO.Ports.SerialPort.GetPortNames();
                if (this.comboBoxAutonicsPortName.Items.Count == 0)
                {
                    this.comboBoxAutonicsPortName.DataSource = null;
                    this.comboBoxAutonicsPortName.Items.Add("COM0");
                }

                string comportName = CommonConfigs.ReadSetting("ComportName"); 
                if (this.comboBoxAutonicsPortName.Items.IndexOf(comportName) >=0 )
                    this.comboBoxAutonicsPortName.SelectedIndex = this.comboBoxAutonicsPortName.Items.IndexOf(comportName);



                //DataTable dataTablePublicPrinterProperties = SQLDatabase.GetDataTable("SELECT TOP 1 * FROM PublicPrinterProperties");
                //if (dataTablePublicPrinterProperties.Rows.Count > 0)
                //{
                //    this.comboBoxImageS8PortName.Text = (string)dataTablePublicPrinterProperties.Rows[0]["ImageS8PortName"];
                //    this.comboBoxAutonicsPortName.Text = (string)dataTablePublicPrinterProperties.Rows[0]["AutonicsPortName"];
                //}


                //string stringEmployeeID = Registries.Read("EmployeeID"); int employeeID = -1;

                //if (stringEmployeeID == null || stringEmployeeID.Length <= "string".Length || !int.TryParse(stringEmployeeID.Substring("string".Length), out employeeID)) employeeID = 1;
                //this.EmployeeID = employeeID; this.buttonListEmployee.Visible = this.EmployeeID == 1;

                //this.commonMetaList = new CommonMetaList();

                //ListMaintenance.ListEmployeeDataTable listEmployeeDataTable = this.commonMetaList.GetListEmployee();
                //this.comboBoxEmployeeID.DataSource = listEmployeeDataTable;
                //this.comboBoxEmployeeID.DisplayMember = listEmployeeDataTable.DescriptionColumn.ColumnName;
                //this.comboBoxEmployeeID.ValueMember = listEmployeeDataTable.EmployeeIDColumn.ColumnName;
                //this.employeeIDBinding = this.comboBoxEmployeeID.DataBindings.Add("SelectedValue", this, "EmployeeID", true, DataSourceUpdateMode.OnPropertyChanged);

                //this.employeeIDBinding.BindingComplete += new BindingCompleteEventHandler(CommonControl_BindingComplete);

                //this.dateTimePickerUserDate.Value = DateTime.Now - DateTime.Today <= new TimeSpan(7, 0, 0) ? DateTime.Today.AddDays(-1) : DateTime.Today;
                this.comboBoxEmployeeID.Text = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);
            }
        }

        private void CommonControl_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState == BindingCompleteState.Exception)
            { ExceptionHandlers.ShowExceptionMessageBox(this, e.ErrorText); e.Cancel = true; }
            else
                this.buttonListEmployee.Visible = this.EmployeeID == 1;
        }

        private void pictureBoxIcon_DoubleClick(object sender, EventArgs e)
        {
            this.labelPortAutonis.Visible = true;
            this.comboBoxAutonicsPortName.Visible = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                ContextAttributes.User = new UserInformation(1, 1, this.comboBoxEmployeeID.Text, new DateTime());

                if (this.comboFillingLineID.SelectedIndex < 0 || this.comboBoxAutonicsPortName.SelectedIndex < 0) throw new System.ArgumentException("Vui lòng chọn chuyền sản xuất (NOF1, NOF2, NOF...), và chọn đúng cổng COM để chạy phần mềm"); // || (this.comboFillingLineID.Enabled && (GlobalVariables.ProductionLine)this.comboFillingLineID.SelectedValue == GlobalVariables.ProductionLine.SERVER)


                GlobalVariables.FillingLineID = (GlobalVariables.FillingLine)this.comboFillingLineID.SelectedValue;
                GlobalVariables.FillingLineCode = ((FillingLineBase)this.comboFillingLineID.SelectedItem).Code;
                GlobalVariables.FillingLineName = ((FillingLineBase)this.comboFillingLineID.SelectedItem).Name;

                GlobalVariables.ComportName = (string)this.comboBoxAutonicsPortName.SelectedValue;

                CommonConfigs.AddUpdateAppSettings("FillingLineID", ((int)GlobalVariables.FillingLineID).ToString());
                CommonConfigs.AddUpdateAppSettings("ComportName", GlobalVariables.ComportName);

                //if (this.comboBoxEmployeeID.SelectedIndex < 0 || this.EmployeeID < 0) throw new System.ArgumentException("Vui lòng chọn tên người sử dụng!");

                //if (!this.commonMetaList.CheckPasswordSuccessful(this.EmployeeID, this.textBoxPassword.Text)) throw new System.ArgumentException("Sai mật khẩu! Vui lòng kiểm tra lại trước khi tiếp tục.");

                //GlobalVariables.GlobalUserInformation = new UserInformation(this.EmployeeID, 1, this.comboBoxEmployeeID.Text, this.dateTimePickerUserDate.Value);


                //if (sender.Equals(this.buttonListEmployee))
                //{
                //    CommonMDI commonMDI = new CommonMDI(GlobalEnum.TaskID.ListEmployee);
                //    if (commonMDI.ShowDialog() == System.Windows.Forms.DialogResult.OK || true)
                //    {
                //        this.comboBoxEmployeeID.DataSource = this.commonMetaList.GetListEmployee();
                //    }
                //    commonMDI.Dispose();
                //}


                //if (sender.Equals(this.buttonOK))
                //{
                //    GlobalRegistry.Write("EmployeeID", "string" + this.EmployeeID);

                //    if (this.comboBoxImageS8PortName.SelectedIndex < 0 || this.comboBoxAutonicsPortName.SelectedIndex < 0) throw new System.ArgumentException("Vui lòng chọn cổng COM!");

                //    if (this.comboBoxImageS8PortName.DataSource == null || this.comboBoxAutonicsPortName.DataSource == null)
                //    {
                //        GlobalVariables.DiminoPortName = "COM 0";
                //        GlobalVariables.AutonicsPortName = "COM 0";
                //    }
                //    else
                //    {
                //        SQLDatabase.ExecuteNonQuery("UPDATE PublicPrinterProperties SET ImageS8PortName = N'" + (string)this.comboBoxImageS8PortName.SelectedValue + "', AutonicsPortName = N'" + (string)this.comboBoxAutonicsPortName.SelectedValue + "' ");

                //        GlobalVariables.DiminoPortName = (string)this.comboBoxImageS8PortName.SelectedValue;
                //        GlobalVariables.AutonicsPortName = (string)this.comboBoxAutonicsPortName.SelectedValue;
                //    }

                //    GlobalVariables.IsNoDomino = this.checkBoxNoDomino.Checked;
                //}

                //if (sender.Equals(this.labelChangePassword))
                //{
                //    PublicAuthenticationPassword publicAuthenticationPassword = new PublicAuthenticationPassword();
                //    publicAuthenticationPassword.ShowDialog();
                //    publicAuthenticationPassword.Dispose();
                //}

            }
            catch (Exception exception)
            {
                ExceptionHandlers.ShowExceptionMessageBox(this, exception);

                this.DialogResult = DialogResult.None;
            }
        }

        private void comboFillingLineID_Validated(object sender, EventArgs e)
        {
            if (this.comboFillingLineID.SelectedIndex < 0) this.comboFillingLineID.Text = "";
        }

        private void labelNoDomino_DoubleClick(object sender, EventArgs e)
        {
            //if ((int)this.comboFillingLineID.SelectedValue != (int)GlobalVariables.ProductionLine.SERVER)
            //{
            //    this.labelNoDomino.Visible = false;
            //    this.checkBoxNoDomino.Visible = true;
            //}
        }

        private void lbProductionLineID_DoubleClick(object sender, EventArgs e)
        {
            //try
            //{
            //    string textInput = "";
            //    if (CustomInputBox.Show("NMVN", "Vui lòng nhập mật khẩu đổi chuyền", ref textInput) == System.Windows.Forms.DialogResult.OK)
            //        this.comboFillingLineID.Enabled = (textInput == "9876543210");
            //}
            //catch (Exception exception)
            //{
            //    GlobalExceptionHandler.ShowExceptionMessageBox(this, exception);
            //}
        }



    }
}
