//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TotalModel.Models
{
    using System;
    
    public partial class GoodsReceiptIndex
    {
        public int GoodsReceiptID { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public string Reference { get; set; }
        public string LocationCode { get; set; }
        public string GoodsReceiptTypeCode { get; set; }
        public string PickupReferences { get; set; }
        public string WarehouseCode { get; set; }
        public string Description { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalQuantitySKU { get; set; }
        public bool Approved { get; set; }
    }
}