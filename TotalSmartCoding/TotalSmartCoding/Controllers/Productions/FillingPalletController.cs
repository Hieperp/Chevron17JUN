using System.Net;
using System.Linq;

using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Commons;

using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

using TotalSmartCoding.Controllers;
using TotalDTO.Productions;
using TotalSmartCoding.ViewModels.Productions;
using TotalSmartCoding.Builders.Productions;


namespace TotalSmartCoding.Controllers.Productions
{
    public class FillingPalletController : GenericSimpleController<FillingPallet, FillingPalletDTO, FillingPalletPrimitiveDTO, FillingPalletViewModel>
    {
        private IFillingPalletService fillingPalletService;

        public FillingPalletController(IFillingPalletService fillingPalletService, IFillingPalletViewModelSelectListBuilder fillingPalletSelectListBuilder, FillingPalletViewModel fillingPalletViewModel)
            : base(fillingPalletService, fillingPalletSelectListBuilder, fillingPalletViewModel)
        {
            this.fillingPalletService = fillingPalletService;
        }
    }
}

