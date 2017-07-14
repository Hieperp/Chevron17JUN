using TotalBase;
using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IOnlinePackRepository : IGenericRepository<OnlinePack>
    {
        void UpdateEntryStatus(string onlinePackIDs, GlobalVariables.BarcodeStatus barcodeStatus);
    }
}
