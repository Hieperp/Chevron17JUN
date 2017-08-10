using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class GoodsReceipt
    {
        private readonly TotalSmartCodingEntities totalSmartCodingEntities;

        public GoodsReceipt(TotalSmartCodingEntities totalSmartCodingEntities)
        {
            this.totalSmartCodingEntities = totalSmartCodingEntities;
        }

        public void RestoreProcedure()
        {
            this.GetGoodsReceiptIndexes();
            
            this.GetGoodsReceiptViewDetails();

            this.GetPendingPickups();
            this.GetPendingPickupWarehouses();
            this.GetPendingPickupDetails();

            this.GoodsReceiptSaveRelative();
            this.GoodsReceiptPostSaveValidate();

            this.GoodsReceiptApproved();
            this.GoodsReceiptEditable();

            this.GoodsReceiptToggleApproved();

            this.GoodsReceiptInitReference();
        }


        private void GetGoodsReceiptIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      GoodsReceipts.GoodsReceiptID, CAST(GoodsReceipts.EntryDate AS DATE) AS EntryDate, GoodsReceipts.Reference, Locations.Code AS LocationCode, GoodsReceiptTypes.Code AS GoodsReceiptTypeCode, GoodsReceipts.PickupReferences, Warehouses.Code AS WarehouseCode, GoodsReceipts.Description, GoodsReceipts.TotalQuantity, GoodsReceipts.TotalVolumn, GoodsReceipts.Approved " + "\r\n";
            queryString = queryString + "       FROM        GoodsReceipts " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON GoodsReceipts.EntryDate >= @FromDate AND GoodsReceipts.EntryDate <= @ToDate AND GoodsReceipts.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsReceipt + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = GoodsReceipts.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsReceiptTypes ON GoodsReceipts.GoodsReceiptTypeID = GoodsReceiptTypes.GoodsReceiptTypeID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Warehouses ON GoodsReceipts.WarehouseID = Warehouses.WarehouseID " + "\r\n";            
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetGoodsReceiptIndexes", queryString);
        }

        private void GetGoodsReceiptViewDetails()
        {
            string queryString;

            queryString = " @GoodsReceiptID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.PickupID, GoodsReceiptDetails.PickupDetailID, Pickups.Reference AS PickupReference, Pickups.EntryDate AS PickupEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Pallets.PalletID, Pallets.Code AS PalletCode, GoodsReceiptDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsReceiptDetails.GoodsReceiptID = @GoodsReceiptID AND GoodsReceiptDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Pickups ON GoodsReceiptDetails.PickupID = Pickups.PickupID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Pallets ON GoodsReceiptDetails.PalletID = Pallets.PalletID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetGoodsReceiptViewDetails", queryString);
        }





        #region Y


        private void GetPendingPickups()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Pickups.PickupID, Pickups.EntryDate, Pickups.Reference, Pickups.WarehouseID, Warehouses.Code AS WarehouseCode, Employees.Name AS ForkliftDriverName, Pickups.TotalQuantity, Pickups.Description " + "\r\n";
            queryString = queryString + "       FROM            Pickups " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses ON Pickups.PickupID IN (SELECT DISTINCT PickupID FROM PickupDetails WHERE LocationID = @LocationID AND GoodsReceiptID IS NULL) AND Pickups.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Employees ON Pickups.ForkliftDriverID = Employees.EmployeeID " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetPendingPickups", queryString);
        }

        private void GetPendingPickupWarehouses()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          PickupWarehouses.WarehouseID, Warehouses.Code AS WarehouseCode, PickupWarehouses.TotalQuantity " + "\r\n";
            queryString = queryString + "       FROM            (SELECT WarehouseID, SUM(Quantity) AS TotalQuantity FROM PickupDetails WHERE LocationID = @LocationID AND GoodsReceiptID IS NULL GROUP BY WarehouseID) PickupWarehouses " + "\r\n";
            queryString = queryString + "                       INNER JOIN Warehouses ON PickupWarehouses.WarehouseID = Warehouses.WarehouseID " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetPendingPickupWarehouses", queryString);
        }

        private void GetPendingPickupDetails()
        {
            string queryString;

            queryString = " @LocationID int, @GoodsReceiptID Int, @PickupID Int, @WarehouseID int, @PickupDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@PickupID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPickup(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPickup(false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetPendingPickupDetails", queryString);
        }

        private string BuildSQLPickup(bool isPickupID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@PickupDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPickupPickupDetailIDs(isPickupID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPickupPickupDetailIDs(isPickupID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLPickupPickupDetailIDs(bool isPickupID, bool isPickupDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@GoodsReceiptID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isPickupID, isPickupDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY PickupDetails.EntryDate, PickupDetails.PickupID, PickupDetails.PickupDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isPickupID, isPickupDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY PickupDetails.EntryDate, PickupDetails.PickupID, PickupDetails.PickupDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLNew(isPickupID, isPickupDetailIDs) + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isPickupID, isPickupDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY PickupDetails.EntryDate, PickupDetails.PickupID, PickupDetails.PickupDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLNew(bool isPickupID, bool isPickupDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      PickupDetails.PickupID, PickupDetails.PickupDetailID, PickupDetails.EntryDate, PickupDetails.Reference, PickupDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, PickupDetails.PalletID, Pallets.Code AS PalletCode, PickupDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, PickupDetails.Remarks, PickupDetails.Quantity, PickupDetails.Volumn, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        PickupDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON " + (isPickupID ? " PickupDetails.PickupID = @PickupID" : "PickupDetails.WarehouseID = @WarehouseID") + " AND PickupDetails.LocationID = @LocationID AND PickupDetails.GoodsReceiptID IS NULL AND PickupDetails.CommodityID = Commodities.CommodityID " + (isPickupDetailIDs ? " AND PickupDetails.PickupDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@PickupDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   LEFT JOIN Pallets ON PickupDetails.PalletID = Pallets.PalletID " + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isPickupID, bool isPickupDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      PickupDetails.PickupID, PickupDetails.PickupDetailID, PickupDetails.EntryDate, PickupDetails.Reference, PickupDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, PickupDetails.PalletID, Pallets.Code AS PalletCode, PickupDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, PickupDetails.Remarks, PickupDetails.Quantity, PickupDetails.Volumn, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        PickupDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PickupDetails.GoodsReceiptID = @GoodsReceiptID AND PickupDetails.CommodityID = Commodities.CommodityID " + (isPickupDetailIDs ? " AND PickupDetails.PickupDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@PickupDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   LEFT JOIN Pallets ON PickupDetails.PalletID = Pallets.PalletID " + "\r\n";

            return queryString;
        }

        #endregion Y




        private void GoodsReceiptSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   IF (SELECT HasPickup FROM GoodsReceipts WHERE GoodsReceiptID = @EntityID) = 1 " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";

            queryString = queryString + "           IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "               UPDATE      PickupDetails" + "\r\n";
            queryString = queryString + "               SET         PickupDetails.GoodsReceiptID = GoodsReceiptDetails.GoodsReceiptID " + "\r\n";
            queryString = queryString + "               FROM        PickupDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                           GoodsReceiptDetails ON GoodsReceiptDetails.GoodsReceiptID = @EntityID AND PickupDetails.Approved = 1 AND PickupDetails.PickupDetailID = GoodsReceiptDetails.PickupDetailID " + "\r\n";

            queryString = queryString + "           ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 
            queryString = queryString + "               UPDATE      PickupDetails" + "\r\n";
            queryString = queryString + "               SET         GoodsReceiptID = NULL " + "\r\n";
            queryString = queryString + "               WHERE       GoodsReceiptID = @EntityID " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> (SELECT COUNT(*) FROM GoodsReceiptDetails WHERE GoodsReceiptID = @EntityID) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE     @msg NVARCHAR(300) = N'Phiếu giao thành phẩm không tồn tại, hoặc chưa duyệt' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GoodsReceiptSaveRelative", queryString);
        }

        private void GoodsReceiptPostSaveValidate()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày giao hàng: ' + CAST(PickupDetails.EntryDate AS nvarchar) FROM GoodsReceiptDetails INNER JOIN PickupDetails ON GoodsReceiptDetails.GoodsReceiptID = @EntityID AND GoodsReceiptDetails.PickupDetailID = PickupDetails.PickupDetailID AND GoodsReceiptDetails.EntryDate < PickupDetails.EntryDate ";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("GoodsReceiptPostSaveValidate", queryArray);
        }




        private void GoodsReceiptApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsReceiptID FROM GoodsReceipts WHERE GoodsReceiptID = @EntityID AND Approved = 1";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("GoodsReceiptApproved", queryArray);
        }


        private void GoodsReceiptEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsReceiptID FROM GoodsReceipts WHERE GoodsReceiptID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = GoodsReceiptID FROM GoodsIssueDetails WHERE GoodsReceiptID = @EntityID ";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("GoodsReceiptEditable", queryArray);
        }


        private void GoodsReceiptToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      GoodsReceipts  SET Approved = @Approved, ApprovedDate = GetDate() WHERE GoodsReceiptID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          GoodsReceiptDetails  SET Approved = @Approved WHERE GoodsReceiptID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, 'hủy', '')  + ' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GoodsReceiptToggleApproved", queryString);
        }

        private void GoodsReceiptInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("GoodsReceipts", "GoodsReceiptID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.GoodsReceipt));
            this.totalSmartCodingEntities.CreateTrigger("GoodsReceiptInitReference", simpleInitReference.CreateQuery());
        }


    }
}
