using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class PaymentTermRepository : IPaymentTermRepository
    {
        private readonly TotalSmartCodingEntities totalSmartCodingEntities;

        public PaymentTermRepository(TotalSmartCodingEntities totalSmartCodingEntities)
        {
            this.totalSmartCodingEntities = totalSmartCodingEntities;
        }

        public IList<PaymentTerm> GetAllPaymentTerms()
        {
            return this.totalSmartCodingEntities.PaymentTerms.ToList();
        }
    }
}

