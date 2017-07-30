using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class FillingPack
    {
        private readonly TotalSmartCodingEntities totalSmartCodingEntities;

        public FillingPack(TotalSmartCodingEntities totalSmartCodingEntities)
        {
            this.totalSmartCodingEntities = totalSmartCodingEntities;
        }

        public void RestoreProcedure()
        {
            this.FillingPackEditable();


            this.GetFillingPacks();

            this.FillingPackUpdateQueueID();
            this.FillingPackUpdateEntryStatus();
        }

        private void FillingPackEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = FillingPackID FROM FillingPacks WHERE FillingPackID = @EntityID AND NOT FillingCartonID IS NULL";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("FillingPackEditable", queryArray);
        }





        private void GetFillingPacks()
        {
            string sqlSelect = "       SELECT * FROM FillingPacks WHERE FillingLineID = @FillingLineID AND EntryStatusID IN (SELECT Id FROM dbo.SplitToIntList (@EntryStatusIDs)) " + "\r\n";

            string queryString = " @FillingLineID int, @EntryStatusIDs varchar(3999), @FillingCartonID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   IF (@FillingCartonID IS NULL) " + "\r\n";
            queryString = queryString + "       " + sqlSelect + "\r\n";
            queryString = queryString + "   ELSE " + "\r\n";
            queryString = queryString + "       " + sqlSelect + " AND FillingCartonID = @FillingCartonID " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetFillingPacks", queryString);
        }

        private void FillingPackUpdateQueueID()
        {
            string queryString = " @FillingPackIDs varchar(3999), @QueueID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      FillingPacks" + "\r\n";
            queryString = queryString + "       SET         QueueID = @QueueID " + "\r\n";
            queryString = queryString + "       WHERE       FillingPackID IN (SELECT Id FROM dbo.SplitToIntList (@FillingPackIDs)) " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT <> ((SELECT (LEN(@FillingPackIDs) - LEN(REPLACE(@FillingPackIDs, ',', '')))) + 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'System Error: Some pack does not exist!' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("FillingPackUpdateQueueID", queryString);
        }

        private void FillingPackUpdateEntryStatus()
        {
            string queryString = " @FillingPackIDs varchar(3999), @EntryStatusID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      FillingPacks" + "\r\n";
            queryString = queryString + "       SET         EntryStatusID = @EntryStatusID " + "\r\n";
            queryString = queryString + "       WHERE       FillingPackID IN (SELECT Id FROM dbo.SplitToIntList (@FillingPackIDs)) " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT <> ((SELECT (LEN(@FillingPackIDs) - LEN(REPLACE(@FillingPackIDs, ',', '')))) + 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'System Error: Some pack does not exist!' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("FillingPackUpdateEntryStatus", queryString);
        }
    }
}
