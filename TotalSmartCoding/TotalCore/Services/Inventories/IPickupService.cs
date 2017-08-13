using TotalModel.Models;

using TotalDTO.Inventories;

namespace TotalCore.Services.Inventories
{
    public interface IPickupService : IGenericWithViewDetailService<Pickup, PickupDetail, PickupViewDetail, PickupDTO, PickupPrimitiveDTO, PickupDetailDTO>
    {
    }
}

