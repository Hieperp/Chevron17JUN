using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using Global.Class.Library;

namespace TotalSmartCoding.CommonLibraries.BP
{
    public class SQLDatabase
    {

        public enum SqlType : int
        {
            StoredProcedure = 1,
            Text = 2
        }

        //public static string connectionString = "Server= .;Database= Northwind; Trusted_Connection= True";

        public static SqlConnection Connection()
        {
            return GlobalMsADO.MainDataAccessConnection();
            ////////SqlConnection connection = new SqlConnection(connectionString);
            ////////return connection;
        }





        public static int InsertData(string sql, IDataParameter[] parameter)
        {
            SqlCommand command = new SqlCommand();
            int id = -1;
            command = AddParameter(parameter, sql);
            command.ExecuteNonQuery();
            return id;
        }



        public static int UpdateData(string sql, IDataParameter[] parameter)
        {
            SqlCommand command = new SqlCommand();
            command = AddParameter(parameter, sql);
            return command.ExecuteNonQuery();
        }

        public static SqlCommand AddParameter(IDataParameter[] objectParameter, string storeName)
        {
            SqlConnection conn = Connection();
            SqlCommand command = new SqlCommand();
            if (conn.State == 0) { conn.Open(); }
            command.Connection = conn;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storeName;
            //đưa tham số vào
            foreach (SqlParameter item in objectParameter)
            {
                command.Parameters.Add(item);
            }
            return command;
        }





        /// <summary>
        /// Create a new store procedure
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="queryString"></param>
        public static void CreateStoredProcedure(string storedProcedureName, string queryString)
        {
            DataTable dataTable = SQLDatabase.GetDataTable("SELECT name FROM sysobjects WHERE name = '" + storedProcedureName + "' AND type = 'P'");
            if (dataTable.Rows.Count > 0) SQLDatabase.ExecuteNonQuery("DROP PROCEDURE " + storedProcedureName);

            SQLDatabase.ExecuteNonQuery("CREATE PROC " + storedProcedureName + "\r\n" + queryString);
        }


        /// <summary>
        /// Create new stored procedure to check a pecific existing, for example: Editable, Revisable, ...
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterString"></param>
        /// <param name="queryArray"></param>
        public static void CreateProcedureToCheckExisting(string storedProcedureName, string[] queryArray)
        {
            string queryString = "";

            queryString = " @FindIdentityID int " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @ExistIdentityID Int " + "\r\n";
            queryString = queryString + "       SET @ExistIdentityID = -1 " + "\r\n";

            if (queryArray != null)
            {
                foreach (string subQuery in queryArray)
                {
                    queryString = queryString + "       DECLARE CursorLocal CURSOR LOCAL FOR " + subQuery + "\r\n";
                    queryString = queryString + "       OPEN CursorLocal " + "\r\n";
                    queryString = queryString + "       FETCH NEXT FROM CursorLocal INTO @ExistIdentityID " + "\r\n";
                    queryString = queryString + "       CLOSE CursorLocal " + "\r\n";
                    queryString = queryString + "       DEALLOCATE CursorLocal " + "\r\n";
                    queryString = queryString + "       IF @ExistIdentityID != -1  RETURN @ExistIdentityID " + "\r\n";
                }
            }

            queryString = queryString + "       RETURN @ExistIdentityID " + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            SQLDatabase.CreateStoredProcedure(storedProcedureName, queryString);
        }

        public static void CreateProcedureToCheckExisting(string storedProcedureName)
        {
            SQLDatabase.CreateProcedureToCheckExisting(storedProcedureName, null);
        }





        public static int GetScalarValue(string queryString, CommandType commandType, IDataParameter[] dataParameter)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GlobalMsADO.ConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection);
                sqlCommand.CommandType = commandType;
                if (dataParameter != null) sqlCommand.Parameters.AddRange(dataParameter);

                sqlConnection.Open();


                int returnValue;
                object returnObject = sqlCommand.ExecuteScalar();
                if (returnObject != DBNull.Value && returnObject != null && int.TryParse(returnObject.ToString(), out returnValue))
                    return (int)returnValue;
                else return -1;
            }
        }

        public static int GetScalarValue(string queryString, CommandType commandType)
        {
            return SQLDatabase.GetScalarValue(queryString, commandType, null);
        }

        public static int GetScalarValue(string queryString)
        {
            return SQLDatabase.GetScalarValue(queryString, CommandType.Text, null);
        }





        public static int GetReturnValue(string queryString, CommandType commandType, IDataParameter[] dataParameter)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GlobalMsADO.ConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection);
                sqlCommand.CommandType = commandType;
                if (dataParameter != null) sqlCommand.Parameters.AddRange(dataParameter);


                sqlConnection.Open();


                SqlParameter returnParameter = sqlCommand.Parameters.Add("@ReturnValue", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                sqlCommand.ExecuteNonQuery();
                return (int)returnParameter.Value;
            }
        }

        public static int GetReturnValue(string queryString, CommandType commandType)
        {
            return SQLDatabase.GetReturnValue(queryString, commandType, null);
        }

        public static int GetReturnValue(string queryString)
        {
            return SQLDatabase.GetReturnValue(queryString, CommandType.Text, null);
        }

        #region Get Datatable
        /// <summary>
        /// GetDataTable
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string queryString, CommandType commandType, IDataParameter[] dataParameter)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GlobalMsADO.ConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection);
                sqlCommand.CommandType = commandType;
                if (dataParameter != null) sqlCommand.Parameters.AddRange(dataParameter);

                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    sqlDataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        public static DataTable GetDataTable(string queryString, CommandType commandType)
        {
            return SQLDatabase.GetDataTable(queryString, commandType, null);
        }

        public static DataTable GetDataTable(string queryString)
        {
            return SQLDatabase.GetDataTable(queryString, CommandType.Text, null);
        }

        #endregion Get Datatable




        //BELOW IS OK STATEMENT ----------------------------------------------

        public static int ExecuteNonQuery(string commandText)
        {
            return SQLDatabase.ExecuteNonQuery(commandText, CommandType.Text);
        }

        public static int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            return SQLDatabase.ExecuteNonQuery(commandText, commandType, null);
        }

        public static int ExecuteNonQuery(string commandText, CommandType commandType, IDataParameter[] dataParameter)
        {
            return SQLDatabase.ExecuteNonQuery(commandText, commandType, dataParameter, -1);
        }

        public static int ExecuteNonQuery(string commandText, CommandType commandType, IDataParameter[] dataParameter, int commandTimeout)
        {
            Console.WriteLine(commandText);
            using (SqlConnection sqlConnection = new SqlConnection(GlobalMsADO.ConnectionString()))
            {
                try
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection);
                    sqlCommand.CommandType = commandType;

                    if (dataParameter != null) sqlCommand.Parameters.AddRange(dataParameter);

                    if (commandTimeout > 0) sqlCommand.CommandTimeout = commandTimeout;

                    return sqlCommand.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }


        public static int ExecuteTransaction(string commandText)
        {
            return SQLDatabase.ExecuteTransaction(commandText, CommandType.Text);
        }

        public static int ExecuteTransaction(string commandText, CommandType commandType)
        {
            using (SqlConnection sqlConnection = new SqlConnection(GlobalMsADO.ConnectionString()))
            {
                int rowsAffected = -1;
                SqlTransaction sqlTransaction = null;
                try
                {
                    sqlConnection.Open();
                    sqlTransaction = sqlConnection.BeginTransaction("MyTransaction");


                    //Begin add command here
                    using (SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection, sqlTransaction))
                    {
                        sqlCommand.CommandType = commandType;
                        rowsAffected = sqlCommand.ExecuteNonQuery();
                    }
                    //End add command here


                    sqlTransaction.Commit();

                    return rowsAffected;

                }
                catch (Exception exception)
                {
                    try
                    {
                        sqlTransaction.Rollback();
                        throw exception;
                    }
                    catch (Exception rollbackException)
                    {
                        throw rollbackException;
                    }
                }
            }
        }








    }

    public class SQLRestore
    {



        public void RestoreProcedure()
        {
            string queryString;

            queryString = "     @HasSent bit ";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   SELECT          ServerID, CartonID, CartonDate, CartonStatus, CartonBarcode, FillingLineID, Pack00Barcode, Pack01Barcode, Pack02Barcode, Pack03Barcode, Pack04Barcode, " + "\r\n";
            queryString = queryString + "                   Pack05Barcode, Pack06Barcode, Pack07Barcode, Pack08Barcode, Pack09Barcode, Pack10Barcode, Pack11Barcode, Pack12Barcode, Pack13Barcode, " + "\r\n";
            queryString = queryString + "                   Pack14Barcode, Pack15Barcode, Pack16Barcode, Pack17Barcode, Pack18Barcode, Pack19Barcode, Pack20Barcode, Pack21Barcode, Pack22Barcode, Pack23Barcode, HasSent " + "\r\n";
            queryString = queryString + "   FROM            DataDetailCartonArchive " + "\r\n";
            queryString = queryString + "   WHERE           HasSent = @HasSent " + "\r\n";
            queryString = queryString + "   ORDER BY        CartonDate, FillingLineID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("DataDetailCartonArchiveGetDataByHasSent", queryString);


            queryString = "     @ServerID int, @CartonID int, @CartonDate DateTime, @FillingLineID Int, @ProductCode nvarchar(10), @LowerFillterDate DateTime, @UpperFillterDate DateTime " + "\r\n";
            ////queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   IF (@ServerID > 0 AND @CartonID > 0) " + "\r\n";
            queryString = queryString + "       " + this.BuildSQL(true, false, false) + "\r\n";
            queryString = queryString + "   ELSE " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           IF (@FillingLineID <> 0) " + "\r\n";
            queryString = queryString + "               " + this.BuildFillingLineSQL(false, true) + "\r\n";
            queryString = queryString + "           ELSE " + "\r\n";
            queryString = queryString + "               " + this.BuildFillingLineSQL(false, false) + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            SQLDatabase.CreateStoredProcedure("DataDetailCartonArchiveListing", queryString);


            queryString = "     @DownLoadID Int, @ServerID Int, @LowerFillterDate DateTime, @UpperFillterDate DateTime " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   SELECT          TOP 50 DownLoadID, DownLoadDate, DownLoadRows, ServerID, Description, Remarks " + "\r\n";
            queryString = queryString + "   FROM            DownLoadLogEvent " + "\r\n";
            queryString = queryString + "   WHERE           DownLoadID = @DownLoadID OR ((ServerID = @ServerID OR @ServerID = 0) AND DownLoadDate >= @LowerFillterDate AND DownLoadDate <= @UpperFillterDate)" + "\r\n";
            queryString = queryString + "   ORDER BY        DownLoadDate DESC " + "\r\n";
            SQLDatabase.CreateStoredProcedure("DownLoadLogEventSelect", queryString);


            queryString = "     @DownLoadDate datetime,	@DownLoadRows int, @ServerID int, @Description nvarchar(1000), @Remarks nvarchar(100) " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   INSERT INTO     DownLoadLogEvent (DownLoadDate, DownLoadRows, ServerID, Description, Remarks) VALUES (@DownLoadDate, @DownLoadRows, @ServerID, @Description, @Remarks)" + "\r\n";
            queryString = queryString + "   SELECT          DownLoadID, DownLoadDate, DownLoadRows, ServerID, Description, Remarks FROM DownLoadLogEvent WHERE (DownLoadID = SCOPE_IDENTITY()) " + "\r\n";
            SQLDatabase.CreateStoredProcedure("DownLoadLogEventInsert", queryString);

            queryString = "     @DownLoadID int, @DownLoadDate datetime, @DownLoadRows int, @ServerID int, @Description nvarchar(200), @Remarks nvarchar(100) " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   UPDATE          DownLoadLogEvent SET DownLoadDate = @DownLoadDate, DownLoadRows = @DownLoadRows, ServerID = @ServerID, Description = @Description, Remarks = @Remarks WHERE DownLoadID = @DownLoadID " + "\r\n";
            queryString = queryString + "   SELECT          DownLoadID, DownLoadDate, DownLoadRows, ServerID, Description, Remarks FROM DownLoadLogEvent WHERE DownLoadID = @DownLoadID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("DownLoadLogEventUpdate", queryString);


            queryString = "     @DownLoadID int " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   DELETE FROM     DownLoadLogEvent WHERE DownLoadID = @DownLoadID " + "\r\n";

            SQLDatabase.CreateStoredProcedure("DownLoadLogEventDelete", queryString);




            queryString = "     @IsSuccessful bit " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   SELECT          FileID, FilePath, FileName, FileDate, FileTimes, FileRows, UpLoadDate, Description, Remarks, IsSuccessful " + "\r\n";
            queryString = queryString + "   FROM            UpLoadLogEvent " + "\r\n";
            queryString = queryString + "   WHERE           IsSuccessful = @IsSuccessful " + "\r\n";
            queryString = queryString + "   ORDER BY        FileDate DESC " + "\r\n";
            SQLDatabase.CreateStoredProcedure("UpLoadLogEventSelectByIsSuccessful", queryString);


            queryString = "     @FileID Int, @LowerFillterDate DateTime, @UpperFillterDate DateTime " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   SELECT          TOP 50 FileID, FilePath, FileName, FileDate, FileTimes, FileRows, UpLoadDate, Description, Remarks, IsSuccessful " + "\r\n";
            queryString = queryString + "   FROM            UpLoadLogEvent " + "\r\n";
            queryString = queryString + "   WHERE           FileID = @FileID OR (FileDate >= @LowerFillterDate AND FileDate <= @UpperFillterDate)" + "\r\n";
            queryString = queryString + "   ORDER BY        FileDate DESC " + "\r\n";
            SQLDatabase.CreateStoredProcedure("UpLoadLogEventSelect", queryString);


            queryString = "     @FilePath nvarchar(150), @FileName nvarchar(150), @FileDate datetime, @FileTimes int, @FileRows int, @UpLoadDate datetime, @Description nvarchar(1000), @Remarks nvarchar(100),	@IsSuccessful bit " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   INSERT INTO     UpLoadLogEvent (FilePath, FileName, FileDate, FileTimes, FileRows, UpLoadDate, Description, Remarks, IsSuccessful) VALUES (@FilePath, @FileName, @FileDate, @FileTimes, @FileRows, @UpLoadDate, @Description, @Remarks, @IsSuccessful)";
            queryString = queryString + "   SELECT          FileID, FilePath, FileName, FileDate, FileTimes, FileRows, UpLoadDate, Description, Remarks, IsSuccessful FROM UpLoadLogEvent WHERE  (FileID = SCOPE_IDENTITY()) ";
            SQLDatabase.CreateStoredProcedure("UpLoadLogEventInsert", queryString);


            queryString = "     @FileID int, @FilePath nvarchar(150), @FileName nvarchar(150), @FileDate datetime, @FileTimes int, @FileRows int, @UpLoadDate datetime, @Description nvarchar(1000),	@Remarks nvarchar(100),	@IsSuccessful bit " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   UPDATE          UpLoadLogEvent SET FilePath = @FilePath, FileName = @FileName, FileDate = @FileDate, FileTimes = @FileTimes, FileRows = @FileRows, UpLoadDate = @UpLoadDate, Description = @Description, Remarks = @Remarks, IsSuccessful = @IsSuccessful WHERE FileID = @FileID " + "\r\n";
            queryString = queryString + "   SELECT          FileID, FilePath, FileName, FileDate, FileTimes, FileRows, UpLoadDate, Description, Remarks, IsSuccessful FROM UpLoadLogEvent WHERE (FileID = @FileID) " + "\r\n";
            SQLDatabase.CreateStoredProcedure("UpLoadLogEventUpDate", queryString);


            queryString = "     @FileID int " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   DELETE FROM     UpLoadLogEvent WHERE FileID = @FileID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("UpLoadLogEventDelete", queryString);


            queryString = "     @FileDate DateTime " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   SELECT          COUNT(FileName) AS CountTextFile FROM UpLoadLogEvent WHERE YEAR(FileDate) = YEAR(@FileDate) AND MONTH(FileDate) = MONTH(@FileDate) AND DAY(FileDate) = DAY(@FileDate)  " + "\r\n";
            SQLDatabase.CreateStoredProcedure("CountTextFileByDate", queryString);






            queryString = "     @BackupID Int " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   SELECT          BackupID, BackupPath, BackupName, BackupDate, BeginningCartonDate, EndingCartonDate, RestoreName, Description, Remarks " + "\r\n";
            queryString = queryString + "   FROM            BackupLogEvent " + "\r\n";
            queryString = queryString + "   WHERE           BackupID = @BackupID OR @BackupID = 0 " + "\r\n";
            queryString = queryString + "   ORDER BY        BackupDate DESC " + "\r\n";
            SQLDatabase.CreateStoredProcedure("BackupLogEventSelect", queryString);


            queryString = "     @BackupPath nvarchar(150), @BackupName nvarchar(150), @BackupDate datetime, @BeginningCartonDate datetime, @EndingCartonDate datetime, @RestoreName nvarchar(150), @Description nvarchar(800), @Remarks nvarchar(100) " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   INSERT INTO     BackupLogEvent (BackupPath, BackupName, BackupDate, BeginningCartonDate, EndingCartonDate, RestoreName, Description, Remarks) VALUES (@BackupPath, @BackupName, @BackupDate, @BeginningCartonDate, @EndingCartonDate, @RestoreName, @Description, @Remarks)";
            queryString = queryString + "   SELECT          BackupID, BackupPath, BackupName, BackupDate, BeginningCartonDate, EndingCartonDate, RestoreName, Description, Remarks FROM BackupLogEvent WHERE (BackupID = SCOPE_IDENTITY()) ";
            SQLDatabase.CreateStoredProcedure("BackupLogEventInsert", queryString);


            queryString = "     @BackupID int, @BackupPath nvarchar(150), @BackupName nvarchar(150), @BackupDate datetime, @BeginningCartonDate datetime, @EndingCartonDate datetime, @RestoreName nvarchar(150), @Description nvarchar(800), @Remarks nvarchar(100) " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   UPDATE          BackupLogEvent SET BackupPath = @BackupPath, BackupName = @BackupName, BackupDate = @BackupDate, BeginningCartonDate = @BeginningCartonDate, EndingCartonDate = @EndingCartonDate, RestoreName = @RestoreName, Description = @Description, Remarks = @Remarks WHERE BackupID = @BackupID " + "\r\n";
            queryString = queryString + "   SELECT          BackupID, BackupPath, BackupName, BackupDate, BeginningCartonDate, EndingCartonDate, RestoreName, Description, Remarks FROM BackupLogEvent WHERE BackupID = @BackupID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("BackupLogEventUpdate", queryString);


            queryString = "     @BackupID int " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   DELETE FROM     BackupLogEvent WHERE BackupID = @BackupID " + "\r\n";
            SQLDatabase.CreateStoredProcedure("BackupLogEventDelete", queryString);

        }

        private string BuildFillingLineSQL(bool byCartonID, bool byFillingLineID)
        {
            string queryString = "  " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@ProductCode <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQL(byCartonID, byFillingLineID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQL(byCartonID, byFillingLineID, false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQL(bool byCartonID, bool byFillingLineID, bool byProductCode)
        {
            string queryString = "  " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SELECT          ListFillingLineName.FillingLineID, ListFillingLineName.FillingLineName, DataDetailCartonArchive.ServerID, DataDetailCartonArchive.CartonID, DataDetailCartonArchive.CartonDate, DataDetailCartonArchive.CartonStatus, DataDetailCartonArchive.CartonBarcode + '#' AS CartonBarcode,  " + "\r\n";
            queryString = queryString + "                       DataDetailCartonArchive.Pack00Barcode, DataDetailCartonArchive.Pack01Barcode, DataDetailCartonArchive.Pack02Barcode, DataDetailCartonArchive.Pack03Barcode,  " + "\r\n";
            queryString = queryString + "                       DataDetailCartonArchive.Pack04Barcode, DataDetailCartonArchive.Pack05Barcode, DataDetailCartonArchive.Pack06Barcode, DataDetailCartonArchive.Pack07Barcode,  " + "\r\n";
            queryString = queryString + "                       DataDetailCartonArchive.Pack08Barcode, DataDetailCartonArchive.Pack09Barcode, DataDetailCartonArchive.Pack10Barcode, DataDetailCartonArchive.Pack11Barcode,  " + "\r\n";
            queryString = queryString + "                       DataDetailCartonArchive.Pack12Barcode, DataDetailCartonArchive.Pack13Barcode, DataDetailCartonArchive.Pack14Barcode, DataDetailCartonArchive.Pack15Barcode,  " + "\r\n";
            queryString = queryString + "                       DataDetailCartonArchive.Pack16Barcode, DataDetailCartonArchive.Pack17Barcode, DataDetailCartonArchive.Pack18Barcode, DataDetailCartonArchive.Pack19Barcode,  " + "\r\n";
            queryString = queryString + "                       DataDetailCartonArchive.Pack20Barcode, DataDetailCartonArchive.Pack21Barcode, DataDetailCartonArchive.Pack22Barcode, DataDetailCartonArchive.Pack23Barcode, DataDetailCartonArchive.HasSent " + "\r\n";
            queryString = queryString + "       FROM            DataDetailCartonArchive INNER JOIN " + "\r\n";
            queryString = queryString + "                       ListFillingLineName ON DataDetailCartonArchive.FillingLineID = ListFillingLineName.FillingLineID " + "\r\n";
            if (byCartonID)
                queryString = queryString + "   WHERE           DataDetailCartonArchive.ServerID = @ServerID AND (DataDetailCartonArchive.CartonID = @CartonID OR (YEAR(CartonDate) = YEAR(@CartonDate) AND MONTH(CartonDate) = MONTH(@CartonDate) AND DAY(CartonDate) = DAY(@CartonDate))) " + "\r\n";
            else
                queryString = queryString + "   WHERE           CartonDate >= @LowerFillterDate AND CartonDate <= @UpperFillterDate " + (byFillingLineID ? " AND DataDetailCartonArchive.FillingLineID = @FillingLineID" : "") + (byProductCode ? " AND LEFT(DataDetailCartonArchive.CartonBarcode, 3) = @ProductCode" : "") + "\r\n";

            queryString = queryString + "       ORDER BY        CartonDate " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }





        public void RestoreBackupDatabase()
        {
            // Copyright © Microsoft Corporation.  All Rights Reserved.
            // This code released under the terms of the
            // Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
            /****** Object:  StoredProcedure [dbo].[sp_BackupDatabases] ******/
            //-- =============================================  
            //-- Author: Microsoft  
            //-- Create date: 2010-02-06
            //-- Description: Backup Databases for SQLExpress 
            //-- Parameter1: databaseName  
            //-- Parameter2: backupType F=full, D=differential, L=log
            //-- Parameter3: backup file location
            //-- ============================================= 


            string queryString;
            queryString = "     @databaseName sysname = null, @backupLocation nvarchar(200), @restoreLocation nvarchar(200), @backupID int, @beginningCartonDate datetime, @endingCartonDate datetime " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       BEGIN TRY " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";

            queryString = queryString + "               DECLARE @BackupDate datetime        DECLARE @BackupName varchar(100)        DECLARE @BackupFile varchar(100)        DECLARE @dateTimeText NVARCHAR(20)          DECLARE @dataDetailCartonTable NVARCHAR(60)           DECLARE @restoreDatabaseName NVARCHAR(100)           DECLARE @sqlCommand NVARCHAR(1000)  " + "\r\n";

            queryString = queryString + "               SET @BackupDate = GETDATE() " + "\r\n";

            queryString = queryString + "               SET @dateTimeText = REPLACE(CONVERT(VARCHAR, @BackupDate, 111),'/','') + '_' +  REPLACE(CONVERT(VARCHAR, @BackupDate, 108),':','') " + "\r\n";
            queryString = queryString + "               SET @BackupFile = @backupLocation + '\\' + REPLACE(REPLACE(@databaseName, '[',''),']','') + '_FULL_' + @dateTimeText + '.BAK'" + "\r\n";
            queryString = queryString + "               SET @BackupName = 'DATA_' + @dateTimeText " + "\r\n";


            //BACKUP DATABASE
            queryString = queryString + "               SET @sqlCommand = 'BACKUP DATABASE ' +@databaseName+  ' TO DISK = '''+@BackupFile+ ''' WITH INIT, NAME= ''' +@BackupName+''', NOSKIP, NOFORMAT' " + "\r\n";
            queryString = queryString + "               EXEC(@sqlCommand) " + "\r\n";
            queryString = queryString + "               UPDATE BackupLogEvent SET BackupName = @BackupName, BackupPath = @BackupFile, BackupDate = @BackupDate WHERE BackupID = @backupID " + "\r\n";


            //RESTORE DATABASE
            queryString = queryString + "               IF (@restoreLocation <> '') " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       SET @dataDetailCartonTable = 'DataDetailCartonArchive' " + "\r\n";

            queryString = queryString + "                       WAITFOR DELAY '00:00:01'; " + "\r\n";
            queryString = queryString + "                       SET @restoreDatabaseName = @databaseName + '_' + @dateTimeText " + "\r\n";
            queryString = queryString + "                       SET @sqlCommand = 'RESTORE DATABASE ' + @restoreDatabaseName + ' FROM DISK = ''' + @BackupFile + ''' WITH MOVE ''BPFillingSystem'' TO ''' + @restoreLocation + '\\' + @restoreDatabaseName + '.mdf'', MOVE ''BPFillingSystem_log'' TO ''' + @restoreLocation + '\\' + @restoreDatabaseName + '.ldf''' " + "\r\n";
            queryString = queryString + "                       EXEC (@sqlCommand) " + "\r\n";
            queryString = queryString + "                       UPDATE BackupLogEvent SET RestoreName = @restoreDatabaseName WHERE BackupID = @backupID " + "\r\n";


            queryString = queryString + "                       WAITFOR DELAY '00:00:03'; " + "\r\n";
            queryString = queryString + "                       SET @sqlCommand = 'DBCC SHRINKDATABASE (' +@restoreDatabaseName+ ') WITH NO_INFOMSGS'; " + "\r\n";
            queryString = queryString + "                       EXEC (@sqlCommand) " + "\r\n";


            queryString = queryString + "                   END " + "\r\n";
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                       SET @dataDetailCartonTable = 'DataDetailCarton' " + "\r\n";

            //REMOVE DATA
            queryString = queryString + "               WAITFOR DELAY '00:00:01'; " + "\r\n";

            queryString = queryString + "               SET @sqlCommand = 'DELETE FROM ' + @restoreDatabaseName + '.dbo.' + @dataDetailCartonTable + ' WHERE CartonDate > CONVERT(smalldatetime, ''' + CONVERT(VARCHAR, @endingCartonDate, 131) + ''', 131)' " + "\r\n";
            queryString = queryString + "               EXEC (@sqlCommand) " + "\r\n";

            queryString = queryString + "               SET @sqlCommand = 'DELETE FROM ' + @dataDetailCartonTable + ' WHERE CartonDate <= CONVERT(smalldatetime, ''' + CONVERT(VARCHAR, @endingCartonDate, 131) + ''', 131)' " + "\r\n";
            queryString = queryString + "               EXEC (@sqlCommand) " + "\r\n";

            queryString = queryString + "               UPDATE BackupLogEvent SET BeginningCartonDate = @beginningCartonDate, EndingCartonDate = @endingCartonDate WHERE BackupID = @backupID " + "\r\n";


            //SHRINKDATABASE
            queryString = queryString + "               WAITFOR DELAY '00:00:03'; " + "\r\n";
            queryString = queryString + "               SET @sqlCommand = 'DBCC SHRINKDATABASE (' +@databaseName+ ') WITH NO_INFOMSGS'; " + "\r\n";
            queryString = queryString + "               EXEC (@sqlCommand) " + "\r\n";



            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       END TRY " + "\r\n";


            queryString = queryString + "       BEGIN CATCH " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE BackupLogEvent SET Description = SUBSTRING(ERROR_MESSAGE(), 1, 700), Remarks = SUBSTRING(ERROR_MESSAGE(), 701, 70)  WHERE BackupID = @backupID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       END CATCH; " + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            SQLDatabase.CreateStoredProcedure("BackupNmvnDatabases", queryString);



            queryString = "     @databaseName sysname = null, @backupLocation nvarchar(200), @restoreLocation nvarchar(200) " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE @sqlCommand NVARCHAR(1000) " + "\r\n";
            queryString = queryString + "       SET @sqlCommand = 'RESTORE DATABASE ' + @databaseName + ' FROM DISK = ''' + @backupLocation + ''' WITH MOVE ''BPFillingSystem'' TO ''' + @restoreLocation + '\\' + @databaseName + '.mdf'', MOVE ''BPFillingSystem_log'' TO ''' + @restoreLocation + '\\' + @databaseName + '.ldf''' " + "\r\n";
            queryString = queryString + "       EXEC (@sqlCommand) " + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";



            SQLDatabase.CreateStoredProcedure("RestoreNmvnDatabases", queryString);

            //==OKKK== ADODatabase.ExecuteNonQuery("EXEC BackupNmvnDatabases 'BPFillingSystem' , 'D:\\VC PROJECTS\\BP\DATA\\bak'")
            //==OKKK== EXEC RestoreNmvnDatabases 'BPFillingSystem_ok', 'D:\\VC PROJECTS\\BP\\DATA\\BakBPFillingSystem_FULL_04192017_202140.BAK' , 'D:\\VC PROJECTS\\BP\DATA\\bak1'







            queryString = "     @barcodeData varchar(150)  " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";


            queryString = queryString + "       DECLARE @DBs TABLE (ID int IDENTITY PRIMARY KEY, DBNAME nvarchar(500))" + "\r\n";

            queryString = queryString + "       INSERT INTO @DBs (DBNAME) " + "\r\n";
            queryString = queryString + "       SELECT Name FROM master.sys.databases WHERE (state = 0) AND (Name LIKE 'BPFillingSystem%') ORDER BY (CASE WHEN Name = 'BPFillingSystem' THEN 1 ELSE 2 END), Name DESC " + "\r\n";


            queryString = queryString + "       IF (1=0) BEGIN SET FMTONLY OFF END " + "\r\n";
            queryString = queryString + "       DECLARE @DataDetailCartonArchiveResult TABLE(DatabaseName varchar(100) NOT NULL, ServerID int NOT NULL, CartonID int NOT NULL, CartonDate datetime NOT NULL) " + "\r\n";
            queryString = queryString + "       CREATE TABLE #DataDetailCartonArchiveResult (DatabaseName varchar(100) NOT NULL, ServerID int NOT NULL, CartonID int NOT NULL, CartonDate datetime NOT NULL) " + "\r\n";


            queryString = queryString + "       DECLARE @DatabaseName varchar(100)                 DECLARE @sqlCommand NVARCHAR(4000)                  DECLARE @Loop int " + "\r\n"; //-- Declare variables

            queryString = queryString + "       SET @Loop = (SELECT MIN(ID) FROM @DBs)" + "\r\n";

            queryString = queryString + "       WHILE @Loop IS NOT NULL " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";

            queryString = queryString + "               SET @DatabaseName = (SELECT DBNAME FROM @DBs WHERE ID = @Loop) " + "\r\n";

            queryString = queryString + "               SET @sqlCommand = ' INSERT INTO #DataDetailCartonArchiveResult (DatabaseName, ServerID, CartonID, CartonDate) " + "\r\n";
            queryString = queryString + "                               SELECT TOP 1 ''' + @DatabaseName + ''' AS DatabaseName, ServerID, CartonID, CartonDate  " + "\r\n";
            queryString = queryString + "                               FROM     ' + @DatabaseName + '.dbo.DataDetailCartonArchive " + "\r\n";
            queryString = queryString + "                               WHERE  (CartonBarcode LIKE ''%' + @barcodeData + '%'') OR (Pack00Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack01Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack02Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack03Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack04Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack05Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack06Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack07Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack08Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack09Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack10Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack11Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack12Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack13Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack14Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack15Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack16Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack17Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack18Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack19Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack20Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack21Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack22Barcode LIKE ''%' + @barcodeData + '%'') OR (Pack23Barcode LIKE ''%' + @barcodeData + '%'')' " + "\r\n";

            queryString = queryString + "               EXEC(@sqlCommand) " + "\r\n"; //-- Execute the search SQL command

            queryString = queryString + "               IF (@@ROWCOUNT > 0) BREAK; " + "\r\n";

            queryString = queryString + "               SET @Loop = (SELECT MIN(ID) FROM @DBs WHERE ID > @Loop) " + "\r\n";//-- Goto the next database
            queryString = queryString + "       END " + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "       INSERT INTO @DataDetailCartonArchiveResult(DatabaseName, ServerID, CartonID, CartonDate) SELECT DatabaseName, ServerID, CartonID, CartonDate FROM #DataDetailCartonArchiveResult " + "\r\n";
            queryString = queryString + "       DROP TABLE #DataDetailCartonArchiveResult " + "\r\n";
            queryString = queryString + "       SELECT DatabaseName, ServerID, CartonID, CartonDate FROM @DataDetailCartonArchiveResult " + "\r\n";

            SQLDatabase.CreateStoredProcedure("SearchBarcodeData", queryString);

        }



    }
}

//Check these link for implement transactional save
//http://msdn.microsoft.com/en-us/library/ms172152.aspx
//http://msdn.microsoft.com/en-us/library/system.transactions.transactionscope.aspx
//http://msdn.microsoft.com/en-us/library/ms229973.aspx

//http://msdn.microsoft.com/en-us/library/ms229973.aspx
//http://msdn.microsoft.com/en-us/library/ms233770.aspx




//Later: We should study about this, to improve code, with ADO.NET
//3 layer application using ADO.NET  - Should study. THIS IS A MUST
//http://msdn.microsoft.com/en-us/library/aa581771.aspx
//http://msdn.microsoft.com/en-us/library/bb288041.aspx


//Should this about this (This is official guide line from Microsoft
//http://msdn.microsoft.com/en-us/library/system.data.dataset.aspx

//DataSet Class

//In a typical multiple-tier implementation, the steps for creating and refreshing a DataSet, and in turn, updating the original data are to:
//1.Build and fill each DataTable in a DataSet with data from a data source using a DataAdapter.
//2.Change the data in individual DataTable objects by adding, updating, or deleting DataRow objects.
//3.Invoke the GetChanges method to create a second DataSet that features only the changes to the data.
//4.Call the Update method of the DataAdapter, passing the second DataSet as an argument.
//5.Invoke the Merge method to merge the changes from the second DataSet into the first.
//6.Invoke the AcceptChanges on the DataSet. Alternatively, invoke RejectChanges to cancel the changes.




//Very good code --- Should we foolow this
//http://rajmsdn.wordpress.com/2009/12/09/strongly-typed-dataset-connection-string/
//http://rajmsdn.wordpress.com/2009/12/09/strongly-typed-dataset-connection-string/confighack-c-2/
//http://rajmsdn.files.wordpress.com/2009/12/confighack-c.pdf
