using System.Linq;
using System.Collections.Generic;

using TotalBase;
using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class FillingPackRepository : GenericRepository<FillingPack>, IFillingPackRepository
    {
        public FillingPackRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities)
        {
        }

        public IList<FillingPack> GetFillingPacks(GlobalVariables.FillingLine fillingLineID, string entryStatusIDs, int? fillingCartonID)
        {
            return this.TotalSmartCodingEntities.GetFillingPacks((int)fillingLineID, entryStatusIDs, fillingCartonID).ToList();
        }

        public void UpdateQueueID(string fillingPackIDs, int queueID)
        {
            this.TotalSmartCodingEntities.FillingPackUpdateQueueID(fillingPackIDs, queueID);
        }

        public void UpdateEntryStatus(string fillingPackIDs, GlobalVariables.BarcodeStatus barcodeStatus)
        {
            this.TotalSmartCodingEntities.FillingPackUpdateEntryStatus(fillingPackIDs, (int) barcodeStatus);
        }
    }
}
