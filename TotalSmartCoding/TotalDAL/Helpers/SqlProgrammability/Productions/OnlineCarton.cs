using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class OnlineCarton
    {
        private readonly TotalSmartCodingEntities totalSmartCodingEntities;

        public OnlineCarton(TotalSmartCodingEntities totalSmartCodingEntities)
        {
            this.totalSmartCodingEntities = totalSmartCodingEntities;
        }

        public void RestoreProcedure()
        {
            this.OnlineCartonSaveRelative();

            this.OnlineCartonEditable();
        }

        private void OnlineCartonSaveRelative()
        {
            //BE CAREFULL WHEN SAVE: NEED TO SET @OnlinePackIDs (FOR BOTH WHEN SAVE - Update AND DELETE - Undo
            string queryString = " @EntityID int, @SaveRelativeOption int, @OnlinePackIDs varchar(3999) " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";

            queryString = queryString + "           IF (@SaveRelativeOption = 1) " + "\r\n";

            queryString = queryString + "               UPDATE      OnlinePacks" + "\r\n";
            queryString = queryString + "               SET         OnlineCartonID = @EntityID " + "\r\n"; //WHERE: NOT BELONG TO ANY CARTON, AND NUMBER OF PACK EFFECTED: IS THE SAME OnlinePackID PASS BY VARIBLE: OnlinePackIDs
            queryString = queryString + "               WHERE       OnlineCartonID IS NULL AND OnlinePackID IN (SELECT Id FROM dbo.SplitToIntList (@OnlinePackIDs)) " + "\r\n";

            queryString = queryString + "           ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 

            queryString = queryString + "               UPDATE      OnlinePacks" + "\r\n";
            queryString = queryString + "               SET         OnlineCartonID = NULL " + "\r\n"; //WHERE: NOT BELONG TO ANY CARTON, AND NUMBER OF PACK EFFECTED: IS THE SAME OnlinePackID PASS BY VARIBLE: OnlinePackIDs
            queryString = queryString + "               WHERE       OnlineCartonID = @EntityID AND OnlinePackID IN (SELECT Id FROM dbo.SplitToIntList (@OnlinePackIDs)) " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> ((SELECT (LEN(@OnlinePackIDs) - LEN(REPLACE(@OnlinePackIDs, ',', '')))) + 1) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE     @msg NVARCHAR(300) = N'System Error: Some pack does not exist!' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("OnlineCartonSaveRelative", queryString);
        }


        private void OnlineCartonEditable()
        {
            string[] queryArray = new string[1];
            
            queryArray[0] = " SELECT TOP 1 @FoundEntity = OnlineCartonID FROM OnlineCartons WHERE OnlineCartonID = @EntityID AND NOT OnlinePalletID IS NULL";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("OnlineCartonEditable", queryArray);
        }


    }
}
