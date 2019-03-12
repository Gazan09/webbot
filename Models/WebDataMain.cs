using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebCrawller.Models
{
    public class WebDataMain
    {
        public List<WebData> GetShopHiveDataAsync(string productName)
        {
            List<WebData> shophive = new List<WebData>();

            try
            {
                var web2 = new HtmlWeb();
                web2.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.137 Safari/537.36";
                web2.BrowserTimeout = TimeSpan.Zero;
                var doc2 = web2.LoadFromBrowser(string.Format("http://www.shophive.com/catalogsearch/result/?cat=0&q={0}", productName), html =>
                {                 
                    // WAIT until the dynamic text is set
                    return !html.Contains("<div class=\"products-grid\"></div>");
                });
                var t1 = doc2.DocumentNode.SelectNodes(".//div[@class='product-block']");

                foreach (HtmlNode ht in t1)
                {
                    shophive.Add(new WebData
                    {
                        Name = ht.SelectNodes(".//div[@class='product-block-inner']/a")[0].Attributes["title"].Value,
                        Link = ht.SelectNodes(".//div[@class='product-block-inner']/a")[0].Attributes["href"].Value,
                        Image = ht.SelectNodes(".//div[@class='product-block-inner']/a/img")[0].Attributes["src"].Value,
                        Price = ht.SelectNodes(".//div[@class='price-box']/span")[0].InnerText
                    });
            }
        }
            catch (Exception ex)
            {
                shophive = null;
            }
            return shophive;                 
        }

        public List<WebData> GetSymbiosDataAsync(string productName)
        {
            List<WebData> symbios = new List<WebData>();
            try
            {
                var web2 = new HtmlWeb();
                web2.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.137 Safari/537.36";
                web2.BrowserTimeout = TimeSpan.Zero;
                var doc2 = web2.LoadFromBrowser(string.Format("https://www.symbios.pk/search.php?search_query={0}", productName), html =>
                {
                    // WAIT until the dynamic text is set
                    return !html.Contains("<div id=\"category-products\"></div>");
                });
                int i = 0;
                foreach (HtmlNode li in doc2.DocumentNode.SelectNodes("//div[contains(@class,'products')]"))
                {
                    symbios.Add(new WebData()
                    {
                        Link = li.SelectNodes(@"//div[@class='product-info text-center']/h3[@class='name']/a")[i].Attributes["href"].Value,
                        Price = li.SelectNodes(@"//div[@class='product-info text-center']/div[@class='product-price']/span[@class='price']")[i].InnerText,
                        Image = li.SelectNodes(@"//div[@class='product-image']/a/img")[i].Attributes["src"].Value,
                        Name = li.SelectNodes(@"//div[@class='product-info text-center']/h3[@class='name']/a")[i].InnerText,
                    });
                    i++;
                }
            }
            catch (Exception ex)
            {
                symbios = null;
            }
            return symbios;
        }
        public List<WebData> GetYayvoDataAsync(string productName)
        {
            List<WebData> yayvo = new List<WebData>();
            try
            {
                var web2 = new HtmlWeb();
                web2.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.137 Safari/537.36";
                web2.BrowserTimeout = TimeSpan.Zero;
                var doc2 = web2.LoadFromBrowser(string.Format("http://yayvo.com/search/result/?q={0}", productName), html =>
                {
                    // WAIT until the dynamic text is set
                    return !html.Contains("<ul class=\"products-grid\"></ul>");
                });

                var liList = doc2.DocumentNode.SelectNodes("//ul[contains(@class, 'products-grid')]");

                foreach (HtmlNode li in liList)
                {
                    yayvo.Add(new WebData()
                    {
                        Link = li.Descendants("a").First().Attributes["href"].Value,
                        Name = li.Descendants("a").First().Attributes["title"].Value,
                        Image = li.Descendants("a").First().Descendants("img").First().Attributes["src"].Value,
                        Price = li.SelectSingleNode(@"//div[@class='price-box']/p[@class='special-price']/span").InnerText
                    });
                }
            }
            catch (Exception ex)
            {
                yayvo = null;
            }
            return yayvo;
        }

        public async Task<List<WebData>> StartDarazCrawlerAsync(string productName)
        {
            List<WebData> daraz = new List<WebData>();
            try
            {
                string albumurl = Uri.EscapeUriString(string.Format("https://www.daraz.pk/catalog/?q={0}", productName));
                string doc = "";
                using (System.Net.WebClient client = new System.Net.WebClient()) // WebClient class inherits IDisposable
                {
                    client.Proxy = null;
                    doc = await client.DownloadStringTaskAsync(new Uri(albumurl));
                }

                var htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(doc);
                var d = htmlDocument.DocumentNode.Descendants().Where(n => n.Name == "script").ToList();

                var pageSizeJson = d[2].InnerHtml;

                var finalJson = d[d.Count - 1].InnerHtml;

                DarazParser message = JsonConvert.DeserializeObject<DarazParser>(finalJson);

                foreach (var a in message.itemListElement)
                {
                    daraz.Add(new WebData
                    {
                        Name = a.name,
                        Image = a.image,
                        Link = a.url,
                        Price = a.offers.price
                    });
                }
            }
            catch (Exception e)
            {
                daraz = null;
            }
            return daraz;
        }
    }
}