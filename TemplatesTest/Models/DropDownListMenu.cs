using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TemplatesTest.Models
{
    public static class DropDownListMenu
    {
        public static IEnumerable<SelectListItem> SelectAgeList()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "0~20歲", Value = "0~20歲" });
            items.Add(new SelectListItem { Text = "21~40歲", Value = "41~60歲" });
            items.Add(new SelectListItem { Text = "61~80歲", Value = "61~80歲" });
            items.Add(new SelectListItem { Text = "80歲以上", Value = "80歲以上" });

            return items;
        }

        public static string ShowSelectAge(string s)
        {
            return SelectAgeList().Where(p => p.Value.Equals(s, StringComparison.CurrentCultureIgnoreCase))
                  .FirstOrDefault().Text;

        }

    }
}