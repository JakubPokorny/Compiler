using Library.InterpretLib;
namespace Library.ParserLib.Expr.UnaryExpression
{
    internal class PlusUnary : UnaryExpression
    {
        public PlusUnary(IExpression expression) : base(expression)
        {
        }

        public override object Evaluate(ExeContext context)
        {

            object value = Expr.Evaluate(context);

            if (value is string)
                return Expr.Evaluate(context);

            if (value is int || Convert.ToInt32(value) == Convert.ToDouble(value))
                return Convert.ToInt32(value);
            else if (value is double)
                return value;

           return Expr.Evaluate(context);
        }
    }
}
