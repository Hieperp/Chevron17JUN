using TotalBase;
using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class OnlinePackRepository : GenericRepository<OnlinePack>, IOnlinePackRepository
    {
        public OnlinePackRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities)
        {
        }
        public void UpdateEntryStatus(string onlinePackIDs, GlobalVariables.BarcodeStatus barcodeStatus)
        {
            this.TotalSmartCodingEntities.OnlinePackUpdateEntryStatus(onlinePackIDs, (int) barcodeStatus);
        }
    }
}
