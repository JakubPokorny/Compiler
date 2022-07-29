using Library.ParserLib.statements;
using Library.ParserLib.Expr;
namespace Library.InterpretLib
{
    public class ProgramContext
    {
        private IDictionary<string, FunctionStatement> functions;
        private IDictionary<string, Statement> libFunctions;
        public ProgramContext()
        {
            functions = new Dictionary<string, FunctionStatement>();
        }
        public object Call(string key, LinkedList<IExpression>? callParams, ExeContext context)
        {
            if (functions.ContainsKey(key))
            {
                FunctionStatement function = functions[key];

                //Validation parameters
                LinkedList<Variable>? defVariable = function.Parameters;
                LinkedListNode<Variable> varNode;
                object[]? values;
                if (callParams != null && defVariable != null)
                {
                    if (callParams.Count == defVariable.Count && defVariable.Count > 0)
                    {
                        values = new object[callParams.Count];
                        int index = 0;
                        LinkedListNode<IExpression> exprNode = callParams.First;
                        varNode = defVariable.First;
                        while (exprNode != null && varNode != null)
                        {
                            values[index] = exprNode.Value.Evaluate(context);

                            if (!varNode.Value.IsDataType(values[index++]))
                                throw new Exception($"Missmatch parameters datatypes {varNode.Value.DataType} != {values[index - 1].GetType()} in calling function");

                            exprNode = exprNode.Next;
                            varNode = varNode.Next;
                        }
                    }
                    else
                        throw new Exception("Missing parameters");
                }
                else if (callParams == null && defVariable == null)
                    values = null;
                else
                    throw new Exception("Missing parameters");

                ExeContext lokalContext = new ExeContext(context);
                lokalContext.ProgramContext = lokalContext.global.ProgramContext;
                if (values != null)
                {
                    varNode = defVariable.First;
                    for (int i = 0; i < values.Length; i++)
                    {
                        lokalContext.Variables.SetVariable(varNode.Value.Identifier, new Variable(varNode.Value.Identifier, values[i], varNode.Value.DataType));
                        varNode = varNode.Next;
                    }
                }

                function.BodyBlock.Execute(lokalContext);
                return function.ReturnExpression.Evaluate(lokalContext);
            }
            else if (key == "vypis") {
                Interpret.Print(callParams.First.Value.Evaluate(context).ToString());
                return 1;
            }   
            else
            {
                throw new Exception($"Function {key} not found");
            }
        }

        public void SetFunction(FunctionStatement function)
        {
            if (functions.ContainsKey(function.Identifier))
                throw new Exception($"Function {function.Identifier} already exist");

            functions.Add(function.Identifier, function);
        }

    }
}
