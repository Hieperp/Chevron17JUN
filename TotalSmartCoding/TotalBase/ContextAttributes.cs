using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalBase
{
    public static class ContextAttributes
    {
        public static UserInformation User;

        public static string AspUserID = "dade9d9f-2a76-4d5d-9322-f4d47a12e50a";

        public static DateTime FromDate { get { return DateTime.Today.AddDays(-60); } }
        public static DateTime ToDate { get {return DateTime.Today.AddDays(2).AddHours(23).AddMinutes(59).AddSeconds(59);}}
    }
}
