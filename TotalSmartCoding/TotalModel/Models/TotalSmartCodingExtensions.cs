using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TotalModel.Helpers;
using TotalModel.Interfaces;

namespace TotalModel.Models
{

    //public partial class SalesOrder : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<SalesOrderDetail>
    //{
    //    public int GetID() { return this.SalesOrderID; }

    //    public virtual Employee Salesperson { get { return this.Employee; } }
    //    public virtual Customer Receiver { get { return this.Customer1; } }

    //    public ICollection<SalesOrderDetail> GetDetails() { return this.SalesOrderDetails; }
    //}


    //public partial class SalesOrderDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    //{
    //    public int GetID() { return this.SalesOrderDetailID; }
    //    public int GetWarehouseID() { return (int)this.WarehouseID; }
    //}


    //public partial class SalesOrderIndex
    //{
    //    public decimal GrandTotalQuantity { get { return this.TotalQuantity + this.TotalFreeQuantity; } }
    //    public decimal GrandTotalQuantityAdvice { get { return this.TotalQuantityAdvice + this.TotalFreeQuantityAdvice; } }
    //}






    //public partial class SalesReturn : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<SalesReturnDetail>
    //{
    //    public int GetID() { return this.SalesReturnID; }

    //    public virtual Employee Salesperson { get { return this.Employee; } }
    //    public virtual Customer Receiver { get { return this.Customer1; } }

    //    public ICollection<SalesReturnDetail> GetDetails() { return this.SalesReturnDetails; }
    //}


    //public partial class SalesReturnDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    //{
    //    public int GetID() { return this.SalesReturnDetailID; }
    //    public int GetWarehouseID() { return (int)this.WarehouseID; }
    //}


    //public partial class SalesReturnIndex
    //{
    //    public decimal GrandTotalQuantity { get { return this.TotalQuantity + this.TotalFreeQuantity; } }
    //    public decimal GrandTotalQuantityReceived { get { return this.TotalQuantityReceived + this.TotalFreeQuantityReceived; } }
    //}



    //public partial class GoodsIssue : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<GoodsIssueDetail>
    //{
    //    public int GetID() { return this.GoodsIssueID; }

    //    public virtual Employee Storekeeper { get { return this.Employee; } }
    //    public virtual Customer Receiver { get { return this.Customer1; } }

    //    public ICollection<GoodsIssueDetail> GetDetails() { return this.GoodsIssueDetails; }
    //}


    //public partial class GoodsIssueDetail : IPrimitiveEntity, IHelperEntryDate, IHelperWarehouseID, IHelperCommodityID, IHelperCommodityTypeID
    //{
    //    public int GetID() { return this.GoodsIssueDetailID; }
    //    public int GetWarehouseID() { return (int)this.WarehouseID; }
    //}

    //public partial class DeliveryAdvicePendingCustomer
    //{
    //    public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.ShippingAddress; } }
    //}

    //public partial class DeliveryAdvicePendingSalesOrder
    //{
    //    public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.ShippingAddress; } }
    //}

    //public partial class PendingDeliveryAdvice
    //{
    //    public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.ShippingAddress; } }
    //}

    //public partial class PendingDeliveryAdviceCustomer
    //{
    //    public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.ShippingAddress; } }
    //}

    //public partial class HandlingUnitIndex
    //{
    //    public string CustomerDescription { get { return this.CustomerName + (this.CustomerName != this.ReceiverName ? ", Người nhận: " + this.ReceiverName : "") + ", Giao hàng: " + this.ShippingAddress; } }
    //}

    //public partial class HandlingUnit : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<HandlingUnitDetail>
    //{
    //    public int GetID() { return this.HandlingUnitID; }

    //    public virtual Employee PackagingStaff { get { return this.Employee; } }
    //    public virtual Customer Receiver { get { return this.Customer1; } }

    //    public ICollection<HandlingUnitDetail> GetDetails() { return this.HandlingUnitDetails; }
    //}


    //public partial class HandlingUnitDetail : IPrimitiveEntity, IHelperEntryDate
    //{
    //    public int GetID() { return this.HandlingUnitDetailID; }
    //}




    public partial class GoodsReceiptIndex : IBaseIndex
    {
        public int Id { get { return this.GoodsReceiptID; } }
    }

    public partial class GoodsReceipt : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<GoodsReceiptDetail>
    {
        public int GetID() { return this.GoodsReceiptID; }

        public ICollection<GoodsReceiptDetail> GetDetails() { return this.GoodsReceiptDetails; }
    }


    public partial class GoodsReceiptDetail : IPrimitiveEntity, IHelperEntryDate
    {
        public int GetID() { return this.GoodsReceiptDetailID; }
    }




    public partial class PickupIndex : IBaseIndex
    {
        public int Id { get { return this.PickupID; } }
    }

    public partial class Pickup : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<PickupDetail>
    {
        public int GetID() { return this.PickupID; }

        public ICollection<PickupDetail> GetDetails() { return this.PickupDetails; }
    }


    public partial class PickupDetail : IPrimitiveEntity, IHelperEntryDate
    {
        public int GetID() { return this.PickupDetailID; }
    }



    //public partial class PendingHandlingUnit
    //{
    //    public string ReceiverDescription { get { return (this.CustomerID == this.ReceiverID ? "" : this.ReceiverName + ", ") + this.ShippingAddress; } }
    //}



    //public partial class AccountInvoice : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<AccountInvoiceDetail>
    //{
    //    public int GetID() { return this.AccountInvoiceID; }

    //    public virtual Customer Consumer { get { return this.Customer1; } }
    //    public virtual Customer Receiver { get { return this.Customer2; } }

    //    public ICollection<AccountInvoiceDetail> GetDetails() { return this.AccountInvoiceDetails; }
    //}


    //public partial class AccountInvoiceDetail : IPrimitiveEntity, IHelperEntryDate
    //{
    //    public int GetID() { return this.AccountInvoiceDetailID; }
    //}

    //public partial class ReceiptIndex
    //{
    //    public string DebitAccountType { get { return (this.MonetaryAccountCode != null ? this.MonetaryAccountCode : (this.AdvanceReceiptReference != null ? "CT TT" : (this.SalesReturnReference != null ? "CT TH" : "CT CK"))); } }
    //    public string DebitAccountCode { get { return (this.MonetaryAccountCode != null ? null : (this.AdvanceReceiptReference != null ? this.AdvanceReceiptReference : (this.SalesReturnReference != null ? this.SalesReturnReference : this.CreditNoteReference))); } }
    //    public Nullable<System.DateTime> DebitAccountDate { get { return (this.MonetaryAccountCode != null ? null : (this.AdvanceReceiptDate != null ? this.AdvanceReceiptDate : (this.SalesReturnDate != null ? this.SalesReturnDate : this.CreditNoteDate))); } }
    //}


    //public partial class ReceiptViewDetail
    //{
    //    public string ReceiverDescription { get { return (this.CustomerID != this.ReceiverID ? this.ReceiverName + ", " : "") + this.Description; } }
    //}


    //public partial class Receipt : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<ReceiptDetail>
    //{
    //    public int GetID() { return this.ReceiptID; }

    //    public virtual Receipt AdvanceReceipt { get { return this.Receipt1; } }
    //    public virtual Employee Cashier { get { return this.Employee; } }

    //    public decimal TotalReceiptAmountSaved { get { return this.TotalReceiptAmount; } }
    //    public decimal TotalFluctuationAmountSaved { get { return this.TotalFluctuationAmount; } }

    //    public ICollection<ReceiptDetail> GetDetails() { return this.ReceiptDetails; }
    //}


    //public partial class ReceiptDetail : IPrimitiveEntity
    //{
    //    public int GetID() { return this.ReceiptDetailID; }
    //}





    //public partial class CreditNote : IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<CreditNoteDetail>
    //{
    //    public int GetID() { return this.CreditNoteID; }

    //    public virtual Employee Salesperson { get { return this.Employee; } }

    //    public ICollection<CreditNoteDetail> GetDetails() { return this.CreditNoteDetails; }
    //}


    //public partial class CreditNoteDetail : IPrimitiveEntity, IHelperEntryDate, IHelperCommodityID, IHelperCommodityTypeID
    //{
    //    public int GetID() { return this.CreditNoteDetailID; }
    //}





    //public partial class VoidType : IPrimitiveEntity, IBaseEntity
    //{
    //    public int GetID() { return this.VoidTypeID; }

    //    public int UserID { get; set; }
    //    public int PreparedPersonID { get; set; }
    //    public int OrganizationalUnitID { get; set; }
    //    public int LocationID { get; set; }

    //    public System.DateTime CreatedDate { get; set; }
    //    public System.DateTime EditedDate { get; set; }
    //}

    public partial class FillingLine : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.FillingLineID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class Warehouse : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.WarehouseID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class BinLocation : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.BinLocationID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    public partial class Employee : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.EmployeeID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }

    //public partial class Commodity : IPrimitiveEntity, IBaseEntity
    //{
    //    public int GetID() { return this.CommodityID; }

    //    public int UserID { get; set; }
    //    public int PreparedPersonID { get; set; }
    //    public int OrganizationalUnitID { get; set; }
    //    public int LocationID { get; set; }

    //    public System.DateTime CreatedDate { get; set; }
    //    public System.DateTime EditedDate { get; set; }
    //}

    //public partial class Customer : IPrimitiveEntity, IBaseEntity
    //{
    //    public int GetID() { return this.CustomerID; }

    //    public int UserID { get; set; }
    //    public int PreparedPersonID { get; set; }
    //    public int OrganizationalUnitID { get; set; }
    //    public int LocationID { get; set; }

    //    public System.DateTime CreatedDate { get; set; }
    //    public System.DateTime EditedDate { get; set; }
    //}

    //public partial class Promotion : IPrimitiveEntity, IBaseEntity
    //{
    //    public int GetID() { return this.PromotionID; }

    //    public int UserID { get; set; }
    //    public int PreparedPersonID { get; set; }
    //    public int OrganizationalUnitID { get; set; }
    //    public int LocationID { get; set; }

    //    public System.DateTime CreatedDate { get; set; }
    //    public System.DateTime EditedDate { get; set; }
    //}

    public partial class CommodityIndex : IBaseIndex
    {
        public int Id { get { return this.CommodityID; } }
    }

    public partial class Commodity : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.CommodityID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }

        public int LocationID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }

    }

    public partial class BatchIndex : IBaseIndex
    {
        public int Id { get { return this.BatchID; } }
    }

    public partial class Batch : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.BatchID; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }
    }


    public partial class FillingPack : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.FillingPackID; }

        public int LocationID { get; set; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }


    public partial class FillingCarton : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.FillingCartonID; }

        public int LocationID { get; set; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }


    public partial class FillingPallet : IPrimitiveEntity, IBaseEntity
    {
        public int GetID() { return this.FillingPalletID; }

        public int LocationID { get; set; }

        public int UserID { get; set; }
        public int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }


}
