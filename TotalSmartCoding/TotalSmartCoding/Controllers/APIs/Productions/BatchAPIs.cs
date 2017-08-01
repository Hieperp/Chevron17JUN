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
        private readonly IBatchAPIRepository goodsReceiptAPIRepository;

        public BatchAPIs(IBatchAPIRepository goodsReceiptAPIRepository)
        {
            this.goodsReceiptAPIRepository = goodsReceiptAPIRepository;
        }


        public ICollection<BatchIndex> GetBatchIndexes()
        {
            ICollection<BatchIndex> goodsReceiptIndexes = this.goodsReceiptAPIRepository.GetEntityIndexes<BatchIndex>(ContextAttributes.AspUserID, ContextAttributes.FromDate, ContextAttributes.ToDate).Where(w => w.FillingLineID == (int)GlobalVariables.FillingLineID).ToList();

            return goodsReceiptIndexes;
        }

        public BatchIndex GetActiveBatchIndex()
        {
            BatchIndex goodsReceiptIndexes = this.GetBatchIndexes().Where(w => w.FillingLineID == (int)GlobalVariables.FillingLineID && w.IsDefault).FirstOrDefault();

            return goodsReceiptIndexes;
        }

    }
}
