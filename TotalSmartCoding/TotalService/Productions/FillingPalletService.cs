using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class FillingPalletService : GenericService<FillingPallet, FillingPalletDTO, FillingPalletPrimitiveDTO>, IFillingPalletService
    {
        public FillingPalletService(IFillingPalletRepository goodsReceiptRepository)
            : base(goodsReceiptRepository)
        {
        }
    }
}
