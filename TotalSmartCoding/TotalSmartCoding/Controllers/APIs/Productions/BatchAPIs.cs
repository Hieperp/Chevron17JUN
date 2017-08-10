using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;



using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Inventories;

using TotalCore.Repositories.Productions;
using TotalBase;

namespace TotalSmartCoding.Controllers.APIs.Productions
{
    public class BatchAPIs
    {
        private readonly IBatchAPIRepository batchAPIRepository;

        public BatchAPIs(IBatchAPIRepository batchAPIRepository)
        {
            this.batchAPIRepository = batchAPIRepository;
        }


        public ICollection<BatchIndex> GetBatchIndexes(GlobalEnums.ActiveOption activeOption)
        {
            this.batchAPIRepository.RepositoryBag["ActiveOption"] = (int)activeOption;
            ICollection<BatchIndex> goodsReceiptIndexes = this.batchAPIRepository.GetEntityIndexes<BatchIndex>(ContextAttributes.AspUserID, ContextAttributes.FromDate, ContextAttributes.ToDate);

            return goodsReceiptIndexes;
        }

        public BatchIndex GetActiveBatchIndex()
        {
            BatchIndex goodsReceiptIndexes = this.GetBatchIndexes(GlobalEnums.ActiveOption.Active).Where(w => w.IsDefault).FirstOrDefault();

            return goodsReceiptIndexes;
        }

    }
}
