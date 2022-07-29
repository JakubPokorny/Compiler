using Library.InterpretLib;
namespace Library.ParserLib.Expr.BinaryExpression
{
    internal abstract class BinaryExpression : IExpression
    {
        protected IExpression Left { get; set; }
        protected IExpression Right { get; set; }

        public BinaryExpression(IExpression left, IExpression right)
        {
            Left = left;
            Right = right;
        }
        public abstract object Evaluate(ExeContext context);
    }
}
