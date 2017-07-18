using TotalBase;
using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IFillingCartonRepository : IGenericRepository<FillingCarton>
    {
        void UpdateEntryStatus(string fillingCartonIDs, GlobalVariables.BarcodeStatus barcodeStatus);
    }
}
