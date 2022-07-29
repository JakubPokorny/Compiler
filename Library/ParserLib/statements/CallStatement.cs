using Library.InterpretLib;
using Library.ParserLib.Expr;

namespace Library.ParserLib.statements
{
    internal class CallStatement : Statement, IExpression
    {
        public string Ident { get; set; }
        protected LinkedList<IExpression>? Parameters { get; private set; }

        public CallStatement(string ident, LinkedList<IExpression> parameters)
        {
            Ident = ident;
            Parameters = parameters;
        }

        public IExpression CallReturn() {
            return Parameters.First();
        }

        public override void Execute(ExeContext context)
        {
            Evaluate(context);
        }

        public object Evaluate(ExeContext context)
        {
            return context.CallFunction(Ident, Parameters);
        }
    }
}
