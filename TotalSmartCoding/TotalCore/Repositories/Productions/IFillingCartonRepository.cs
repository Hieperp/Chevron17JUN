using System.Collections.Generic;

using TotalBase;
using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IFillingCartonRepository : IGenericRepository<FillingCarton>
    {
        IList<FillingCarton> GetFillingCartons(GlobalVariables.FillingLine fillingLineID, string entryStatusIDs, int? fillingPalletID);

        void UpdateEntryStatus(string fillingCartonIDs, GlobalVariables.BarcodeStatus barcodeStatus);
    }
}
