using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;
using TotalDTO.Helpers;
using TotalDTO.Commons;

namespace TotalDTO.Inventories
{
    public class PickupDTO
    {
    }


    public interface IPickupBoxDTO //This DTO is used to display related Pickup data only
    {
        Nullable<int> PickupID { get; set; }
        [Display(Name = "Số phiếu ĐNGH")]
        string Reference { get; set; }
        [Display(Name = "Ngày ĐNGH")]
        DateTime? EntryDate { get; set; }
    }

    public class PickupBoxDTO : IPickupBoxDTO
    {
        public Nullable<int> PickupID { get; set; }
        public string Reference { get; set; }
        public DateTime? EntryDate { get; set; }
    }

}
