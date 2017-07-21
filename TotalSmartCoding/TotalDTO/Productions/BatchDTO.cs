using System;

using TotalBase;
using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Productions
{
    public class BatchPrimitiveDTO : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Batch; } }

        public virtual int GetID() { return this.BatchID; }
        public virtual void SetID(int id) { this.BatchID = id; }

        public int BatchID { get; set; }

        public string Code { get; set; }

        public int FillingLineID { get { return (int)GlobalVariables.FillingLineID; } }
        public int CommodityID { get; set; }

        public string NextPackNo { get; set; }
        public string NextCartonNo { get; set; }
        public string NextPalletNo { get; set; }

        public bool IsDefault { get; set; }
    }

    public class BatchDTO : BatchPrimitiveDTO
    {
    }
}
