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
    public class OnlineCartonController : GenericSimpleController<OnlineCarton, OnlineCartonDTO, OnlineCartonPrimitiveDTO, OnlineCartonViewModel>
    {
        public IOnlineCartonService onlineCartonService;

        public OnlineCartonController(IOnlineCartonService onlineCartonService, IOnlineCartonViewModelSelectListBuilder onlineCartonSelectListBuilder, OnlineCartonViewModel onlineCartonViewModel)
            : base(onlineCartonService, onlineCartonSelectListBuilder, onlineCartonViewModel)
        {
            this.onlineCartonService = onlineCartonService;
        }
    }
}

