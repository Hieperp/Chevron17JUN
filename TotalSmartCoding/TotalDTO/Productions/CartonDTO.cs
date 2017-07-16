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
        public int FillingCartonID { get; set; }
        public Nullable<int> PalletID { get; set; }
    }

    public class FillingCartonPrimitiveDTO : CartonPrimitiveDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.FillingCarton; } }

        public override int GetID() { return this.FillingCartonID; }
        public override void SetID(int id) { this.FillingCartonID = id; }

        public FillingCartonPrimitiveDTO()
        {
            this.FillingLineID = 1; //this.FillingLineData.FillingLineID
            this.CommodityID = 1; //this.FillingLineData.CommodityID
            this.PCID = "ABCD123456EF";
            this.EntryStatusID = 1; //(byte)GlobalVariables.BarcodeStatus.Normal
        }

        public Nullable<int> FillingPalletID { get; set; }
    }


    public class CartonDTO : CartonPrimitiveDTO
    {
    }

    public class FillingCartonDTO : FillingCartonPrimitiveDTO, IShallowClone<FillingCartonDTO>
    {
        public string FillingPackIDs { get; set; }

        public FillingCartonDTO ShallowClone()
        {
            return (FillingCartonDTO)this.MemberwiseClone();
        }
    }
}
