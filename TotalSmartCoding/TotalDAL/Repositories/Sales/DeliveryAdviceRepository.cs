using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Sales;


namespace TotalDAL.Repositories.Sales
{
    public class DeliveryAdviceRepository : GenericWithDetailRepository<DeliveryAdvice, DeliveryAdviceDetail>, IDeliveryAdviceRepository
    {
        //1-Set Balance Date to 23.59.59
        //2-Copy 3 table (D.A/ GoodsIssue)
        //3-Add two Store procedure (Update balance/ WH journal)
        //4-Modify to VB Project, verify report 1280.rpt (-> create new report in SSRS -> publish to server)

        public DeliveryAdviceRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "DeliveryAdviceEditable", "DeliveryAdviceApproved", null, "DeliveryAdviceVoidable")
        {


            //return;

            //Helpers.SqlProgrammability.Reports.SaleReports saleReports = new Helpers.SqlProgrammability.Reports.SaleReports(totalSmartCodingEntities);
            //saleReports.RestoreProcedure();

        }
    }








    public class DeliveryAdviceAPIRepository : GenericAPIRepository, IDeliveryAdviceAPIRepository
    {
        public DeliveryAdviceAPIRepository(TotalSmartCodingEntities totalSmartCodingEntities)
            : base(totalSmartCodingEntities, "GetDeliveryAdviceIndexes")
        {
        }

        //public IEnumerable<DeliveryAdvicePendingCustomer> GetCustomers(int? locationID)
        //{
        //    this.TotalSmartCodingEntities.Configuration.ProxyCreationEnabled = false;
        //    IEnumerable<DeliveryAdvicePendingCustomer> pendingSalesOrderCustomers = base.TotalSmartCodingEntities.GetDeliveryAdvicePendingCustomers(locationID).ToList();
        //    this.TotalSmartCodingEntities.Configuration.ProxyCreationEnabled = true;

        //    return pendingSalesOrderCustomers;
        //}

        //public IEnumerable<DeliveryAdvicePendingSalesOrder> GetSalesOrders(int? locationID)
        //{
        //    this.TotalSmartCodingEntities.Configuration.ProxyCreationEnabled = false;
        //    IEnumerable<DeliveryAdvicePendingSalesOrder> pendingSalesOrders = base.TotalSmartCodingEntities.GetDeliveryAdvicePendingSalesOrders(locationID).ToList();
        //    this.TotalSmartCodingEntities.Configuration.ProxyCreationEnabled = true;

        //    return pendingSalesOrders;
        //}

        //public IEnumerable<DeliveryAdvicePendingSalesOrderDetail> GetPendingSalesOrderDetails(int? locationID, int? deliveryAdviceID, int? salesOrderID, int? customerID, int? receiverID, int? priceCategoryID, int? warehouseID, string shippingAddress, decimal? tradeDiscountRate, decimal? vatPercent, DateTime? entryDate, string salesOrderDetailIDs, bool isReadonly)
        //{
        //    this.TotalSmartCodingEntities.Configuration.ProxyCreationEnabled = false;
        //    IEnumerable<DeliveryAdvicePendingSalesOrderDetail> pendingSalesOrderDetails = base.TotalSmartCodingEntities.GetDeliveryAdvicePendingSalesOrderDetails(locationID, deliveryAdviceID, salesOrderID, customerID, receiverID, priceCategoryID, warehouseID, shippingAddress, tradeDiscountRate, vatPercent, entryDate, salesOrderDetailIDs, isReadonly).ToList();
        //    this.TotalSmartCodingEntities.Configuration.ProxyCreationEnabled = true;

        //    return pendingSalesOrderDetails;
        //}

    }


}
