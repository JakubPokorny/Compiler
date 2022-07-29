using Library.LexerLib;
using Library.LexerLib.tokens;
using Library.ParserLib.blocks;
using Library.ParserLib.Cond;
using Library.ParserLib.Cond.BinaryRel;
using Library.ParserLib.Expr;
using Library.ParserLib.Expr.BinaryExpression;
using Library.ParserLib.Expr.UnaryExpression;
using Library.ParserLib.statements;
using Library.InterpretLib;
using System.Globalization;

namespace Library.ParserLib
{
    public class Parser
    {
        private Lexer lexer;
        private ExeContext context;
        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
            context = new ExeContext(null);
        }

        public ExeContext GetExeContext() { 
            return context;
        }
        public Block ParseProgram()
        {
            if (PopToken().Type != TokenType.BEGIN)
                throw new Exception("Expexted \"BEGIN\"");

            Block block = ReadBlock();

            if (PopToken().Type != TokenType.END)
                throw new Exception("Expected \"END\"");  
            if (PopToken().Type != TokenType.SEMICOLON)
                throw new Exception("Expected \';\'");

            return block;
        }
        #region BLOCK
        private Block ReadBlock()
        {
            Block block = new Block();
            Statement? statement;
            LinkedList<Variable>? vars;

            while ((vars = ReadVariables()) != null)
                block.AddVars(vars);

            while ((statement = ReadStatement()) != null)
                if (statement is FunctionStatement)
                    block.AddFunctions((FunctionStatement) statement);
                   // context.AddProgramContext(statement);
                else
                    block.AddStatement(statement);
            return block;
        }
        private LinkedList<Variable>? ReadVariables()
        {
            DataType? dataType = ReadDataType();

            if (dataType == null || PeekToken() != TokenType.IDENT)
              return null;
            

            string ident = ReadIdentifier();

            LinkedList<Variable> vars = new LinkedList<Variable>();
            vars.AddFirst(new Variable(ident, null, (DataType)dataType));

            while (PeekToken() == TokenType.COMMA)
            {
                PopToken();
                ident = ReadIdentifier();
                vars.AddLast(new Variable(ident, null, (DataType)dataType));
            }

            if (PopToken().Type != TokenType.SEMICOLON)
                throw new Exception("Variable declaration expected \';\'");

            return vars;
        }
        private Block ReadCurlyBlock() {
            if (PopToken().Type != TokenType.LEFT_COURLY_BRACKET)
                throw new Exception("Expected \'{\' ");
           
            Block block = ReadBlock();
            
            if (PopToken().Type != TokenType.RIGHT_COURLY_BRACKET)
                throw new Exception("Expected \'}\' ");
            
            return block;
        }
        #endregion
        #region STATEMENT
        private Statement? ReadStatement()
        {
            switch (PeekToken())
            {
                case TokenType.IDENT:
                    SetStatement setStatement = ReadSetStatement();
                    if (PopToken().Type != TokenType.SEMICOLON)
                        throw new Exception("Expeceted \';\'");
                    return setStatement;
                case TokenType.FUNCTION:
                    return ReadFunctionStatement();
                case TokenType.CALL:

                    CallStatement callStatement =  ReadCallStatement();
                    if (PopToken().Type != TokenType.SEMICOLON)
                        throw new Exception("Expeceted \';\'");
                    return callStatement;
                case TokenType.IF:
                    return ReadIfStatement();
                case TokenType.FOR:
                    return ReadForStatement();
                case TokenType.WHILE:
                    return ReadWhileStatement();
                default:
                    return null;
            }
        }
        private IfStatement ReadIfStatement()
        {
            if (PopToken().Type != TokenType.IF)
                throw new Exception("Expected \"IF\" ");

            ICondition condition = ReadRoundCondition();
            
            Block block = ReadCurlyBlock();

            if (PeekToken() == TokenType.ELSE)
            {
                PopToken();
                return new IfStatement(block, ReadCurlyBlock(), condition);
            }
            else {
                return new IfStatement(block, condition);
            }
        }
        private WhileStatement ReadWhileStatement()
        {
            if (PopToken().Type != TokenType.WHILE)
                throw new Exception("Expected \"WHILE\" ");

            ICondition condition = ReadRoundCondition();

            return new WhileStatement(ReadCurlyBlock(), condition);
        }
        private ForStatement ReadForStatement()
        {
            if (PopToken().Type != TokenType.FOR)
                throw new Exception("Expected \"FOR\" ");

            if (PopToken().Type != TokenType.LEFT_ROUND_BRACKET)
                throw new Exception("Expected \"(\" ");

            SetStatement start = ReadSetStatement();

            if (PopToken().Type != TokenType.SEMICOLON)
                throw new Exception("Expected \";\" ");
            
            ICondition condition = ReadCondition();

            if (PopToken().Type != TokenType.SEMICOLON)
                throw new Exception("Expected \";\" ");

            SetStatement increment = ReadSetStatement();

            if (PopToken().Type != TokenType.RIGHT_ROUND_BRACKET)
                throw new Exception("Expected \")\" ");

            return new ForStatement(start, condition, increment, ReadCurlyBlock());
        }
        private SetStatement ReadSetStatement()
        {
            string ident = ReadIdentifier();
            
            if (PopToken().Type != TokenType.EQUAL)
                throw new Exception("Expected \"=\"");

            return new SetStatement(ident, ReadExpression());
        }
        private Statement ReadFunctionStatement()
        {
            if (PopToken().Type != TokenType.FUNCTION)
                throw new Exception("Expeceted \"FUNCTION\"");

            string ident = ReadIdentifier();

            if (PopToken().Type != TokenType.LEFT_ROUND_BRACKET)
                throw new Exception("Expected \"(\" ");

            LinkedList<Variable>? parameters;

            if (PeekToken() == TokenType.IDENT)
            {
                parameters = new LinkedList<Variable>();
                parameters.AddFirst(ReadVarDeclaration());

                while (PeekToken() == TokenType.COMMA)
                {
                    PopToken();
                    parameters.AddLast(ReadVarDeclaration());
                }
            }
            else {
                parameters = null;
            }


            if (PopToken().Type != TokenType.RIGHT_ROUND_BRACKET)
                throw new Exception("Expected \")\" ");

            if (PopToken().Type != TokenType.LEFT_COURLY_BRACKET)
                throw new Exception("Expected \'{\' ");

            Block block = ReadBlock();

            if (PopToken().Type != TokenType.RETURN)
                throw new Exception("Expected \"RETURN\" ");
            
            IExpression expression = ReadExpression();

            if (PopToken().Type != TokenType.SEMICOLON)
                throw new Exception("Expected \';\' ");

            if (PopToken().Type != TokenType.RIGHT_COURLY_BRACKET)
                throw new Exception("Expected \'}\' ");

            return new FunctionStatement(ident, parameters, block, expression);
        }
        private CallStatement ReadCallStatement()
        {
            if (PopToken().Type != TokenType.CALL)
                throw new Exception("Expected \"CALL\"");
    
            string ident = ReadIdentifier();

            if (PopToken().Type != TokenType.LEFT_ROUND_BRACKET)
                throw new Exception("Expected \"(\" ");

            LinkedList<IExpression>? parameters = null;
            if (PeekToken() != TokenType.RIGHT_ROUND_BRACKET)
            {
                parameters = new LinkedList<IExpression>();
                parameters.AddFirst(ReadExpression());
                while (PeekToken() == TokenType.COMMA) {
                    PopToken();
                    parameters.AddLast(ReadExpression());
                }
            }

            if (PopToken().Type != TokenType.RIGHT_ROUND_BRACKET)
                throw new Exception("Expected \")\" ");
            
            return new CallStatement(ident, parameters);

        }
        #endregion


        private DataType? ReadDataType() {
            Token? token = lexer.Peek();
            if (token != null && token.Type == TokenType.IDENT && token.Value != null) {

                DataType? dataType = Variable.IsDataType(token.Value.ToUpperInvariant());

                if (dataType != null)
                {
                    PopToken();
                    return dataType;
                }
            }
            return null;
                              
        }

        private Variable ReadVarDeclaration() {
            DataType? dataType = ReadDataType();

            if (dataType == null)
                throw new Exception("Expected dataType");

            string ident = ReadIdentifier();

            return new Variable(ident, null,  (DataType) dataType);
        }

        private string ReadIdentifier() {
            if (PeekToken() != TokenType.IDENT)
                throw new Exception("Expected \"IDENT\"");

            string? ident = PopToken().Value;
            if (ident == null)
                throw new Exception("Empty IDENT");
            return ident;
        }

        private TokenType? PeekToken()
        {
            Token? token = lexer.Peek();
            return token != null ? token.Type : null;
        }
        private Token PopToken()
        {
            Token? token = lexer.Pop();
            return token != null ? token : throw new Exception("Expected Token");
        }

        private ICondition ReadRoundCondition()
        {
            if (PopToken().Type != TokenType.LEFT_ROUND_BRACKET)
                throw new Exception("Expected \'(\' ");

            ICondition condition = ReadCondition();

            if (PopToken().Type != TokenType.RIGHT_ROUND_BRACKET)
                throw new Exception("Expected \')\' ");

            return condition;
        }
        private ICondition ReadCondition()
        {
            ICondition cond;
            IExpression expr;
            expr = ReadExpression();
            cond = ReadBinaryCondition(expr);
            return cond;
        }

        private ICondition ReadBinaryCondition(IExpression expr)
        {
            switch (PopToken().Type)
            {
                case TokenType.DOUBLE_EQUAL:
                    return new EqualsRel(expr, ReadExpression());
                case TokenType.NOT_EQUAL:
                    return new NotEqualRel(expr, ReadExpression());
                case TokenType.LESS:
                    return new LessThanRel(expr, ReadExpression());
                case TokenType.LESS_EQUAL:
                    return new LessEqualRel(expr, ReadExpression());
                case TokenType.GREATER:
                    return new GreaterThanRel(expr, ReadExpression());
                case TokenType.GREATER_EQUAL:
                    return new GreaterEqualRel(expr, ReadExpression());
                default:
                    throw new Exception("Expected condition sign");
            }
        }

        private IExpression ReadExpression()
        {
            IExpression expr;
            if (PeekToken() == TokenType.QUATATION_MARK)
                expr = ReadString();
            else
            {
                if (PeekToken() == TokenType.PLUS || PeekToken() == TokenType.MINUS)
                    expr = ReadUnaryExpression();
                else
                    expr = ReadTerm();

                while (PeekToken() != null)
                {
                    switch (PeekToken())
                    {
                        case TokenType.PLUS:
                            PopToken();
                            expr = new Plus(expr, ReadTerm());
                            continue;
                        case TokenType.MINUS:
                            PopToken();
                            expr = new Minus(expr, ReadTerm());
                            continue;
                        default:
                            return expr;
                    }
                }
            }
            return expr;
        }

        private IExpression ReadString() {
            if (PopToken().Type != TokenType.QUATATION_MARK)
                throw new Exception("Expected \'\"\'");

            if (PeekToken() != TokenType.STRING)
                throw new Exception("Expected \"STRING\"");

            string? @string = PopToken().Value;
            if (@string == null)
                @string = "";
            IExpression expr =  new StringExpression(@string);
    
            if (PopToken().Type != TokenType.QUATATION_MARK)
                throw new Exception("Expected \'\"\'");

            return expr;
        }

        private IExpression ReadTerm()
        {
            IExpression expr = ReadFactor();
            while (PeekToken() != null)
            {
                switch (PeekToken())
                {
                    case TokenType.MULTIPLE:
                        PopToken();
                        expr = new Multiply(expr, ReadFactor());
                        continue;
                    case TokenType.DEVIDE:
                        PopToken();
                        expr = new Divide(expr, ReadFactor());
                        continue;
                    default:
                        return expr;
                }
            }
            return expr;
        }

        private IExpression ReadFactor()
        {
            if (PeekToken() == TokenType.CALL)
                return ReadCallStatement();
            if (PeekToken() == TokenType.IDENT)
                return ReadIdentExpression();
            if (PeekToken() == TokenType.NUMBER)
                return ReadLiteralExpression();
            
            if (PopToken().Type != TokenType.LEFT_ROUND_BRACKET)
                throw new Exception("Expected \'(\'");
            
            IExpression expr = ReadExpression();
            
            if (PopToken().Type != TokenType.RIGHT_ROUND_BRACKET)
               throw new Exception("Expected \')\'");
            else        
                return expr;
            
            throw new Exception("Expected factor");
        }

        private IExpression ReadUnaryExpression()
        {
            switch (PopToken().Type)
            {
                case TokenType.PLUS:
                    return new PlusUnary(ReadTerm());
                case TokenType.MINUS:
                    return new MinusUnary(ReadTerm());
            }

            throw new Exception("Expected unary expression");
        }

        private LiteralExpression ReadLiteralExpression()
        {
            Token token;
            if ((token = PopToken()).Type == TokenType.NUMBER)
            {
                if (token.Value.IndexOf('.') != -1 || token.Value.IndexOf(',') != -1)
                {
                    if (double.TryParse(token.Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double number))
                        return new LiteralExpression(number);
                }
                else
                {
                    if (int.TryParse(token.Value, out int number))
                        return new LiteralExpression(number);
                }
                throw new Exception("Expected number");
            }
            else
                throw new Exception("Expected Literal Expression");
        }

        private IdentExpression ReadIdentExpression()
        {
            if (PeekToken() != TokenType.IDENT)
                throw new Exception("Expected IDENT");
            return new IdentExpression(ReadIdentifier(), null);
        }

    }
}