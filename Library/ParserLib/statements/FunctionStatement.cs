using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.InterpretLib;
using Library.ParserLib.blocks;
using Library.ParserLib.Expr;


namespace Library.ParserLib.statements
{
    public class FunctionStatement : Statement
    {
        public string Identifier { get; private set; }
        public LinkedList<Variable>? Parameters{ get; private set; }
        public Block BodyBlock{ get; private set; }
        public IExpression ReturnExpression { get; private set; }

        public FunctionStatement(string ident, LinkedList<Variable>? parameters, Block block, IExpression expression)
        {
            Identifier = ident;
            Parameters = parameters;
            BodyBlock = block;
            ReturnExpression = expression;
        }

        public override void Execute(ExeContext context)
        {
            return;
        }
    }
}
