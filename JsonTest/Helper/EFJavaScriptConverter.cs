using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Data.Entity.Core.Objects.DataClasses;

namespace JsonTest.Helper
{
    public class EFJavaScriptConverter : JavaScriptConverter
    {
        private int _currentDepth = 1;
        private readonly int _maxDepth = 1;
        private readonly List<object> _processedObjects = new List<object>();

        private readonly Type[] _builtInTypes = new[]
        {
            typeof(bool),
            typeof(byte),
            typeof(sbyte),
            typeof(char),
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(short),
            typeof(ushort),
            typeof(string),
            typeof(DateTime),
            typeof(Guid),
            typeof(bool?),
            typeof(byte?),
            typeof(sbyte?),
            typeof(char?),
            typeof(decimal?),
            typeof(double?),
            typeof(float?),
            typeof(int?),
            typeof(uint?),
            typeof(long?),
            typeof(ulong?),
            typeof(short?),
            typeof(ushort?),
            typeof(DateTime?),
            typeof(Guid?)
        };

        public EFJavaScriptConverter() : this(1, null)
        {

        }

        public EFJavaScriptConverter(int maxDepth = 1, EFJavaScriptConverter parent = null)
        {
            _maxDepth = maxDepth;
            if (parent != null)
            {
                _currentDepth += parent._currentDepth;
            }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            _processedObjects.Add(obj.GetHashCode());
            var type = obj.GetType();
            var properties = type.GetProperties().
                Where(a => a.CanRead
                && a.GetIndexParameters().Count() == 0
                && _builtInTypes.Contains(a.PropertyType));

            var result = properties.ToDictionary(a => a.Name, a => (object)TryGetStringValue(a, obj));

            if (_maxDepth >= _currentDepth)
            {
                var complexProperties = type.GetProperties().
                   Where(a => a.CanRead
                       && a.CanWrite
                       && !a.Name.EndsWith("Reference")
                       && !_builtInTypes.Contains(a.PropertyType)
                       && !AllreadyAdded(a, obj)
                       && !_processedObjects.Contains(a.GetValue(obj, null) == null ? 0 : a.GetValue(obj, null).GetHashCode()));

                foreach (var property in complexProperties)
                {
                    var complexValue = TryGetValue(property, obj);
                    if (complexValue != null)
                    {
                        var js = new EFJavaScriptConverter(_maxDepth - _currentDepth, this);
                        result.Add(property.Name, js.Serialize(complexValue, new EFJavaScriptSerializer()));
                    }
                }
            }

            return result;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get 
            {
                var types = new List<Type>();
                types.AddRange(Assembly.GetAssembly(typeof(EntityObject)).GetTypes());
                return types;
            }
        }

        private bool AllreadyAdded(PropertyInfo p, object obj)
        {
            var val = TryGetValue(p, obj);
            return _processedObjects.Contains(val == null ? 0 : val.GetHashCode());
        }

        private static object TryGetValue(PropertyInfo p, object obj)
        {
            var parameters = p.GetIndexParameters();
            if (parameters.Length == 0)
            {
                var val = p.GetValue(obj, null);
                return val;
            }
            else
            {
                return string.Empty;
            }
        }

        private static object TryGetStringValue(PropertyInfo p, object obj)
        {
            if (p.GetIndexParameters().Length == 0)
            {
                var val = p.GetValue(obj, null);
                return val;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}