using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Equin.ApplicationFramework;

using TotalModel;
using TotalBase.Enums;
using TotalDTO.Helpers;
using TotalDTO.Commons;

namespace TotalDTO.Inventories
{
    public class PickupPrimitiveDTO : QuantityDTO<PickupDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public override GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Pickup; } }

        public override int GetID() { return this.PickupID; }
        public void SetID(int id) { this.PickupID = id; }

        private int pickupID;
        [DefaultValue(0)]
        public int PickupID
        {
            get { return this.pickupID; }
            set { ApplyPropertyChange<PickupPrimitiveDTO, int>(ref this.pickupID, o => o.PickupID, value); }
        }


        private int warehouseID;
        [DefaultValue(null)]
        public int WarehouseID
        {
            get { return this.warehouseID; }
            set { ApplyPropertyChange<PickupPrimitiveDTO, int>(ref this.warehouseID, o => o.WarehouseID, value); }
        }
        private string warehouseName;
        [DefaultValue("")]
        public string WarehouseName
        {
            get { return this.warehouseName; }
            set { ApplyPropertyChange<PickupDTO, string>(ref this.warehouseName, o => o.WarehouseName, value); }
        }



        private int forkliftDriverID;
        [DefaultValue(1)]
        public int ForkliftDriverID
        {
            get { return this.forkliftDriverID; }
            set { ApplyPropertyChange<PickupPrimitiveDTO, int>(ref this.forkliftDriverID, o => o.ForkliftDriverID, value); }
        }

        private int storekeeperID;
        [DefaultValue(1)]
        public int StorekeeperID
        {
            get { return this.storekeeperID; }
            set { ApplyPropertyChange<PickupPrimitiveDTO, int>(ref this.storekeeperID, o => o.StorekeeperID, value); }
        }



        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.DtoDetails().ToList().ForEach(e => { e.WarehouseID = this.WarehouseID; });
        }
    }

    public class PickupDTO : PickupPrimitiveDTO, IBaseDetailEntity<PickupDetailDTO>
    {
        public PickupDTO()
        {
            this.PickupViewDetails = new BindingList<PickupDetailDTO>();






            this.PackDetails = new BindingListView<PickupDetailDTO>(this.PickupViewDetails);
            this.CartonDetails = new BindingListView<PickupDetailDTO>(this.PickupViewDetails);
            this.PalletDetails = new BindingListView<PickupDetailDTO>(this.PickupViewDetails);

            this.PalletDetails.ApplyFilter(f => f.PackID != null);
            this.PalletDetails.ApplyFilter(f => f.CartonID != null);
            this.PalletDetails.ApplyFilter(f => f.PalletID != null);
        }


        public BindingList<PickupDetailDTO> PickupViewDetails { get; set; }
        public BindingList<PickupDetailDTO> ViewDetails { get { return this.PickupViewDetails; } set { this.PickupViewDetails = value; } }

        public ICollection<PickupDetailDTO> GetDetails() { return this.PickupViewDetails; }

        protected override IEnumerable<PickupDetailDTO> DtoDetails() { return this.PickupViewDetails; }






        public BindingListView<PickupDetailDTO> PackDetails { get; private set; }
        public BindingListView<PickupDetailDTO> CartonDetails { get; private set; }
        public BindingListView<PickupDetailDTO> PalletDetails { get; private set; }

    }

}
