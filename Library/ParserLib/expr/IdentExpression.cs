using Library.InterpretLib;
namespace Library.ParserLib.Expr
{
    public class IdentExpression : IExpression
    {
        public string Identifier { get; set; }
        public IExpression Expression { get; set; }

        public IdentExpression(string identifier, IExpression expression)
        {
            Identifier = identifier;
            Expression = expression;
        }

        public virtual object Evaluate(ExeContext context)
        {
            Variable var = context.GetVariable(Identifier);
            if (var.Value != null)
                return var.Value;
            else
                throw new Exception($"Variable {Identifier} is empty");
        }
    }
}
