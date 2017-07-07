using System.Collections.Generic;
using System.Linq;

using TotalModel.Models;

namespace TotalSmartCoding.Builders.Commons
{
    public interface IAspNetUserSelectListBuilder
    {
        //IEnumerable<SelectListItem> BuildSelectListItemsForAspNetUsers(IEnumerable<AspNetUser> aspNetUsers, int userID);
    }

    public class AspNetUserSelectListBuilder : IAspNetUserSelectListBuilder
    {
        //public IEnumerable<SelectListItem> BuildSelectListItemsForAspNetUsers(IEnumerable<AspNetUser> aspNetUsers, int userID)
        //{
        //    return aspNetUsers.OrderBy(od => od.UserID == userID ? 1 : 2).Select(pt => new SelectListItem { Text = pt.UserName, Value = pt.UserID.ToString() }).ToList(); //Text = pt.LastName + ' ' + pt.FirstName
        //}
    }
}
