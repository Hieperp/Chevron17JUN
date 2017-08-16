using System.ComponentModel;

using TotalBase;
using TotalDTO;
using TotalBase.Enums;
using TotalCore.Services;

//using TotalSmartCoding.APIs.Sessions;


namespace TotalSmartCoding.Controllers
{
    public class BaseController : CoreController
    {
        private readonly IBaseService baseService;

        public BaseController() : this(null) { } //JUST FOR CREATE AN EMPTY BaseController IN BaseView AT DESIGN TIME ONLY (NOT FUNCTIONAL AT RUN TIME) 

        public BaseController(IBaseService baseService)
        {
            if (baseService != null)
            {
                this.baseService = baseService;
                this.baseService.UserID = ContextAttributes.User.UserID; //(Tamthoi)
            }
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




        protected virtual bool AccessLevelAuthorize()
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





        public virtual void Create() { }
        public virtual void Edit(int? id) { }
        public virtual void CancelDirty(bool withRestore) { }

        public virtual bool Save() { return true; }
        public virtual bool Delete(int id) { return true; }

        public virtual void Approve(int? id) { }
        public virtual bool ApproveConfirmed() { return true; }

        public virtual void Void(int? id) { }
        public virtual bool VoidConfirmed() { return true; }
    }
}