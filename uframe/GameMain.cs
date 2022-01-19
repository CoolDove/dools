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
        public int order;
        public IGameSystem system;
        public UnityMonoDelegate update;
        public UnityMonoDelegate on_draw_gizmos;
    }
    private List<GSys> systems_ = new List<GSys>();
    private delegate void UnityMonoDelegate();

    public static T GetSystem<T>() where T : class, IGameSystem {
        return Instance.GetIGameSystem(typeof(T)) as T;
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
            if (t.IsAssignableFrom(typeof(IGameSystem))) {
                Debug.Log($"type: {t}, didnt implement interface [IGameSystem], cannot use [GameSystem] attribute.");
                continue;
            }

            // search constructors
            var constructor_info = t.GetConstructor(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance,
                null, CallingConventions.Any, new Type[0], null);
            if (constructor_info == null) {
                Debug.Log($"No private constructor find in GameSystem class: {t}");
            }
            else {
                var sys = (IGameSystem)constructor_info.Invoke(new object[0]);    
                if (sys != null) {
                    GSys gsys = new GSys() {
                        type = t,
                        system = sys,
                        order = GetOrder(t) };

                    BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                    var update_method = t.GetMethod("Update",
                        flags, null, CallingConventions.Any, new Type[0], null);
                    if (update_method != null)
                        gsys.update = (UnityMonoDelegate)update_method.CreateDelegate(typeof(UnityMonoDelegate), sys);

                    var on_draw_gizmos_method = t.GetMethod("OnDrawGizmos",
                        flags, null, CallingConventions.Any, new Type[0], null);
                    if (on_draw_gizmos_method != null)
                        gsys.on_draw_gizmos = (UnityMonoDelegate)on_draw_gizmos_method.CreateDelegate(typeof(UnityMonoDelegate), sys);
                    
                    systems_.Add(gsys);
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

                // Inject dependencies
                var game_propt = sys.type.GetProperty("Game");
                game_propt?.SetValue(sys.system, GameMain.Instance);

                var properties = sys.type.GetProperties();
                foreach (var p in properties) {
                    var atb = p.GetCustomAttributes(typeof(GameSystemInjectAttribute), false);
                    if (atb.Length > 0) {
                        p.SetValue(sys.system, GetIGameSystem(sys.type));
                        Debug.Log($"inject [{GetIGameSystem(sys.type)}] into [{p}]");
                    }
                }
            }
            foreach (var sys in systems_) {
                sys.system.OnInit();
            }
        }
    }

    public void UpdateGame() {
        foreach (var s in systems_) {
            s.update?.Invoke();
        }
    }

    public void DrawGizmos() {
        foreach (var s in systems_)
            s.on_draw_gizmos?.Invoke();
    }

    public void ReleaseGame() {
        systems_.ForEach(s => {
            s.system.OnRelease();
        });
    }
}
}
