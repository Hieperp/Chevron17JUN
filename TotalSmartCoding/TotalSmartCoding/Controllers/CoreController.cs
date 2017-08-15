using System.Net;
using System.Text;
using System.Collections.Generic;

using TotalModel.Helpers;
using TotalDTO.Commons;
using TotalBase.Enums;
using TotalSmartCoding.Configurations;


namespace TotalSmartCoding.Controllers
{
    public class CoreController //: Controller
    {
        public Dictionary<string, object> TempData { get; private set; }
        public Dictionary<string, object> ViewData { get; private set; }

        public ModelState ModelState { get; set; }

        public CoreController()
        {
            TempData = new Dictionary<string, object>();
            ViewData = new Dictionary<string, object>();

            ModelState = new ModelState();
        }

        //THIS WILL INIT var SubmitTypeOption + var SettingsManager CHO WEB BROWSER
        //public ActionResult GlobalJavaScriptEnums()
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    stringBuilder.Append("var SubmitTypeOption = " + typeof(GlobalEnums.SubmitTypeOption).EnumToJson() + "; ");
        //    stringBuilder.Append("var SettingsManager = " + System.Web.Helpers.Json.Encode(new MySettingsManager()) + "; ");

        //    return JavaScript(stringBuilder.ToString());
        //}

        public void RedirectToAction(string actionName)
        { }
        public void RedirectToAction(string actionName, object id)
        { }
        public void RedirectToAction(string actionName, object id, object myObject)
        { }
    }

    public class ModelState
    {
        public bool IsValid { get { return true; } set { ;} }
        public void Clear() { }
    }

    public class HttpStatusCodeResult
    {
        public HttpStatusCodeResult(HttpStatusCode httpStatusCode)
        {

        }
    }
}