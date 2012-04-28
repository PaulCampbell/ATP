using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATP.Web.Models
{
    public class UnprocessableEntity
    {
        public string Message { get; set; }
        public List<Error> Errors { get; protected set; } 
       
        public UnprocessableEntity()
        {
            Message = "Validation Failed";
            Errors = new List<Error>();
        }

        public void AddError(Error error)
        {
            Errors.Add(error);
        }

        public void AddRange(List<Error> errors )
        {
            Errors.AddRange(errors);
        }
    }

    public class Error
    {
        public string Resource { get; set; }
        public string Field { get; set; }
        public ErrorCode Code { get; set; }
        public string Message { get; set; }

        public Error()
        {
            Code = ErrorCode.Unknown;
        }
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