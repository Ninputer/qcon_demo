using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBF.Compilers;

namespace ErrorRecoveryCombinators
{
    public class SyntaxError
    {
        public const int MissingToken = 0;
        public const int UnexpectedToken = 1;
        
        public int Code { get; }
        public string Description { get; }
        public SourceSpan Location { get; }

        public SyntaxError(int code, string description, SourceSpan location)
        {
            Code = code;
            Description = description;
            Location = location;
        } 
    }
}
