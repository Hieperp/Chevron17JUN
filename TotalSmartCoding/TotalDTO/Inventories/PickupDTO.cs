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
using TotalModel.Helpers;
using TotalBase;

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


        private Nullable<int> fillingLineID;
        [DefaultValue(null)]
        public Nullable<int> FillingLineID
        {
            get { return this.fillingLineID; }
            set { ApplyPropertyChange<PickupPrimitiveDTO, Nullable<int>>(ref this.fillingLineID, o => o.FillingLineID, value); }
        }
        private string fillingLineName;
        [DefaultValue("")]
        public string FillingLineName
        {
            get { return this.fillingLineName; }
            set { ApplyPropertyChange<PickupDTO, string>(ref this.fillingLineName, o => o.FillingLineName, value, false); }
        }


        private Nullable<int> warehouseID;
        [DefaultValue(null)]
        public Nullable<int> WarehouseID
        {
            get { return this.warehouseID; }
            set { ApplyPropertyChange<PickupPrimitiveDTO, Nullable<int>>(ref this.warehouseID, o => o.WarehouseID, value); }
        }
        private string warehouseName;
        [DefaultValue("")]
        public string WarehouseName
        {
            get { return this.warehouseName; }
            set { ApplyPropertyChange<PickupDTO, string>(ref this.warehouseName, o => o.WarehouseName, value, false); }
        }



        private Nullable<int> forkliftDriverID;
        [DefaultValue(null)]
        public Nullable<int> ForkliftDriverID
        {
            get { return this.forkliftDriverID; }
            set { ApplyPropertyChange<PickupPrimitiveDTO, Nullable<int>>(ref this.forkliftDriverID, o => o.ForkliftDriverID, value); }
        }

        private Nullable<int> storekeeperID;
        [DefaultValue(null)]
        public Nullable<int> StorekeeperID
        {
            get { return this.storekeeperID; }
            set { ApplyPropertyChange<PickupPrimitiveDTO, Nullable<int>>(ref this.storekeeperID, o => o.StorekeeperID, value); }
        }

        public override int PreparedPersonID { get { return 1; } }


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



        protected override List<ValidationRule> CreateRules()
        {
            List<ValidationRule> validationRules = base.CreateRules();
            validationRules.Add(new SimpleValidationRule(CommonExpressions.PropertyName<PickupDTO>(p => p.FillingLineID), "Vui lòng chọn chuyền.", delegate { return (this.FillingLineID != null && this.FillingLineID > 0); }));
            validationRules.Add(new SimpleValidationRule(CommonExpressions.PropertyName<PickupDTO>(p => p.WarehouseID), "Vui lòng chọn kho.", delegate { return (this.WarehouseID != null && this.WarehouseID > 0); }));
            validationRules.Add(new SimpleValidationRule(CommonExpressions.PropertyName<PickupDTO>(p => p.ForkliftDriverID), "Vui lòng chọn tài xế.", delegate { return (this.ForkliftDriverID != null && this.ForkliftDriverID > 0); }));
            validationRules.Add(new SimpleValidationRule(CommonExpressions.PropertyName<PickupDTO>(p => p.StorekeeperID), "Vui lòng chọn nhân viên kho.", delegate { return (this.StorekeeperID != null && this.StorekeeperID > 0); }));

            return validationRules;

        }
    }

}
