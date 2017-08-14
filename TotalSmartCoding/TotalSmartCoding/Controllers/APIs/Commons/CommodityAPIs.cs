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
        private readonly ICommodityAPIRepository commodityAPIRepository;

        public CommodityAPIs(ICommodityAPIRepository commodityAPIRepository)
        {
            this.commodityAPIRepository = commodityAPIRepository;
        }


        public ICollection<CommodityIndex> GetCommodityIndexes()
        {
            return this.commodityAPIRepository.GetEntityIndexes<CommodityIndex>(ContextAttributes.AspUserID, ContextAttributes.FromDate, ContextAttributes.ToDate).ToList();
        }

        public IList<CommodityBase> GetCommodityBases()
        {
            return this.commodityAPIRepository.GetCommodityBases();
        }

    }
}
