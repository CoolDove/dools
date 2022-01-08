using System;
using System.Collections;
using System.Collections.Generic;

using System.Reflection;

namespace dove
{

[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
public class GameSystemAttribute : Attribute
{
    public int Order { get; private set; }
    public GameSystemAttribute(int _order = 0) {
        Order = _order;
    }
}

public interface IGameSystem
{
    public GameMain Game { get; }
    void OnInit();
    void OnRelease();
}
// 
// public interface IGameSystemUpdate
// {
    // void Update();
// }
}
