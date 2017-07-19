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


        public void UpdateEntryStatus(string fillingPalletIDs, GlobalVariables.BarcodeStatus barcodeStatus)
        {
            this.TotalSmartCodingEntities.FillingPalletUpdateEntryStatus(fillingPalletIDs, (int)barcodeStatus);
        }
    }
}
