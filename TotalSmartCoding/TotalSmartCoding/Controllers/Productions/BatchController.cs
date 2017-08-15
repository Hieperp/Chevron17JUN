using TotalModel.Models;
using TotalDTO.Productions;

using TotalCore.Services.Productions;
using TotalSmartCoding.ViewModels.Productions;

namespace TotalSmartCoding.Controllers.Productions
{
    public class BatchController : GenericSimpleController<Batch, BatchDTO, BatchPrimitiveDTO, BatchViewModel>
    {
        public BatchController(IBatchService batchService, BatchViewModel batchViewModel)
            : base(batchService, batchViewModel)
        {
        }
    }
}

