using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace dove.rendering
{
public class IndirectArgBuffer
{
    /* [Layout]
    *  IndexCountPerInstance
    *  InstanceCount
    *  StartIndexLocation
    *  BaseVertexLocation
    *  StartInstanceLocation
    */
    public GraphicsBuffer Buffer { get; set; }

    private bool fdirt_;

    public void UpdateBuffer() {
        var data = new uint[5];
        data[0] = IndexCountPerInstance;
        data[1] = InstanceCount;
        data[2] = StartIndexLocation;
        data[3] = BaseVertexLocation;
        data[4] = StartInstanceLocation;
        if (fdirt_) Buffer.SetData(data);
    }

    public IndirectArgBuffer() {
        Buffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, 5, sizeof(uint));
    }

    public void Release() {
        Buffer?.Release();
    }

    public uint IndexCountPerInstance {
        get => IndexCountPerInstance_;
        set {
            IndexCountPerInstance_ = value;
            fdirt_ = true;
        }
    }
    public uint InstanceCount {
        get => InstanceCount_;
        set {
            InstanceCount_ = value;
            fdirt_ = true;
        }
    }
    public uint StartIndexLocation {
        get => StartIndexLocation_;
        set {
            StartIndexLocation_ = value;
            fdirt_ = true;
        }
    }
    public uint BaseVertexLocation {
        get => BaseVertexLocation_;
        set {
            BaseVertexLocation_ = value;
            fdirt_ = true;
        }
    }
    public uint StartInstanceLocation {
        get => StartInstanceLocation_;
        set {
            StartInstanceLocation_ = value;
            fdirt_ = true;
        }
    }

    private uint IndexCountPerInstance_;
    private uint InstanceCount_;
    private uint StartIndexLocation_;
    private uint BaseVertexLocation_;
    private uint StartInstanceLocation_;
}
}
