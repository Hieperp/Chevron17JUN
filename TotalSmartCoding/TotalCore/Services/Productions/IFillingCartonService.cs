using System.Collections.Generic;

using TotalBase;
using TotalModel.Models;

using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{
    public interface IFillingCartonService : IGenericService<FillingCarton, FillingCartonDTO, FillingCartonPrimitiveDTO>
    {
        IList<FillingCarton> GetFillingCartons(GlobalVariables.FillingLine fillingLineID, string entryStatusIDs, int? fillingPalletID);

        bool UpdateEntryStatus(string fillingCartonIDs, GlobalVariables.BarcodeStatus barcodeStatus);
    }
}
