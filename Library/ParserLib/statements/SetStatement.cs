using Library.InterpretLib;
using Library.ParserLib.Expr;

namespace Library.ParserLib.statements
{
    internal class SetStatement : Statement
    {
        private IdentExpression Ident { get; set; }

        public SetStatement(string ident, IExpression expr)
        {
            Ident = new IdentExpression(ident, expr);
        }
        public override void Execute(ExeContext context)
        {
            Variable var = context.GetVariable(Ident.Identifier);
            var.Value = Ident.Expression.Evaluate(context);

            if (var.DataType == DataType.STRING)
            {
                if (!(var.Value is string))
                    throw new Exception($"Cannot set {var.Value.GetType()} value into STRING variable.");
            }else if (var.Value is string && var.DataType != DataType.STRING)
            {
                    throw new Exception($"Cannot set {var.Value.GetType()} value into {var.DataType} variable.");
            }else{
                if (var.DataType == DataType.INT)
                {
                    if (var.Value is double)
                    {
                        throw new Exception("Cannot set DOUBLE value into INT variable.");
                    }
                    else
                        var.Value = Convert.ToInt32(var.Value);

                }
                else if (var.DataType is DataType.DOUBLE) { 
                    var.Value = Convert.ToDouble(var.Value);
                }
            }

            //Console.WriteLine($"{Ident.Identifier} = {var.Value} -> {var.Value.GetType()} == {var.DataType} TRUE");
            context.Variables.SetVariable(Ident.Identifier, var);
        }
    }
}
