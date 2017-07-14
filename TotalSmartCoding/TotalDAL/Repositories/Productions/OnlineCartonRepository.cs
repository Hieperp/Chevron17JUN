using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class OnlineCartonRepository : GenericRepository<OnlineCarton>, IOnlineCartonRepository
    {
        public OnlineCartonRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities)
        {
        }
    }
}
