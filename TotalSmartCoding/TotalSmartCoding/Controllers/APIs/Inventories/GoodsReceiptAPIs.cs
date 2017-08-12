using System.Collections.Generic;

using TotalBase;
using TotalModel.Models;
using TotalCore.Repositories.Inventories;

namespace TotalSmartCoding.Controllers.APIs.Inventories
{
    public class GoodsReceiptAPIs
    {
        private readonly IGoodsReceiptAPIRepository goodsReceiptAPIRepository;

        public GoodsReceiptAPIs(IGoodsReceiptAPIRepository goodsReceiptAPIRepository)
        {
            this.goodsReceiptAPIRepository = goodsReceiptAPIRepository;
        }


        public ICollection<GoodsReceiptIndex> GetGoodsReceiptIndexes()
        {
            return this.goodsReceiptAPIRepository.GetEntityIndexes<GoodsReceiptIndex>(ContextAttributes.AspUserID, ContextAttributes.FromDate, ContextAttributes.ToDate);
        }

        public List<PendingPickup> GetPendingPickups(int? locationID)
        {
            return this.goodsReceiptAPIRepository.GetPendingPickups(locationID);
        }


        public List<PendingPickupWarehouse> GetPendingPickupWarehouses(int? locationID)
        {
            return this.goodsReceiptAPIRepository.GetPendingPickupWarehouses(locationID);
        }

        public List<PendingPickupDetail> GetPendingPickupDetails(int? locationID, int? goodsReceiptID, int? pickupID, int? warehouseID, string pickupDetailIDs, bool isReadonly)
        {
            return this.goodsReceiptAPIRepository.GetPendingPickupDetails(locationID, goodsReceiptID, pickupID, warehouseID, pickupDetailIDs, isReadonly);
        }
    }
}
