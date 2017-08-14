using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Warehouse
    {
        private readonly TotalSmartCodingEntities totalSmartCodingEntities;

        public Warehouse(TotalSmartCodingEntities totalSmartCodingEntities)
        {
            this.totalSmartCodingEntities = totalSmartCodingEntities;
        }

        public void RestoreProcedure()
        {
            this.GetWarehouseIndexes();

            //this.WarehouseEditable(); 
            //this.WarehouseSaveRelative();

            this.GetWarehouseBases();
        }


        private void GetWarehouseIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Warehouses.WarehouseID, Warehouses.Code, Warehouses.Name " + "\r\n";
            queryString = queryString + "       FROM        Warehouses " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetWarehouseIndexes", queryString);
        }


        private void WarehouseSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";

            queryString = queryString + "               INSERT INTO WarehouseWarehouses (WarehouseID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n";
            queryString = queryString + "               SELECT      WarehouseID, 46 AS WarehouseID, " + (int)GlobalEnums.NmvnTaskID.SalesOrder + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Warehouses WHERE WarehouseID = @EntityID " + "\r\n";

            queryString = queryString + "               INSERT INTO WarehouseWarehouses (WarehouseID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n";
            queryString = queryString + "               SELECT      Warehouses.WarehouseID, Warehouses.WarehouseID, " + (int)GlobalEnums.NmvnTaskID.DeliveryAdvice + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Warehouses INNER JOIN Warehouses ON Warehouses.WarehouseID = @EntityID AND Warehouses.WarehouseCategoryID NOT IN (4, 5, 7, 9, 10, 11, 12) AND Warehouses.WarehouseCategoryID = Warehouses.WarehouseCategoryID " + "\r\n";

            queryString = queryString + "               INSERT INTO WarehouseWarehouses (WarehouseID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n";
            queryString = queryString + "               SELECT      WarehouseID, 82 AS WarehouseID, " + (int)GlobalEnums.NmvnTaskID.DeliveryAdvice + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Warehouses WHERE WarehouseID = @EntityID AND WarehouseCategoryID IN (4, 5, 7, 9, 10, 11, 12) " + "\r\n";

            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 
            queryString = queryString + "           DELETE      WarehouseWarehouses WHERE WarehouseID = @EntityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("WarehouseSaveRelative", queryString);
        }


        private void WarehouseEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = WarehouseID FROM Warehouses WHERE WarehouseID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = WarehouseID FROM GoodsIssueDetails WHERE WarehouseID = @EntityID ";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("WarehouseEditable", queryArray);
        }


        private void GetWarehouseBases()
        {
            string queryString;

            queryString = " " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      WarehouseID, Code, Name " + "\r\n";
            queryString = queryString + "       FROM        Warehouses " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetWarehouseBases", queryString);
        }

    }
}
