using System.Collections;
using System.Collections.Generic;

// @Temp: will use another Json lib instead of unity version
using UnityEngine;

namespace dove
{
public interface IDJsonable<T> {
    T GetData();
    void ApplyData(T _data);
}

public static class DJson {
    public static string ToJson<T>(IDJsonable<T> _obj) {
        T data = _obj.GetData();
        return JsonUtility.ToJson(data);
    }
    public static T FromJson<T>(string _json) {
        return JsonUtility.FromJson<T>(_json);
    }
}

}
