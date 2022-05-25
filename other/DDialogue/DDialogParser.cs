using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dove
{ 
class DDialogueParser<NodeType> where NodeType : System.Enum
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
    private class Token { 
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

    private Stack<Token> tokenStackRecover = new Stack<Token>();
    private Stack<Token> tokenStackUsing = new Stack<Token>();

    public DDialogueParser(string source) {
        this.source = source;
    }

    private Token PeekToken() {
        if (tokenStackRecover.Count != 0) return tokenStackRecover.Peek();
        int posStash = pos;
        Token token = NextToken();
        pos = posStash;
        return token;
    }

    private Token NextToken() {
        Token tok = getNextToken();
        tokenStackUsing.Push(tok);
        return tok;
    }
    private Token getNextToken() {
        if (tokenStackRecover.Count != 0) return tokenStackRecover.Pop();

        while (current == ' ' || current == '\n' || current == '\r' || current == '\t') { pos++; }

        Token tok;

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
                if (current == ' ' || current == '\n' || current == '\r' || current == '\t') {
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
        // var root = ParseNode();
        // DDialogNode<NodeType> root = new DDialogNode<NodeType>();
        // return root;
        Token tok = new Token(TokenType.None);
        while (tok.type != TokenType.End) {
            tok = NextToken();
        }
        return null;
    }

    // recover token stack if failed
    // private DDialogNode<NodeType> ParseNode() {
    // }
    // private DDialogCondition ParseCondition() {
    // }
    // private DDialogNodeProperty ParseProperty() {
    // }

}
}
