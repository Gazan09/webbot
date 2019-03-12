using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebCrawller.Models;

namespace WebCrawller.Controllers
{
    public class HomeController : ApiController
    {
        WebData[] webData = new WebData[]
        {
            new WebData { Name = "", Price = "Hammer", Link = "Hardware", Image = "" }
        };
        [System.Web.Http.HttpGet]
        public IEnumerable<WebData> GetAllProducts()
        {
            return webData;
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult GetProduct(string productName)
        {
            var webDatList = webData;
            if (webDatList == null)
            {
                return NotFound();
            }
            return Ok(webDatList);
        }
       
    }
}
