using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class WarehouseRepository : GenericRepository<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "WarehouseEditable")
        {
        }
    }





    public class WarehouseAPIRepository : GenericAPIRepository, IWarehouseAPIRepository
    {
        public WarehouseAPIRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "GetWarehouseIndexes")
        {
        }

        public IList<WarehouseBase> GetWarehouseBases()
        {
            return this.TotalSmartCodingEntities.GetWarehouseBases().ToList();
        }
    }
}
