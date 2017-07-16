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
    public class FillingPackController : GenericSimpleController<FillingPack, FillingPackDTO, FillingPackPrimitiveDTO, FillingPackViewModel>
    {
        public IFillingPackService fillingPackService;

        public FillingPackController(IFillingPackService fillingPackService, IFillingPackViewModelSelectListBuilder fillingPackSelectListBuilder, FillingPackViewModel fillingPackViewModel)
            : base(fillingPackService, fillingPackSelectListBuilder, fillingPackViewModel)
        {
            this.fillingPackService = fillingPackService;
        }
    }
}

