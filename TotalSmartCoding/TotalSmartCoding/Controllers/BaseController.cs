using TotalBase;
using TotalBase.Enums;
using TotalCore.Services;
//using TotalSmartCoding.APIs.Sessions;


namespace TotalSmartCoding.Controllers
{
    public abstract class BaseController : CoreController
    {
        private readonly IBaseService baseService;
        public BaseController(IBaseService baseService)
        { 
            this.baseService = baseService;
            this.baseService.UserID = ContextAttributes.User.UserID; //(Tamthoi)
        }


        public IBaseService BaseService { get { return this.baseService; } }



        public virtual void AddRequireJsOptions()
        {
            //int nmvnModuleID = this.baseService.NmvnModuleID;
            //MenuSession.SetModuleID(this.HttpContext, nmvnModuleID);

            //RequireJsOptions.Add("LocationID", this.baseService.LocationID, RequireJsOptionsScope.Page);
            //RequireJsOptions.Add("NmvnModuleID", nmvnModuleID, RequireJsOptionsScope.Page);
            //RequireJsOptions.Add("NmvnTaskID", this.baseService.NmvnTaskID, RequireJsOptionsScope.Page);
        }




        protected virtual bool AccessLevelAuthorize ()
        {
            return this.AccessLevelAuthorize(GlobalEnums.AccessLevel.Editable);
        }

        protected virtual bool AccessLevelAuthorize(GlobalEnums.AccessLevel accessLevel)
        {
            return this.BaseService.GetAccessLevel() >= accessLevel;
        }
























        public virtual void Open(int? id)
        {
        }

    }
}