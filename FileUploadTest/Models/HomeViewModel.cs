using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FileUploadTest.Models
{
    public class HomeViewModel
    {
        [DataType(DataType.Upload)]
        public string UploadUrl { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("選擇日期")]
        [Required]
        public DateTime UploadDate { get; set; }

        [Required()]
        public string Date { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "必填")]
        public string Phone { get; set; }

        [UIHint("MemberID")]
        [Required(ErrorMessage = "必填")]
        public string MemberID { get; set; }
    }
}