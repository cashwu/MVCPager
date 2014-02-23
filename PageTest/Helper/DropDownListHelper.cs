using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PageTest.Helper
{
    public static class DropDownListHelper
    {
        public static MvcHtmlString GetDropdownList(
           string id,
           string name,
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
            select.Attributes.Add("id", id);

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