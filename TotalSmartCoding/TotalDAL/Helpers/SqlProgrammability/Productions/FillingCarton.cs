﻿using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class FillingCarton
    {
        private readonly TotalSmartCodingEntities totalSmartCodingEntities;

        public FillingCarton(TotalSmartCodingEntities totalSmartCodingEntities)
        {
            this.totalSmartCodingEntities = totalSmartCodingEntities;
        }

        public void RestoreProcedure()
        {
            this.FillingCartonSaveRelative();

            this.FillingCartonEditable();

            this.FillingCartonUpdateEntryStatus();
        }

        private void FillingCartonSaveRelative()
        {
            //BE CAREFULL WHEN SAVE: NEED TO SET @FillingPackIDs (FOR BOTH WHEN SAVE - Update AND DELETE - Undo
            string queryString = " @EntityID int, @SaveRelativeOption int, @FillingPackIDs varchar(3999) " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";

            queryString = queryString + "           IF (@SaveRelativeOption = 1) " + "\r\n";

            queryString = queryString + "               UPDATE      FillingPacks" + "\r\n";
            queryString = queryString + "               SET         FillingCartonID = @EntityID " + "\r\n"; //WHERE: NOT BELONG TO ANY CARTON, AND NUMBER OF PACK EFFECTED: IS THE SAME FillingPackID PASS BY VARIBLE: FillingPackIDs
            queryString = queryString + "               WHERE       FillingCartonID IS NULL AND FillingPackID IN (SELECT Id FROM dbo.SplitToIntList (@FillingPackIDs)) " + "\r\n";

            queryString = queryString + "           ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 

            queryString = queryString + "               UPDATE      FillingPacks" + "\r\n";
            queryString = queryString + "               SET         FillingCartonID = NULL " + "\r\n"; //WHERE: NOT BELONG TO ANY CARTON, AND NUMBER OF PACK EFFECTED: IS THE SAME FillingPackID PASS BY VARIBLE: FillingPackIDs
            queryString = queryString + "               WHERE       FillingCartonID = @EntityID AND FillingPackID IN (SELECT Id FROM dbo.SplitToIntList (@FillingPackIDs)) " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> ((SELECT (LEN(@FillingPackIDs) - LEN(REPLACE(@FillingPackIDs, ',', '')))) + 1) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE     @msg NVARCHAR(300) = N'System Error: Some pack does not exist!' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("FillingCartonSaveRelative", queryString);
        }


        private void FillingCartonEditable()
        {
            string[] queryArray = new string[1];
            
            queryArray[0] = " SELECT TOP 1 @FoundEntity = FillingCartonID FROM FillingCartons WHERE FillingCartonID = @EntityID AND NOT FillingPalletID IS NULL";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("FillingCartonEditable", queryArray);
        }



        private void FillingCartonUpdateEntryStatus()
        {
            //BE CAREFULL WHEN SAVE: NEED TO SET @FillingCartonIDs (FOR BOTH WHEN SAVE - Update AND DELETE - Undo
            string queryString = " @FillingCartonIDs varchar(3999), @EntryStatusID int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      FillingCartons" + "\r\n";
            queryString = queryString + "       SET         EntryStatusID = @EntryStatusID " + "\r\n";
            queryString = queryString + "       WHERE       FillingCartonID IN (SELECT Id FROM dbo.SplitToIntList (@FillingCartonIDs)) " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("FillingCartonUpdateEntryStatus", queryString);
        }

    }
}
