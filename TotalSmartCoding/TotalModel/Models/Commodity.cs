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
    
    public partial class Commodity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Commodity()
        {
            this.DeliveryAdviceDetails = new HashSet<DeliveryAdviceDetail>();
            this.Cartons = new HashSet<Carton>();
            this.Packs = new HashSet<Pack>();
            this.Pallets = new HashSet<Pallet>();
            this.GoodsReceiptDetails = new HashSet<GoodsReceiptDetail>();
            this.PickupDetails = new HashSet<PickupDetail>();
            this.Batches = new HashSet<Batch>();
        }
    
        public int CommodityID { get; set; }
        public string Code { get; set; }
        public string OfficialCode { get; set; }
        public string Name { get; set; }
        public string OfficialName { get; set; }
        public string OriginalName { get; set; }
        public int CommodityCategoryID { get; set; }
        public int CommodityTypeID { get; set; }
        public int SupplierID { get; set; }
        public Nullable<int> PiecePerPack { get; set; }
        public Nullable<int> QuantityAlert { get; set; }
        public string PurchaseUnit { get; set; }
        public string SalesUnit { get; set; }
        public string Packing { get; set; }
        public string Origin { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<double> LeadTime { get; set; }
        public Nullable<bool> Discontinue { get; set; }
        public Nullable<bool> InActive { get; set; }
        public string Remarks { get; set; }
        public string CodePartA { get; set; }
        public string CodePartB { get; set; }
        public string CodePartC { get; set; }
        public string CodePartD { get; set; }
        public Nullable<int> PreviousCommodityID { get; set; }
        public decimal ListedPrice { get; set; }
        public decimal GrossPrice { get; set; }
        public string HSCode { get; set; }
        public bool IsRegularCheckUps { get; set; }
        public string Specifycation { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryAdviceDetail> DeliveryAdviceDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Carton> Cartons { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pack> Packs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pallet> Pallets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsReceiptDetail> GoodsReceiptDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PickupDetail> PickupDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Batch> Batches { get; set; }
    }
}
