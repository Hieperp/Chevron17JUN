using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class BatchService : GenericService<Batch, BatchDTO, BatchPrimitiveDTO>, IBatchService
    {
        public BatchService(IBatchRepository batchRepository)
            : base(batchRepository)
        {
        }
    }
}
