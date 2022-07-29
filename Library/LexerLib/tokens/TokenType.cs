namespace Library.LexerLib.tokens
{
    public enum TokenType
    {
        DATATYPE,
        IDENT,
        NUMBER,
        STRING,
        VAR,
        PROCEDURE,

        FUNCTION,
        RETURN,
        CALL,
        COND,
        EXPR,
        BEGIN,
        END,
        IF,
        ELSE,
        FOR,
        WHILE,

        COMMA,
        SEMICOLON,
        QUATATION_MARK,

        EQUAL,
        DOUBLE_EQUAL,
        NOT_EQUAL,

        LESS_EQUAL,
        GREATER_EQUAL,
        LESS,
        GREATER,

        PLUS,
        MINUS,
        MULTIPLE,
        DEVIDE,

        LEFT_ROUND_BRACKET,
        RIGHT_ROUND_BRACKET,
        LEFT_COURLY_BRACKET,
        RIGHT_COURLY_BRACKET,

        DOT,
    }
}
