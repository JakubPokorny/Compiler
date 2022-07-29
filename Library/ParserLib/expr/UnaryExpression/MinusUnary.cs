using Library.InterpretLib;
namespace Library.ParserLib.Expr.UnaryExpression
{
    internal class MinusUnary : UnaryExpression
    {
        public MinusUnary(IExpression expression) : base(expression)
        {
        }

        public override object Evaluate(ExeContext context)
        {
            object value = Expr.Evaluate(context);

            try
            {
                if (value is int || Convert.ToDouble(value) == Convert.ToInt32(value))
                    return -Convert.ToInt32(value);
                else if (value is double)
                    return -(double)value;
            }
            catch (Exception) { 
                throw new Exception("Not a Number");
            }

            throw new Exception("Not a Number");

           
        }
    }
}
