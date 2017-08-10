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
    public class FillingCartonController : GenericSimpleController<FillingCarton, FillingCartonDTO, FillingCartonPrimitiveDTO, FillingCartonViewModel>
    {
        public IFillingCartonService fillingCartonService;

        public FillingCartonController(IFillingCartonService fillingCartonService, FillingCartonViewModel fillingCartonViewModel)
            : base(fillingCartonService, fillingCartonViewModel)
        {
            this.fillingCartonService = fillingCartonService;
        }
    }
}

