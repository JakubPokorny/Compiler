namespace Library.InterpretLib
{
    public class Variables
    {
        private IDictionary<string, Variable> vars;

        public Variables()
        {
            vars = new Dictionary<string, Variable>();
        }

        public Variable? GetVariable(string key) {
            if (vars.TryGetValue(key, out Variable? variable))
                return variable;
            return null;
        }
        public void SetVariable(string key, Variable value) {
            if (vars.ContainsKey(key))
                vars[key] = value;
            else
                vars.Add(key, value);
        }
    }
}
