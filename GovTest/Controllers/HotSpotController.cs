using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PagedList;
using GovTest.Models;
using System.Text;
using System.Web.Routing;

namespace GovTest.Controllers
{
    public class HotSpotController : Controller
    {
        //
        // GET: /HotSpot/
        public async Task<ActionResult> Index(int? page, string districts, string types, string companys)
        {
            var districtsList = this.GetSelectList(await this.GetDistricts(), districts);
            ViewBag.SelectedDistrict = districts;

            var dict = new Dictionary<string, string>();
            districtsList.ToList().ForEach(a => dict.Add(a.Text, a.Value));

            var districtsDDL = GetDropdownList("Districts",
                dict,
                new { id = "Districts", @class = "form-control" },
                districts,
                true,
                "Please select");

            ViewBag.Districts = districtsDDL;

            var typeSelectList = this.GetSelectList(await this.GetHotSpotTyptes(), types);
            ViewBag.Types = typeSelectList.ToList();
            ViewBag.SelectedType = types;

            var companySelectList = this.GetSelectList(await this.GetCompanys(), companys);
            ViewBag.Companys = companySelectList.ToList();
            ViewBag.SelectedCompany = companys;

            var source = await this.GetHotSpotData();
            source = source.AsQueryable();

            if (!string.IsNullOrWhiteSpace(districts))
            {
                source = source.Where(a => a.District == districts);
            }
            if (!string.IsNullOrWhiteSpace(types))
            {
                source = source.Where(a => a.Type == types);
            }
            if (!string.IsNullOrWhiteSpace(companys))
            {
                source = source.Where(a => a.Company == companys);
            }

            int pageIndex = page ?? 1;
            int pageSize = 10;
            int totalCount = 0;

            totalCount = source.Count();

            source = source.OrderBy(a => a.District)
                           .Skip((pageIndex - 1) * pageSize)
                           .Take(pageSize);

            var pagedResult = new StaticPagedList<Models.HotSpot>(source, pageIndex, pageSize, totalCount);

            var district = await this.GetDistricts();
            List<SelectListItem> infos = new List<SelectListItem>();
            int index = 1;
            district.ForEach(item =>
            {
                infos.Add(new SelectListItem()
                {
                    Text = item + "-" + index,
                    Value = item
                });
                index++;
            });

            ViewBag.List = infos;

            return View(pagedResult);
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

        private async Task<List<string>> GetDistricts()
        {
            var source = await this.GetHotSpotData();
            if (source != null)
            {
                var districts = source.OrderBy(a => a.District)
                                      .Select(a => a.District)
                                      .Distinct();

                return districts.ToList();
            }

            return new List<string>();
        }

        private async Task<List<string>> GetHotSpotTyptes()
        {
            var source = await this.GetHotSpotData();
            if (source != null)
            {
                var types = source.OrderBy(a => a.Type)
                                  .Select(a => a.Type)
                                  .Distinct();

                return types.ToList();
            }

            return new List<string>();
        }

        private async Task<List<string>> GetCompanys()
        {
            var source = await this.GetHotSpotData();
            if (source != null)
            {
                var companys = source.OrderBy(a => a.Company)
                                     .Select(a => a.Company)
                                     .Distinct();

                return companys.ToList();
            }

            return new List<string>();
        }

        private IEnumerable<SelectListItem> GetSelectList(
            IEnumerable<string> source,
            string selectedItem)
        {
            var selectList = source.Select(item => new SelectListItem()
                {
                    Text = item,
                    Value = item,
                    Selected = !string.IsNullOrWhiteSpace(selectedItem)
                               && item.Equals(selectedItem, StringComparison.OrdinalIgnoreCase)
                });

            return selectList;
        }

        private static MvcHtmlString GetDropdownList(string name,
            IDictionary<string, string> optionData, 
            object htmlAttributes, 
            string defaultSelectValue, 
            bool appendOptionLabel, 
            string optionLabel)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException();
            }

            TagBuilder select = new TagBuilder("select");
            select.Attributes.Add("name", name);

            StringBuilder renderHtmlTag = new StringBuilder();
            IDictionary<string, string> newOptionData = new Dictionary<string, string>();
            if (appendOptionLabel)
            {
                newOptionData.Add(new KeyValuePair<string, string>(optionLabel ?? "請選擇", ""));
            }

            foreach (var item in optionData)
            {
                newOptionData.Add(item);
            }

            foreach (var option in newOptionData)
            {
                TagBuilder optionTag = new TagBuilder("option");
                optionTag.Attributes.Add("value", option.Value);
                if (!string.IsNullOrEmpty(defaultSelectValue) 
                    && string.Equals(defaultSelectValue, option.Value, StringComparison.OrdinalIgnoreCase))
                {
                    optionTag.Attributes.Add("selected", "selected");
                }

                optionTag.SetInnerText(option.Key);
                renderHtmlTag.AppendLine(optionTag.ToString(TagRenderMode.Normal));
            }

            select.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            select.InnerHtml = renderHtmlTag.ToString();

            return new MvcHtmlString(select.ToString(TagRenderMode.Normal));
        }
	}
}