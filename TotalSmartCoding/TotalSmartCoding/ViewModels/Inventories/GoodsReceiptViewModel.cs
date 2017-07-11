using System;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalDTO.Inventories;
using TotalSmartCoding.Builders;
using TotalSmartCoding.ViewModels.Helpers;

namespace TotalSmartCoding.ViewModels.Inventories
{
    public class GoodsReceiptViewModel : GoodsReceiptDTO, IViewDetailViewModel<GoodsReceiptDetailDTO>, IA02SimpleViewModel//, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IPaymentTermDropDownViewModel
    {
        //public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        //public IEnumerable<SelectListItem> PaymentTermSelectList { get; set; }
    }

}
