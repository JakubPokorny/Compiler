﻿using Library.InterpretLib;
using Library.ParserLib.Expr;

namespace Library.ParserLib.Cond.BinaryRel
{
    internal class GreaterEqualRel : BinaryRelCondition
    {
        public GreaterEqualRel(IExpression left, IExpression right) : base(left, right) { }
        public override object Evaluate(ExeContext context)
        {
            object left = Left.Evaluate(context);
            object right = Right.Evaluate(context);

            if (left is string || right is string)
                throw new Exception($"Invalid condition {left.GetType()} >= {right.GetType()}");
            else
                return Convert.ToDouble(left) >= Convert.ToDouble(right);
        }
    }
}
