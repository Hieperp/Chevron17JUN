using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using AutoMapper;

using TotalBase.Enums;
using TotalModel.Models;
using TotalDTO.Sales;
using TotalCore.Repositories.Sales;
using TotalCore.Services.Sales;

namespace TotalService.Sales
{
    public class DeliveryAdviceService : GenericWithViewDetailService<DeliveryAdvice, DeliveryAdviceDetail, DeliveryAdviceViewDetail, DeliveryAdviceDTO, DeliveryAdvicePrimitiveDTO, DeliveryAdviceDetailDTO>, IDeliveryAdviceService
    {
        public DeliveryAdviceService(IDeliveryAdviceRepository deliveryAdviceRepository)
            : base(deliveryAdviceRepository, "DeliveryAdvicePostSaveValidate", "DeliveryAdviceSaveRelative", "DeliveryAdviceToggleApproved", "DeliveryAdviceToggleVoid", "DeliveryAdviceToggleVoidDetail", "GetDeliveryAdviceViewDetails")
        {
        }

        public override ICollection<DeliveryAdviceViewDetail> GetViewDetails(int deliveryAdviceID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("DeliveryAdviceID", deliveryAdviceID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(DeliveryAdviceDTO deliveryAdviceDTO)
        {
            deliveryAdviceDTO.DeliveryAdviceViewDetails.RemoveAll(x => x.Quantity == 0 && x.FreeQuantity == 0);
            return base.Save(deliveryAdviceDTO);
        }


    }

}
