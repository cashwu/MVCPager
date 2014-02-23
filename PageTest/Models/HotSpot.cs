using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PageTest.Models
{
    public class HotSpot
    {
        [DisplayName("熱點代碼")]
        public string ID { get; set; }

        [DisplayName("熱點名稱")]
        public string Spot_Name { get; set; }

        [DisplayName("熱點類別")]
        public string Type { get; set; }

        [DisplayName("業者")]
        public string Company { get; set; }

        [DisplayName("鄉鎮市區")]
        public string District { get; set; }

        [DisplayName("地址")]
        public string Address { get; set; }

        [DisplayName("機關構名稱")]
        public string Apparatus_Name { get; set; }

        [DisplayName("緯度")]
        public string latitude { get; set; }

        [DisplayName("經度")]
        public string longitude { get; set; }
    }

}
