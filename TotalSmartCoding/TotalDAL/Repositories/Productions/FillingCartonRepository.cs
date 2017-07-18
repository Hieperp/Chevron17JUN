using TotalBase;
using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class FillingCartonRepository : GenericRepository<FillingCarton>, IFillingCartonRepository
    {
        public FillingCartonRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities)
        {
        }


        public void UpdateEntryStatus(string fillingCartonIDs, GlobalVariables.BarcodeStatus barcodeStatus)
        {
            this.TotalSmartCodingEntities.FillingCartonUpdateEntryStatus(fillingCartonIDs, (int)barcodeStatus);
        }
    }
}
