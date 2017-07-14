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
    using System.Collections.Generic;
    
    public partial class Batch
    {
        public int BatchID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Reference { get; set; }
        public string Code { get; set; }
        public int FillingLineID { get; set; }
        public int CommodityID { get; set; }
        public string LastPackNo { get; set; }
        public string LastCartonNo { get; set; }
        public string LastPalletNo { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
        public bool IsDefault { get; set; }
    
        public virtual Commodity Commodity { get; set; }
        public virtual FillingLine FillingLine { get; set; }
    }
}
