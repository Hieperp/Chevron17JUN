using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class BinLocation
    {
        private readonly TotalSmartCodingEntities totalSmartCodingEntities;

        public BinLocation(TotalSmartCodingEntities totalSmartCodingEntities)
        {
            this.totalSmartCodingEntities = totalSmartCodingEntities;
        }

        public void RestoreProcedure()
        {
            this.GetBinLocationIndexes();

            //this.BinLocationEditable(); 
            //this.BinLocationSaveRelative();

            this.GetBinLocationBases();
        }


        private void GetBinLocationIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      BinLocations.BinLocationID, BinLocations.Code, BinLocations.Name " + "\r\n";
            queryString = queryString + "       FROM        BinLocations " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetBinLocationIndexes", queryString);
        }


        private void BinLocationSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";

            queryString = queryString + "               INSERT INTO BinLocationBinLocations (BinLocationID, BinLocationID, BinLocationTaskID, EntryDate, Remarks, InActive) " + "\r\n";
            queryString = queryString + "               SELECT      BinLocationID, 46 AS BinLocationID, " + (int)GlobalEnums.NmvnTaskID.SalesOrder + " AS BinLocationTaskID, GETDATE(), '', 0 FROM BinLocations WHERE BinLocationID = @EntityID " + "\r\n";

            queryString = queryString + "               INSERT INTO BinLocationBinLocations (BinLocationID, BinLocationID, BinLocationTaskID, EntryDate, Remarks, InActive) " + "\r\n";
            queryString = queryString + "               SELECT      BinLocations.BinLocationID, BinLocations.BinLocationID, " + (int)GlobalEnums.NmvnTaskID.DeliveryAdvice + " AS BinLocationTaskID, GETDATE(), '', 0 FROM BinLocations INNER JOIN BinLocations ON BinLocations.BinLocationID = @EntityID AND BinLocations.BinLocationCategoryID NOT IN (4, 5, 7, 9, 10, 11, 12) AND BinLocations.BinLocationCategoryID = BinLocations.BinLocationCategoryID " + "\r\n";

            queryString = queryString + "               INSERT INTO BinLocationBinLocations (BinLocationID, BinLocationID, BinLocationTaskID, EntryDate, Remarks, InActive) " + "\r\n";
            queryString = queryString + "               SELECT      BinLocationID, 82 AS BinLocationID, " + (int)GlobalEnums.NmvnTaskID.DeliveryAdvice + " AS BinLocationTaskID, GETDATE(), '', 0 FROM BinLocations WHERE BinLocationID = @EntityID AND BinLocationCategoryID IN (4, 5, 7, 9, 10, 11, 12) " + "\r\n";

            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 
            queryString = queryString + "           DELETE      BinLocationBinLocations WHERE BinLocationID = @EntityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("BinLocationSaveRelative", queryString);
        }


        private void BinLocationEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = BinLocationID FROM BinLocations WHERE BinLocationID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = BinLocationID FROM GoodsIssueDetails WHERE BinLocationID = @EntityID ";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("BinLocationEditable", queryArray);
        }


        private void GetBinLocationBases()
        {
            string queryString;

            queryString = " " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      BinLocationID, Code, Name " + "\r\n";
            queryString = queryString + "       FROM        BinLocations " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetBinLocationBases", queryString);
        }

    }
}
