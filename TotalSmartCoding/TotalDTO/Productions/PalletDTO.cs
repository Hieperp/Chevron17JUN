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
        public int FillingPalletID { get; set; }
    }

    public class FillingPalletPrimitiveDTO : PalletPrimitiveDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public new GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.FillingPallet; } }

        public override int GetID() { return this.FillingPalletID; }
        public override void SetID(int id) { this.FillingPalletID = id; }
    }


    public class PalletDTO : PalletPrimitiveDTO
    {
    }

    public class FillingPalletDTO : FillingPalletPrimitiveDTO, IShallowClone<FillingPalletDTO>
    {
        public string FillingCartonIDs { get; set; }


        public FillingPalletDTO ShallowClone()
        {
            return (FillingPalletDTO)this.MemberwiseClone();
        }
    }
}
