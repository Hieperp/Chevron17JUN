using System.Linq;
using System.Collections.Generic;

using TotalBase;
using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class FillingPalletRepository : GenericRepository<FillingPallet>, IFillingPalletRepository
    {
        public FillingPalletRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities)
        {
        }


        public IList<FillingPallet> GetFillingPallets(GlobalVariables.FillingLine fillingLineID, string entryStatusIDs)
        {
            return this.TotalSmartCodingEntities.GetFillingPallets((int)fillingLineID, entryStatusIDs).ToList();
        }

        public void UpdateEntryStatus(string fillingPalletIDs, GlobalVariables.BarcodeStatus barcodeStatus)
        {
            this.TotalSmartCodingEntities.FillingPalletUpdateEntryStatus(fillingPalletIDs, (int)barcodeStatus);
        }
    }
}
