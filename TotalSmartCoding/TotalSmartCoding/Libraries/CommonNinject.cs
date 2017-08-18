using Ninject;

using TotalCore.Repositories;
using TotalModel.Models;

//using TotalModel.Models;

//using TotalCore.Repositories;
//using TotalCore.Repositories.Inventories;

//using TotalCore.Services.Inventories;

using TotalDAL.Repositories;
using TotalCore.Repositories.Commons;
using TotalDAL.Repositories.Commons;
using TotalService.Inventories;
using TotalCore.Services.Inventories;
using TotalDAL.Repositories.Inventories;
using TotalCore.Repositories.Inventories;
using TotalSmartCoding.ViewModels.Inventories;
using TotalCore.Services.Productions;
using TotalService.Productions;
using TotalDAL.Repositories.Productions;
using TotalCore.Repositories.Productions;
using TotalDTO.Productions;
using TotalSmartCoding.ViewModels.Productions;
//using TotalDAL.Repositories.Inventories;

//using TotalService.Inventories;


namespace TotalSmartCoding.Libraries
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


                Kernel.Bind<TotalSmartCodingEntities>().ToSelf();// NOW: WE USE DEFAULT: .InTransientScope(); //.InRequestScope()

                Kernel.Bind<IBaseRepository>().To<BaseRepository>();





                Kernel.Bind<IPickupService>().To<PickupService>();
                Kernel.Bind<IPickupRepository>().To<PickupRepository>();
                Kernel.Bind<IPickupAPIRepository>().To<PickupAPIRepository>();
                Kernel.Bind<PickupViewModel>().ToSelf();

                Kernel.Bind<IGoodsReceiptService>().To<GoodsReceiptService>();
                Kernel.Bind<IGoodsReceiptRepository>().To<GoodsReceiptRepository>();
                Kernel.Bind<IGoodsReceiptAPIRepository>().To<GoodsReceiptAPIRepository>();
                Kernel.Bind<GoodsReceiptViewModel>().ToSelf();





                Kernel.Bind<IAspNetUserRepository>().To<AspNetUserRepository>();




                //Kernel.Bind<ICommodityService>().To<CommodityService>();
                Kernel.Bind<ICommodityRepository>().To<CommodityRepository>();
                Kernel.Bind<ICommodityAPIRepository>().To<CommodityAPIRepository>();
                //Kernel.Bind<CommodityViewModel>().ToSelf();


                //Kernel.Bind<IEmployeeService>().To<EmployeeService>();
                Kernel.Bind<IEmployeeRepository>().To<EmployeeRepository>();
                Kernel.Bind<IEmployeeAPIRepository>().To<EmployeeAPIRepository>();
                //Kernel.Bind<EmployeeViewModel>().ToSelf();


                //Kernel.Bind<IWarehouseService>().To<WarehouseService>();
                Kernel.Bind<IWarehouseRepository>().To<WarehouseRepository>();
                Kernel.Bind<IWarehouseAPIRepository>().To<WarehouseAPIRepository>();
                //Kernel.Bind<WarehouseViewModel>().ToSelf();


                //Kernel.Bind<IBinLocationService>().To<BinLocationService>();
                Kernel.Bind<IBinLocationRepository>().To<BinLocationRepository>();
                Kernel.Bind<IBinLocationAPIRepository>().To<BinLocationAPIRepository>();
                //Kernel.Bind<BinLocationViewModel>().ToSelf();


                //Kernel.Bind<IFillingLineService>().To<FillingLineService>();
                Kernel.Bind<IFillingLineRepository>().To<FillingLineRepository>();
                Kernel.Bind<IFillingLineAPIRepository>().To<FillingLineAPIRepository>();
                //Kernel.Bind<FillingLineViewModel>().ToSelf();






                Kernel.Bind<IBatchService>().To<BatchService>();
                Kernel.Bind<IBatchRepository>().To<BatchRepository>();
                Kernel.Bind<IBatchAPIRepository>().To<BatchAPIRepository>();
                Kernel.Bind<BatchViewModel>().ToSelf();







                Kernel.Bind<IFillingPackService>().To<FillingPackService>();
                Kernel.Bind<IFillingPackRepository>().To<FillingPackRepository>();
                Kernel.Bind<FillingPackViewModel>().ToSelf();

                Kernel.Bind<IFillingCartonService>().To<FillingCartonService>();
                Kernel.Bind<IFillingCartonRepository>().To<FillingCartonRepository>();
                Kernel.Bind<FillingCartonViewModel>().ToSelf();

                Kernel.Bind<IFillingPalletService>().To<FillingPalletService>();
                Kernel.Bind<IFillingPalletRepository>().To<FillingPalletRepository>();
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
