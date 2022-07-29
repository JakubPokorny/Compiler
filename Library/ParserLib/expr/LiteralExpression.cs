using Library.InterpretLib;
namespace Library.ParserLib.Expr
{
    public class LiteralExpression : IExpression
    {
        public object Number { get; set; }
        public LiteralExpression(object number)
        {
            Number = number;
        }
        public virtual object Evaluate(ExeContext context)
        {
            if (Number is double)
                return (double) Number;
            else if (Number is int)
                return (int) Number;
            
            throw new Exception($"{Number} not a number");
        }
    }
}
