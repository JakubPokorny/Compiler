using Library.LexerLib.tokens;
using System.Text;

namespace Library.LexerLib
{
    public class Lexer
    {
        public Stack<Token> Tokens { get; set; }
        private IDictionary<string, TokenType> KeyWords { set; get; } = new Dictionary<string, TokenType>()
            {
                { "EXPR", TokenType.EXPR},
                { "COND", TokenType.COND},
                { "VAR", TokenType.VAR},
                { "PROCEDURE", TokenType.PROCEDURE},
                { "CALL", TokenType.CALL},
                { "BEGIN", TokenType.BEGIN},
                { "END", TokenType.END},
                { "IF", TokenType.IF},
                { "ELSE", TokenType.ELSE},
                { "FOR", TokenType.FOR},
                { "WHILE", TokenType.WHILE},
                { "FUNCTION", TokenType.FUNCTION},
                { "RETURN", TokenType.RETURN},
            };

        public Lexer(string code)
        {
            Tokens = new Stack<Token>();
            LoadTokens(code);

            Stack<Token> reverse = new Stack<Token>();
            while (Tokens.Count != 0)
            {
                reverse.Push(Tokens.Pop());
            }
            Tokens = reverse;
        }

        public Token? Peek()
        {
            if (Tokens.Count != 0)
                return Tokens.Peek();
            else
                return null;
        }

        public Token? Pop()
        {
            if (Tokens.Count != 0)
                return Tokens.Pop();
            else
                return null;
        }

        public void LoadTokens(string code)
        {
            List<string> list = new List<string>();
            int cur, start, end;
            string tmp;
            while ((cur = code.IndexOf('"')) != -1)
            {
                tmp = code.Substring(0, cur);
                list.AddRange(tmp.Split(new char[0], StringSplitOptions.RemoveEmptyEntries));

                tmp = code.Substring(cur, code.Length - cur);
                end = cur;
                start = 1;
                while ((cur = tmp.IndexOf('"', start)) != -1)
                {
                    if (tmp[cur - 1] != '\\')
                    {
                        list.Add(tmp.Substring(0, cur+1));
                        cur--;
                        break;
                    }
                    else
                    {
                        start = cur + 1;
                    }
                }
                end += cur + 2;
                tmp = code.Substring(end, code.Length - end);
                code = tmp;
            }

            list.AddRange(code.Split(new char[0], StringSplitOptions.RemoveEmptyEntries));
            foreach (string token in list)
            {
                AddToken(token);
            }
        }
        private void AddToken(string token)
        {
            StringBuilder builder;
            for (int i = 0; i < token.Length; i++)
            {
                if (char.IsLetter(token[i]) || token[i] == '_')
                {
                    builder = new StringBuilder();
                    while (i < token.Length && (char.IsLetter(token[i]) || char.IsDigit(token[i]) || token[i] == '_'))
                    {
                        builder.Append(token[i]);
                        i++;
                    }
                    if (KeyWords.TryGetValue(builder.ToString().ToUpperInvariant(), out TokenType type))
                    {
                        Tokens.Push(new Token(type));
                    }
                    else
                    {
                        Tokens.Push(new Token(TokenType.IDENT, builder.ToString()));
                    }
                    i--;
                    continue;
                }
                else if (char.IsDigit(token[i]))
                {
                    builder = new StringBuilder();
                    while (i < token.Length)
                    {
                        if (char.IsDigit(token[i]) || token[i] == '.')
                        {
                            //if (token[i] == ',')
                            //    if ((i + 1) < token.Length && !char.IsDigit(token[i + 1]))
                            //        break;
                            builder.Append(token[i]);
                            i++;
                        }
                        else
                        {
                            break;
                        }

                    }
                    Tokens.Push(new Token(TokenType.NUMBER, builder.ToString()));
                    i--;
                    continue;
                }
                else if (!IsSymbol(token, ref i))
                {
                    throw new Exception($"{token[i]} neplatny znak");
                }
            }
        }
        private bool IsSymbol(string token, ref int pos)
        {
            switch (token[pos])
            {
                case '+':
                    Tokens.Push(new Token(TokenType.PLUS));
                    return true;
                case ',':
                    Tokens.Push(new Token(TokenType.COMMA));
                    return true;
                case ';':
                    Tokens.Push(new Token(TokenType.SEMICOLON));
                    return true;
                case '-':
                    Tokens.Push(new Token(TokenType.MINUS));
                    return true;
                case '*':
                    Tokens.Push(new Token(TokenType.MULTIPLE));
                    return true;
                case '/':
                    Tokens.Push(new Token(TokenType.DEVIDE));
                    return true;
                case '=':
                    if (pos + 1 < token.Length && token[pos + 1] == '=')
                    {
                        Tokens.Push(new Token(TokenType.DOUBLE_EQUAL));
                        pos++;
                        return true;
                    }
                    else
                    Tokens.Push(new Token(TokenType.EQUAL));
                    return true;
                case '(':
                    Tokens.Push(new Token(TokenType.LEFT_ROUND_BRACKET));
                    return true;
                case ')':
                    Tokens.Push(new Token(TokenType.RIGHT_ROUND_BRACKET));
                    return true;
                case '{':
                    Tokens.Push(new Token(TokenType.LEFT_COURLY_BRACKET));
                    return true;
                case '}':
                    Tokens.Push(new Token(TokenType.RIGHT_COURLY_BRACKET));
                    return true;
                case '"':
                    if (token[token.Length - 1] == '"')
                    {
                        Tokens.Push(new Token(TokenType.QUATATION_MARK));
                        Tokens.Push(new Token(TokenType.STRING, token.Substring(1, token.Length - 2)));
                        Tokens.Push(new Token(TokenType.QUATATION_MARK));
                        pos = token.Length;
                        return true;
                    }
                    else
                        return false;
                case '.':
                    Tokens.Push(new Token(TokenType.DOT));
                    return true;
                case '!':
                    if (pos + 1 < token.Length && token[pos + 1] == '=')
                    {
                        Tokens.Push(new Token(TokenType.NOT_EQUAL));
                        pos++;
                        return true;
                    }
                    else
                        return false;
                case '<':
                    if (pos + 1 < token.Length && token[pos + 1] == '=')
                    {
                        Tokens.Push(new Token(TokenType.LESS_EQUAL));
                        pos++;
                        return true;
                    }
                    else
                    {
                        Tokens.Push(new Token(TokenType.LESS));
                        return true;
                    }
                case '>':
                    if (pos + 1 < token.Length && token[pos + 1] == '=')
                    {
                        Tokens.Push(new Token(TokenType.GREATER_EQUAL));
                        pos++;
                        return true;
                    }
                    else
                    {
                        Tokens.Push(new Token(TokenType.GREATER));
                        return true;
                    }
                default:
                    break;
            }
            return false;
        }
        public void PrintLexer()
        {
            foreach (Token token in Tokens)
            {
                Console.WriteLine(token.ToString());
            }
        }
    }
}
