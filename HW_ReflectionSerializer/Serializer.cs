using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HW_ReflectionSerializer
{
    public static class Serializer
    {
        public static string Serialize(this object obj)
        {
            var type = obj.GetType();
            var sb = new StringBuilder();
            sb.Append("{");
            var properties = type.GetProperties();
            foreach ( var property in properties )
            {
                sb.Append($"""
                    "{property.Name}":{property.GetValueInJSON(obj)},
                    """);
            }
            sb.Length--;
            sb.Append("}");
            return sb.ToString();
        }

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();

            var arr = json.Replace("{", String.Empty).Replace("}", String.Empty).Replace(@"""",String.Empty).Trim().Split(",");
            IDictionary<string,string> map = new Dictionary<string,string>();
            foreach ( var prop in arr)
            {
                var item = prop.Split(':');
                if(item.Length == 2) 
                {
                    map.Add(item[0], item[1]);
                }
            }

            var type = typeof(T);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                string value = String.Empty; 
                map.TryGetValue(property.Name,out value);
                if (value != String.Empty)
                {
                    property.SetValue(obj, GetValuefromJSON(property,value.Trim()));
                }
            }
            return obj;
        }

        private static object GetValuefromJSON(this PropertyInfo info, string value)
        {
            var objName = info.PropertyType.Name;
            switch (objName)
            {
                case "String": return Convert.ToString(value);
                default: return Convert.ToInt32(value);
            }
        }
        private static string GetValueInJSON(this PropertyInfo info,object obj )
        {
            var typeName = info.PropertyType.Name;
            switch (typeName)
            {
                case "String": return $@"""{info.GetValue(obj)}""";
                case "Char": return $@"""{info.GetValue(obj)}""";
                default: return $@"{info.GetValue(obj)}";
            }
        }
    }

    
}
