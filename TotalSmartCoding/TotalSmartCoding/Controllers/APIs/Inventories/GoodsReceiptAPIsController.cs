using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;



using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Inventories;

using TotalCore.Repositories.Inventories;
using TotalBase;

namespace TotalSmartCoding.Controllers.APIs.Inventories
{
    public class GoodsReceiptAPIsController
    {
        private readonly IGoodsReceiptAPIRepository goodsReceiptAPIRepository;

        public GoodsReceiptAPIsController(IGoodsReceiptAPIRepository goodsReceiptAPIRepository)
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
