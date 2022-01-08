using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace dove
{
[ExecutionOrder(-500)]
public class GameEntry : MonoBehaviour
{
    public GameMain game { get => GameMain.Instance; }

    private void Awake() {
        game.InitGame();
    }

    private void Update() {
        game.UpdateGame();
    }

    private void OnDestroy() {
        game.ReleaseGame();
    }
}
}
