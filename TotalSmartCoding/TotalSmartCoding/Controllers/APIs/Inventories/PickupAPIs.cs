using System.Collections.Generic;

using TotalBase;
using TotalModel.Models;
using TotalCore.Repositories.Inventories;

namespace TotalSmartCoding.Controllers.APIs.Inventories
{
    public class PickupAPIs
    {
        private readonly IPickupAPIRepository pickupAPIRepository;

        public PickupAPIs(IPickupAPIRepository pickupAPIRepository)
        {
            this.pickupAPIRepository = pickupAPIRepository;
        }


        public ICollection<PickupIndex> GetPickupIndexes()
        {
            return this.pickupAPIRepository.GetEntityIndexes<PickupIndex>(ContextAttributes.AspUserID, ContextAttributes.FromDate, ContextAttributes.ToDate);
        }

        public List<PendingPallet> GetPendingPallets(int? locationID, int? pickupID, string palletIDs, bool isReadonly)
        {
            return this.pickupAPIRepository.GetPendingPallets(locationID, pickupID, palletIDs, isReadonly);
        }
    }
}
