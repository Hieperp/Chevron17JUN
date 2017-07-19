using TotalBase;
using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IFillingPalletRepository : IGenericRepository<FillingPallet>
    {
        void UpdateEntryStatus(string fillingCartonIDs, GlobalVariables.BarcodeStatus barcodeStatus);
    }
}
