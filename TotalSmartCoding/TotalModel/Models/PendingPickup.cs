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
    
    public partial class PendingPickup
    {
        public int PickupID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Reference { get; set; }
        public int WarehouseID { get; set; }
        public string WarehouseCode { get; set; }
        public string ForkliftDriverName { get; set; }
        public decimal TotalQuantity { get; set; }
        public string Description { get; set; }
    }
}
