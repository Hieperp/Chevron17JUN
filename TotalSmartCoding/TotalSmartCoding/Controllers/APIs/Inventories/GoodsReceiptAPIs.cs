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

        public ICollection<PendingPickup> GetPendingPickups(int? locationID)
        {
            return this.goodsReceiptAPIRepository.GetPendingPickups(locationID);
        }


        public ICollection<PendingPickupWarehouse> GetPendingPickupWarehouses(int? locationID)
        {
            return this.goodsReceiptAPIRepository.GetPendingPickupWarehouses(locationID);
        }


        //public ICollection<GoodsReceiptIndex> GetGoodsReceiptIndexesa()
        //{
        //    return this.goodsReceiptAPIRepository.GetPickups<GoodsReceiptIndex>(ContextAttributes.AspUserID, ContextAttributes.FromDate, ContextAttributes.ToDate);
        //}
    }
}
