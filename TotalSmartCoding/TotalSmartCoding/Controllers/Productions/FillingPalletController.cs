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


namespace TotalSmartCoding.Controllers.Productions
{
    public class FillingPalletController : GenericSimpleController<FillingPallet, FillingPalletDTO, FillingPalletPrimitiveDTO, FillingPalletViewModel>
    {
        public IFillingPalletService fillingPalletService;

        public FillingPalletController(IFillingPalletService fillingPalletService, FillingPalletViewModel fillingPalletViewModel)
            : base(fillingPalletService, fillingPalletViewModel)
        {
            this.fillingPalletService = fillingPalletService;
        }
    }
}

