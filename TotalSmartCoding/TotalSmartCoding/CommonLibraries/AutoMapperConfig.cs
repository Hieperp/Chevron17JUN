﻿using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;


using TotalModel.Models;

using TotalDTO.Sales;
using TotalDTO.Inventories;
using TotalDTO.Commons;

using TotalSmartCoding.ViewModels.Sales;
using TotalSmartCoding.ViewModels.Inventories;
using TotalDTO.Productions;
using TotalSmartCoding.ViewModels.Productions;

namespace TotalSmartCoding.CommonLibraries
{
    public static class AutoMapperConfig
    {
        public static void SetupMappings()
        {
            ////////https://github.com/AutoMapper/AutoMapper/wiki/Static-and-Instance-API

            Mapper.Initialize(cfg =>
            {
               


                cfg.CreateMap<DeliveryAdvice, DeliveryAdviceViewModel>();
                cfg.CreateMap<DeliveryAdvice, DeliveryAdviceDTO>();
                cfg.CreateMap<DeliveryAdvicePrimitiveDTO, DeliveryAdvice>();
                cfg.CreateMap<DeliveryAdviceViewDetail, DeliveryAdviceDetailDTO>();
                cfg.CreateMap<DeliveryAdviceDetailDTO, DeliveryAdviceDetail>();

                cfg.CreateMap<DeliveryAdvice, DeliveryAdviceBoxDTO>();



                cfg.CreateMap<GoodsReceipt, GoodsReceiptViewModel>();
                cfg.CreateMap<GoodsReceipt, GoodsReceiptDTO>();
                cfg.CreateMap<GoodsReceiptPrimitiveDTO, GoodsReceipt>();
                cfg.CreateMap<GoodsReceiptViewDetail, GoodsReceiptDetailDTO>();
                cfg.CreateMap<GoodsReceiptDetailDTO, GoodsReceiptDetail>();



                cfg.CreateMap<OnlinePack, OnlinePackViewModel>();
                cfg.CreateMap<OnlinePack, OnlinePackDTO>();
                cfg.CreateMap<OnlinePackPrimitiveDTO, OnlinePack>();


                cfg.CreateMap<OnlineCarton, OnlineCartonViewModel>();
                cfg.CreateMap<OnlineCarton, OnlineCartonDTO>();
                cfg.CreateMap<OnlineCartonPrimitiveDTO, OnlineCarton>();


                cfg.CreateMap<OnlinePallet, OnlinePalletViewModel>();
                cfg.CreateMap<OnlinePallet, OnlinePalletDTO>();
                cfg.CreateMap<OnlinePalletPrimitiveDTO, OnlinePallet>();



                //cfg.CreateMap<Employee, EmployeeBaseDTO>();
                cfg.CreateMap<Customer, CustomerBaseDTO>();
                cfg.CreateMap<Warehouse, WarehouseBaseDTO>();

                cfg.CreateMap<Pickup, PickupBoxDTO>();
            });
        }
    }
}