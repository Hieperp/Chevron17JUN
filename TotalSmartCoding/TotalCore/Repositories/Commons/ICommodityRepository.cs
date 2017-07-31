using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface ICommodityRepository
    {
        
    }

    public interface ICommodityAPIRepository : IGenericAPIRepository
    {
        IList<CommodityBase> GetCommodityBases();
    }
}
