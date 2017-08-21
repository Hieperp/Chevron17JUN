using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class FillingPallet
    {
        private readonly TotalSmartCodingEntities totalSmartCodingEntities;

        public FillingPallet(TotalSmartCodingEntities totalSmartCodingEntities)
        {
            this.totalSmartCodingEntities = totalSmartCodingEntities;
        }

        public void RestoreProcedure()
        {
            this.FillingPalletSaveRelative();

            this.FillingPalletEditable();


            this.GetFillingPallets();
            this.FillingPalletUpdateEntryStatus();
        }

        private void FillingPalletSaveRelative()
        {
            //BE CAREFULL WHEN SAVE: NEED TO SET @FillingCartonIDs (FOR BOTH WHEN SAVE - Update AND DELETE - Undo
            string queryString = " @EntityID int, @SaveRelativeOption int, @FillingCartonIDs varchar(3999) " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   IF (@FillingCartonIDs <> '') " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";

            queryString = queryString + "           IF (@SaveRelativeOption = 1) " + "\r\n";

            queryString = queryString + "               UPDATE      FillingCartons" + "\r\n";
            queryString = queryString + "               SET         FillingPalletID = @EntityID, EntryStatusID = " + (int)GlobalVariables.BarcodeStatus.Wrapped + "\r\n"; //WHERE: NOT BELONG TO ANY CARTON, AND NUMBER OF PACK EFFECTED: IS THE SAME FillingCartonID PASS BY VARIBLE: FillingCartonIDs
            queryString = queryString + "               WHERE       FillingPalletID IS NULL AND EntryStatusID = " + (int)GlobalVariables.BarcodeStatus.Readytoset + " AND FillingCartonID IN (SELECT Id FROM dbo.SplitToIntList (@FillingCartonIDs)) " + "\r\n";

            queryString = queryString + "           ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 

            queryString = queryString + "               UPDATE      FillingCartons" + "\r\n";
            queryString = queryString + "               SET         FillingPalletID = NULL, EntryStatusID = " + (int)GlobalVariables.BarcodeStatus.Readytoset + "\r\n"; //WHERE: NOT BELONG TO ANY CARTON, AND NUMBER OF PACK EFFECTED: IS THE SAME FillingCartonID PASS BY VARIBLE: FillingCartonIDs
            queryString = queryString + "               WHERE       FillingPalletID = @EntityID AND EntryStatusID = " + (int)GlobalVariables.BarcodeStatus.Wrapped + " AND FillingCartonID IN (SELECT Id FROM dbo.SplitToIntList (@FillingCartonIDs)) " + "\r\n";

            queryString = queryString + "           " + "\r\n";
            queryString = queryString + "           IF @@ROWCOUNT <> (SELECT TotalCartons FROM FillingPallets WHERE FillingPalletID = @EntityID)  OR  @@ROWCOUNT <> ((SELECT (LEN(@FillingCartonIDs) - LEN(REPLACE(@FillingCartonIDs, ',', '')))) + 1) " + "\r\n"; //CHECK BOTH CONDITION FOR SURE. BUT: WE CAN OMIT THE SECOND CONDITION 
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE     @msg NVARCHAR(300) = N'System Error: Some carton does not exist!' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("FillingPalletSaveRelative", queryString);
        }


        private void FillingPalletEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = FillingPalletID FROM FillingPallets WHERE FillingPalletID = @EntityID AND NOT  some ID: pickup already IS NULL"; SHOULD CHECK AGAIN

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("FillingPalletEditable", queryArray);
        }





        private void GetFillingPallets()
        {
            string queryString = " @FillingLineID int, @EntryStatusIDs varchar(3999) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT * FROM FillingPallets WHERE FillingLineID = @FillingLineID AND EntryStatusID IN (SELECT Id FROM dbo.SplitToIntList (@EntryStatusIDs))  " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetFillingPallets", queryString);
        }

        private void FillingPalletUpdateEntryStatus()
        {
            //BE CAREFULL WHEN SAVE: NEED TO SET @FillingPalletIDs (FOR BOTH WHEN SAVE - Update AND DELETE - Undo
            string queryString = " @FillingPalletIDs varchar(3999), @EntryStatusID int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      FillingPallets" + "\r\n";
            queryString = queryString + "       SET         EntryStatusID = @EntryStatusID " + "\r\n";
            queryString = queryString + "       WHERE       FillingPalletID IN (SELECT Id FROM dbo.SplitToIntList (@FillingPalletIDs)) " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("FillingPalletUpdateEntryStatus", queryString);
        }

    }
}
