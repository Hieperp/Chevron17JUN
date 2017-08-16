using System;

using TotalBase;
using TotalModel;
using TotalBase.Enums;
using System.ComponentModel;
using System.Collections.Generic;
using TotalModel.Helpers;

namespace TotalDTO.Productions
{
    public class BatchPrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Batch; } }

        public override int GetID() { return this.BatchID; }
        public virtual void SetID(int id) { this.BatchID = id; }

        private int batchID;
        [DefaultValue(0)]
        public int BatchID
        {
            get { return this.batchID; }
            set { ApplyPropertyChange<BatchPrimitiveDTO, int>(ref this.batchID, o => o.BatchID, value); }
        }

        private string code;
        [DefaultValue("")]
        public string Code
        {
            get { return this.code; }
            set { ApplyPropertyChange<BatchPrimitiveDTO, string>(ref this.code, o => o.Code, value); }
        }


        public int FillingLineID { get; set; }


        private int commodityID;
        [DefaultValue(null)]
        public int CommodityID
        {
            get { return this.commodityID; }
            set { ApplyPropertyChange<BatchPrimitiveDTO, int>(ref this.commodityID, o => o.CommodityID, value); }
        }


        private string nextPackNo;
        [DefaultValue("000001")]
        public string NextPackNo
        {
            get { return this.nextPackNo; }
            set { ApplyPropertyChange<BatchPrimitiveDTO, string>(ref this.nextPackNo, o => o.NextPackNo, value); }
        }

        private string nextCartonNo;
        [DefaultValue("000001")]
        public string NextCartonNo
        {
            get { return this.nextCartonNo; }
            set { ApplyPropertyChange<BatchPrimitiveDTO, string>(ref this.nextCartonNo, o => o.NextCartonNo, value); }
        }

        private string nextPalletNo;
        [DefaultValue("000001")]
        public string NextPalletNo
        {
            get { return this.nextPalletNo; }
            set { ApplyPropertyChange<BatchPrimitiveDTO, string>(ref this.nextPalletNo, o => o.NextPalletNo, value); }
        }

        public override int PreparedPersonID { get { return 1; } }

        [DefaultValue(false)]
        public bool IsDefault { get; set; }
    }

    public class BatchDTO : BatchPrimitiveDTO
    {
        public BatchDTO()
        {
            this.FillingLineID = (int)GlobalVariables.FillingLineID;
            this.LocationID = GlobalVariables.LocationID;
        }

        private string commodityName;
        [DefaultValue("")]
        public string CommodityName
        {
            get { return this.commodityName; }
            set { ApplyPropertyChange<BatchDTO, string>(ref this.commodityName, o => o.CommodityName, value, false); }
        }


        protected override List<ValidationRule> CreateRules()
        {
            List<ValidationRule> validationRules = base.CreateRules(); int value;
            validationRules.Add(new SimpleValidationRule("CommodityID", "Vui lòng chọn mã sản phẩm.", delegate { return this.CommodityID > 0; }));
            validationRules.Add(new SimpleValidationRule("Code", "Số batch quy định là 8 ký tự.", delegate { return this.Code.Length == 8; }));
            validationRules.Add(new SimpleValidationRule("NextPackNo", "Số thứ tự chai quy định là 6 chữ số.", delegate { return this.NextPackNo.Length == 6 && int.TryParse(this.NextPackNo, out value); }));
            validationRules.Add(new SimpleValidationRule("NextCartonNo", "Số thứ tự carton quy định là 6 chữ số.", delegate { return this.NextCartonNo.Length == 6 && int.TryParse(this.NextCartonNo, out value); }));
            validationRules.Add(new SimpleValidationRule("NextPalletNo", "Số thứ tự pallet quy định là 6 chữ số.", delegate { return this.NextPalletNo.Length == 6 && int.TryParse(this.NextPalletNo, out value); }));

            return validationRules;

        }
    }
}
