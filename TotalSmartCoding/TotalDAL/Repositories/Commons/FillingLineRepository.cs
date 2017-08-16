using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class FillingLineRepository : GenericRepository<FillingLine>, IFillingLineRepository
    {
        public FillingLineRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "FillingLineEditable")
        {
        }
    }





    public class FillingLineAPIRepository : GenericAPIRepository, IFillingLineAPIRepository
    {
        public FillingLineAPIRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "GetFillingLineIndexes")
        {
        }

        public IList<FillingLineBase> GetFillingLineBases()
        {
            return this.TotalSmartCodingEntities.GetFillingLineBases().ToList();
        }
    }
}
