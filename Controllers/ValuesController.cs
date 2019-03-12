using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using WebCrawller.Models;

namespace WebCrawller.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values/5
        [HttpGet]
        [ActionName("GetAsync")]
        public async Task<WebDataCollection> GetAsync(string productName)
        {
            WebDataCollection webDataCollection = new WebDataCollection();
            WebDataMain webDataMain = new WebDataMain();

            if (string.IsNullOrEmpty(productName))
            {
                return webDataCollection;
            }
            else
            {
                Thread th1 = new Thread(() => webDataCollection.shopHiveList = webDataMain.GetShopHiveDataAsync(productName));
                Thread th2 = new Thread(() => webDataCollection.symbiosList = webDataMain.GetSymbiosDataAsync(productName));
                Thread th3 = new Thread(() => webDataCollection.YavooList = webDataMain.GetYayvoDataAsync(productName));

                th1.SetApartmentState(ApartmentState.STA);
                th2.SetApartmentState(ApartmentState.STA);
                th3.SetApartmentState(ApartmentState.STA);
                th1.Start();
                th2.Start();
                th3.Start();

                th1.Join();
                th2.Join();
                th3.Join();

                webDataCollection.DarazList = await webDataMain.StartDarazCrawlerAsync(productName);

               // Task t1 = Task.Factory.StartNew(() => webDataCollection.shopHiveList = webDataMain.GetShopHiveDataAsync(productName));
               // Task t2 = Task.Factory.StartNew(() => webDataCollection.symbiosList = webDataMain.GetSymbiosDataAsync(productName));
               // Task t3 = Task.Factory.StartNew(() => webDataCollection.YavooList = webDataMain.GetYayvoDataAsync(productName));


               // Task.WaitAll(t1, t2, t3);
                return webDataCollection;


                // th2.SetApartmentState(ApartmentState.STA);
                // th3.SetApartmentState(ApartmentState.STA);

                //Task t1 = Task.Run(() => th1.Start());
                //Task t2 = Task.Run(() => th2.Start());
                // Task t3 = Task.Run(() => th3.Start());
                //t1.Wait();

                //  await Task.WhenAll(t1, t2, t3);
            }
        }

        [HttpGet]
        [ActionName("GetShopHiveListAsync")]
        public async Task<WebDataCollection> GetShopHiveListAsync(string productName)
        {
            WebDataCollection webDataCollection = new WebDataCollection();
            WebDataMain webDataMain = new WebDataMain();

            if (string.IsNullOrEmpty(productName))
            {
                return webDataCollection;
            }
            else
            {
                Thread th1 = new Thread(() => webDataCollection.shopHiveList = webDataMain.GetShopHiveDataAsync(productName));
                
                th1.SetApartmentState(ApartmentState.STA);                
                th1.Start();                
                th1.Join();                
                return webDataCollection;                
            }
        }

        [HttpGet]
        [ActionName("GetSymbiosListAsync")]
        public async Task<WebDataCollection> GetSymbiosListAsync(string productName)
        {
            WebDataCollection webDataCollection = new WebDataCollection();
            WebDataMain webDataMain = new WebDataMain();

            if (string.IsNullOrEmpty(productName))
            {
                return webDataCollection;
            }
            else
            {
                Thread th1 = new Thread(() => webDataCollection.symbiosList = webDataMain.GetSymbiosDataAsync(productName));

                th1.SetApartmentState(ApartmentState.STA);
                th1.Start();
                th1.Join();
                return webDataCollection;
            }
        }

        [HttpGet]
        [ActionName("GetYavyooListAsync")]
        public async Task<WebDataCollection> GetYavyooListAsync(string productName)
        {
            WebDataCollection webDataCollection = new WebDataCollection();
            WebDataMain webDataMain = new WebDataMain();

            if (string.IsNullOrEmpty(productName))
            {
                return webDataCollection;
            }
            else
            {
                Thread th1 = new Thread(() => webDataCollection.YavooList = webDataMain.GetYayvoDataAsync(productName));

                th1.SetApartmentState(ApartmentState.STA);
                th1.Start();
                th1.Join();
                return webDataCollection;
            }
        }

        [HttpGet]
        [ActionName("GetDarazListAsync")]
        public async Task<WebDataCollection> GetDarazListAsync(string productName)
        {
            WebDataCollection webDataCollection = new WebDataCollection();
            WebDataMain webDataMain = new WebDataMain();

            if (string.IsNullOrEmpty(productName))
            {
                return webDataCollection;
            }
            else
            {
                webDataCollection.DarazList = await webDataMain.StartDarazCrawlerAsync(productName);                
                return webDataCollection;
            }
        }

        public static Task StartSTATask(Action func)
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {
                try
                {
                    func();
                    tcs.SetResult(null);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        // POST api/values
        public WebDataCollection Post([FromBody]string productName)
        {
            WebDataCollection webDataCollection = new WebDataCollection();
            WebDataMain webDataMain = new WebDataMain();

            if (string.IsNullOrEmpty(productName))
            {
                return webDataCollection;
            }
            else
            {
                Parallel.Invoke(() =>
                {
                    webDataCollection.shopHiveList = webDataMain.GetShopHiveDataAsync(productName);
                },
                () =>
                {
                    webDataCollection.shopHiveList = webDataMain.GetSymbiosDataAsync(productName);
                },
                () =>
                {
                    webDataCollection.shopHiveList = webDataMain.GetYayvoDataAsync(productName);
                }
                    );
                return webDataCollection;
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
