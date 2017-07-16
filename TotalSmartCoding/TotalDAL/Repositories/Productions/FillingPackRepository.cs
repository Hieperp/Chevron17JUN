using TotalBase;
using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class FillingPackRepository : GenericRepository<FillingPack>, IFillingPackRepository
    {
        public FillingPackRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities)
        {
        }
        public void UpdateEntryStatus(string fillingPackIDs, GlobalVariables.BarcodeStatus barcodeStatus)
        {
            this.TotalSmartCodingEntities.FillingPackUpdateEntryStatus(fillingPackIDs, (int) barcodeStatus);
        }
    }
}
