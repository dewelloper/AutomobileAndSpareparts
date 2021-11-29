using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Mvc4WebRole.rss
{
    public class SyndicationFeedResult : ContentResult
    {
        public SyndicationFeedResult(SyndicationFeed feed)
        {
            using (var memstream = new MemoryStream())
            using (var writer = new XmlTextWriter(memstream, System.Text.Encoding.UTF8))
            {
                feed.SaveAsRss20(writer);
                writer.Flush();
                memstream.Position = 0;
                Content = new StreamReader(memstream).ReadToEnd();
                ContentType = "application/xml";
            }


        }

    }
}