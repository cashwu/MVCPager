using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FileUploadTest.Models
{
    public class Carrer
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please provide fullname", AllowEmptyStrings = false)]
        public string FullName { get; set; }

        public string EmailID { get; set; }

        public string CV { get; set; }
    }
}