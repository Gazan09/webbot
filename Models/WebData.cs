using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCrawller.Models
{
    public class WebData
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Price { get; set; }
    }
    public class WebDataCollection
    {
        public List<WebData> YavooList {get;set;}
        public List<WebData> symbiosList {get;set;}
        public List<WebData> shopHiveList {get;set;}
        public List<WebData> DarazList { get; set; }
    }
    public class ItemListElement
    {
        public Offers offers { get; set; }
        public string image { get; set; }
        public string @type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Offers
    {
        public string priceCurrency { get; set; }
        public string @type { get; set; }
        public string price { get; set; }
        public string availability { get; set; }
    }

    public class DarazParser
    {
        public string @context { get; set; }
        public string @type { get; set; }
        public IList<ItemListElement> itemListElement { get; set; }
    }
}