using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;
using TotalDTO.Helpers;
using TotalDTO.Commons;

namespace TotalDTO.Inventories
{
    public class GoodsReceiptPrimitiveDTO : QuantityDTO<GoodsReceiptDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.GoodsReceipt; } }

        public override int GetID() { return this.GoodsReceiptID; }
        public void SetID(int id) { this.GoodsReceiptID = id; }

        public int GoodsReceiptID { get; set; }
        public int GoodsReceiptTypeID { get; set; }

        public virtual Nullable<int> PickupID { get; set; }
        public string PickupReferences { get; set; }

        public bool HasPickup { get; set; }

        public int WarehouseID { get; set; }
        public int ForkliftDriverID { get; set; }
        public int StorekeeperID { get; set; }
        
        public override decimal TotalQuantity { get { return this.DtoDetails().Select(o => o.Quantity).Sum(); } }
        public decimal TotalQuantitySKU { get { return this.DtoDetails().Select(o => o.QuantitySKU).Sum(); } }
    }

    public class GoodsReceiptDTO : GoodsReceiptPrimitiveDTO, IBaseDetailEntity<GoodsReceiptDetailDTO>
    {
        public GoodsReceiptDTO()
        {
            this.GoodsReceiptViewDetails = new BindingList<GoodsReceiptDetailDTO>();
        }

        public override Nullable<int> PickupID { get { return (this.Pickup != null ? (this.Pickup.PickupID > 0 ? (Nullable<int>)this.Pickup.PickupID : null) : null); } }
        [UIHint("Commons/PickupBox")]
        public PickupBoxDTO Pickup { get; set; }

        public BindingList<GoodsReceiptDetailDTO> GoodsReceiptViewDetails { get; set; }
        public BindingList<GoodsReceiptDetailDTO> ViewDetails { get { return this.GoodsReceiptViewDetails; } set { this.GoodsReceiptViewDetails = value; } }

        public ICollection<GoodsReceiptDetailDTO> GetDetails() { return this.GoodsReceiptViewDetails; }

        protected override IEnumerable<GoodsReceiptDetailDTO> DtoDetails() { return this.GoodsReceiptViewDetails; }
    }

}
