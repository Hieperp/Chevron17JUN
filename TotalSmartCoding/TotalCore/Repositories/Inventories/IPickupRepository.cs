using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IPickupRepository : IGenericWithDetailRepository<Pickup, PickupDetail>
    {
    }

    public interface IPickupAPIRepository : IGenericAPIRepository
    {
        List<PendingPallet> GetPendingPallets(int? locationID, int? pickupID, string palletIDs, bool isReadonly);
    }

}

