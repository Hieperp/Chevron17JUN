using System;

using TotalModel.Models;
using TotalDTO.Productions;
using TotalCore.Repositories.Productions;
using TotalCore.Services.Productions;

namespace TotalService.Productions
{
    public class BatchService : GenericService<Batch, BatchDTO, BatchPrimitiveDTO>, IBatchService
    {
        private IBatchRepository batchRepository;
        public BatchService(IBatchRepository batchRepository)
            : base(batchRepository, "BatchPostSaveValidate", null, "BatchToggleApproved", "BatchToggleVoid")        
        {
            this.batchRepository = batchRepository;
        }

        public bool CommonUpdate(int batchID, string nextPackNo, string nextCartonNo, string nextPalletNo)
        {
            try
            {
                this.batchRepository.CommonUpdate(batchID, nextPackNo, nextCartonNo, nextPalletNo);
                return true;
            }
            catch (Exception ex)
            {
                this.ServiceTag = ex.Message;
                return false;
            }
        }
    }
}
