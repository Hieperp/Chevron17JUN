using System.Linq;

using TotalBase.Enums;

namespace TotalSmartCoding.Controllers
{
    /// <summary>
    /// THIS CHECK AccessLevel BEFORE EXECUTE ACTION METHOD
    /// AT THIS WINFORM PROJECT: WE REMOVE THIS METHOD => WE HAVE TO CALL if (this.AccessLevelAuthorize()) throw new System.ArgumentException("Lỗi phân quyền", "Không có quyền truy cập dữ liệu"); MANUALLY
    /// SEE HOW TO CALL if (this.AccessLevelAuthorize()) throw new System.ArgumentException("Lỗi phân quyền", "Không có quyền truy cập dữ liệu"); IN GenericSimpleController FOR MORE INFORMATION
    /// </summary>
    //public class AccessLevelAuthorizeAttribute : AuthorizeAttribute
    //{
    //    private BaseController baseController;

    //    private GlobalEnums.AccessLevel accessLevel;

    //    public AccessLevelAuthorizeAttribute()
    //        : this(GlobalEnums.AccessLevel.Editable)
    //    { }

    //    public AccessLevelAuthorizeAttribute(GlobalEnums.AccessLevel accessLevel)
    //    {
    //        this.accessLevel = accessLevel;
    //    }

    //    public override void OnAuthorization(AuthorizationContext filterContext)
    //    {
    //        this.baseController = filterContext.Controller as BaseController;

    //        base.OnAuthorization(filterContext);
    //    }

    //    protected override bool AuthorizeCore(HttpContextBase httpContext)
    //    {
    //        var authorized = base.AuthorizeCore(httpContext);
    //        if (!authorized) return false;

    //        return this.baseController.BaseService.GetAccessLevel() >= this.accessLevel;
    //    }
    //}




    /// <summary>
    /// THIS ATTRIBUTE: TO EXECUTE AddRequireJsOptions AT EVERY NEEDED METHOD
    /// AT THIS WINFORM PROJECT: WE REMOVE THIS METHOD => WE HAVE TO CALL base.AddRequireJsOptions(); MANUALLY
    /// SEE HOW TO CALL base.AddRequireJsOptions(); IN GenericSimpleController FOR MORE INFORMATION
    /// </summary>
    //public class OnResultExecutingFilterAttribute : ActionFilterAttribute
    //{
    //    public override void OnResultExecuting(ResultExecutingContext filterContext)
    //    {
    //        base.OnResultExecuting(filterContext);

    //        if (filterContext.Result is ViewResult)
    //        {
    //            var controller = filterContext.Controller as BaseController;
    //            controller.AddRequireJsOptions();
    //        }
    //    }
    //}







    /// <summary>
    /// NO NEED TO EXPORT/ IMPORT ModelState IN WINFORM
    /// </summary>
    //public abstract class ModelStateTempDataTransfer : ActionFilterAttribute
    //{
    //    protected static readonly string Key = typeof(ModelStateTempDataTransfer).FullName;
    //}

    //public class ExportModelStateToTempData : ModelStateTempDataTransfer
    //{
    //    public override void OnActionExecuted(ActionExecutedContext filterContext)
    //    {
    //        //Only export when ModelState is not valid
    //        if (!filterContext.Controller.ViewData.ModelState.IsValid)
    //        {
    //            //Export if we are redirecting
    //            if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
    //            {
    //                filterContext.Controller.TempData[Key] = filterContext.Controller.ViewData.ModelState;
    //            }
    //        }

    //        base.OnActionExecuted(filterContext);
    //    }
    //}

    //public class ImportModelStateFromTempData : ModelStateTempDataTransfer
    //{
    //    public override void OnActionExecuted(ActionExecutedContext filterContext)
    //    {
    //        ModelStateDictionary modelState = filterContext.Controller.TempData[Key] as ModelStateDictionary;

    //        if (modelState != null)
    //        {
    //            //Only Import if we are viewing
    //            if (filterContext.Result is ViewResult)
    //            {
    //                filterContext.Controller.ViewData.ModelState.Merge(modelState);
    //            }
    //            else
    //            {
    //                //Otherwise remove it.
    //                filterContext.Controller.TempData.Remove(Key);
    //            }
    //        }

    //        base.OnActionExecuted(filterContext);
    //    }
    //}




}