using System;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalDTO.Sales;
using TotalSmartCoding.Builders;
using TotalSmartCoding.ViewModels.Helpers;

namespace TotalSmartCoding.ViewModels.Sales
{
    public class DeliveryAdviceViewModel : DeliveryAdviceDTO, IViewDetailViewModel<DeliveryAdviceDetailDTO>, IA02SimpleViewModel//, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IPaymentTermDropDownViewModel
    {
        //public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        //public IEnumerable<SelectListItem> PaymentTermSelectList { get; set; }
    }

}
