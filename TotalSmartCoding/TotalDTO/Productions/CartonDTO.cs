using System;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Productions
{
    public class CartonPrimitiveDTO : BarcodeDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public CartonPrimitiveDTO()
            : this(null)
        { }
        public CartonPrimitiveDTO(FillingData fillingData)
            : base(fillingData)
        { }


        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Carton; } }

        public override int GetID() { return this.CartonID; }
        public virtual void SetID(int id) { this.CartonID = id; }

        public int CartonID { get; set; }
        public int FillingCartonID { get; set; }
        public Nullable<int> PalletID { get; set; }
    }

    public class FillingCartonPrimitiveDTO : CartonPrimitiveDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public FillingCartonPrimitiveDTO()
            : this(null)
        { }
        public FillingCartonPrimitiveDTO(FillingData fillingData)
            : base(fillingData)
        { }


        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.FillingCarton; } }

        public override int GetID() { return this.FillingCartonID; }
        public override void SetID(int id) { this.FillingCartonID = id; }       

        public Nullable<int> FillingPalletID { get; set; }
    }


    public class CartonDTO : CartonPrimitiveDTO
    {
        public CartonDTO()
            : this(null)
        { }
        public CartonDTO(FillingData fillingData)
            : base(fillingData)
        { }
    }

    public class FillingCartonDTO : FillingCartonPrimitiveDTO, IShallowClone<FillingCartonDTO>
    {
        public FillingCartonDTO()
            : this(null)
        { }
        public FillingCartonDTO(FillingData fillingData)
            : base(fillingData)
        { }


        public string FillingPackIDs { get; set; }

        public FillingCartonDTO ShallowClone()
        {
            return (FillingCartonDTO)this.MemberwiseClone();
        }
    }
}
