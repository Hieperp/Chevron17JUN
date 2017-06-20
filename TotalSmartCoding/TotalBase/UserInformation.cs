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

        public UserInformation() : this(-1, -1) { }

        public UserInformation(int userID, int userOrganizationID)
        {
            this.UserID = userID;
            this.UserOrganizationID = userOrganizationID;
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

    }
}
