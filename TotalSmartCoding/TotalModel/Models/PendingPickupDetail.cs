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
    
    public partial class PendingPickupDetail
    {
        public int PickupID { get; set; }
        public int PickupDetailID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Reference { get; set; }
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public Nullable<int> PalletID { get; set; }
        public string PalletCode { get; set; }
        public int CommodityID1 { get; set; }
        public string CommodityCode1 { get; set; }
        public string CommodityName1 { get; set; }
        public string Remarks { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantitySKU { get; set; }
        public Nullable<bool> IsSelected { get; set; }
    }
}