namespace Library.LexerLib.tokens
{
    public class Token
    {
        public TokenType Type { get; private set; }
        public string? Value { get; set; }

        public Token(TokenType _type)
        {
            Type = _type;
            Value = null;
        }
        public Token(TokenType _type, string _value)
        {
            Type = _type;
            Value = _value;
        }

        public override string ToString()
        {
            if (Value == null)
                return $"token {Type}";
            else
                return $"token {Type}, value \"{Value}\"";

        }
    }
}
