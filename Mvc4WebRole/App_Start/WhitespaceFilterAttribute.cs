using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc4WebRole.App_Start
{
    public class WhitespaceFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            if (response.ContentType != "text/html" || response.Filter == null) return;

            response.Filter = new WhitespaceStream(response.Filter);
        }

    }
}