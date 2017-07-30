using System.Collections.Generic;

using TotalBase;
using TotalModel.Models;

using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{
    public interface IFillingPackService : IGenericService<FillingPack, FillingPackDTO, FillingPackPrimitiveDTO>
    {
        IList<FillingPack> GetFillingPacks(GlobalVariables.FillingLine fillingLineID, string entryStatusIDs, int? fillingCartonID);

        bool UpdateEntryStatus(string fillingPackIDs, GlobalVariables.BarcodeStatus barcodeStatus);

        bool UpdateQueueID(string fillingPackIDs, int queueID);
    }
}
