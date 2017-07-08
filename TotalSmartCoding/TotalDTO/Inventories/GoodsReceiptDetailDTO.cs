using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;

namespace TotalDTO.Inventories
{
    public class GoodsReceiptDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.GoodsReceiptDetailID; }

        public int GoodsReceiptDetailID { get; set; }
        public int GoodsReceiptID { get; set; }

        public Nullable<int> PickupID { get; set; }
        public Nullable<int> PickupDetailID { get; set; }

        public override int CommodityTypeID { get { return 1; } }


        public string PickupReference { get; set; }
        public Nullable<System.DateTime> PickupEntryDate { get; set; }
        public Nullable<int> PalletID { get; set; }
        public string PalletCode { get; set; }

        [UIHint("DecimalReadonly")]
        public override decimal Quantity { get; set; }

        [UIHint("DecimalReadonly")]
        public decimal QuantitySKU { get; set; }

    }


        


}
