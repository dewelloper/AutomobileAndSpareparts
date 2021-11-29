using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Http.Routing;

namespace Mvc4WebRole.rss
{
    public class Syndiction
    {

        public static SyndicationFeed CreateFeed()
        {
            Entities _entity = new Entities();

            var feed = new SyndicationFeed("Başlık", "Açıklama", new Uri("http://otomotivist.com"));
            feed.Categories.Add(new SyndicationCategory("Otomotivist Blog"));
            feed.Language = "tr-TR";
            feed.Copyright = new TextSyndicationContent(
            String.Format("{0} {1}", DateTime.Now.Year, "Oto Editör"));
            feed.LastUpdatedTime = DateTime.Now;
            feed.Authors.Add(
            new SyndicationPerson
            {
                Name = "Oto Editör"
            });
            var feedItems = new List<SyndicationItem>();

            //Url.Action("aracozellikleri", "otomobilvasita", new { prdcId = @t.Id, GroupId = @t.GroupId, CategoryId = @t.CategoryId, PriceCurrencyId = @t.PriceCurrencyId, Name = Mvc4WebRole.Controllers.StringHelpers.ToSeoUrl(@t.Name)})

            //Makaleleler içinde dönüyoruz.
             foreach (var item in _entity.Products.Where(k => k.ProductType == 1).OrderByDescending(m => m.Id).Take(25))
            {
                Uri urri = new Uri(@"http://otomotivist/otomobilvasita/aracozellikleri/"+ item.Name.Replace(" ","-").Replace(".","-") +"/"+ item.GroupId +"/di-er/"+item.Category_Id+"/"+item.MarkId+"/"+item.Id);
                var sItem =
                new SyndicationItem(
                item.Name, // Makale Başlığı
                null,
                new Uri(urri.ToString())) //Makale URL'si
                {
                    Summary = new TextSyndicationContent(item.Explanation),
                    PublishDate = item.RecordDate //Makalenin oluşturulma tarihi
                };
                sItem.Links.Add(
                new SyndicationLink
                {
                    Title = item.Name,
                    Uri = urri,
                    Length = item.Explanation.Length,
                    MediaType = "html"
                });
                feedItems.Add(sItem);
            }
            feed.Items = feedItems;

            return feed;
        } 
    }
}