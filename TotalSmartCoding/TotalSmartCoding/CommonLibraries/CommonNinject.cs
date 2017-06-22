using Ninject;
using TotalCore.Repositories;

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
                //NinjectKernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                //NinjectKernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();


                //NinjectKernel.Bind<TotalSalesPortalEntities>().ToSelf().InRequestScope();

                //Kernel.Bind<IBaseRepository>().To<BaseRepository>();
            }
            catch
            {
                Kernel.Dispose();
                throw;
            }
        }
    }
}
