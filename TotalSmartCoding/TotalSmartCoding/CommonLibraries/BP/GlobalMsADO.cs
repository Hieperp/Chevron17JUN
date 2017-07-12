using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace TotalSmartCoding.CommonLibraries.BP
{
    public class GlobalMsADO
    {
        public static string ServerName = "(LOCAL)";
        public static string DatabaseName = "Northwind";


        public static string ConnectionString()
        { return GlobalMsADO.ConnectionString(GlobalMsADO.ServerName, GlobalMsADO.DatabaseName); }
        //{ return "Server = " + ServerName + "; Database= " + DatabaseName + "; Trusted_Connection= True"; } 

        public static string ConnectionString(string serverName, string databaseName)
        {
            ////////if (serverName.ToUpper() == "SERVER\\SQLEXPRESS" || serverName.ToUpper() == "DELL-E7240T\\SQLEXPRESS")
            ////////    return "Server = " + serverName + "; Database= " + databaseName + "; Trusted_Connection= True";
            ////////else
            ////////    return "Server = " + serverName + "; Database= " + databaseName + "; User Id=LMHIEP;Password=nomore2013";

            //{ return "Server = " + serverName + "; Database= " + databaseName + "; User Id=LMHIEP;Password=nomore2013"; }
            { return "Server = " + ServerName + "; Database= " + DatabaseName + "; Trusted_Connection= True"; }
        }




        private static SqlConnection mainDataAccessConnection;

        public static SqlConnection MainDataAccessConnection(bool openConnection)
        {
            try
            {
                if (mainDataAccessConnection == null || mainDataAccessConnection.State != ConnectionState.Open)
                {
                    mainDataAccessConnection = new SqlConnection(GlobalMsADO.ConnectionString());
                    mainDataAccessConnection.Open();
                }

                return mainDataAccessConnection;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static SqlConnection MainDataAccessConnection()
        {
            return mainDataAccessConnection;
        }
    }


}
