using System.Linq;
using System.Collections.Generic;

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


        public IList<FillingCarton> GetFillingCartons(GlobalVariables.FillingLine fillingLineID, string entryStatusIDs, int? fillingPalletID)
        {
            return this.TotalSmartCodingEntities.GetFillingCartons((int)fillingLineID, entryStatusIDs, fillingPalletID).ToList();
        }

        public void UpdateEntryStatus(string fillingCartonIDs, GlobalVariables.BarcodeStatus barcodeStatus)
        {
            this.TotalSmartCodingEntities.FillingCartonUpdateEntryStatus(fillingCartonIDs, (int)barcodeStatus);
        }
    }
}
