using TotalCore.Repositories.Commons;
using TotalSmartCoding.Builders;
using TotalSmartCoding.Builders.Commons;
using TotalSmartCoding.ViewModels.Sales;


namespace TotalSmartCoding.Builders.Sales
{
    public interface IDeliveryAdviceViewModelSelectListBuilder : IViewModelSelectListBuilder<DeliveryAdviceViewModel>
    {
    }

    public class DeliveryAdviceViewModelSelectListBuilder : A02ViewModelSelectListBuilder<DeliveryAdviceViewModel>, IDeliveryAdviceViewModelSelectListBuilder
    {
        public DeliveryAdviceViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IPaymentTermSelectListBuilder paymentTermSelectListBuilder, IPaymentTermRepository paymentTermRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, paymentTermSelectListBuilder, paymentTermRepository)
        {
        }
    }

}