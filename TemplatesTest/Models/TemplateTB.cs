using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TemplatesTest.Models
{
    public class TemplateTB
    {
        public Guid ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("選擇日期")]
        [Required(ErrorMessage = "請選擇日期")]
        public DateTime Date { get; set; }

        [UIHint("SelectAge")]
        [DisplayName("年齡")]
        [Required(ErrorMessage = "請選擇年齡")]
        public string Age { get; set; }

        [DisplayName("性別")]
        [Required(ErrorMessage = "請選擇性別")]
        public bool Gender { get; set; }
    }
    //[MetadataTypeAttribute(typeof(TemplateTBPartial))]
    //public partial class TemplateTB
    //{
    //    private class TemplateTBPartial
    //    {
    //        public Guid ID { get; set; }

    //        [DataType(DataType.Date)]
    //        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    //        [DisplayName("選擇日期")]
    //        [Required(ErrorMessage = "請選擇日期")]
    //        public DateTime Date { get; set; }

    //        [UIHint("SelectAge")]
    //        [DisplayName("年齡")]
    //        [Required(ErrorMessage = "請選擇年齡")]
    //        public string Age { get; set; }

    //        [DisplayName("性別")]
    //        [Required(ErrorMessage = "請選擇性別")]
    //        public bool 姓別 { get; set; }
    //    }
    //}
}