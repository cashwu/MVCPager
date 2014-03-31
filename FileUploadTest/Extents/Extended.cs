using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FileUploadTest
{
    public static class Extended
    {
        public static void GetDataAnnotation<T>(this IQueryable<T> data)
        {
            var props = new AssociatedMetadataTypeTypeDescriptionProvider(typeof(T)).GetTypeDescriptor(typeof(T)).GetProperties();

            foreach (PropertyDescriptor p in props)
            {
                //取Required標記
                var r = p.Attributes.OfType<RequiredAttribute>().FirstOrDefault();

                //取DisplayName標記
                var d = p.Attributes.OfType<DisplayNameAttribute>().FirstOrDefault();

                //取StringLength標記
                var s = p.Attributes.OfType<StringLengthAttribute>().FirstOrDefault();
            }
        }
    }
}