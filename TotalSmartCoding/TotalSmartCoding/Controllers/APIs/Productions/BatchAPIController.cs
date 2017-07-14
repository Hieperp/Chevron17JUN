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
    public class BatchAPIController
    {
        private readonly IBatchAPIRepository goodsReceiptAPIRepository;

        public BatchAPIController(IBatchAPIRepository goodsReceiptAPIRepository)
        {
            this.goodsReceiptAPIRepository = goodsReceiptAPIRepository;
        }


        public ICollection<BatchIndex> GetBatchIndexes()
        {
            ICollection<BatchIndex> goodsReceiptIndexes = this.goodsReceiptAPIRepository.GetEntityIndexes<BatchIndex>(ContextAttributes.AspUserID, ContextAttributes.FromDate, ContextAttributes.ToDate);

            return goodsReceiptIndexes;
        }

        public BatchIndex GetActiveBatchIndex()
        {
            BatchIndex goodsReceiptIndexes = this.goodsReceiptAPIRepository.GetEntityIndexes<BatchIndex>(ContextAttributes.AspUserID, ContextAttributes.FromDate, ContextAttributes.ToDate).Where(a => a.IsDefault = true).FirstOrDefault();

            return goodsReceiptIndexes;
        }

    }
}
