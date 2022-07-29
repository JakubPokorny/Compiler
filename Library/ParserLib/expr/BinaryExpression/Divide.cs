using Library.InterpretLib;
namespace Library.ParserLib.Expr.BinaryExpression
{
    internal class Divide : BinaryExpression
    {
        public Divide(IExpression left, IExpression right) : base(left, right)
        {
        }

        public override object Evaluate(ExeContext context)
        {
            object left = Left.Evaluate(context);
            object right = Right.Evaluate(context);

            if (left is string || right is string)
            {
                throw new Exception("Not divisible expressions");
            }
            else if (Convert.ToDouble(right) != 0)
            {
                if (left is int && right is int)
                    return Convert.ToInt32(left) / Convert.ToInt32(right);
                else
                    return Convert.ToDouble(left) / Convert.ToDouble(right);
            }
            throw new Exception("Not divisible expressions");
        }
    }
}
