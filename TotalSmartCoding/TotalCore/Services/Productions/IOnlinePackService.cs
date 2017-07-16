using TotalBase;
using TotalModel.Models;

using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{
    public interface IOnlinePackService : IGenericService<OnlinePack, OnlinePackDTO, OnlinePackPrimitiveDTO>
    {
        bool UpdateEntryStatus(string onlinePackIDs, GlobalVariables.BarcodeStatus barcodeStatus);

        bool UpdateListOfPackSubQueueID(string onlinePackIDs, int QueueID);
    }
}
