using TotalModel.Models;
using TotalDTO.Inventories;

using TotalCore.Services.Inventories;
using TotalSmartCoding.ViewModels.Inventories;

namespace TotalSmartCoding.Controllers.Inventories
{
    public class PickupController : GenericViewDetailController<Pickup, PickupDetail, PickupViewDetail, PickupDTO, PickupPrimitiveDTO, PickupDetailDTO, PickupViewModel>
    {
        public PickupViewModel PickupViewModel { get; private set; }
        public PickupController(IPickupService pickupService, PickupViewModel pickupViewModel)
            : base(pickupService, pickupViewModel)
        {
            this.PickupViewModel = pickupViewModel;
        }
    }
}
