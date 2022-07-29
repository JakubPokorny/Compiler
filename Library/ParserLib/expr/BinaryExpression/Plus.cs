using Library.InterpretLib;
namespace Library.ParserLib.Expr.BinaryExpression
{
    internal class Plus : BinaryExpression
    {
        public Plus(IExpression left, IExpression right) : base(left, right)
        {
        }

        public override object Evaluate(ExeContext context)
        {
            object left = Left.Evaluate(context);
            object right = Right.Evaluate(context);

            if (left is string || right is string)
            {
                if (left is string && right is string)
                    return (string)left + (string)right;
            }
            else {
                if (left is int && right is int)
                    return Convert.ToInt32(left) + Convert.ToInt32(right);
                else
                    return Convert.ToDouble(left) + Convert.ToDouble(right);
            }
            throw new Exception("Not plusable expressions");
        }
    }
}
