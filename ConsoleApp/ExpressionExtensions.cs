using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ConsoleApp
{
    public static class ExpressionExtensions
    {
        public static List<string> GetProperties<T>() where T : class
        {
            var properties = new List<string>();

            var type = typeof(T);
            IList<PropertyInfo> props = new List<PropertyInfo>(typeof(T).GetProperties());

            foreach (PropertyInfo prop in props)
            {
                properties.Add(prop.Name);
                //object propValue = prop.GetValue(myObject, null);

                //// Do something with propValue
            }

            return properties;

        }
    }
}
