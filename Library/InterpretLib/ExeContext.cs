using Library.ParserLib.Expr;
using Library.ParserLib.statements;
namespace Library.InterpretLib
{
    public class ExeContext
    {
        public ExeContext? global;
        public Variables Variables { get; set; }
        public ProgramContext ProgramContext { get; set; }
        public ExeContext(ExeContext? global)
        {
            this.global = global;
            this.ProgramContext = new ProgramContext();
            this.Variables = new Variables();
        }
        
        public Variable GetVariable(string key)
        {
            Variable? var;
            if ((var = Variables.GetVariable(key)) != null)
                return var;
            else if (global != null && (var = global.Variables.GetVariable(key)) != null)
                return var;
            
            throw new Exception($" Variable \"{key}\" not found.");
        }
        public object CallFunction(string key, LinkedList<IExpression>? parameters)
        {
            return ProgramContext.Call(key, parameters, this);
        }

        public void AddProgramContext(Statement statement)
        {
            if (statement is FunctionStatement)
                ProgramContext.SetFunction((FunctionStatement)statement);
        }
        public void AddVariables(LinkedList<Variable> vars)
        {
            foreach (Variable var in vars)
            {
                Variables.SetVariable(var.Identifier, var);
            }
        }


    }


}
