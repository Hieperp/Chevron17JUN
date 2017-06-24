﻿using Ninject;

using TotalCore.Repositories;
using TotalModel.Models;

using TotalModel.Models;

using TotalCore.Repositories;
//using TotalCore.Repositories.Inventories;

//using TotalCore.Services.Inventories;

using TotalDAL.Repositories;
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


                Kernel.Bind<TotalSmartCodingEntities>().ToSelf(); //.InRequestScope()

                Kernel.Bind<IBaseRepository>().To<BaseRepository>();
            }
            catch
            {
                Kernel.Dispose();
                throw;
            }
        }
    }
}
