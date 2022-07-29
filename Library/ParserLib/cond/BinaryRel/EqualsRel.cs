using Library.InterpretLib;
using Library.ParserLib.Expr;
namespace Library.ParserLib.Cond.BinaryRel
{
    internal class EqualsRel : BinaryRelCondition
    {
        public EqualsRel(IExpression left, IExpression right) : base(left, right) { }

        public override object Evaluate(ExeContext context)
        {
            object left = Left.Evaluate(context);
            object right = Right.Evaluate(context);
            if (left is string || right is string)
            {
                if (left is string && right is string)
                    return (string)left == (string)right;
                else
                    throw new Exception($"Invalid condition {left} == {right}");
            }else
                return Convert.ToDouble(left) == Convert.ToDouble(right);
        }
    }
}
