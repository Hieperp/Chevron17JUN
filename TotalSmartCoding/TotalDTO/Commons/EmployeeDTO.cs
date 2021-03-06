﻿using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;

namespace TotalDTO.Commons
{
    public interface IEmployeeBaseDTO
    {
        int EmployeeID { get; set; }
        [Display(Name = "Tên nhân viên")]
        [Required(ErrorMessage = "Vui lòng nhập tên nhân viên")]
        string Name { get; set; }        
    }
    public class EmployeeBaseDTO : BaseDTO, IEmployeeBaseDTO
    {
        public int EmployeeID { get; set; }
        
        [Display(Name = "Tên nhân viên")]        
        public string Name { get ; set; }
    }

    public class EmployeePrimitiveDTO : EmployeeBaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.Employee; } }

        public override int GetID() { return this.EmployeeID; }
        public void SetID(int id) { this.EmployeeID = id; }

        [Display(Name = "Mã nhân viên")]
        [Required(ErrorMessage = "Vui lòng nhập mã nhân viên")]
        public string Code { get; set; }
        [Display(Name = "Chức vụ")]
        [Required(ErrorMessage = "Vui lòng nhập chức vụ")]
        public string Title { get; set; }
        public string Birthday { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }

        public override int PreparedPersonID { get { return 1; } }
    }


    public class EmployeeDTO : EmployeePrimitiveDTO
    {
    }

}
