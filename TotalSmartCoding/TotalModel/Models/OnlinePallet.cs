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
    
    public partial class OnlinePallet
    {
        public int OnlinePalletID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public int FillingLineID { get; set; }
        public int CommodityID { get; set; }
        public string PCID { get; set; }
        public string Code { get; set; }
        public int EntryStatusID { get; set; }
    }
}
