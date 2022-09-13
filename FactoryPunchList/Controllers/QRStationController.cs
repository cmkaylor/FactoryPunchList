using FactoryPunchList.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FactoryPunchList.Controllers
{
    public class QRStationController : Controller
    {
        private Authenticate authenticate = new Authenticate();
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (authenticate.CookieExists(Request))
            {
                base.OnActionExecuted(filterContext);
            }

            else
            {
                filterContext.Result = RedirectToAction("Signin", "Account");
                base.OnActionExecuted(filterContext);
            }
        }

        // redirect user to qr code based off punchListID, so they can easily right click and print
        public ActionResult GenerateQR(int id)
        {
            string url = GenerateQRCode.GetCodeUrl(id);
            return Redirect(url);
        }
    }
}