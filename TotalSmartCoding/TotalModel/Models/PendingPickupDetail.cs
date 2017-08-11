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
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public Nullable<int> PalletID { get; set; }
        public string PalletCode { get; set; }
        public string Remarks { get; set; }
        public int Quantity { get; set; }
        public Nullable<bool> IsSelected { get; set; }
        public string PickupReference { get; set; }
        public System.DateTime PickupEntryDate { get; set; }
        public int BinLocationID { get; set; }
        public string BinLocationCode { get; set; }
        public Nullable<int> PackID { get; set; }
        public string PackCode { get; set; }
        public Nullable<int> CartonID { get; set; }
        public string CartonCode { get; set; }
        public Nullable<decimal> QuantityRemains { get; set; }
        public string Description { get; set; }
    }
}
