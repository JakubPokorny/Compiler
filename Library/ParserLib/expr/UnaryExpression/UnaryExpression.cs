using Library.InterpretLib;
namespace Library.ParserLib.Expr.UnaryExpression
{
    internal abstract class UnaryExpression : IExpression
    {
        protected IExpression Expr { get; set; }

        public UnaryExpression(IExpression expression)
        {
            Expr = expression;
        }

        public abstract object Evaluate(ExeContext context);
    }
}
