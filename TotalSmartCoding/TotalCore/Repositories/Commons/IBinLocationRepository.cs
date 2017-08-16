using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IBinLocationRepository
    {

    }

    public interface IBinLocationAPIRepository : IGenericAPIRepository
    {
        IList<BinLocationBase> GetBinLocationBases();
    }
}
