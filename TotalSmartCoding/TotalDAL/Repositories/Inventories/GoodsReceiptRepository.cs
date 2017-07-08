using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Inventories;

namespace TotalDAL.Repositories.Inventories
{
    public class GoodsReceiptRepository : GenericWithDetailRepository<GoodsReceipt, GoodsReceiptDetail>, IGoodsReceiptRepository
    {
        public GoodsReceiptRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "GoodsReceiptEditable", "GoodsReceiptApproved")
        {
        }
    }








    public class GoodsReceiptAPIRepository : GenericAPIRepository, IGoodsReceiptAPIRepository
    {
        public GoodsReceiptAPIRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "GetGoodsReceiptIndexes")
        {
        }

        public IEnumerable<PendingPickup> GetPickups(int? locationID)
        {
            this.TotalSmartCodingEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingPickup> pendingPickups = base.TotalSmartCodingEntities.GetPendingPickups(locationID).ToList();
            this.TotalSmartCodingEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPickups;
        }


        public IEnumerable<PendingPickupWarehouse> GetWarehouses(int? locationID)
        {
            this.TotalSmartCodingEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingPickupWarehouse> pendingPickupWarehouses = base.TotalSmartCodingEntities.GetPendingPickupWarehouses(locationID).ToList();
            this.TotalSmartCodingEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPickupWarehouses;
        }

        public IEnumerable<PendingPickupDetail> GetPendingPickupDetails(int? locationID, int? goodsReceiptID, int? pickupID, int? warehouseID, string pickupDetailIDs, bool isReadonly)
        {
            this.TotalSmartCodingEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingPickupDetail> pendingPickupDetails = base.TotalSmartCodingEntities.GetPendingPickupDetails(locationID, goodsReceiptID, pickupID, warehouseID, pickupDetailIDs, isReadonly).ToList();
            this.TotalSmartCodingEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPickupDetails;
        }

    }


}
