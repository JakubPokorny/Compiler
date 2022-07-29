using Library.ParserLib.Expr;
namespace Library.InterpretLib
{
    public enum DataType
    {
        INT,
        DOUBLE,
        STRING,
    }


    public class Variable
    {
        public string Identifier { get; private set; }
        public object? Value { get; set;}
        public DataType DataType { get; private set; }

        public Variable(string identifier, object? value, DataType dataType)
        {
            Identifier = identifier;
            Value = value;
            DataType = dataType;
        }
        public static DataType? IsDataType(string token) {
            foreach (DataType dataType in Enum.GetValues(typeof(DataType)))
            {
                if (token ==  dataType.ToString())
                    return dataType;
            }
            return null;
        }
        public static bool IsNumber(DataType? type) {
            if (type == null)
                return false;
            return type == DataType.INT || type == DataType.DOUBLE;
        }

        public bool IsDataType(object value)
        {
            switch (this.DataType)
            {
                case DataType.INT:
                    return value is int;
                case DataType.DOUBLE:
                    return value is double;
                case DataType.STRING:
                    return value is string;
                default:
                    return false;
            }
        }
    }
}
