using System;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Productions
{
    public class PalletPrimitiveDTO : BarcodeDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Pallet; } }

        public virtual int GetID() { return this.PalletID; }
        public virtual void SetID(int id) { this.PalletID = id; }

        public int PalletID { get; set; }
        public int OnlinePalletID { get; set; }
    }

    public class OnlinePalletPrimitiveDTO : PalletPrimitiveDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public new GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.OnlinePallet; } }

        public override int GetID() { return this.OnlinePalletID; }
        public override void SetID(int id) { this.OnlinePalletID = id; }
    }


    public class PalletDTO : PalletPrimitiveDTO
    {
    }

    public class OnlinePalletDTO : OnlinePalletPrimitiveDTO, IShallowClone<OnlinePalletDTO>
    {
        public string OnlineCartonIDs { get; set; }


        public OnlinePalletDTO ShallowClone()
        {
            return (OnlinePalletDTO)this.MemberwiseClone();
        }
    }
}
