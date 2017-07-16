using TotalBase;
using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IFillingPackRepository : IGenericRepository<FillingPack>
    {
        void UpdateEntryStatus(string fillingPackIDs, GlobalVariables.BarcodeStatus barcodeStatus);
    }
}
