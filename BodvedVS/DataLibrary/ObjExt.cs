using System.Collections.Generic;
using System;
using System.Reflection;

namespace BodvedVS.DataLibrary;

public static class ObjectExtensions
{
    public static T ToObject<T>(this IDictionary<string, object> source)
        where T : class, new()
    {
        var someObject = new T();
        var someObjectType = someObject.GetType();

        foreach (var item in source)
        {
            someObjectType
                     .GetProperty(item.Key)
                     .SetValue(someObject, item.Value, null);
        }

        return someObject;
    }

    public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
    {
        IDictionary<string, object> dic = new Dictionary<string, object>();
        foreach (var property in source.GetType().GetProperties())
            dic.Add(property.Name, property.GetValue(source, null));
        return dic;

        /*
        return source.GetType().GetProperties(bindingAttr).ToDictionary
        (
            propInfo => propInfo.Name,
            propInfo => propInfo.GetValue(obj: source, null)
        );
        */
    }
}