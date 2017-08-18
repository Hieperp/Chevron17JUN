using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class Pickup
    {
        private readonly TotalSmartCodingEntities totalSmartCodingEntities;

        public Pickup(TotalSmartCodingEntities totalSmartCodingEntities)
        {
            this.totalSmartCodingEntities = totalSmartCodingEntities;
        }

        public void RestoreProcedure()
        {
            this.GetPickupIndexes();

            this.GetPickupViewDetails();

            this.GetPendingPallets();

            this.PickupSaveRelative();
            this.PickupPostSaveValidate();

            this.PickupApproved();
            this.PickupEditable();

            this.PickupToggleApproved();

            this.PickupInitReference();
        }


        private void GetPickupIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Pickups.PickupID, CAST(Pickups.EntryDate AS DATE) AS EntryDate, Pickups.Reference, Locations.Code AS LocationCode, Warehouses.Name AS WarehouseName, Pickups.Description, Pickups.TotalQuantity, Pickups.TotalVolume, Pickups.Approved " + "\r\n";
            queryString = queryString + "       FROM        Pickups " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON Pickups.EntryDate >= @FromDate AND Pickups.EntryDate <= @ToDate AND Pickups.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.Pickup + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = Pickups.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Warehouses ON Pickups.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetPickupIndexes", queryString);
        }


        #region X


        private void GetPickupViewDetails()
        {
            string queryString;

            queryString = " @PickupID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      PickupDetails.PickupDetailID, PickupDetails.PickupID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, PickupDetails.BinLocationID, BinLocations.Code AS BinLocationCode, " + "\r\n";
            queryString = queryString + "                   PickupDetails.PackID, Packs.Code AS PackCode, Packs.EntryDate AS PackEntryDate, PickupDetails.CartonID, Cartons.Code AS CartonCode, Cartons.EntryDate AS CartonEntryDate, PickupDetails.PalletID, Pallets.Code AS PalletCode, Pallets.EntryDate AS PalletEntryDate, " + "\r\n";
            queryString = queryString + "                   PickupDetails.Quantity, PickupDetails.Volume, PickupDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        PickupDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PickupDetails.PickupID = @PickupID AND PickupDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN BinLocations ON PickupDetails.BinLocationID = BinLocations.BinLocationID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Packs ON PickupDetails.PackID = Packs.PackID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Cartons ON PickupDetails.CartonID = Cartons.CartonID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Pallets ON PickupDetails.PalletID = Pallets.PalletID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetPickupViewDetails", queryString);
        }


        #region Y

        private void GetPendingPallets()
        {
            string queryString;

            queryString = " @LocationID Int, @PickupID Int, @PalletIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@PalletIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPalletIDs(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPalletIDs(false) + "\r\n";

            
            queryString = queryString + "   END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetPendingPallets", queryString);
        }

        private string BuildSQLPalletIDs(bool isPalletIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@PickupID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isPalletIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY Pallets.EntryDate DESC, Pallets.PalletID DESC  " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isPalletIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY Pallets.EntryDate DESC, Pallets.PalletID DESC  " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLNew(isPalletIDs) + " WHERE Pallets.PalletID NOT IN (SELECT PalletID FROM PickupDetails WHERE PickupID = @PickupID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isPalletIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY Pallets.EntryDate DESC, Pallets.PalletID DESC  " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLNew(bool isPalletIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      Pallets.PalletID, Pallets.EntryDate, Pallets.Code, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, " + "\r\n";
            queryString = queryString + "                   ROUND(Pallets.Quantity - Pallets.QuantityPickup,  " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, CAST(0 AS decimal(18, 2)) AS Quantity, Pallets.Volume " + "\r\n";

            queryString = queryString + "       FROM        Pallets " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON Pallets.LocationID = @LocationID AND ROUND(Pallets.Quantity - Pallets.QuantityPickup, " + (int)GlobalEnums.rndQuantity + ") > 0 AND Pallets.CommodityID = Commodities.CommodityID " + (isPalletIDs ? " AND Pallets.PalletID NOT IN (SELECT Id FROM dbo.SplitToIntList (@PalletIDs))" : "") + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isPalletIDs)
        {
            string queryString = "";
            queryString = queryString + "       SELECT      Pallets.PalletID, Pallets.EntryDate, Pallets.Code, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, " + "\r\n";
            queryString = queryString + "                   ROUND(Pallets.Quantity - Pallets.QuantityPickup + Pallets.Quantity,  " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, CAST(0 AS decimal(18, 2)) AS Quantity, Pallets.Volume " + "\r\n";

            queryString = queryString + "       FROM        PickupDetails" + "\r\n";
            queryString = queryString + "                   INNER JOIN Pallets ON PickupDetails.PickupID = @PickupID AND PickupDetails.PalletID = Pallets.PalletID " + (isPalletIDs ? " AND Pallets.PalletID NOT IN (SELECT Id FROM dbo.SplitToIntList (@PalletIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PickupDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            return queryString;
        }

        #endregion Y




        private void PickupSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           UPDATE          Pallets " + "\r\n";
            queryString = queryString + "           SET             Pallets.QuantityPickup = ROUND(Pallets.QuantityPickup + PickupDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "           FROM            PickupDetails " + "\r\n";
            queryString = queryString + "                           INNER JOIN Pallets ON PickupDetails.PickupID = @EntityID AND PickupDetails.PalletID = Pallets.PalletID " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> (SELECT COUNT(*) FROM PickupDetails WHERE PickupID = @EntityID) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE     @msg NVARCHAR(300) = N'Phiếu giao hàng đã hủy, hoặc chưa duyệt' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("PickupSaveRelative", queryString);
        }

        private void PickupPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày giao hàng: ' + CAST(Pallets.EntryDate AS nvarchar) FROM PickupDetails INNER JOIN Pallets ON PickupDetails.PickupID = @EntityID AND PickupDetails.PalletID = Pallets.PalletID AND PickupDetails.EntryDate < Pallets.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng nhập kho vượt quá số lượng giao: ' + CAST(ROUND(Quantity - QuantityPickup, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM Pallets WHERE (ROUND(Quantity - QuantityPickup, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("PickupPostSaveValidate", queryArray);
        }




        private void PickupApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PickupID FROM Pickups WHERE PickupID = @EntityID AND Approved = 1";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("PickupApproved", queryArray);
        }


        private void PickupEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PickupID FROM GoodsReceipts WHERE PickupID = @EntityID ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = PickupID FROM GoodsReceiptDetails WHERE PickupID = @EntityID ";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("PickupEditable", queryArray);
        }

        private void PickupToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      Pickups  SET Approved = @Approved, ApprovedDate = GetDate() WHERE PickupID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          PickupDetails  SET Approved = @Approved WHERE PickupID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, 'hủy', '')  + ' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("PickupToggleApproved", queryString);
        }

        private void PickupInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("Pickups", "PickupID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.Pickup));
            this.totalSmartCodingEntities.CreateTrigger("PickupInitReference", simpleInitReference.CreateQuery());
        }


        #endregion
    }
}
