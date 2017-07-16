using TotalBase;
using TotalModel.Models;

using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{
    public interface IFillingPackService : IGenericService<FillingPack, FillingPackDTO, FillingPackPrimitiveDTO>
    {
        bool UpdateEntryStatus(string fillingPackIDs, GlobalVariables.BarcodeStatus barcodeStatus);

        bool UpdateListOfPackSubQueueID(string fillingPackIDs, int QueueID);
    }
}
