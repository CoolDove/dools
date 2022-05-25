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
            {
                this.value = "";
            }
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

    private void StackRecover() {
        while (tokenStackUsing.Count != 0) {
            tokenStackRecover.Push(tokenStackUsing.Pop());
        }
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
            return new Token(TokenType.RParen);
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
        // Token tok = new Token(TokenType.None);
        // while (tok.type != TokenType.End) {
            // tok = NextToken();
        // }
        var prop = ParseProperty();
        
        return null;
    }

    // recover token stack if failed
    private DDialogNode<NodeType> ParseNode() {
        return null;
    }
    private DDialogCondition ParseCondition() {
        Token tok = NextToken();

        string typeStr = "";
        DDConditionType type = DDConditionType.None;
        string content = "";
        if (tok.type == TokenType.LAngleBracket) {
            tok = NextToken();
            if (tok.type == TokenType.String) {
                typeStr = tok.value;
                try {
                    type = (DDConditionType)System.Enum.Parse(typeof(DDConditionType), typeStr);
                } catch {
                    StackRecover();
                    return null;
                }
                tok = NextToken();
                if (tok.type == TokenType.LParen) {
                    tok = NextToken();
                    if (tok.type == TokenType.String) {
                        content = tok.value;
                        if (NextToken().type == TokenType.RParen && NextToken().type == TokenType.RAngleBracket) {
                            // <String(String)>
                            DDialogCondition cond = new DDialogCondition(type, content);
                            return cond;
                        } else {
                            StackRecover();
                            return null;
                        }
                    } else {
                        StackRecover();
                        return null;
                    }
                } else {
                    tok = NextToken();
                    if (tok.type == TokenType.RAngleBracket) {
                        // <String>
                        DDialogCondition cond = new DDialogCondition(type);
                        return cond;
                    } else {
                        StackRecover();
                        return null;
                    }
                }
            } else {
                StackRecover();
                return null;
            }
        } else {
            StackRecover();
            return null;
        }
    }
    private DDialogNodeProperty ParseProperty() {
        string name = "";
        string value = "";
        Token tok = NextToken();
        if (tok.type == TokenType.LSquareBracket) {
            tok = NextToken();
            if (tok.type == TokenType.String) {
                name = tok.value;
                tok = NextToken();
                if (tok.type == TokenType.RSquareBracket) {
                    var prop = new DDialogNodeProperty(name, value);
                    return prop;
                } else if (tok.type == TokenType.LParen) {
                    tok = NextToken();
                    if (tok.type == TokenType.String) {
                        value = tok.value;
                        if (NextToken().type == TokenType.RParen && NextToken().type == TokenType.RSquareBracket) {
                            return new DDialogNodeProperty(name, value);
                        } else {
                            StackRecover();
                            return null;
                        }
                    } else {
                        StackRecover();
                        return null;
                    }
                } else {
                    StackRecover();
                    return null;
                }
            } else {
                StackRecover();
                return null;
            }
        } else {
            StackRecover();
            return null;
        }
    }

}
}
