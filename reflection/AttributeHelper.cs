using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace dove.reflection
{
public class AttributeHelper : Attribute
{
    public static List<Type> GetTypesByAttribute<T>() where T : Attribute {
        return GetTypesByAttribute(typeof(T));
    }
    private static List<Type> GetTypesByAttribute(Type _attrib_type) {
        var output = new List<Type>();
        var assembly = typeof(AttributeHelper).Assembly;
        var types = assembly.GetTypes();
        foreach (var t in types) {
            var attribs = t.GetCustomAttributes();
            foreach (var atb in attribs) {
                if (atb.GetType() == _attrib_type) {
                    output.Add(t);
                    continue;
                }
            }
        }
        return output;
    }
}

}
