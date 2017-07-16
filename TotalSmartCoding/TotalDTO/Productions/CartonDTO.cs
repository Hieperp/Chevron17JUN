using System;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Productions
{
    public class CartonPrimitiveDTO : BarcodeDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Carton; } }

        public virtual int GetID() { return this.CartonID; }
        public virtual void SetID(int id) { this.CartonID = id; }

        public int CartonID { get; set; }
        public int OnlineCartonID { get; set; }
        public Nullable<int> PalletID { get; set; }
    }

    public class OnlineCartonPrimitiveDTO : CartonPrimitiveDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.OnlineCarton; } }

        public override int GetID() { return this.OnlineCartonID; }
        public override void SetID(int id) { this.OnlineCartonID = id; }

        public OnlineCartonPrimitiveDTO()
        {
            this.FillingLineID = 1; //this.FillingLineData.FillingLineID
            this.CommodityID = 1; //this.FillingLineData.CommodityID
            this.PCID = "ABCD123456EF";
            this.EntryStatusID = 1; //(byte)GlobalVariables.BarcodeStatus.Normal
        }
        
        public Nullable<int> OnlinePalletID { get; set; }
    }


    public class CartonDTO : CartonPrimitiveDTO
    {
    }

    public class OnlineCartonDTO : OnlineCartonPrimitiveDTO, IShallowClone<OnlineCartonDTO>
    {
        public string OnlinePackIDs { get; set; }

        public OnlineCartonDTO ShallowClone()
        {
            return (OnlineCartonDTO)this.MemberwiseClone();
        }
    }
}
