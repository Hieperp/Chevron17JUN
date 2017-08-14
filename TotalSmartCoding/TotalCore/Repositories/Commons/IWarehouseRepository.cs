using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IWarehouseRepository
    {

    }

    public interface IWarehouseAPIRepository : IGenericAPIRepository
    {
        IList<WarehouseBase> GetWarehouseBases();
    }
}
