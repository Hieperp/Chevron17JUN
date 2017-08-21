using System;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Productions
{
    public class PackPrimitiveDTO : BarcodeDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public PackPrimitiveDTO()
            : this(null)
        { }
        public PackPrimitiveDTO(FillingData fillingData)
            : base(fillingData)
        {
            if (fillingData != null)
                this.Volume = fillingData.Volume;
        }



        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Pack; } }

        public override int GetID() { return this.PackID; }
        public virtual void SetID(int id) { this.PackID = id; }

        public int PackID { get; set; }
        public int FillingPackID { get; set; }
        public Nullable<int> CartonID { get; set; }

        public override int TotalPacks { get { return 1; } set { } }
    }

    public class FillingPackPrimitiveDTO : PackPrimitiveDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public FillingPackPrimitiveDTO()
            : this(null)
        { }
        public FillingPackPrimitiveDTO(FillingData fillingData)
            : base(fillingData)
        { }


        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.FillingPack; } }

        public override int GetID() { return this.FillingPackID; }
        public override void SetID(int id) { this.FillingPackID = id; }

        public Nullable<int> FillingCartonID { get; set; }
    }

    public class PackDTO : PackPrimitiveDTO
    {
        public PackDTO()
            : this(null)
        { }
        public PackDTO(FillingData fillingData)
            : base(fillingData)
        { }
    }

    public class FillingPackDTO : FillingPackPrimitiveDTO, IShallowClone<FillingPackDTO>
    {
        public FillingPackDTO()
            : this(null)
        { }
        public FillingPackDTO(FillingData fillingData)
            : base(fillingData)
        { }


        public FillingPackDTO ShallowClone()
        {
            return (FillingPackDTO)this.MemberwiseClone();
        }
    }
}
