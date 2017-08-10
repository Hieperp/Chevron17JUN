using TotalBase.Enums;

using TotalModel.Models;

using TotalCore.Services.Sales;

using TotalDTO.Sales;

using TotalSmartCoding.Controllers;
using TotalSmartCoding.ViewModels.Sales;


namespace TotalSmartCoding.Controllers.Sales
{   
    public class DeliveryAdvicesController : GenericViewDetailController<DeliveryAdvice, DeliveryAdviceDetail, DeliveryAdviceViewDetail, DeliveryAdviceDTO, DeliveryAdvicePrimitiveDTO, DeliveryAdviceDetailDTO, DeliveryAdviceViewModel>
    {
        public DeliveryAdvicesController(IDeliveryAdviceService deliveryAdviceService, DeliveryAdviceViewModel deliveryAdviceViewModel)
            : base(deliveryAdviceService, deliveryAdviceViewModel)
        {
        }
    }
}
