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
        public int FillingPackID { get; set; }
        public Nullable<int> CartonID { get; set; }
    }

    public class FillingPackPrimitiveDTO : PackPrimitiveDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.FillingPack; } }

        public override int GetID() { return this.FillingPackID; }
        public override void SetID(int id) { this.FillingPackID = id; }

        public Nullable<int> FillingCartonID { get; set; }
    }

    public class PackDTO : PackPrimitiveDTO
    {
    }

    public class FillingPackDTO : FillingPackPrimitiveDTO, IShallowClone<FillingPackDTO>
    {
        public FillingPackDTO ShallowClone()
        {
            return (FillingPackDTO)this.MemberwiseClone();
        }
    }
}
