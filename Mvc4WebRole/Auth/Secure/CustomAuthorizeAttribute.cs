using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace Otomotivist.Auth.Secure
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string UsersConfigKey { get; set; }
        public string RolesConfigKey { get; set; }

        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        public List<string> CurrentUserRoles
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["_currentUserRoles"] != null)
                    return System.Web.HttpContext.Current.Session["_currentUserRoles"] as List<string>;
                return null;
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //filterContext.Result = new JsonResult
                //{
                //    Data = new { success = false, error = "Bu işlem için yeterli yetkilere sahip değilsiniz!." },
                //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                //};
            }

            

        }

        //protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        //{
        //    if (context.HttpContext.Request.IsAuthenticated)
        //    {
        //        context.Result = new ViewResult
        //        {
        //            ViewName = "~/Views/Shared/Forbidden.cshtml"
        //        };
        //    }
        //    else
        //    {
        //        base.HandleUnauthorizedRequest(context);
        //    }
        //}


    }
}