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
            this.FillingPackUpdateEntryStatus();

            this.FillingPackEditable();
        }

        private void FillingPackUpdateEntryStatus()
        {
            //BE CAREFULL WHEN SAVE: NEED TO SET @FillingPackIDs (FOR BOTH WHEN SAVE - Update AND DELETE - Undo
            string queryString = " @FillingPackIDs varchar(3999), @EntryStatusID int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      FillingPacks" + "\r\n";
            queryString = queryString + "       SET         EntryStatusID = @EntryStatusID " + "\r\n";
            queryString = queryString + "       WHERE       FillingPackID IN (SELECT Id FROM dbo.SplitToIntList (@FillingPackIDs)) " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("FillingPackUpdateEntryStatus", queryString);
        }


        private void FillingPackEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = FillingPackID FROM FillingPacks WHERE FillingPackID = @EntityID AND NOT FillingCartonID IS NULL";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("FillingPackEditable", queryArray);
        }


    }
}
