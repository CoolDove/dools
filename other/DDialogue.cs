using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dove
{
public enum DefaultDDialogType
{ 
    Dialog,
    Ask,
    Answer,
    Sig,
    NSig,
    Action, 
    Item
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


internal class DDialogueParser<NodeType> where NodeType : System.Enum
{
    private enum TokenType
    { 
        None,
        End,
        LAngleBracket,
        RAngleBracket,
        LParen,
        RParen,
        LSquareBracket,
        RSquareBracket,
        LBrace,
        RBrace,
        String
    }
    private struct Token { 
        public TokenType type;
        public string value;
            public Token(TokenType type)
            {
                this.type = type;
                this.value = "";
            }
        public Token(TokenType type, string value) {
            this.type = type;
            this.value = value;
        }
    }
    private string source;
    private int length { get => source.Length; }
    private int pos = 0;
    private char current { get {
        if (pos >= length) return '\0';
        return source[pos];
    } }
    private char next { get {
        if (pos >= length - 1) return '\0';
        return source[pos + 1];
    } }


    public DDialogueParser(string source) {
        this.source = source;
    }

    private Token PeekToken() { 
        int posStash = pos;
        Token token = NextToken();
        pos = posStash;
        return token;
    }

    private Token NextToken() {
        while (current == ' ' || current == '\n' || current == '\r' || current == '\t') { pos++; }

        if (current == '\0') {
            return new Token(TokenType.End);
        }
        if (current == '(') {
            pos++;
            return new Token(TokenType.LParen);
        }
        if (current == ')') {
            pos++;
            return new Token(TokenType.LParen);
        }
        if (current == '[') {
            pos++;
            return new Token(TokenType.LSquareBracket);
        }
        if (current == ']') {
            pos++;
            return new Token(TokenType.RSquareBracket);
        }
        if (current == '<') {
            pos++;
            return new Token(TokenType.LAngleBracket);
        }
        if (current == '>') {
            pos++;
            return new Token(TokenType.RAngleBracket);
        }
        if (current == '{') {
            pos++;
            return new Token(TokenType.LBrace);
        }
        if (current == '}') {
            pos++;
            return new Token(TokenType.RBrace);
        }

        string str = "";
        System.Func<char, bool> isValid = x => {
            if (x == '(' || x == ')') return false;
            if (x == '[' || x == ']') return false;
            if (x == '<' || x == '>') return false;
            if (x == '{' || x == '}') return false;
            return true;
        };

        while(true) {
            if (isValid(current))
            {
                if (current == '\n' || current == '\r' || current == '\t') {
                    pos++;
                    continue;
                }
                str += current;
                pos++;
            } else {
                break;
            }
        }
        return new Token(TokenType.String, str);
    }

    public DDialogNode<NodeType> Parse() {
        Token token;
        while (true) {
            token = NextToken();
            Debug.Log($"token: {System.Enum.GetName(typeof(TokenType), token.type)}");
            if (token.type == TokenType.End) break;
        }
        NodeType type = (NodeType)System.Enum.Parse(typeof(NodeType), "ErrorEnum");
        return new DDialogNode<NodeType>(type, "");
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

public class DDialogContext
{
}

}
