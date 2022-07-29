using Library.InterpretLib;
using Library.ParserLib.Expr;
namespace Library.ParserLib.Cond.BinaryRel
{
    internal abstract class BinaryRelCondition : ICondition
    {
        protected IExpression Left { get; set; }
        protected IExpression Right { get; set; }

        public BinaryRelCondition(IExpression left, IExpression right)
        {
            Left = left;
            Right = right;
        }

        public abstract object Evaluate(ExeContext context);
    }

}
