using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;


using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Inventories;

using TotalCore.Repositories.Commons;
using TotalBase;

namespace TotalSmartCoding.Controllers.APIs.Commons
{
    public class EmployeeAPIs
    {
        private readonly IEmployeeAPIRepository employeeAPIRepository;

        public EmployeeAPIs(IEmployeeAPIRepository employeeAPIRepository)
        {
            this.employeeAPIRepository = employeeAPIRepository;
        }


        public ICollection<EmployeeIndex> GetEmployeeIndexes()
        {
            return this.employeeAPIRepository.GetEntityIndexes<EmployeeIndex>(ContextAttributes.AspUserID, ContextAttributes.FromDate, ContextAttributes.ToDate).ToList();
        }

        public IList<EmployeeBase> GetEmployeeBases()
        {
            return this.employeeAPIRepository.GetEmployeeBases();
        }

    }
}
