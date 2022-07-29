using Library.LexerLib;
using Library.ParserLib;
using Library.ParserLib.blocks;
using Library.ParserLib.Expr;
using Library.ParserLib.statements;

namespace Library.InterpretLib
{
    public delegate void PrintCallback(string text);
    public class Interpret
    {
        public static PrintCallback Print { get; set; }
        private Parser Parser { get; set; }
        private ExeContext Context { get; set; }
        

        public Interpret(string source) { 
            Parser = new Parser(new Lexer(source));
            Context = Parser.GetExeContext();
        }

    
        public void Run()
        {
            Block program = Parser.ParseProgram();
            program.Execute(Context);
        }

        //public void Print(PrintCallback print) {
        //    print(this);
        //   // Console.WriteLine(output.ToString());
        //}

    }
}