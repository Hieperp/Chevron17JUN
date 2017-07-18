using TotalBase;
using TotalModel.Models;

using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{
    public interface IFillingCartonService : IGenericService<FillingCarton, FillingCartonDTO, FillingCartonPrimitiveDTO>
    {
        bool UpdateEntryStatus(string fillingCartonIDs, GlobalVariables.BarcodeStatus barcodeStatus);
    }
}
