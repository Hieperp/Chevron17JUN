using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class OnlinePalletService : GenericService<OnlinePallet, OnlinePalletDTO, OnlinePalletPrimitiveDTO>, IOnlinePalletService
    {
        public OnlinePalletService(IOnlinePalletRepository goodsReceiptRepository)
            : base(goodsReceiptRepository)
        {
        }
    }
}
