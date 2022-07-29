using Library.InterpretLib;
namespace Library.ParserLib.statements
{
    public abstract class Statement : IExecutable
    {
        public abstract void Execute(ExeContext context);
    }
}
