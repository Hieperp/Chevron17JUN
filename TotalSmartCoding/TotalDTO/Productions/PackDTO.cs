using System;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Productions
{
    public class PackPrimitiveDTO : BarcodeDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Pack; } }

        public virtual int GetID() { return this.PackID; }
        public virtual void SetID(int id) { this.PackID = id; }

        public int PackID { get; set; }
        public int OnlinePackID { get; set; }
        public Nullable<int> CartonID { get; set; }
    }

    public class OnlinePackPrimitiveDTO : PackPrimitiveDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.OnlinePack; } }

        public override int GetID() { return this.OnlinePackID; }
        public override void SetID(int id) { this.OnlinePackID = id; }

        public OnlinePackPrimitiveDTO()
        {
            this.FillingLineID = 1; //this.FillingLineData.FillingLineID
            this.CommodityID = 1; //this.FillingLineData.CommodityID
            this.PCID = "ABCD123456EF";
            this.EntryStatusID = 1; ////STATUS: dataDetailCartonRow.CartonStatus = (byte)(cartonBarcode == GlobalVariables.BlankBarcode ? GlobalVariables.BarcodeStatus.BlankBarcode : (this.packInOneCarton.Count == this.packInOneCarton.NoItemPerCarton || this.FillingLineData.FillingLineID == GlobalVariables.FillingLine.Pail ? GlobalVariables.BarcodeStatus.Normal : GlobalVariables.BarcodeStatus.EmptyCarton));
        }

        public Nullable<int> OnlineCartonID { get; set; }
    }

    public class PackDTO : PackPrimitiveDTO
    {
    }

    public class OnlinePackDTO : OnlinePackPrimitiveDTO, IShallowClone<OnlinePackDTO>
    {
        public OnlinePackDTO ShallowClone()
        {
            return (OnlinePackDTO)this.MemberwiseClone();
        }
    }
}
