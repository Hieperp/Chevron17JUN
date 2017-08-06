using System;

using TotalBase;
using TotalModel;
using TotalBase.Enums;
using System.ComponentModel;

namespace TotalDTO.Productions
{
    public class BatchPrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Batch; } }

        public virtual int GetID() { return this.BatchID; }
        public virtual void SetID(int id) { this.BatchID = id; }

        public int BatchID { get; set; }

        public string Code { get; set; }

        public int FillingLineID { get; set; }


        private int commodityID;
        [DefaultValue(-1)]
        public int CommodityID
        {
            get { return this.commodityID; }
            set { ApplyPropertyChange<BatchPrimitiveDTO, int>(ref this.commodityID, o => o.CommodityID, value); }
        }


        [DefaultValue("000001")]
        public string NextPackNo { get; set; }
        public string NextCartonNo { get; set; }
        public string NextPalletNo { get; set; }

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
        public string CommodityName
        {
            get { return this.commodityName; }
            set { ApplyPropertyChange<BatchDTO, string>(ref this.commodityName, o => o.CommodityName, value); }
        }

    }
}
