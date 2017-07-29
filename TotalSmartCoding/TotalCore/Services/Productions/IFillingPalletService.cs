using System.Collections.Generic;

using TotalBase;
using TotalModel.Models;
using TotalDTO.Productions;

namespace TotalCore.Services.Productions
{
    public interface IFillingPalletService : IGenericService<FillingPallet, FillingPalletDTO, FillingPalletPrimitiveDTO>
    {
        IList<FillingPallet> GetFillingPallets(GlobalVariables.FillingLine fillingLineID, string entryStatusIDs);
    }
}
