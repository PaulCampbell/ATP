using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATP.Domain.Models.Validators
{
    public class ValidationError
    {
      
        public string Resource { get; set; }
        public string Field { get; set; }
        public ErrorCode Code { get; set; }
        public string Message { get; set; }

        public ValidationError()
        {
            Code = ErrorCode.Unknown;
        }
        

        public enum ErrorCode
        {
            Unknown,
            Missing,
            MissingField,
            Invalid,
            AlreadyExists
        }
    }
}
