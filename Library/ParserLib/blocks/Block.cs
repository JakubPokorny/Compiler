using Library.InterpretLib;
using Library.ParserLib.statements;

namespace Library.ParserLib.blocks
{
    public class Block : IExecutable
    {
        private LinkedList<Variable> vars;
        private LinkedList<Statement> statements;
        private LinkedList<FunctionStatement> functions;
        public Block()
        {
            statements = new LinkedList<Statement>();
            vars = new LinkedList<Variable>();
            functions = new LinkedList<FunctionStatement>();
        }

        public void AddStatement(Statement statement)
        {
                statements.AddLast(statement);
        }
        public void AddVars(LinkedList<Variable> variables)
        {
            foreach (Variable var in variables)
                vars.AddLast(var);
        }
        public void AddFunctions(FunctionStatement func)
        {
            functions.AddLast(func);
        }

        public void Execute(ExeContext context) {
            context.AddVariables(vars);

            if (context.global != null && functions.Count > 0)
                throw new Exception("Closures not implemented");
            
            foreach (FunctionStatement func in functions)
                context.AddProgramContext(func);

            foreach (Statement statement in statements)
                statement.Execute(context);
        }

    }
}
