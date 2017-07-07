using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalBase
{
    public class UserInformation
    {
        private int userID;
        private int userOrganizationID;
        private string userDescription;

        public DateTime UserDate { get; set; }

        public UserInformation() : this(-1, -1, "", DateTime.MinValue) { }

        public UserInformation(int userID, int userOrganizationID, string userDescription, DateTime userDate)
        {
            this.UserID = userID;
            this.UserOrganizationID = userOrganizationID;
            this.UserDescription = userDescription;
            this.UserDate = userDate;
        }

        public int UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }

        public int UserOrganizationID
        {
            get { return this.userOrganizationID; }
            set { this.userOrganizationID = value; }
        }

        public string UserDescription
        {
            get { return this.userDescription; }
            set { this.userDescription = value; }
        }
    }
}
