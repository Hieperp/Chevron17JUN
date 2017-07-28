using TotalBase;
using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IFillingPackRepository : IGenericRepository<FillingPack>
    {
        void UpdateQueueID(string fillingPackIDs, int queueID);
        void UpdateEntryStatus(string fillingPackIDs, GlobalVariables.BarcodeStatus barcodeStatus);        
    }
}
