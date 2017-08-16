using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IFillingLineRepository
    {

    }

    public interface IFillingLineAPIRepository : IGenericAPIRepository
    {
        IList<FillingLineBase> GetFillingLineBases();
    }
}
