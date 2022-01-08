using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace dove
{
public class GameMain : Singleton<GameMain> {
    public int SystemCount { get => systems_.Count; }

    private struct GSys {
        public Type type;
        public IGameSystem system;
        public int order;
    }
    private List<GSys> systems_ = new List<GSys>();

    public T GetSystem<T>() where T : class, IGameSystem {
        return GetIGameSystem(typeof(T)) as T;
    }

    public IGameSystem GetIGameSystem(Type _type) {
        foreach (var s in systems_)
            if (s.type == _type) return s.system;
        return null;
    }

    public void InitGame() {
        Debug.Log("= Game Init");
        Func<Type, int> GetOrder = type => {
            var atb = type.GetCustomAttributes(typeof(GameSystemAttribute), false);
            return (atb[0] as GameSystemAttribute).Order;
        };
        
        var systypes = reflection.AttributeHelper.GetTypesByAttribute<dove.GameSystemAttribute>();
        foreach (var t in systypes) {
            if (t.GetInterface("IGameSystem") == null) {
                Debug.Log($"type: {t}, didnt implement interface [IGameSystem], cannot use [GameSystem] attribute.");
                continue;
            }
            var constructor_info = t.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, CallingConventions.Any, new Type[0], null);
            if (constructor_info == null) Debug.Log($"no private constructor find in GameSystem class: {t}");
            else {
                var sys = (IGameSystem)constructor_info.Invoke(new object[0]);    
                if (sys != null) {
                    Debug.Log($"GameSystem: [{t}] loaded.");
                    systems_.Add(new GSys(){ type = t, system = sys, order = GetOrder(t) });
                }
            }
        }

        Comparison<GSys> CompareGSys = (l, r) => {
            if (l.order < r.order) return -1;
            else if (l.order > r.order) return 1;
            else return 0;
        };
        systems_.Sort(CompareGSys);

        if (systems_.Count == 0) {
            Debug.Log("= No GameSystems Being Loaded");
        } else {
            Debug.Log("= Loaded Systems: ");
            foreach (var sys in systems_) {
                Debug.Log($"-{sys.type} (order: {sys.order})");
                sys.system.OnInit();
            }
        }
    }

    public void ReleaseGame() {
        systems_.ForEach(s => {
            s.system.OnRelease();
        });
    }
}
}
