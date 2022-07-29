using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.ParserLib.Expr;
using Library.ParserLib.Cond;
using Library.ParserLib.blocks;
using Library.InterpretLib;

namespace Library.ParserLib.statements
{
    internal class ForStatement : Statement
    {
        protected SetStatement Start { get; private set; }
        protected ICondition Condition { get; private set; }
        protected SetStatement Increment { get; private set; }
        protected Block Block { get; private set; }

        public ForStatement(SetStatement start, ICondition condition, SetStatement increment, Block block)
        {
            Start = start;
            Condition = condition;
            Increment = increment;
            Block = block;
        }

        public override void Execute(ExeContext context)
        {
            for (Start.Execute(context); (bool) Condition.Evaluate(context); Increment.Execute(context))
               Block.Execute(context);
        }
    }
}
