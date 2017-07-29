using System.Collections.Generic;

using TotalBase;
using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IFillingPalletRepository : IGenericRepository<FillingPallet>
    {
        IList<FillingPallet> GetFillingPallets(GlobalVariables.FillingLine fillingLineID, string entryStatusIDs);

        void UpdateEntryStatus(string fillingCartonIDs, GlobalVariables.BarcodeStatus barcodeStatus);
    }
}
