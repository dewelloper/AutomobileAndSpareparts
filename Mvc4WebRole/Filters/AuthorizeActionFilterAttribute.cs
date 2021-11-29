using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


public class AuthorizeActionFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        HttpSessionStateBase session = filterContext.HttpContext.Session;
        Controller controller = filterContext.Controller as Controller;

        if (controller != null)
        {
            if (session["UserIdInSession"] == null)
            {
                filterContext.Result = new HttpNotFoundResult();
                controller.HttpContext.Response.Redirect("../Account/Login");
                return;
            }
        }

        base.OnActionExecuting(filterContext);
    }
}


