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

            this.FillingPackUpdateQueueID();
            this.FillingPackUpdateEntryStatus();
        }

        private void FillingPackEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = FillingPackID FROM FillingPacks WHERE FillingPackID = @EntityID AND NOT FillingCartonID IS NULL";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("FillingPackEditable", queryArray);
        }

        private void FillingPackUpdateQueueID()
        {
            string queryString = " @FillingPackIDs varchar(3999), @QueueID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      FillingPacks" + "\r\n";
            queryString = queryString + "       SET         QueueID = @QueueID " + "\r\n";
            queryString = queryString + "       WHERE       FillingPackID IN (SELECT Id FROM dbo.SplitToIntList (@FillingPackIDs)) " + "\r\n";

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

            this.totalSmartCodingEntities.CreateStoredProcedure("FillingPackUpdateEntryStatus", queryString);
        }
    }
}
