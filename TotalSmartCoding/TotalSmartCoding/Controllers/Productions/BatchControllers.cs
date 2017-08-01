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
    public class BatchControllers : GenericSimpleController<Batch, BatchDTO, BatchPrimitiveDTO, BatchViewModel>
    {
        public BatchViewModel BatchViewModel { get; private set; }
        public BatchControllers(IBatchService batchService, IBatchViewModelSelectListBuilder batchSelectListBuilder, BatchViewModel batchViewModel)
            : base(batchService, batchSelectListBuilder, batchViewModel)
        {
            this.BatchViewModel = batchViewModel;
        }
    }
}

