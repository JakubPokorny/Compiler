using Library.InterpretLib;
using Library.ParserLib.Cond;
using Library.ParserLib.blocks;

namespace Library.ParserLib.statements
{
    internal class WhileStatement : Statement
    {
        protected Block Block { get; set; }
        protected ICondition Condition { get; set; }

        public WhileStatement(Block block, ICondition condition)
        {
            Block = block;
            Condition = condition;
        }

        public override void Execute(ExeContext context)
        {
            while ((bool) Condition.Evaluate(context))
                Block.Execute(context);
        }
    }
}
