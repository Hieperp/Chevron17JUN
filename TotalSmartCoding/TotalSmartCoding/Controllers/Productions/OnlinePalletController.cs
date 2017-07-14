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
    public class OnlinePalletController : GenericSimpleController<OnlinePallet, OnlinePalletDTO, OnlinePalletPrimitiveDTO, OnlinePalletViewModel>
    {
        private IOnlinePalletService onlinePalletService;

        public OnlinePalletController(IOnlinePalletService onlinePalletService, IOnlinePalletViewModelSelectListBuilder onlinePalletSelectListBuilder, OnlinePalletViewModel onlinePalletViewModel)
            : base(onlinePalletService, onlinePalletSelectListBuilder, onlinePalletViewModel)
        {
            this.onlinePalletService = onlinePalletService;
        }
    }
}

