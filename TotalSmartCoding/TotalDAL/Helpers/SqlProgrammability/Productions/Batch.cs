using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class Batch
    {
        private readonly TotalSmartCodingEntities totalSmartCodingEntities;

        public Batch(TotalSmartCodingEntities totalSmartCodingEntities)
        {
            this.totalSmartCodingEntities = totalSmartCodingEntities;
        }

        public void RestoreProcedure()
        {
            this.GetBatchIndexes();

            this.BatchEditable();
            this.BatchPostSaveValidate();

            this.BatchToggleApproved();
            this.BatchToggleVoid();

            this.BatchInitReference();


            this.BatchCommonUpdate();
        }


        private void GetBatchIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime, @FillingLineID int, @ActiveOption int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Batches.BatchID, CAST(Batches.EntryDate AS DATE) AS EntryDate, Batches.Reference, Batches.Code AS BatchCode, Batches.FillingLineID, Batches.CommodityID, Commodities.Code AS CommodityCode, Commodities.OfficialCode AS CommodityOfficialCode, Commodities.Name AS CommodityName, Commodities.Volume, Commodities.PackPerCarton, Commodities.CartonPerPallet, Commodities.NoExpiryDate, " + "\r\n";
            queryString = queryString + "                   Batches.NextPackNo, Batches.NextCartonNo, Batches.NextPalletNo, Batches.Description, Batches.Remarks, Batches.CreatedDate, Batches.EditedDate, Batches.IsDefault, Batches.InActive " + "\r\n";
            queryString = queryString + "       FROM        Batches INNER JOIN " + "\r\n";
            queryString = queryString + "                   Commodities ON Batches.FillingLineID = @FillingLineID AND (@ActiveOption = -1 OR Batches.InActive = @ActiveOption) AND ((Batches.EntryDate >= @FromDate AND Batches.EntryDate <= @ToDate) OR Batches.IsDefault = 1) AND Batches.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetBatchIndexes", queryString);
        }



        private void BatchEditable()
        {
            string[] queryArray = new string[4];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = BatchID FROM Batches WHERE BatchID = @EntityID AND InActive = 1 "; //Don't allow edit after void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = BatchID FROM FillingPallets WHERE BatchID = @EntityID ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = BatchID FROM FillingCartons WHERE BatchID = @EntityID ";
            queryArray[3] = " SELECT TOP 1 @FoundEntity = BatchID FROM FillingPacks WHERE BatchID = @EntityID ";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("BatchEditable", queryArray);
        }

        private void BatchPostSaveValidate()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày xuất kho: ' + CAST(GoodsIssueDetails.EntryDate AS nvarchar) FROM BatchDetails INNER JOIN GoodsIssueDetails ON BatchDetails.BatchID = @EntityID AND BatchDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID AND BatchDetails.EntryDate < GoodsIssueDetails.EntryDate ";
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Ngày xuất kho: ' + CAST(CAST(GoodsIssueDetails.EntryDate AS Date) AS nvarchar) + N' (Ngày HĐ phải sau ngày xuất kho)' FROM BatchDetails INNER JOIN GoodsIssueDetails ON BatchDetails.BatchID = @EntityID AND BatchDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID AND BatchDetails.VATInvoiceDate < CAST(GoodsIssueDetails.EntryDate AS Date) ";
            //queryArray[2] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất hóa đơn vượt quá số lượng xuất kho: ' + CAST(ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) + ' OR free quantity: ' + CAST(ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM BatchDetails INNER JOIN GoodsIssueDetails ON BatchDetails.BatchID = @EntityID AND BatchDetails.GoodsIssueDetailID = GoodsIssueDetails.GoodsIssueDetailID AND (ROUND(GoodsIssueDetails.Quantity - GoodsIssueDetails.QuantityInvoice, " + (int)GlobalEnums.rndQuantity + ") < 0 OR ROUND(GoodsIssueDetails.FreeQuantity - GoodsIssueDetails.FreeQuantityInvoice, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("BatchPostSaveValidate", queryArray);
        }

        private void BatchToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      Batches  SET IsDefault = ~@Approved WHERE BatchID <> @EntityID AND IsDefault =  @Approved" + "\r\n";
            queryString = queryString + "       UPDATE      Batches  SET IsDefault =  @Approved WHERE BatchID =  @EntityID AND IsDefault = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT <> 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Không thể cài đặt batch này cho sản xuất' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("BatchToggleApproved", queryString);
        }

        private void BatchToggleVoid()
        {
            string queryString = " @EntityID int, @InActive bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      Batches  SET InActive = @InActive, InActiveDate = GetDate() WHERE BatchID = @EntityID AND InActive = ~@InActive" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT <> 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Batch không tồn tại hoặc ' + iif(@InActive = 0, 'đang', 'dừng')  + ' sản xuất' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            this.totalSmartCodingEntities.CreateStoredProcedure("BatchToggleVoid", queryString);
        }

        private void BatchInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("Batches", "BatchID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.Batch));
            this.totalSmartCodingEntities.CreateTrigger("BatchInitReference", simpleInitReference.CreateQuery());
        }


        private void BatchCommonUpdate()
        {
            string queryString = " @BatchID int, @NextPackNo nvarchar(10), @NextCartonNo nvarchar(10), @NextPalletNo nvarchar(10) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE      Batches" + "\r\n";
            queryString = queryString + "       SET         NextPackNo = CASE WHEN @NextPackNo != '' THEN @NextPackNo ELSE NextPackNo END, NextCartonNo = CASE WHEN @NextCartonNo != '' THEN @NextCartonNo ELSE NextCartonNo END, NextPalletNo = CASE WHEN @NextPalletNo != '' THEN @NextPalletNo ELSE NextPalletNo END " + "\r\n";
            queryString = queryString + "       WHERE       BatchID = @BatchID " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("BatchCommonUpdate", queryString);
        }
    }
}
