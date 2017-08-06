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
        private readonly BaseDTO baseDTO;

        public BaseController() : this(null, null) { } //JUST FOR CREATE AN EMPTY BaseController IN BaseView AT DESIGN TIME ONLY (NOT FUNCTIONAL AT RUN TIME) 

        public BaseController(IBaseService baseService, BaseDTO baseDTO)
        {
            if (baseService != null && baseDTO != null)
            {
                this.baseService = baseService;
                this.baseDTO = baseDTO;

                this.baseDTO.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(baseDTO_PropertyChanged);

                this.baseService.UserID = ContextAttributes.User.UserID; //(Tamthoi)
            }
            else
                this.baseDTO = new BaseDTO(); //JUST FOR CREATE AN EMPTY BaseController IN BaseView AT DESIGN TIME ONLY (NOT FUNCTIONAL AT RUN TIME) 
        }


        public IBaseService BaseService { get { return this.baseService; } }
        public BaseDTO BaseDTO { get { return this.baseDTO; } }


        protected virtual void baseDTO_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SetDirty();  //SHOULD USE this.SetDirty(); INSTEAD OF FORWARD EVENT BY USING: this.NotifyPropertyChanged(e.PropertyName); BECAUSE: IF e.PropertyName NOT FOUND IN THE RECEIVED OBJECT (HERE IS BaseController OBJECT): IT WILL NOT ADD TO Changes Dictionary => SO THE OBJECT IS NOT Dirty
        }

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

        public virtual void Create()
        {
        }

        public virtual void Edit(int? id)
        {
        }
    }
}