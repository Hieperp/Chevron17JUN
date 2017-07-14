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
    public class OnlinePackController : GenericSimpleController<OnlinePack, OnlinePackDTO, OnlinePackPrimitiveDTO, OnlinePackViewModel>
    {
        public IOnlinePackService onlinePackService;

        public OnlinePackController(IOnlinePackService onlinePackService, IOnlinePackViewModelSelectListBuilder onlinePackSelectListBuilder, OnlinePackViewModel onlinePackViewModel)
            : base(onlinePackService, onlinePackSelectListBuilder, onlinePackViewModel)
        {
            this.onlinePackService = onlinePackService;
        }
    }
}

