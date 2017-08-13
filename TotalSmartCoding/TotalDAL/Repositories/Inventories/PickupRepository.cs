using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Inventories;

namespace TotalDAL.Repositories.Inventories
{
    public class PickupRepository : GenericWithDetailRepository<Pickup, PickupDetail>, IPickupRepository
    {
        public PickupRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "PickupEditable", "PickupApproved")
        {
        }
    }








    public class PickupAPIRepository : GenericAPIRepository, IPickupAPIRepository
    {
        public PickupAPIRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "GetPickupIndexes")
        {
        }


        public List<PendingPallet> GetPendingPallets(int? locationID, int? pickupID, string palletIDs, bool isReadonly)
        {
            return base.TotalSmartCodingEntities.GetPendingPallets(locationID, pickupID, palletIDs, isReadonly).ToList();
        }

    }


}
