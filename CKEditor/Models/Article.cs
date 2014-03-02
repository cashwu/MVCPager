using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CKEditor.Models
{
    public class Article
    {
        [DisplayName("Subject")]
        public string Subject { get; set; }

        [DisplayName("Content")]
        public string Content { get; set; }
    }
}