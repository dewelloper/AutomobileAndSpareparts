using Mvc4WebRole.rss;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Mvc4WebRole.Controllers
{

    
    public class SitemapController : Controller
    {
        public ActionResult Generate() { 
            var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes(); 
            var targets = new List<Type>(); 
            foreach (var t in types) 
            { 
                if (t.IsSubclassOf(typeof(Controller)) && t != typeof(SitemapController)) 
                    targets.Add(t); 
            }  
            var sitemap = new XDocument(); 
            var ns = XNamespace.Get("http://www.sitemaps.org/schemas/sitemap/0.9"); 
            var root = new XElement(ns + "urlset");  
            foreach (var t in targets) 
            { 
                var pos = t.Name.IndexOf("Controller", StringComparison.InvariantCultureIgnoreCase); 
                var controller = t.Name.Substring(0, pos); 
                foreach (var action in t.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly)) 
                { 
                    // don't include POST methods 
                    if (action.GetCustomAttributes(typeof(HttpPostAttribute), false).Count() > 0) 
                        continue;  
                    var node = new XElement(ns + "url" , new XElement(ns + "loc", Request.Url.GetLeftPart(UriPartial.Authority) + "/" + controller + "/" + action.Name) , new XElement(ns + "lastmod", DateTime.Today.ToString("yyyy-MM-dd")) , new XElement(ns + "changefreq", "always") , new XElement(ns + "priority", "1")); 
                    root.Add(node); 
                } 
            }  
            sitemap.Add(root); 
            var stream = new MemoryStream(); 
            var writer = new StreamWriter(stream, Encoding.UTF8); 
            sitemap.Save(writer); 
            stream.Seek(0, SeekOrigin.Begin);  
            return File(stream, "text/xml"); 
        }

        public SyndicationFeedResult rss()
        {
            var feed = Syndiction.CreateFeed();
            return new SyndicationFeedResult(feed);
        }
    }



}