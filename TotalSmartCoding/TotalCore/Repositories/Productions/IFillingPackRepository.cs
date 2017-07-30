using System.Collections.Generic;

using TotalBase;
using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IFillingPackRepository : IGenericRepository<FillingPack>
    {
        IList<FillingPack> GetFillingPacks(GlobalVariables.FillingLine fillingLineID, string entryStatusIDs, int? fillingCartonID);

        void UpdateQueueID(string fillingPackIDs, int queueID);
        void UpdateEntryStatus(string fillingPackIDs, GlobalVariables.BarcodeStatus barcodeStatus);        
    }
}
