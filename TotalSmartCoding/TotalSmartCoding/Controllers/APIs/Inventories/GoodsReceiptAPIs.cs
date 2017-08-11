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
            ICollection<GoodsReceiptIndex> goodsReceiptIndexes = this.goodsReceiptAPIRepository.GetEntityIndexes<GoodsReceiptIndex>(ContextAttributes.AspUserID, ContextAttributes.FromDate, ContextAttributes.ToDate);

            return goodsReceiptIndexes;
        }
    }
}
