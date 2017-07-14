using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class OnlinePack
    {
        private readonly TotalSmartCodingEntities totalSmartCodingEntities;

        public OnlinePack(TotalSmartCodingEntities totalSmartCodingEntities)
        {
            this.totalSmartCodingEntities = totalSmartCodingEntities;
        }

        public void RestoreProcedure()
        {
            this.OnlinePackUpdateEntryStatus();

            this.OnlinePackEditable();
        }

        private void OnlinePackUpdateEntryStatus()
        {
            //BE CAREFULL WHEN SAVE: NEED TO SET @OnlinePackIDs (FOR BOTH WHEN SAVE - Update AND DELETE - Undo
            string queryString = " @OnlinePackIDs varchar(3999), @EntryStatusID int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      OnlinePacks" + "\r\n";
            queryString = queryString + "       SET         EntryStatusID = @EntryStatusID " + "\r\n";
            queryString = queryString + "       WHERE       OnlinePackID IN (SELECT Id FROM dbo.SplitToIntList (@OnlinePackIDs)) " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("OnlinePackUpdateEntryStatus", queryString);
        }


        private void OnlinePackEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = OnlinePackID FROM OnlinePacks WHERE OnlinePackID = @EntityID AND NOT OnlineCartonID IS NULL";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("OnlinePackEditable", queryArray);
        }


    }
}
