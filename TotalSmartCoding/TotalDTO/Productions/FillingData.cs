using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotalModel.Helpers;

using TotalBase;

namespace TotalDTO.Productions
{
    public class FillingData : NotifyPropertyChangeObject
    {

        public int NoSubQueue { get { return GlobalVariables.NoSubQueue(); } }
        public int ItemPerSubQueue { get { return GlobalVariables.NoItemDiverter(); } }
        public bool RepeatSubQueueIndex { get { return GlobalVariables.RepeatedSubQueueIndex(); } }

        public string PCID { get { return "ABCD123456EF"; } }





        private int commodityID;
        private string commodityCode;
        private string commodityOfficialCode;

        private int noExpiryDate;
        private bool isPailLabel;

        private string batchCode;
        private DateTime settingDate;


        private string lastPackNo;
        private string lastCartonNo;
        private string lastPalletNo;

        private string remarks;


        #region Contructor

        public FillingData()
        {

            //DataTable defaultFillingLineData = ADODatabase.GetDataTable("SELECT FillingLineData.ProductID, ListProductName.ProductCode, ListProductName.ProductCodeOriginal, ListProductName.NoItemPerCarton, ListProductName.NoExpiryDate, ListProductName.IsPailLabel, FillingLineData.BatchNo, FillingLineData.SettingDate, FillingLineData.SettingMonthID, FillingLineData.LastPackNo, FillingLineData.MonthSerialNumber, FillingLineData.LastCartonNo, FillingLineData.MonthCartonNumber FROM FillingLineData INNER JOIN ListProductName ON FillingLineData.ProductID = ListProductName.ProductID WHERE FillingLineData.FillingLineID = " + (int)this.FillingLineID + " AND FillingLineData.IsDefault = 1");

            //if (defaultFillingLineData.Rows.Count > 0)
            //{
            //    this.StartTracking();

            //    this.ProductID = int.Parse(defaultFillingLineData.Rows[0]["ProductID"].ToString());
            //    this.ProductCode = defaultFillingLineData.Rows[0]["ProductCode"].ToString();
            //    this.ProductCodeOriginal = defaultFillingLineData.Rows[0]["ProductCodeOriginal"].ToString();

            //    this.NoExpiryDate = int.Parse(defaultFillingLineData.Rows[0]["NoExpiryDate"].ToString());
            //    this.IsPailLabel = bool.Parse(defaultFillingLineData.Rows[0]["IsPailLabel"] is DBNull ? "False" : defaultFillingLineData.Rows[0]["IsPailLabel"].ToString());

            //    GlobalVariables.noItemPerCartonSetByProductID = int.Parse(defaultFillingLineData.Rows[0]["NoItemPerCarton"].ToString());

            //    this.BatchNo = defaultFillingLineData.Rows[0]["BatchNo"].ToString();

            //    this.SettingDate = DateTime.Parse(defaultFillingLineData.Rows[0]["SettingDate"].ToString());
            //    this.SettingMonthID = int.Parse(defaultFillingLineData.Rows[0]["SettingMonthID"].ToString());

            //    this.LastPackNo = defaultFillingLineData.Rows[0]["LastPackNo"].ToString();
            //    this.MonthSerialNumber = defaultFillingLineData.Rows[0]["MonthSerialNumber"].ToString();

            //    this.LastCartonNo = defaultFillingLineData.Rows[0]["LastCartonNo"].ToString();
            //    this.MonthCartonNumber = defaultFillingLineData.Rows[0]["MonthCartonNumber"].ToString();

            //    this.StartTracking();
            //}


        }

        #endregion Contructor


        #region Public Properties


        public GlobalVariables.FillingLine FillingLineID
        {
            get { return GlobalVariables.FillingLineID; }

        }

        public string FillingLineCode
        {
            get { return GlobalVariables.FillingLineCode; }
        }

        public string FillingLineName
        {
            get { return GlobalVariables.FillingLineName; }
        }


        public int CommodityID    //ResetSerialNumber
        {
            get { return this.commodityID; }
            set
            {
                if (this.commodityID != value)
                {
                    ApplyPropertyChange<FillingData, int>(ref this.commodityID, o => o.CommodityID, value);

                    //DataTable dataTableFillingLineData = SQLDatabase.GetDataTable("SELECT BatchNo, LastPackNo, MonthSerialNumber, LastCartonNo, MonthCartonNumber FROM FillingLineData WHERE FillingLineID = " + (int)this.FillingLineID + " AND ProductID = " + this.ProductID + " AND SettingMonthID = " + this.SettingMonthID);
                    //if (dataTableFillingLineData.Rows.Count > 0)
                    //    this.ResetSerialNumber(dataTableFillingLineData.Rows[0]["BatchNo"].ToString() == this.BatchNo ? dataTableFillingLineData.Rows[0]["LastPackNo"].ToString() : "000001", dataTableFillingLineData.Rows[0]["MonthSerialNumber"].ToString(), dataTableFillingLineData.Rows[0]["BatchNo"].ToString() == this.BatchNo ? dataTableFillingLineData.Rows[0]["LastCartonNo"].ToString() : "900001", dataTableFillingLineData.Rows[0]["MonthCartonNumber"].ToString());
                    //else
                    //    this.ResetSerialNumber("000001", "000001", "900001", "900001");
                }
            }
        }


        public string CommodityCode
        {
            get { return this.commodityCode; }
            set { ApplyPropertyChange<FillingData, string>(ref this.commodityCode, o => o.CommodityCode, value); }
        }

        public string CommodityOfficialCode
        {
            get { return this.commodityOfficialCode; }
            set { ApplyPropertyChange<FillingData, string>(ref this.commodityOfficialCode, o => o.CommodityOfficialCode, value); }
        }



        public int NoExpiryDate
        {
            get { return this.noExpiryDate; }
            set { ApplyPropertyChange<FillingData, int>(ref this.noExpiryDate, o => o.NoExpiryDate, value); }
        }

        public bool IsPailLabel
        {
            get { return this.isPailLabel; }
            set { ApplyPropertyChange<FillingData, bool>(ref this.isPailLabel, o => o.IsPailLabel, value); }
        }


        //-------------------------


        public string BatchCode   //ResetSerialNumber
        {
            get { return this.batchCode; }
            set
            {
                if (this.batchCode != value)
                {
                    if (value.Length == 8)
                    {
                        ApplyPropertyChange<FillingData, string>(ref this.batchCode, o => o.BatchCode, value);

                        //DataTable dataTableFillingLineData = SQLDatabase.GetDataTable("SELECT LastPackNo, LastCartonNo FROM FillingLineData WHERE FillingLineID = " + (int)this.FillingLineID + " AND ProductID = " + this.ProductID + " AND SettingMonthID = " + this.SettingMonthID + " AND BatchNo = " + this.BatchNo);
                        //if (dataTableFillingLineData.Rows.Count > 0)
                        //    this.ResetSerialNumber(dataTableFillingLineData.Rows[0]["LastPackNo"].ToString(), this.MonthSerialNumber, dataTableFillingLineData.Rows[0]["LastCartonNo"].ToString(), this.MonthCartonNumber);
                        //else
                        //    this.ResetSerialNumber("000001", this.MonthSerialNumber, "900001", this.MonthCartonNumber);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("NMVN: Invalid batch number format.");
                    }
                }
            }
        }


        public DateTime SettingDate
        {
            get { return this.settingDate; }
            set
            {
                ApplyPropertyChange<FillingData, DateTime>(ref this.settingDate, o => o.SettingDate, value);
                //this.SettingMonthID = GlobalStaticFunction.DateToContinuosMonth(this.SettingDate);
            }
        }



        //-------------------------

        public string LastPackNo
        {
            get { return this.lastPackNo; }

            set
            {
                if (value != this.lastPackNo)
                {
                    int intValue = 0;
                    if (int.TryParse(value, out intValue) && value.Length == 6)
                    {
                        ApplyPropertyChange<FillingData, string>(ref this.lastPackNo, o => o.LastPackNo, value);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Lỗi sai định dạng số đếm");
                    }
                }
            }
        }

        public string LastCartonNo
        {
            get { return this.lastCartonNo; }

            set
            {
                if (value != this.lastCartonNo)
                {
                    int intValue = 0;
                    if (int.TryParse(value, out intValue) && value.Length == 6)
                    {
                        ApplyPropertyChange<FillingData, string>(ref this.lastCartonNo, o => o.LastCartonNo, value);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Lỗi sai định dạng số đếm");
                    }
                }
            }
        }


        public string LastPalletNo
        {
            get { return this.lastPalletNo; }

            set
            {
                if (value != this.lastPalletNo)
                {
                    int intValue = 0;
                    if (int.TryParse(value, out intValue) && value.Length == 6)
                    {
                        ApplyPropertyChange<FillingData, string>(ref this.lastPalletNo, o => o.LastPalletNo, value);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Lỗi sai định dạng số đếm");
                    }
                }
            }
        }


        //-------------------------



        public string Remarks
        {
            get { return this.remarks; }
            set { ApplyPropertyChange<FillingData, string>(ref this.remarks, o => o.Remarks, value); }
        }













        private int packPerCarton;
        public int PackPerCarton
        {
            get { return this.packPerCarton; }
            set { ApplyPropertyChange<FillingData, int>(ref this.packPerCarton, o => o.PackPerCarton, value); }
        }

        
        private int cartonPerPallet;
        public int CartonPerPallet
        {
            get { return this.cartonPerPallet; }
            set { ApplyPropertyChange<FillingData, int>(ref this.cartonPerPallet, o => o.CartonPerPallet, value); }
        }
    








        #endregion Public Properties

        #region Method

        public FillingData ShallowClone()
        {
            return (FillingData)this.MemberwiseClone();
        }

        private void ResetSerialNumber(string batchSerialNumber, string monthSerialNumber, string LastCartonNo, string monthCartonNumber)
        {
            if (this.LastPackNo != monthSerialNumber) this.LastPackNo = monthSerialNumber;
            if (this.LastCartonNo != LastCartonNo) this.LastCartonNo = LastCartonNo;
        }

        public bool DataValidated()
        {
            return this.FillingLineID != 0 && this.CommodityID != 0 && this.BatchCode != "" & this.LastPackNo != "" & this.LastCartonNo != "" & this.LastPalletNo != "";
        }

        public bool Update()
        {
            return true;
            try
            {
                //int rowsAffected = ADODatabase.ExecuteTransaction("UPDATE FillingLineData SET IsDefault = 0 WHERE FillingLineID = " + (int)this.FillingLineID + "; " +
                //                                                  "UPDATE FillingLineData SET BatchNo = N'" + this.BatchNo.ToString() + "', " +
                //                                                                            " SettingDate = CONVERT(smalldatetime, '" + this.SettingDate.ToString("dd/MM/yyyy") + "', 103), " +
                //                                                                            " SettingMonthID = " + this.SettingMonthID.ToString() + ", " +
                //                                                                            " LastPackNo = N'" + this.LastPackNo.ToString() + "', " +
                //                                                                            " MonthSerialNumber = N'" + this.MonthSerialNumber.ToString() + "', " +
                //                                                                            " LastCartonNo = N'" + this.LastCartonNo.ToString() + "', " +
                //                                                                            " MonthCartonNumber = N'" + this.MonthCartonNumber.ToString() + "', " +
                //                                                                            " LastSerialDate = GetDate(), IsDefault = 1 " +
                //                                                  "WHERE FillingLineID   =  " + (int)this.FillingLineID + " AND ProductID = " + this.ProductID);
                //return rowsAffected > 0;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool Save()
        {
            return true;
            try
            {
                //int rowsAffected = ADODatabase.ExecuteNonQuery("UPDATE FillingLineData SET IsDefault = 1 WHERE FillingLineID = " + (int)this.FillingLineID + " AND ProductID = " + this.ProductID);


                //if (rowsAffected <= 0) //Add New
                //{
                //    rowsAffected = ADODatabase.ExecuteTransaction("UPDATE FillingLineData SET IsDefault = 0 WHERE FillingLineID = " + (int)this.FillingLineID + "; " +
                //                                                  "INSERT INTO FillingLineData (FillingLineID, ProductID, BatchNo, SettingDate, SettingMonthID, LastPackNo, MonthSerialNumber, LastCartonNo, MonthCartonNumber, Remarks, LastSettingDate, LastSerialDate, IsDefault) " +
                //                                                  "VALUES (" + (int)this.FillingLineID + ", " + this.ProductID + ", N'" + this.BatchNo + "', CONVERT(smalldatetime, '" + this.SettingDate.ToString("dd/MM/yyyy") + "',103), " + this.SettingMonthID.ToString() + ", N'" + this.LastPackNo + "', N'" + this.MonthSerialNumber + "', N'" + this.LastCartonNo + "', N'" + this.MonthCartonNumber + "', N'" + this.Remarks + "', GetDate(), GetDate(), 1) ");

                //    return rowsAffected > 0;
                //}

                //else //Update Only
                //{
                //    return Update();
                //}
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion Method
    }
}
