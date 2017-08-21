using System;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Productions
{
    public class PalletPrimitiveDTO : BarcodeDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public PalletPrimitiveDTO()
            : this(null)
        { }
        public PalletPrimitiveDTO(FillingData fillingData)
            : base(fillingData)
        { }


        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Pallet; } }

        public override int GetID() { return this.PalletID; }
        public virtual void SetID(int id) { this.PalletID = id; }

        public int PalletID { get; set; }
        public int FillingPalletID { get; set; }

        public int TotalCartons { get; set; }
    }

    public class FillingPalletPrimitiveDTO : PalletPrimitiveDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public FillingPalletPrimitiveDTO()
            : this(null)
        { }
        public FillingPalletPrimitiveDTO(FillingData fillingData)
            : base(fillingData)
        { }


        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.FillingPallet; } }

        public override int GetID() { return this.FillingPalletID; }
        public override void SetID(int id) { this.FillingPalletID = id; }
    }


    public class PalletDTO : PalletPrimitiveDTO
    {
        public PalletDTO()
            : this(null)
        { }
        public PalletDTO(FillingData fillingData)
            : base(fillingData)
        { }
    }

    public class FillingPalletDTO : FillingPalletPrimitiveDTO, IShallowClone<FillingPalletDTO>
    {
        public FillingPalletDTO()
            : this(null)
        { }
        public FillingPalletDTO(FillingData fillingData)
            : base(fillingData)
        { }


        public string FillingCartonIDs { get; set; }


        public FillingPalletDTO ShallowClone()
        {
            return (FillingPalletDTO)this.MemberwiseClone();
        }
    }
}
