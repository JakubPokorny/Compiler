using Library.InterpretLib;
using Library.ParserLib.Cond;
using Library.ParserLib.blocks;
namespace Library.ParserLib.statements
{
    internal class IfStatement : Statement
    {
        protected Block IfBlock { get; set; }
        protected Block? ElseBlock { get; set; }
        protected ICondition Condition { get; set; }

        public IfStatement(Block ifBlock, Block? elseBlock, ICondition condition)
        {
            IfBlock = ifBlock;
            ElseBlock = elseBlock;
            Condition = condition;
        }
        public IfStatement(Block ifBlock, ICondition condition)
        {
            IfBlock = ifBlock;
            ElseBlock = null;
            Condition = condition;
        }

        public override void Execute(ExeContext context)
        {
            if (ElseBlock == null)
            {
                if ( (bool) Condition.Evaluate(context))
                    IfBlock.Execute(context);
            }
            else
            {
                if ( (bool) Condition.Evaluate(context))
                   IfBlock.Execute(context);
                else 
                   ElseBlock.Execute(context);
            }
        }
    }
}
