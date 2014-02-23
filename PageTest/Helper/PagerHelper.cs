using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace PageTest.Helper
{
    public static class PagerHelper
    {
        private const string currentPageStr = "page";
        public static MvcHtmlString MvcSimplePager(this HtmlHelper html, int pageSize, int totalCount)
        {
            return MvcSimplePager(html, pageSize, totalCount, false);
        }

        public static MvcHtmlString MvcSimplePager(this HtmlHelper html, int pageSize, int totalCount, bool showNumberLink)
        {
            var queryString = html.ViewContext.HttpContext.Request.QueryString;

            int currentPage = 1;

            var totalpages = Math.Max((totalCount + pageSize - 1) / pageSize, 1);

            var routeValueDict = new RouteValueDictionary(html.ViewContext.RouteData.Values);

            var _renderPager = new StringBuilder();

            if (!string.IsNullOrEmpty(queryString.Get(currentPageStr)))
            {
                foreach (string key in queryString.Keys)
                {
                    if (queryString[key] != null && !string.IsNullOrEmpty(key))
                    {
                        routeValueDict[key] = queryString[key];
                    }
                }
                int.TryParse(queryString[currentPageStr], out currentPage);
            }
            else
            {
                if (routeValueDict.ContainsKey(currentPageStr))
                {
                    int.TryParse(routeValueDict[currentPageStr].ToString(), out currentPage);
                }
            }

            foreach (string key in queryString.Keys)
            {
                routeValueDict[key] = queryString[key];
            }

            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            string emptyAtagFormat = "<a href=\"#\" style=\"cursor:pointer;\">{0}</a>";

            if (totalpages == 1)
            {
                _renderPager.AppendFormat(emptyAtagFormat, "第一頁");
                _renderPager.AppendFormat(emptyAtagFormat, "上一頁");
                _renderPager.AppendFormat(emptyAtagFormat, "下一頁");
                _renderPager.AppendFormat(emptyAtagFormat, "最後一頁");
            }
            else if (totalpages > 1)
            {
                if (currentPage != 1)
                {
                    routeValueDict[currentPageStr] = 1;
                    _renderPager.Append(html.RouteLink("第一頁", routeValueDict));
                }
                else
                {
                    _renderPager.AppendFormat(emptyAtagFormat, "第一頁");
                }

                if (currentPage > 1)
                {
                    routeValueDict[currentPageStr] = currentPage - 1;
                    _renderPager.Append(html.RouteLink("上一頁", routeValueDict));
                }
                else
                {
                    _renderPager.AppendFormat(emptyAtagFormat, "上一頁");
                }
                #region 顯示頁數連結

                if (showNumberLink)
                {
                    var pageCount = (int)Math.Ceiling(totalCount / (double)pageSize);
                    const int nrOfPagesToDispolay = 10;

                    var start = 1;
                    var end = pageCount;

                    if (pageCount > nrOfPagesToDispolay)
                    {
                        var middle = (int)Math.Ceiling(nrOfPagesToDispolay / 2d) - 1;
                        var below = currentPage - middle;
                        var above = currentPage + middle;

                        if (below < 4)
                        {
                            above = nrOfPagesToDispolay;
                            below = 1;
                        }
                        else if (above > (pageCount - 4))
                        {
                            above = pageCount;
                            below = pageCount - nrOfPagesToDispolay;
                        }

                        start = below;
                        end = above;
                    }

                    if (start > 3)
                    {
                        routeValueDict[currentPageStr] = "1";
                        _renderPager.Append(html.RouteLink("1", routeValueDict));

                        routeValueDict[currentPageStr] = "2";
                        _renderPager.Append(html.RouteLink("2", routeValueDict));

                        _renderPager.Append("...");
                    }

                    for (int i = start; i <= end; i++)
                    {
                        if (i == currentPage || (currentPage <= 0 && i == 0))
                        {
                            _renderPager.AppendFormat("<span class=\"current\">{0}</span>", i);
                        }
                        else
                        {
                            routeValueDict[currentPageStr] = i.ToString();
                            _renderPager.Append(html.RouteLink(i.ToString(), routeValueDict));
                        }
                    }

                    if (end < (pageCount - 3))
                    {
                        _renderPager.Append("...");

                        routeValueDict[currentPageStr] = (pageCount - 1).ToString();
                        _renderPager.Append(html.RouteLink((pageCount - 1).ToString(), routeValueDict));

                        routeValueDict[currentPageStr] = pageCount.ToString();
                        _renderPager.Append(html.RouteLink(pageCount.ToString(), routeValueDict));
                    }
                }
                #endregion

                if (currentPage < totalpages)
                {
                    routeValueDict[currentPageStr] = currentPage + 1;
                    _renderPager.Append(html.RouteLink("下一頁", routeValueDict));
                }
                else
                {
                    _renderPager.AppendFormat(emptyAtagFormat, "下一頁");
                }

                if (currentPage != totalpages)
                {
                    routeValueDict[currentPageStr] = totalpages;
                    _renderPager.Append(html.RouteLink("最後一頁", routeValueDict));
                }
                else
                {
                    _renderPager.AppendFormat(emptyAtagFormat, "最後一頁");
                }
            }

            _renderPager.AppendFormat("第 {0} 頁 / 共 {0} 頁 共 {2} 筆", currentPage, totalpages, totalCount);

            return new MvcHtmlString(_renderPager.ToString());
        }

        public static MvcHtmlString MvcSimplePostPager(this HtmlHelper html, int currentPage, int pageSize, int totalCount)
        {
            return MvcSimplePostPager(html, currentPage, pageSize, totalCount, false);
        }

        public static MvcHtmlString MvcSimplePostPager(this HtmlHelper html, int currentPage, int PageSize, int totalCount, bool showNumberLink)
        {
            if (totalCount == 0)
            {
                return new MvcHtmlString(string.Empty);
            }
            else
            {
                currentPage++;

                var totalPages = Math.Max((totalCount + PageSize - 1) / PageSize, 1);
                var _renderPager = new StringBuilder();

                if (currentPage <= 0)
                {
                    currentPage = 1;
                }

                string emptyAtagFormat = "<a href=\"#\" style=\"cursor:pointer;\">{0}</a>";

                if (totalPages == 1)
                {
                    _renderPager.AppendFormat(emptyAtagFormat, "第一頁");
                    _renderPager.AppendFormat(emptyAtagFormat, "上一頁");
                    _renderPager.AppendFormat(emptyAtagFormat, "下一頁");
                    _renderPager.AppendFormat(emptyAtagFormat, "最後一頁");
                }
                else if (totalPages > 1)
                {
                    #region 處理首頁連接

                    if (currentPage != 1)
                    {
                        _renderPager.Append("<a class=\"PostPager first-page\">第一頁</a>");
                    }
                    else
                    {
                        _renderPager.AppendFormat(emptyAtagFormat, "第一頁");
                    }

                    #endregion

                    #region 處理上一頁的連接

                    if (currentPage > 1)
                    {
                        _renderPager.AppendFormat("<a class=\"PostPager previous-page\" value=\"{0}\">上一頁</a>", currentPage - 1);
                    }
                    else
                    {
                        _renderPager.AppendFormat(emptyAtagFormat, "上一頁");
                    }

                    #endregion

                    #region 顯示頁數連結

                    if (showNumberLink)
                    {
                        var pageCount = (int)Math.Ceiling(totalCount / (double)PageSize);
                        const int nrOfPagesToDisplay = 10;

                        var start = 1;
                        var end = pageCount;

                        if (pageCount > nrOfPagesToDisplay)
                        {
                            var middle = (int)Math.Ceiling(nrOfPagesToDisplay / 2d) - 1;
                            var below = currentPage - middle;
                            var above = currentPage + middle;

                            if (below < 4)
                            {
                                above = nrOfPagesToDisplay;
                                below = 1;
                            }
                            else if (above > (pageCount - 4))
                            {
                                above = pageCount;
                                below = pageCount - nrOfPagesToDisplay;
                            }

                            start = below;
                            end = above;
                        }

                        if (start > 3)
                        {
                            _renderPager.AppendFormat("<a class=\"PostPager number-page\" value=\"{0}\">{0}</a>", "1");
                            _renderPager.AppendFormat("<a class=\"PostPager number-page\" value=\"{0}\">{0}</a>", "2");
                            _renderPager.Append("...");
                        }

                        for (int i = start; i <= end; i++)
                        {
                            if (i == currentPage || (currentPage <= 0 && i == 0))
                            {
                                _renderPager.AppendFormat("<span class=\"current\">{0}</span>", i);
                            }
                            else
                            {
                                _renderPager.AppendFormat("<a class=\"PostPager number-page\" value=\"{0}\">{0}</a>", i.ToString());
                            }
                        }

                        if (end < (pageCount - 3))
                        {
                            _renderPager.AppendFormat("...");
                            _renderPager.AppendFormat("<a class=\"PostPager number-page\" value=\"{0}\">{0}</a>", (pageCount - 1).ToString());
                            _renderPager.AppendFormat("<a class=\"PostPager number-page\" value=\"{0}\">{0}</a>", pageCount.ToString());
                        }
                    }

                    #endregion

                    #region 處理下一頁的連結

                    if (currentPage < totalPages)
                    {
                        _renderPager.AppendFormat("<a class=\"PostPager next-page\" value=\"{0}\">下一頁</a>", currentPage + 1);
                    }
                    else
                    {
                        _renderPager.AppendFormat(emptyAtagFormat, "下一頁");
                    }
                    #endregion

                    #region 最後一頁

                    if (currentPage != totalPages)
                    {
                        _renderPager.AppendFormat("<a class=\"PostPager last-page\" value=\"{0}\">最後一頁</a>", totalPages);
                    }
                    else
                    {
                        _renderPager.AppendFormat(emptyAtagFormat, "最後一頁");
                    }

                    #endregion
                }

                // 目前頁數/總頁數
                _renderPager.AppendFormat("  第 {0} 頁  /  共 {1} 頁  共 {2} 筆", currentPage, totalPages, totalCount);

                return MvcHtmlString.Create(_renderPager.ToString());
            }
        }
    }
}