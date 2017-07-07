using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;



using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Sales;

using TotalCore.Repositories.Sales;
//using TotalSmartCoding.Areas.Sales.ViewModels;
//using TotalSmartCoding.APIs.Sessions;

using TotalBase;

namespace TotalSmartCoding.Controllers.APIs.Sales
{
    public class DeliveryAdviceAPIsController
    {
        private readonly IDeliveryAdviceAPIRepository deliveryAdviceAPIRepository;

        public DeliveryAdviceAPIsController(IDeliveryAdviceAPIRepository deliveryAdviceAPIRepository)
        {
            this.deliveryAdviceAPIRepository = deliveryAdviceAPIRepository;
        }


        public ICollection<DeliveryAdviceIndex> GetDeliveryAdviceIndexes()
        {
            ICollection<DeliveryAdviceIndex> deliveryAdviceIndexes = this.deliveryAdviceAPIRepository.GetEntityIndexes<DeliveryAdviceIndex>(ContextAttributes.AspUserID, ContextAttributes.FromDate, ContextAttributes.ToDate);

            return deliveryAdviceIndexes;
        }





        //public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        //{
        //    var result = this.deliveryAdviceAPIRepository.GetCustomers(locationID);
        //    return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetSalesOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        //{
        //    var result = this.deliveryAdviceAPIRepository.GetSalesOrders(locationID);
        //    return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        //}


        //public JsonResult GetPendingSalesOrderDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? deliveryAdviceID, int? salesOrderID, int? customerID, int? receiverID, int? priceCategoryID, int? warehouseID, string shippingAddress, decimal? tradeDiscountRate, decimal? vatPercent, DateTime? entryDate, string salesOrderDetailIDs, bool isReadonly)
        //{
        //    var result = this.deliveryAdviceAPIRepository.GetPendingSalesOrderDetails(locationID, deliveryAdviceID, salesOrderID, customerID, receiverID, priceCategoryID, warehouseID, shippingAddress, tradeDiscountRate, vatPercent, entryDate, salesOrderDetailIDs, false);
        //    return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        //}



    }
}
