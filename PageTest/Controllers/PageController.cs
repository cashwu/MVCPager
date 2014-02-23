using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using PageTest.Models;
using PageTest.Helper;

namespace PageTest.Controllers
{
    public class PageController : Controller
    {
        private readonly NorthwindEntities entity = new NorthwindEntities();

        #region
        //
        // GET: /Page/
        public async Task<ActionResult> Index(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            var result = await this.GetHotSpotData();

            return View(result.ToPagedList(currentPageIndex, 10));
        }

        private async Task<IEnumerable<Models.HotSpot>> GetHotSpotData()
        {
            string cacheName = "WIFI_HOTSPOT";

            ObjectCache cache = MemoryCache.Default;
            CacheItem cacheContents = cache.GetCacheItem(cacheName);

            if (cacheContents == null)
            {
                return await RetriveHotSpotData(cacheName);
            }
            else
            {
                return cacheContents.Value as IEnumerable<Models.HotSpot>;
            }
        }

        private async Task<IEnumerable<Models.HotSpot>> RetriveHotSpotData(string cacheName)
        {
            string targetURI = "http://data.ntpc.gov.tw/NTPC/od/data/api/IMC123/?$format=json";

            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = Int32.MaxValue;
            var response = await client.GetStringAsync(targetURI);

            var collection = JsonConvert.DeserializeObject<IEnumerable<Models.HotSpot>>(response);

            CacheItemPolicy ploicy = new CacheItemPolicy();
            ploicy.AbsoluteExpiration = DateTime.Now.AddMinutes(30);

            ObjectCache cacheItem = MemoryCache.Default;
            cacheItem.Add(cacheName, collection, ploicy);

            return collection;
        }

        #endregion

        #region PageMethod3

        public ActionResult PageMethod3(int? page)
        {
            ViewData["CategoryDDL"] = this.GenerateCategoryDDL(null);
            return View();
        }

        public ActionResult PageContent(int? page, string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
            {
                return View();
            }
            else
            {
                int id = int.Parse(categoryId);
                var result = this.entity.Products.Where(a => a.CategoryID == id).ToList();
                int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
                
                ViewData["CurrentPage"] = currentPageIndex;
                var dataPaged = result.ToPagedList(currentPageIndex, 5);

                return View(dataPaged);
            }
        }

        [HttpPost]
        public ActionResult PageMethod3(int? page, FormCollection formCollection)
        {
            ViewData["CategoryDDL"] = this.GenerateCategoryDDL(formCollection["CategoryDDL"] ?? string.Empty);

            if (formCollection.AllKeys.Length.Equals(0))
            {
                return View();
            }
            else
            {
                int check = 0;
                if (!int.TryParse(formCollection["CategoryDDL"], out check))
                {
                    return View();
                }
                else
                {
                    int categoryId = int.Parse(formCollection["CategoryDDL"]);
                    var result = this.entity.Products.Where(a => a.CategoryID == categoryId).ToList();

                    int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
                    ViewData["CurrentPage"] = currentPageIndex;

                    return View(result.ToPagedList(currentPageIndex, 5));
                }
            }
        }

        private MvcHtmlString GenerateCategoryDDL(string selectValue)
        {
            string tagIdName = "CategoryDDL";

            Dictionary<string, string> optionData = new Dictionary<string, string>();

            var query = this.entity.Categories.ToList();
            foreach (var item in query)
            {
                if (optionData.Where(a => a.Key.Equals(item.CategoryName.ToString())).Count().Equals(0))
                {
                    optionData.Add(item.CategoryName.ToString(), item.CategoryID.ToString());
                }
            }

            var html = DropDownListHelper.GetDropdownList(tagIdName, tagIdName, optionData, null, selectValue, true, null);

            return html;
        }

        #endregion
    }
}