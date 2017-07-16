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

            this.BatchInitReference();
        }


        private void GetBatchIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Batches.BatchID, CAST(Batches.EntryDate AS DATE) AS EntryDate, Batches.Reference, Batches.Code, Batches.FillingLineID, Batches.CommodityID, Commodities.Code AS CommodityCode, Commodities.OfficialCode AS CommodityOfficialCode, Commodities.Name AS CommodityName, Commodities.PackPerCarton, Commodities.CartonPerPallet, Commodities.NoExpiryDate, Commodities.IsPailLabel, " + "\r\n";
            queryString = queryString + "                   Batches.LastPackNo, Batches.LastCartonNo, Batches.LastPalletNo, Batches.Description, Batches.Remarks, Batches.CreatedDate, Batches.EditedDate, Batches.IsDefault " + "\r\n";
            queryString = queryString + "       FROM        Batches INNER JOIN " + "\r\n";
            queryString = queryString + "                   Commodities ON Batches.FillingLineID = 1 AND ((Batches.EntryDate >= @FromDate AND Batches.EntryDate <= @ToDate) OR Batches.IsDefault = 1) AND Batches.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetBatchIndexes", queryString);
        }

        private void BatchEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = BatchID FROM Batches WHERE BatchID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = BatchID FROM GoodsIssueDetails WHERE BatchID = @EntityID ";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("BatchEditable", queryArray);
        }

        private void BatchInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("Batches", "BatchID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.Batch));
            this.totalSmartCodingEntities.CreateTrigger("BatchInitReference", simpleInitReference.CreateQuery());
        }


    }
}
