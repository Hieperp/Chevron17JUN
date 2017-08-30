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

        public int NoSubQueue { get { return this.PackPerCarton == 0 ? 1 : (this.PackPerCarton / 4); } } //GlobalVariables.NoSubQueue()
        public int ItemPerSubQueue { get { return GlobalVariables.NoItemDiverter(); } }
        public bool RepeatSubQueueIndex { get { return GlobalVariables.RepeatedSubQueueIndex(); } }

        public string PCID { get { return "ABCD123456EF"; } }



        public int CartonsetQueueCount { get; set; }
        public GlobalVariables.ZebraStatus CartonsetQueueZebraStatus { get; set; } //AT INITIALIZE, THIS = 0 (RIGHT AFTER CartonsetQueue IS SET). AFTER PRINT: THIS = 1. WHEN USER PRESS RE-PRINT => THIS = -1



        private int commodityID;
        private string commodityCode;
        private string commodityOfficialCode;
        private decimal volume;

        private int noExpiryDate;
        private bool isPailLabel;

        private string batchCode;
        private DateTime settingDate;


        private string nextPackNo;
        private string nextCartonNo;
        private string nextPalletNo;

        private string remarks;


        #region Contructor

        public FillingData()
        {
            this.settingDate = DateTime.Now;
        }

        #endregion Contructor


        #region Public Properties


        public GlobalVariables.FillingLine FillingLineID { get { return GlobalVariables.FillingLineID; } }
        public string FillingLineCode { get { return GlobalVariables.FillingLineCode; } }
        public string FillingLineName { get { return GlobalVariables.FillingLineName; } }

        public bool HasPack { get { return this.FillingLineID == GlobalVariables.FillingLine.Smallpack; } }
        public bool HasCarton { get { return this.FillingLineID == GlobalVariables.FillingLine.Smallpack || this.FillingLineID == GlobalVariables.FillingLine.Pail; } }
        public bool HasPallet { get { return true; } }


        public int CommodityID    //ResetSerialNumber
        {
            get { return this.commodityID; }
            set
            {
                if (this.commodityID != value)
                {
                    ApplyPropertyChange<FillingData, int>(ref this.commodityID, o => o.CommodityID, value);

                    //DataTable dataTableFillingLineData = SQLDatabase.GetDataTable("SELECT BatchNo, LastPackNo, LastPackNo, LastCartonNo, MonthCartonNumber FROM FillingLineData WHERE FillingLineID = " + (int)this.FillingLineID + " AND ProductID = " + this.ProductID + " AND SettingMonthID = " + this.SettingMonthID);
                    //if (dataTableFillingLineData.Rows.Count > 0)
                    //    this.ResetSerialNumber(dataTableFillingLineData.Rows[0]["BatchNo"].ToString() == this.BatchNo ? dataTableFillingLineData.Rows[0]["NextPackNo"].ToString() : "000001", dataTableFillingLineData.Rows[0]["NextPackNo"].ToString(), dataTableFillingLineData.Rows[0]["BatchNo"].ToString() == this.BatchNo ? dataTableFillingLineData.Rows[0]["NextCartonNo"].ToString() : "900001", dataTableFillingLineData.Rows[0]["MonthCartonNumber"].ToString());
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

        public decimal Volume
        {
            get { return this.volume; }
            set { ApplyPropertyChange<FillingData, decimal>(ref this.volume, o => o.Volume, value); }
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
        private int batchID;
        public int BatchID
        {
            get { return this.batchID; }
            set { ApplyPropertyChange<FillingData, int>(ref this.batchID, o => o.BatchID, value); }
        }

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
                        //    this.ResetSerialNumber(dataTableFillingLineData.Rows[0]["NextPackNo"].ToString(), this.LastPackNo, dataTableFillingLineData.Rows[0]["NextCartonNo"].ToString(), this.MonthCartonNumber);
                        //else
                        //    this.ResetSerialNumber("000001", this.LastPackNo, "900001", this.MonthCartonNumber);
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

        public string NextPackNo
        {
            get { return this.nextPackNo; }

            set
            {
                if (value != this.nextPackNo)
                {
                    int intValue = 0;
                    if (int.TryParse(value, out intValue) && value.Length == 6)
                    {
                        ApplyPropertyChange<FillingData, string>(ref this.nextPackNo, o => o.NextPackNo, value);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Lỗi sai định dạng số đếm");
                    }
                }
            }
        }

        public string NextCartonNo
        {
            get { return this.nextCartonNo; }

            set
            {
                if (value != this.nextCartonNo)
                {
                    int intValue = 0;
                    if (int.TryParse(value, out intValue) && value.Length == 6)
                    {
                        ApplyPropertyChange<FillingData, string>(ref this.nextCartonNo, o => o.NextCartonNo, value);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Lỗi sai định dạng số đếm");
                    }
                }
            }
        }


        public string NextPalletNo
        {
            get { return this.nextPalletNo; }

            set
            {
                if (value != this.nextPalletNo)
                {
                    int intValue = 0;
                    if (int.TryParse(value, out intValue) && value.Length == 6)
                    {
                        ApplyPropertyChange<FillingData, string>(ref this.nextPalletNo, o => o.NextPalletNo, value);
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
            if (this.NextPackNo != monthSerialNumber) this.NextPackNo = monthSerialNumber;
            if (this.NextCartonNo != LastCartonNo) this.NextCartonNo = LastCartonNo;
        }

        public bool DataValidated()
        {
            return this.FillingLineID != 0 && this.CommodityID != 0 && this.BatchCode != "" & this.NextPackNo != "" & this.NextCartonNo != "" & this.NextPalletNo != "";
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
                //                                                  "INSERT INTO FillingLineData (FillingLineID, ProductID, BatchNo, SettingDate, SettingMonthID, LastPackNo, LastPackNo, LastCartonNo, MonthCartonNumber, Remarks, LastSettingDate, LastSerialDate, IsDefault) " +
                //                                                  "VALUES (" + (int)this.FillingLineID + ", " + this.ProductID + ", N'" + this.BatchNo + "', CONVERT(smalldatetime, '" + this.SettingDate.ToString("dd/MM/yyyy") + "',103), " + this.SettingMonthID.ToString() + ", N'" + this.LastPackNo + "', N'" + this.LastPackNo + "', N'" + this.LastCartonNo + "', N'" + this.MonthCartonNumber + "', N'" + this.Remarks + "', GetDate(), GetDate(), 1) ");

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














        public string FirstLine(bool isReadableText)
        {
            return this.CommodityCode;
        }


        public string SecondLine(bool isReadableText)
        {
            return this.SecondLineA1(isReadableText) + this.SecondLineA2(isReadableText);
        }

        public string SecondLineA1(bool isReadableText)
        {
            return this.BatchCode.Substring(0, 6);
        }

        public string SecondLineA2(bool isReadableText)
        {
            return this.SettingDate.ToString("ddMMyy");
        }

        public string ThirdLine(bool isReadableText)
        {
            return this.FillingLineCode;
        }

    }
}
