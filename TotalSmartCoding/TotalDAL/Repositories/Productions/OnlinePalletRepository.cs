using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class OnlinePalletRepository : GenericRepository<OnlinePallet>, IOnlinePalletRepository
    {
        public OnlinePalletRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities)
        {
        }
    }
}
