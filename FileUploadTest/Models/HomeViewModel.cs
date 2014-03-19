using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FileUploadTest.Models
{
    public class HomeViewModel
    {
        [UIHint("FineUploader")]
        public string UploadUrl { get; set; }
    }
}