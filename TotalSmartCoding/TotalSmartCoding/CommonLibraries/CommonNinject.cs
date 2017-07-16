﻿using Ninject;

using TotalCore.Repositories;
using TotalModel.Models;

//using TotalModel.Models;

//using TotalCore.Repositories;
//using TotalCore.Repositories.Inventories;

//using TotalCore.Services.Inventories;

using TotalDAL.Repositories;
using TotalDAL.Repositories.Sales;
using TotalCore.Repositories.Sales;
using TotalCore.Services.Sales;
using TotalService.Sales;
using TotalSmartCoding.Builders.Sales;
using TotalSmartCoding.Builders.Commons;
using TotalCore.Repositories.Commons;
using TotalDAL.Repositories.Commons;
using TotalService.Inventories;
using TotalCore.Services.Inventories;
using TotalDAL.Repositories.Inventories;
using TotalCore.Repositories.Inventories;
using TotalSmartCoding.Builders.Inventories;
using TotalSmartCoding.ViewModels.Inventories;
using TotalSmartCoding.ViewModels.Sales;
using TotalCore.Services.Productions;
using TotalService.Productions;
using TotalDAL.Repositories.Productions;
using TotalCore.Repositories.Productions;
using TotalDTO.Productions;
using TotalSmartCoding.Builders.Productions;
using TotalSmartCoding.ViewModels.Productions;
//using TotalDAL.Repositories.Inventories;

//using TotalService.Inventories;

//using TotalSmartCoding.Areas.Inventories.Builders;
//using TotalSmartCoding.Areas.Commons.Builders;

namespace TotalSmartCoding.CommonLibraries
{
    public static class CommonNinject
    {
        public static readonly IKernel Kernel;
        
        /// <summary>
        ///  static constructor NinjectCommon is called automatically before the first instance is created or any static members are referenced
        /// </summary>
        static CommonNinject()
        {
            Kernel = new StandardKernel();
            try
            {
                //Kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                //Kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();


                Kernel.Bind<TotalSmartCodingEntities>().ToSelf().InSingletonScope(); //.InRequestScope()

                Kernel.Bind<IBaseRepository>().To<BaseRepository>();



                Kernel.Bind<IDeliveryAdviceService>().To<DeliveryAdviceService>();
                Kernel.Bind<IDeliveryAdviceRepository>().To<DeliveryAdviceRepository>();
                Kernel.Bind<IDeliveryAdviceAPIRepository>().To<DeliveryAdviceAPIRepository>();
                Kernel.Bind<IDeliveryAdviceViewModelSelectListBuilder>().To<DeliveryAdviceViewModelSelectListBuilder>();
                Kernel.Bind<DeliveryAdviceViewModel>().ToSelf();
                


                Kernel.Bind<IGoodsReceiptService>().To<GoodsReceiptService>();
                Kernel.Bind<IGoodsReceiptRepository>().To<GoodsReceiptRepository>();
                Kernel.Bind<IGoodsReceiptAPIRepository>().To<GoodsReceiptAPIRepository>();
                Kernel.Bind<IGoodsReceiptViewModelSelectListBuilder>().To<GoodsReceiptViewModelSelectListBuilder>();
                Kernel.Bind<GoodsReceiptViewModel>().ToSelf();


                

                Kernel.Bind<IAspNetUserSelectListBuilder>().To<AspNetUserSelectListBuilder>();
                Kernel.Bind<IPaymentTermSelectListBuilder>().To<PaymentTermSelectListBuilder>();


                Kernel.Bind<IAspNetUserRepository>().To<AspNetUserRepository>();
                Kernel.Bind<IPaymentTermRepository>().To<PaymentTermRepository>();









                Kernel.Bind<IBatchRepository>().To<BatchRepository>();
                Kernel.Bind<IBatchAPIRepository>().To<BatchAPIRepository>();




                Kernel.Bind<IFillingPackService>().To<FillingPackService>();
                Kernel.Bind<IFillingPackRepository>().To<FillingPackRepository>();
                Kernel.Bind<IFillingPackViewModelSelectListBuilder>().To<FillingPackViewModelSelectListBuilder>();
                Kernel.Bind<FillingPackViewModel>().ToSelf();

                Kernel.Bind<IFillingCartonService>().To<FillingCartonService>();
                Kernel.Bind<IFillingCartonRepository>().To<FillingCartonRepository>();
                Kernel.Bind<IFillingCartonViewModelSelectListBuilder>().To<FillingCartonViewModelSelectListBuilder>();
                Kernel.Bind<FillingCartonViewModel>().ToSelf();

                Kernel.Bind<IFillingPalletService>().To<FillingPalletService>();
                Kernel.Bind<IFillingPalletRepository>().To<FillingPalletRepository>();
                Kernel.Bind<IFillingPalletViewModelSelectListBuilder>().To<FillingPalletViewModelSelectListBuilder>();
                Kernel.Bind<FillingPalletViewModel>().ToSelf();



                Kernel.Bind<FillingData>().ToSelf();




            }
            catch
            {
                Kernel.Dispose();
                throw;
            }
        }
    }
}
