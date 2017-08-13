using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;

namespace TotalService.Inventories
{
    public class PickupService : GenericWithViewDetailService<Pickup, PickupDetail, PickupViewDetail, PickupDTO, PickupPrimitiveDTO, PickupDetailDTO>, IPickupService
    {
        public PickupService(IPickupRepository pickupRepository)
            : base(pickupRepository, "PickupPostSaveValidate", "PickupSaveRelative", "PickupToggleApproved", null, null, "GetPickupViewDetails")
        {
        }

        public override ICollection<PickupViewDetail> GetViewDetails(int pickupID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PickupID", pickupID) };
            return this.GetViewDetails(parameters);
        }
    }
}
