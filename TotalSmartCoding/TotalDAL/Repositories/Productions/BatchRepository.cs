using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Productions;
using System.Data.Entity.Core.Objects;
using TotalBase;

namespace TotalDAL.Repositories.Productions
{
    public class BatchRepository : GenericRepository<Batch>, IBatchRepository
    {
        public BatchRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "BatchEditable")
        {
        }

        public void CommonUpdate(int batchID, string nextPackNo, string nextCartonNo, string nextPalletNo)
        {
            this.TotalSmartCodingEntities.BatchCommonUpdate(batchID, nextPackNo, nextCartonNo, nextPalletNo);
        }
    }








    public class BatchAPIRepository : GenericAPIRepository, IBatchAPIRepository
    {
        public BatchAPIRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "GetBatchIndexes")
        {
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {
            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);

            return new ObjectParameter[] { baseParameters[0], baseParameters[1], baseParameters[2], new ObjectParameter("FillingLineID", (int)GlobalVariables.FillingLineID), new ObjectParameter("ActiveOption", (int)(this.RepositoryBag["ActiveOption"] != null ? this.RepositoryBag["ActiveOption"] : GlobalEnums.ActiveOption.Both)) };
        }
    }


}
