using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IEmployeeRepository
    {

    }

    public interface IEmployeeAPIRepository : IGenericAPIRepository
    {
        IList<EmployeeBase> GetEmployeeBases();
    }
}
