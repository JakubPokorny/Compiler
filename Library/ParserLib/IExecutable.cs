using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.InterpretLib;

namespace Library.ParserLib
{
    public interface IExecutable
    {
        public void Execute(ExeContext context);
    }
}
