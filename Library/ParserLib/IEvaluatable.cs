using Library.InterpretLib;
namespace Library.ParserLib
{
    public interface IEvaluatable
    {
        public object Evaluate(ExeContext context);
    }
}
