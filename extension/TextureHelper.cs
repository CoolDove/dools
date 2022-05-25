using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace dove.extension
{
public static class TextureHelper
{
    public static Texture2D ToTexture2D(this RenderTexture _rt) {
        if (_rt == null) return null;
        var stash = RenderTexture.active;
        RenderTexture.active = _rt;
        var tex = new Texture2D(_rt.width, _rt.height);
        tex.ReadPixels(new Rect(0, 0, _rt.width, _rt.height), 0, 0, false);
        tex.Apply();
        RenderTexture.active = stash;
        return tex;
    }
}
}
