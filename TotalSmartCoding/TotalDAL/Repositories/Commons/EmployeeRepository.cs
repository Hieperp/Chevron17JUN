using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;


namespace TotalDAL.Repositories.Commons
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "EmployeeEditable")
        {
        }
    }





    public class EmployeeAPIRepository : GenericAPIRepository, IEmployeeAPIRepository
    {
        public EmployeeAPIRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "GetEmployeeIndexes")
        {
        }

        public IList<EmployeeBase> GetEmployeeBases()
        {
            return this.TotalSmartCodingEntities.GetEmployeeBases().ToList();
        }
    }
}
