using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dove
{
public enum DefaultDDialogType
{ 
    None,
    Sequence,
    Dialog,
    Action,
    Item
}

public enum DDConditionType { 
    None,
    Sig,
    NSig,
    Ask,
    Answer
}

public class DDialogue<NodeType> where NodeType : System.Enum
{
    private DDialogNode<NodeType> nodes;
    private DDialogProcessor<NodeType> processor;

    public DDialogNode<NodeType> currentNode;

    private List<DDialogContext> contexts = new List<DDialogContext>();

    public DDialogue(string source,DDialogProcessor<NodeType> processor) {
        currentNode = Parse(source);
        this.processor = processor;
    }

    // return root
    private DDialogNode<NodeType> Parse(string source) {
        DDialogueParser<NodeType> parser = new DDialogueParser<NodeType>(source);
        return parser.Parse();
    }

    public void Next() {
        currentNode = processor?.Process(currentNode);
    }
}

public abstract class DDialogProcessor<NodeType> where NodeType : System.Enum
{
    // return next node
    public abstract DDialogNode<NodeType> Process(DDialogNode<NodeType> node);
}

public class DDialogNode<NodeType> where NodeType : System.Enum
{
    public NodeType type { get; private set; }

    public DDialogNode<NodeType> parent;
    public List<DDialogNode<NodeType>> childs { get; private set; }

    public DDialogNode(NodeType type, string content) {
        this.type = type;
        this.content = content;
    }

    private string content;
}

public class DDialogCondition
{ 
    public DDConditionType type;
    public string content;
    public DDialogCondition(DDConditionType type, string content = "") {
        this.type = type;
        this.content = content;
    }
}

public class DDialogNodeProperty
{
    public string name;
    public string value;
    public DDialogNodeProperty(string name, string value) {
        this.name = name;
        this.value = value;
    }
}

public class DDialogContext
{
}

}
