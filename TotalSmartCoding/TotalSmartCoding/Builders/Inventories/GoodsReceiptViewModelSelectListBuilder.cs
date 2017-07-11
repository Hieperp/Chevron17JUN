using TotalCore.Repositories.Commons;
using TotalSmartCoding.Builders;
using TotalSmartCoding.Builders.Commons;
using TotalSmartCoding.ViewModels.Inventories;
namespace TotalSmartCoding.Builders.Inventories
{
    public interface IGoodsReceiptViewModelSelectListBuilder : IViewModelSelectListBuilder<GoodsReceiptViewModel>
    {
    }

    public class GoodsReceiptViewModelSelectListBuilder : A02ViewModelSelectListBuilder<GoodsReceiptViewModel>, IGoodsReceiptViewModelSelectListBuilder
    {
        public GoodsReceiptViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IPaymentTermSelectListBuilder paymentTermSelectListBuilder, IPaymentTermRepository paymentTermRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository, paymentTermSelectListBuilder, paymentTermRepository)
        {
        }
    }

}