using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class BinLocationRepository : GenericRepository<BinLocation>, IBinLocationRepository
    {
        public BinLocationRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "BinLocationEditable")
        {
        }
    }





    public class BinLocationAPIRepository : GenericAPIRepository, IBinLocationAPIRepository
    {
        public BinLocationAPIRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "GetBinLocationIndexes")
        {
        }

        public IList<BinLocationBase> GetBinLocationBases()
        {
            return this.TotalSmartCodingEntities.GetBinLocationBases().ToList();
        }
    }
}
