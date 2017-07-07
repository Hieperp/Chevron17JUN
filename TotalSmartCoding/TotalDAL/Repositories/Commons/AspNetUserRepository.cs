using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class AspNetUserRepository : IAspNetUserRepository
    {
        private readonly TotalSmartCodingEntities totalSmartCodingEntities;

        public AspNetUserRepository(TotalSmartCodingEntities totalSmartCodingEntities)
        {
            this.totalSmartCodingEntities = totalSmartCodingEntities;
        }

        public IList<AspNetUser> GetAllAspNetUsers()
        {
            return this.totalSmartCodingEntities.AspNetUsers.ToList();
        }
    }
}
