using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class CommodityRepository : GenericRepository<Commodity>, ICommodityRepository
    {
        public CommodityRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "CommodityEditable")
        {
        }
    }





    public class CommodityAPIRepository : GenericAPIRepository, ICommodityAPIRepository
    {
        public CommodityAPIRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "GetCommodityIndexes")
        {
        }

        public IList<CommodityBase> GetCommodityBases()
        {
            return this.TotalSmartCodingEntities.GetCommodityBases().ToList();
        }
    }
}
