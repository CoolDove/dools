using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

[System.AttributeUsage(AttributeTargets.All)]
public class CollectedAttribAttribute : Attribute {}

public class AttributeHelper : MonoBehaviour
{
    private void Start() {
        AttributeHelper.GetTypes<CollectedAttribAttribute>();
    }

    public static List<Type> GetTypes<T>() where T : Attribute {
        return GetTypesByAttribute(typeof(T));
    }

    private static List<Type> GetTypesByAttribute(Type _attrib_type) {
        var output = new List<Type>();
        var assembly = Type.GetType("AttributeHelper").Assembly;
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
