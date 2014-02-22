using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GovTest.Helpers
{
    public static class RadioButtonListExtensions
    {
        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listInfo)
        {
            return htmlHelper.RadioButtonList(name, listInfo, (IDictionary<string, object>)null, 0);
        }

        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listInfo,
            object htmlAttributes)
        {
            return htmlHelper.RadioButtonList(name, listInfo, (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes), 0);
        }

        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlhelper,
            string name,
            IEnumerable<SelectListItem> listInfo,
            IDictionary<string, object> htmlAttributes,
            int number)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException();
            }

            if (listInfo == null)
            {
                throw new ArgumentNullException();
            }

            if (listInfo.Count() < 0)
            {
                throw new ArgumentException();
            }

            StringBuilder sb = new StringBuilder();
            int lineNumber = 0;

            foreach (SelectListItem info in listInfo)
            {
                lineNumber++;

                sb.Append(CreateTag(info, name, htmlAttributes).ToString());

                if (number == 0 || (lineNumber % number == 0))
                {
                    sb.Append("<br />");
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString RadioButtonListVertical(this HtmlHelper htmlhelper,
            string name,
            IEnumerable<SelectListItem> listInfo,
            IDictionary<string, object> htmlAttributes,
            int columnNumber = 1)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException();
            }

            if (listInfo == null)
            {
                throw new ArgumentNullException();
            }

            if (listInfo.Count() < 0)
            {
                throw new ArgumentException();
            }

            int dataCount = listInfo.Count();

            int rows = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(dataCount) / Convert.ToDecimal(columnNumber)));
            if (dataCount <= columnNumber || dataCount - columnNumber == 1)
            {
                rows = dataCount;
            }

            TagBuilder wrapBuilder = new TagBuilder("div");
            wrapBuilder.MergeAttribute("style", "float: left; light-height: 25px; padding-right: 5px;");

            string wrapStart = wrapBuilder.ToString(TagRenderMode.StartTag);
            string wrapClose = string.Concat(wrapBuilder.ToString(TagRenderMode.EndTag), " <div style=\"clear:both;\"></div>");
            string wrapBreak = string.Concat("</div>", wrapBuilder.ToString(TagRenderMode.StartTag));

            StringBuilder sb = new StringBuilder();
            sb.Append(wrapStart);

            int lineNumber = 0;

            foreach (var info in listInfo)
            {
                sb.Append(CreateTag(info, name, htmlAttributes).ToString());

                lineNumber++;

                if (lineNumber.Equals(rows))
                {
                    sb.Append(wrapBreak);
                    lineNumber = 0;
                }
                else
                {
                    sb.Append("<br />");
                }
            }

            sb.Append(wrapClose);

            return MvcHtmlString.Create(sb.ToString());
        }

        private static string CreateTag(SelectListItem info, string name, IDictionary<string, object> htmlAttributes)
        {
            StringBuilder sb = new StringBuilder();

            TagBuilder builder = new TagBuilder("input");
            if (info.Selected)
            {
                builder.MergeAttribute("checked", "checked");
            }
            builder.MergeAttributes<string, object>(htmlAttributes);
            builder.MergeAttribute("type", "radio");
            builder.MergeAttribute("value", info.Value);
            builder.MergeAttribute("name", name);
            sb.Append(builder.ToString(TagRenderMode.Normal));

            TagBuilder labelBuilder = new TagBuilder("label");
            labelBuilder.MergeAttribute("for", name);
            labelBuilder.InnerHtml = info.Text;

            sb.Append(labelBuilder.ToString(TagRenderMode.Normal));

            return sb.ToString();
        }
    }
}