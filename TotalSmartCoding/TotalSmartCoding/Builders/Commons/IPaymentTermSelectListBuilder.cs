using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalSmartCoding.Builders.Commons
{
    public interface IPaymentTermSelectListBuilder
    {
        //IEnumerable<SelectListItem> BuildSelectListItemsForPaymentTerms(IEnumerable<PaymentTerm> paymentTerms);
    }

    public class PaymentTermSelectListBuilder : IPaymentTermSelectListBuilder
    {
        //public IEnumerable<SelectListItem> BuildSelectListItemsForPaymentTerms(IEnumerable<PaymentTerm> paymentTerms)
        //{
        //    return paymentTerms.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.PaymentTermID.ToString() }).ToList();
        //}
    }
}
