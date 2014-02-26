using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace JsonTest.Helper
{
    public class EFJavaScriptSerializer : JavaScriptSerializer
    {
        public EFJavaScriptSerializer()
        {
            RegisterConverters(new List<JavaScriptConverter> { new EFSimpleJavaScriptConvert() });
        }

        public EFJavaScriptSerializer(int maxDepth = 1, EFJavaScriptConverter parent = null)
        {
            RegisterConverters(new List<JavaScriptConverter> { new EFJavaScriptConverter(maxDepth, parent) });
        }
    }
}