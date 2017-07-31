﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TotalSmartCodingEntities : DbContext
    {
        public TotalSmartCodingEntities()
            : base("name=TotalSmartCodingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DeliveryAdviceDetail> DeliveryAdviceDetails { get; set; }
        public virtual DbSet<DeliveryAdvice> DeliveryAdvices { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<ModuleDetail> ModuleDetails { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<OrganizationalUnit> OrganizationalUnits { get; set; }
        public virtual DbSet<OrganizationalUnitUser> OrganizationalUnitUsers { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<PaymentTerm> PaymentTerms { get; set; }
        public virtual DbSet<BinLocation> BinLocations { get; set; }
        public virtual DbSet<FillingLine> FillingLines { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }
        public virtual DbSet<GoodsReceiptDetail> GoodsReceiptDetails { get; set; }
        public virtual DbSet<GoodsReceipt> GoodsReceipts { get; set; }
        public virtual DbSet<GoodsReceiptType> GoodsReceiptTypes { get; set; }
        public virtual DbSet<PickupDetail> PickupDetails { get; set; }
        public virtual DbSet<Pickup> Pickups { get; set; }
        public virtual DbSet<Commodity> Commodities { get; set; }
        public virtual DbSet<Carton> Cartons { get; set; }
        public virtual DbSet<FillingCarton> FillingCartons { get; set; }
        public virtual DbSet<FillingPack> FillingPacks { get; set; }
        public virtual DbSet<FillingPallet> FillingPallets { get; set; }
        public virtual DbSet<Pack> Packs { get; set; }
        public virtual DbSet<Pallet> Pallets { get; set; }
        public virtual DbSet<Batch> Batches { get; set; }
    
        public virtual ObjectResult<Nullable<int>> GetAccessLevel(Nullable<int> userID, Nullable<int> nMVNTaskID, Nullable<int> organizationalUnitID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var nMVNTaskIDParameter = nMVNTaskID.HasValue ?
                new ObjectParameter("NMVNTaskID", nMVNTaskID) :
                new ObjectParameter("NMVNTaskID", typeof(int));
    
            var organizationalUnitIDParameter = organizationalUnitID.HasValue ?
                new ObjectParameter("OrganizationalUnitID", organizationalUnitID) :
                new ObjectParameter("OrganizationalUnitID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("GetAccessLevel", userIDParameter, nMVNTaskIDParameter, organizationalUnitIDParameter);
        }
    
        public virtual ObjectResult<Nullable<bool>> GetApprovalPermitted(Nullable<int> userID, Nullable<int> nMVNTaskID, Nullable<int> organizationalUnitID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var nMVNTaskIDParameter = nMVNTaskID.HasValue ?
                new ObjectParameter("NMVNTaskID", nMVNTaskID) :
                new ObjectParameter("NMVNTaskID", typeof(int));
    
            var organizationalUnitIDParameter = organizationalUnitID.HasValue ?
                new ObjectParameter("OrganizationalUnitID", organizationalUnitID) :
                new ObjectParameter("OrganizationalUnitID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("GetApprovalPermitted", userIDParameter, nMVNTaskIDParameter, organizationalUnitIDParameter);
        }
    
        public virtual ObjectResult<Nullable<bool>> GetShowDiscount(Nullable<int> userID, Nullable<int> nMVNTaskID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var nMVNTaskIDParameter = nMVNTaskID.HasValue ?
                new ObjectParameter("NMVNTaskID", nMVNTaskID) :
                new ObjectParameter("NMVNTaskID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("GetShowDiscount", userIDParameter, nMVNTaskIDParameter);
        }
    
        public virtual ObjectResult<Nullable<bool>> GetShowDiscountByCustomer(Nullable<int> customerID)
        {
            var customerIDParameter = customerID.HasValue ?
                new ObjectParameter("CustomerID", customerID) :
                new ObjectParameter("CustomerID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("GetShowDiscountByCustomer", customerIDParameter);
        }
    
        public virtual ObjectResult<Nullable<bool>> GetUnApprovalPermitted(Nullable<int> userID, Nullable<int> nMVNTaskID, Nullable<int> organizationalUnitID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var nMVNTaskIDParameter = nMVNTaskID.HasValue ?
                new ObjectParameter("NMVNTaskID", nMVNTaskID) :
                new ObjectParameter("NMVNTaskID", typeof(int));
    
            var organizationalUnitIDParameter = organizationalUnitID.HasValue ?
                new ObjectParameter("OrganizationalUnitID", organizationalUnitID) :
                new ObjectParameter("OrganizationalUnitID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("GetUnApprovalPermitted", userIDParameter, nMVNTaskIDParameter, organizationalUnitIDParameter);
        }
    
        public virtual ObjectResult<Nullable<bool>> GetUnVoidablePermitted(Nullable<int> userID, Nullable<int> nMVNTaskID, Nullable<int> organizationalUnitID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var nMVNTaskIDParameter = nMVNTaskID.HasValue ?
                new ObjectParameter("NMVNTaskID", nMVNTaskID) :
                new ObjectParameter("NMVNTaskID", typeof(int));
    
            var organizationalUnitIDParameter = organizationalUnitID.HasValue ?
                new ObjectParameter("OrganizationalUnitID", organizationalUnitID) :
                new ObjectParameter("OrganizationalUnitID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("GetUnVoidablePermitted", userIDParameter, nMVNTaskIDParameter, organizationalUnitIDParameter);
        }
    
        public virtual ObjectResult<Nullable<bool>> GetVoidablePermitted(Nullable<int> userID, Nullable<int> nMVNTaskID, Nullable<int> organizationalUnitID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var nMVNTaskIDParameter = nMVNTaskID.HasValue ?
                new ObjectParameter("NMVNTaskID", nMVNTaskID) :
                new ObjectParameter("NMVNTaskID", typeof(int));
    
            var organizationalUnitIDParameter = organizationalUnitID.HasValue ?
                new ObjectParameter("OrganizationalUnitID", organizationalUnitID) :
                new ObjectParameter("OrganizationalUnitID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<bool>>("GetVoidablePermitted", userIDParameter, nMVNTaskIDParameter, organizationalUnitIDParameter);
        }
    
        public virtual ObjectResult<string> DeliveryAdviceApproved(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("DeliveryAdviceApproved", entityIDParameter);
        }
    
        public virtual ObjectResult<string> DeliveryAdviceEditable(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("DeliveryAdviceEditable", entityIDParameter);
        }
    
        public virtual ObjectResult<string> DeliveryAdvicePostSaveValidate(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("DeliveryAdvicePostSaveValidate", entityIDParameter);
        }
    
        public virtual int DeliveryAdviceSaveRelative(Nullable<int> entityID, Nullable<int> saveRelativeOption)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            var saveRelativeOptionParameter = saveRelativeOption.HasValue ?
                new ObjectParameter("SaveRelativeOption", saveRelativeOption) :
                new ObjectParameter("SaveRelativeOption", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeliveryAdviceSaveRelative", entityIDParameter, saveRelativeOptionParameter);
        }
    
        public virtual int DeliveryAdviceToggleApproved(Nullable<int> entityID, Nullable<bool> approved)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            var approvedParameter = approved.HasValue ?
                new ObjectParameter("Approved", approved) :
                new ObjectParameter("Approved", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeliveryAdviceToggleApproved", entityIDParameter, approvedParameter);
        }
    
        public virtual ObjectResult<DeliveryAdviceIndex> GetDeliveryAdviceIndexes(string aspUserID, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var aspUserIDParameter = aspUserID != null ?
                new ObjectParameter("AspUserID", aspUserID) :
                new ObjectParameter("AspUserID", typeof(string));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DeliveryAdviceIndex>("GetDeliveryAdviceIndexes", aspUserIDParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<DeliveryAdviceViewDetail> GetDeliveryAdviceViewDetails(Nullable<int> deliveryAdviceID)
        {
            var deliveryAdviceIDParameter = deliveryAdviceID.HasValue ?
                new ObjectParameter("DeliveryAdviceID", deliveryAdviceID) :
                new ObjectParameter("DeliveryAdviceID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DeliveryAdviceViewDetail>("GetDeliveryAdviceViewDetails", deliveryAdviceIDParameter);
        }
    
        public virtual ObjectResult<PendingPickupDetail> GetPendingPickupDetails(Nullable<int> locationID, Nullable<int> goodsReceiptID, Nullable<int> pickupID, Nullable<int> warehouseID, string pickupDetailIDs, Nullable<bool> isReadonly)
        {
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            var goodsReceiptIDParameter = goodsReceiptID.HasValue ?
                new ObjectParameter("GoodsReceiptID", goodsReceiptID) :
                new ObjectParameter("GoodsReceiptID", typeof(int));
    
            var pickupIDParameter = pickupID.HasValue ?
                new ObjectParameter("PickupID", pickupID) :
                new ObjectParameter("PickupID", typeof(int));
    
            var warehouseIDParameter = warehouseID.HasValue ?
                new ObjectParameter("WarehouseID", warehouseID) :
                new ObjectParameter("WarehouseID", typeof(int));
    
            var pickupDetailIDsParameter = pickupDetailIDs != null ?
                new ObjectParameter("PickupDetailIDs", pickupDetailIDs) :
                new ObjectParameter("PickupDetailIDs", typeof(string));
    
            var isReadonlyParameter = isReadonly.HasValue ?
                new ObjectParameter("IsReadonly", isReadonly) :
                new ObjectParameter("IsReadonly", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PendingPickupDetail>("GetPendingPickupDetails", locationIDParameter, goodsReceiptIDParameter, pickupIDParameter, warehouseIDParameter, pickupDetailIDsParameter, isReadonlyParameter);
        }
    
        public virtual ObjectResult<string> GoodsReceiptApproved(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("GoodsReceiptApproved", entityIDParameter);
        }
    
        public virtual ObjectResult<string> GoodsReceiptEditable(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("GoodsReceiptEditable", entityIDParameter);
        }
    
        public virtual ObjectResult<string> GoodsReceiptPostSaveValidate(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("GoodsReceiptPostSaveValidate", entityIDParameter);
        }
    
        public virtual int GoodsReceiptSaveRelative(Nullable<int> entityID, Nullable<int> saveRelativeOption)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            var saveRelativeOptionParameter = saveRelativeOption.HasValue ?
                new ObjectParameter("SaveRelativeOption", saveRelativeOption) :
                new ObjectParameter("SaveRelativeOption", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GoodsReceiptSaveRelative", entityIDParameter, saveRelativeOptionParameter);
        }
    
        public virtual int GoodsReceiptToggleApproved(Nullable<int> entityID, Nullable<bool> approved)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            var approvedParameter = approved.HasValue ?
                new ObjectParameter("Approved", approved) :
                new ObjectParameter("Approved", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GoodsReceiptToggleApproved", entityIDParameter, approvedParameter);
        }
    
        public virtual ObjectResult<GoodsReceiptIndex> GetGoodsReceiptIndexes(string aspUserID, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var aspUserIDParameter = aspUserID != null ?
                new ObjectParameter("AspUserID", aspUserID) :
                new ObjectParameter("AspUserID", typeof(string));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GoodsReceiptIndex>("GetGoodsReceiptIndexes", aspUserIDParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<GoodsReceiptViewDetail> GetGoodsReceiptViewDetails(Nullable<int> goodsReceiptID)
        {
            var goodsReceiptIDParameter = goodsReceiptID.HasValue ?
                new ObjectParameter("GoodsReceiptID", goodsReceiptID) :
                new ObjectParameter("GoodsReceiptID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GoodsReceiptViewDetail>("GetGoodsReceiptViewDetails", goodsReceiptIDParameter);
        }
    
        public virtual ObjectResult<PendingPickup> GetPendingPickups(Nullable<int> locationID)
        {
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PendingPickup>("GetPendingPickups", locationIDParameter);
        }
    
        public virtual ObjectResult<PendingPickupWarehouse> GetPendingPickupWarehouses(Nullable<int> locationID)
        {
            var locationIDParameter = locationID.HasValue ?
                new ObjectParameter("LocationID", locationID) :
                new ObjectParameter("LocationID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PendingPickupWarehouse>("GetPendingPickupWarehouses", locationIDParameter);
        }
    
        public virtual ObjectResult<string> BatchEditable(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("BatchEditable", entityIDParameter);
        }
    
        public virtual ObjectResult<BatchIndex> GetBatchIndexes(string aspUserID, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<int> fillingLineID)
        {
            var aspUserIDParameter = aspUserID != null ?
                new ObjectParameter("AspUserID", aspUserID) :
                new ObjectParameter("AspUserID", typeof(string));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            var fillingLineIDParameter = fillingLineID.HasValue ?
                new ObjectParameter("FillingLineID", fillingLineID) :
                new ObjectParameter("FillingLineID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BatchIndex>("GetBatchIndexes", aspUserIDParameter, fromDateParameter, toDateParameter, fillingLineIDParameter);
        }
    
        public virtual ObjectResult<string> FillingCartonEditable(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("FillingCartonEditable", entityIDParameter);
        }
    
        public virtual int FillingCartonSaveRelative(Nullable<int> entityID, Nullable<int> saveRelativeOption, string fillingPackIDs)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            var saveRelativeOptionParameter = saveRelativeOption.HasValue ?
                new ObjectParameter("SaveRelativeOption", saveRelativeOption) :
                new ObjectParameter("SaveRelativeOption", typeof(int));
    
            var fillingPackIDsParameter = fillingPackIDs != null ?
                new ObjectParameter("FillingPackIDs", fillingPackIDs) :
                new ObjectParameter("FillingPackIDs", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("FillingCartonSaveRelative", entityIDParameter, saveRelativeOptionParameter, fillingPackIDsParameter);
        }
    
        public virtual ObjectResult<string> FillingPackEditable(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("FillingPackEditable", entityIDParameter);
        }
    
        public virtual int FillingPackUpdateEntryStatus(string fillingPackIDs, Nullable<int> entryStatusID)
        {
            var fillingPackIDsParameter = fillingPackIDs != null ?
                new ObjectParameter("FillingPackIDs", fillingPackIDs) :
                new ObjectParameter("FillingPackIDs", typeof(string));
    
            var entryStatusIDParameter = entryStatusID.HasValue ?
                new ObjectParameter("EntryStatusID", entryStatusID) :
                new ObjectParameter("EntryStatusID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("FillingPackUpdateEntryStatus", fillingPackIDsParameter, entryStatusIDParameter);
        }
    
        public virtual int FillingCartonUpdateEntryStatus(string fillingCartonIDs, Nullable<int> entryStatusID)
        {
            var fillingCartonIDsParameter = fillingCartonIDs != null ?
                new ObjectParameter("FillingCartonIDs", fillingCartonIDs) :
                new ObjectParameter("FillingCartonIDs", typeof(string));
    
            var entryStatusIDParameter = entryStatusID.HasValue ?
                new ObjectParameter("EntryStatusID", entryStatusID) :
                new ObjectParameter("EntryStatusID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("FillingCartonUpdateEntryStatus", fillingCartonIDsParameter, entryStatusIDParameter);
        }
    
        public virtual ObjectResult<string> FillingPalletEditable(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("FillingPalletEditable", entityIDParameter);
        }
    
        public virtual int FillingPalletSaveRelative(Nullable<int> entityID, Nullable<int> saveRelativeOption, string fillingCartonIDs)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            var saveRelativeOptionParameter = saveRelativeOption.HasValue ?
                new ObjectParameter("SaveRelativeOption", saveRelativeOption) :
                new ObjectParameter("SaveRelativeOption", typeof(int));
    
            var fillingCartonIDsParameter = fillingCartonIDs != null ?
                new ObjectParameter("FillingCartonIDs", fillingCartonIDs) :
                new ObjectParameter("FillingCartonIDs", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("FillingPalletSaveRelative", entityIDParameter, saveRelativeOptionParameter, fillingCartonIDsParameter);
        }
    
        public virtual int FillingPalletUpdateEntryStatus(string fillingPalletIDs, Nullable<int> entryStatusID)
        {
            var fillingPalletIDsParameter = fillingPalletIDs != null ?
                new ObjectParameter("FillingPalletIDs", fillingPalletIDs) :
                new ObjectParameter("FillingPalletIDs", typeof(string));
    
            var entryStatusIDParameter = entryStatusID.HasValue ?
                new ObjectParameter("EntryStatusID", entryStatusID) :
                new ObjectParameter("EntryStatusID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("FillingPalletUpdateEntryStatus", fillingPalletIDsParameter, entryStatusIDParameter);
        }
    
        public virtual int BatchCommonUpdate(Nullable<int> batchID, string nextPackNo, string nextCartonNo, string nextPalletNo)
        {
            var batchIDParameter = batchID.HasValue ?
                new ObjectParameter("BatchID", batchID) :
                new ObjectParameter("BatchID", typeof(int));
    
            var nextPackNoParameter = nextPackNo != null ?
                new ObjectParameter("NextPackNo", nextPackNo) :
                new ObjectParameter("NextPackNo", typeof(string));
    
            var nextCartonNoParameter = nextCartonNo != null ?
                new ObjectParameter("NextCartonNo", nextCartonNo) :
                new ObjectParameter("NextCartonNo", typeof(string));
    
            var nextPalletNoParameter = nextPalletNo != null ?
                new ObjectParameter("NextPalletNo", nextPalletNo) :
                new ObjectParameter("NextPalletNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("BatchCommonUpdate", batchIDParameter, nextPackNoParameter, nextCartonNoParameter, nextPalletNoParameter);
        }
    
        public virtual int FillingPackUpdateQueueID(string fillingPackIDs, Nullable<int> queueID)
        {
            var fillingPackIDsParameter = fillingPackIDs != null ?
                new ObjectParameter("FillingPackIDs", fillingPackIDs) :
                new ObjectParameter("FillingPackIDs", typeof(string));
    
            var queueIDParameter = queueID.HasValue ?
                new ObjectParameter("QueueID", queueID) :
                new ObjectParameter("QueueID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("FillingPackUpdateQueueID", fillingPackIDsParameter, queueIDParameter);
        }
    
        public virtual ObjectResult<FillingPack> GetFillingPacks(Nullable<int> fillingLineID, string entryStatusIDs, Nullable<int> fillingCartonID)
        {
            var fillingLineIDParameter = fillingLineID.HasValue ?
                new ObjectParameter("FillingLineID", fillingLineID) :
                new ObjectParameter("FillingLineID", typeof(int));
    
            var entryStatusIDsParameter = entryStatusIDs != null ?
                new ObjectParameter("EntryStatusIDs", entryStatusIDs) :
                new ObjectParameter("EntryStatusIDs", typeof(string));
    
            var fillingCartonIDParameter = fillingCartonID.HasValue ?
                new ObjectParameter("FillingCartonID", fillingCartonID) :
                new ObjectParameter("FillingCartonID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FillingPack>("GetFillingPacks", fillingLineIDParameter, entryStatusIDsParameter, fillingCartonIDParameter);
        }
    
        public virtual ObjectResult<FillingPack> GetFillingPacks(Nullable<int> fillingLineID, string entryStatusIDs, Nullable<int> fillingCartonID, MergeOption mergeOption)
        {
            var fillingLineIDParameter = fillingLineID.HasValue ?
                new ObjectParameter("FillingLineID", fillingLineID) :
                new ObjectParameter("FillingLineID", typeof(int));
    
            var entryStatusIDsParameter = entryStatusIDs != null ?
                new ObjectParameter("EntryStatusIDs", entryStatusIDs) :
                new ObjectParameter("EntryStatusIDs", typeof(string));
    
            var fillingCartonIDParameter = fillingCartonID.HasValue ?
                new ObjectParameter("FillingCartonID", fillingCartonID) :
                new ObjectParameter("FillingCartonID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FillingPack>("GetFillingPacks", mergeOption, fillingLineIDParameter, entryStatusIDsParameter, fillingCartonIDParameter);
        }
    
        public virtual ObjectResult<FillingCarton> GetFillingCartons(Nullable<int> fillingLineID, string entryStatusIDs)
        {
            var fillingLineIDParameter = fillingLineID.HasValue ?
                new ObjectParameter("FillingLineID", fillingLineID) :
                new ObjectParameter("FillingLineID", typeof(int));
    
            var entryStatusIDsParameter = entryStatusIDs != null ?
                new ObjectParameter("EntryStatusIDs", entryStatusIDs) :
                new ObjectParameter("EntryStatusIDs", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FillingCarton>("GetFillingCartons", fillingLineIDParameter, entryStatusIDsParameter);
        }
    
        public virtual ObjectResult<FillingCarton> GetFillingCartons(Nullable<int> fillingLineID, string entryStatusIDs, MergeOption mergeOption)
        {
            var fillingLineIDParameter = fillingLineID.HasValue ?
                new ObjectParameter("FillingLineID", fillingLineID) :
                new ObjectParameter("FillingLineID", typeof(int));
    
            var entryStatusIDsParameter = entryStatusIDs != null ?
                new ObjectParameter("EntryStatusIDs", entryStatusIDs) :
                new ObjectParameter("EntryStatusIDs", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FillingCarton>("GetFillingCartons", mergeOption, fillingLineIDParameter, entryStatusIDsParameter);
        }
    
        public virtual ObjectResult<FillingPallet> GetFillingPallets(Nullable<int> fillingLineID, string entryStatusIDs)
        {
            var fillingLineIDParameter = fillingLineID.HasValue ?
                new ObjectParameter("FillingLineID", fillingLineID) :
                new ObjectParameter("FillingLineID", typeof(int));
    
            var entryStatusIDsParameter = entryStatusIDs != null ?
                new ObjectParameter("EntryStatusIDs", entryStatusIDs) :
                new ObjectParameter("EntryStatusIDs", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FillingPallet>("GetFillingPallets", fillingLineIDParameter, entryStatusIDsParameter);
        }
    
        public virtual ObjectResult<FillingPallet> GetFillingPallets(Nullable<int> fillingLineID, string entryStatusIDs, MergeOption mergeOption)
        {
            var fillingLineIDParameter = fillingLineID.HasValue ?
                new ObjectParameter("FillingLineID", fillingLineID) :
                new ObjectParameter("FillingLineID", typeof(int));
    
            var entryStatusIDsParameter = entryStatusIDs != null ?
                new ObjectParameter("EntryStatusIDs", entryStatusIDs) :
                new ObjectParameter("EntryStatusIDs", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FillingPallet>("GetFillingPallets", mergeOption, fillingLineIDParameter, entryStatusIDsParameter);
        }
    
        public virtual ObjectResult<string> BatchPostSaveValidate(Nullable<int> entityID)
        {
            var entityIDParameter = entityID.HasValue ?
                new ObjectParameter("EntityID", entityID) :
                new ObjectParameter("EntityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("BatchPostSaveValidate", entityIDParameter);
        }
    
        public virtual ObjectResult<CommodityIndex> GetCommodityIndexes(string aspUserID, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var aspUserIDParameter = aspUserID != null ?
                new ObjectParameter("AspUserID", aspUserID) :
                new ObjectParameter("AspUserID", typeof(string));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CommodityIndex>("GetCommodityIndexes", aspUserIDParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<CommodityBase> GetCommodityBases()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CommodityBase>("GetCommodityBases");
        }
    }
}
