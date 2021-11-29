using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;


namespace Mvc4WebRole
{

    public class OtomotivistHttpHandler : IHttpHandler
    {
        public OtomotivistHttpHandler(RequestContext context)
        {
            ProcessRequest(context);
        }

        private static void ProcessRequest(RequestContext requestContext)
        {
            requestContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
            requestContext.HttpContext.Response.Cache.SetMaxAge(TimeSpan.FromSeconds(3600));
            requestContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(3600));

            var response = requestContext.HttpContext.Response;
            var request = requestContext.HttpContext.Request;
            var server = requestContext.HttpContext.Server;
            var validRequestFile = requestContext.RouteData.Values["filename"].ToString();
            const string invalidRequestFile = "~/Content/images/logo.png";
            var path = server.MapPath("~/Images/ProductImages/");

            response.Clear();
            response.ContentType = GetContentType(request.Url.ToString());

            if (request.ServerVariables["HTTP_REFERER"] != null &&
                request.ServerVariables["HTTP_REFERER"].Contains("otomotivist.com"))
            {
                response.TransmitFile(path + validRequestFile);
            }
            else
            {
                response.TransmitFile(invalidRequestFile);
            }
            response.End();
        }

        private static string GetContentType(string url)
        {
            switch (Path.GetExtension(url))
            {
                case ".gif":
                    return "Image/gif";
                case ".jpg":
                    return "Image/jpeg";
                case ".png":
                    return "Image/png";
                default:
                    break;
            }
            return null;
        }

        public void ProcessRequest(HttpContext context)
        {
        }

        public bool IsReusable
        {
            get { return false; }
        }


    }
}