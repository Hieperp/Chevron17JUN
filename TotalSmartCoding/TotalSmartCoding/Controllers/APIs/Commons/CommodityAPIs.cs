using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;


using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Inventories;

using TotalCore.Repositories.Commons;
using TotalBase;

namespace TotalSmartCoding.Controllers.APIs.Commons
{
    public class CommodityAPIs
    {
        private readonly ICommodityAPIRepository goodsReceiptAPIRepository;

        public CommodityAPIs(ICommodityAPIRepository goodsReceiptAPIRepository)
        {
            this.goodsReceiptAPIRepository = goodsReceiptAPIRepository;
        }


        public ICollection<CommodityIndex> GetCommodityIndexes()
        {
            ICollection<CommodityIndex> goodsReceiptIndexes = this.goodsReceiptAPIRepository.GetEntityIndexes<CommodityIndex>(ContextAttributes.AspUserID, ContextAttributes.FromDate, ContextAttributes.ToDate).ToList();

            return goodsReceiptIndexes;
        }

        public IList<CommodityBase> GetCommodityBases()
        {
            return this.goodsReceiptAPIRepository.GetCommodityBases();
        }

    }
}
