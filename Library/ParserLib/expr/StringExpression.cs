using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.InterpretLib;
namespace Library.ParserLib.Expr
{
    public class StringExpression : IExpression
    {
        protected string String { get; private set; }

        public StringExpression(string @string)
        {
            String = @string;
        }

        public virtual object Evaluate(ExeContext context)
        {
            return String;
        }
    }
}
