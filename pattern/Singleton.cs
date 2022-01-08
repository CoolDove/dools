using System;

namespace dove
{
public class Singleton<T> where T : class, new()
{
    public static T Instance {
        get {
            if (_instance == null)
                lock (ilock)
                    if (_instance == null) _instance = new T();
            return _instance;
        }
    }

    private static readonly object ilock = new object();

    private static T _instance = null;
}
}
