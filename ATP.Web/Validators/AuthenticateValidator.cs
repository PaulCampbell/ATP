using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATP.Web.Resources;

namespace ATP.Web.Validators
{
    public class AuthenticateValidator : BaseValidator<Authenticate>
    {
        public override List<Error> Validate(Resource resource)
        {
            var authResource = resource as Authenticate;
            if (authResource == null) throw new ArgumentException();

            if(authResource.ConfirmPassword != authResource.Password)
                Errors.Add( new Error
                    {
                        Field = "ConfirmPassword",
                        Message = "Passwords must match",
                        Code = ErrorCode.Invalid
                    });

            if (string.IsNullOrEmpty(authResource.Email))
                Errors.Add(new Error
                {
                    Field = "Username",
                    Message = "Username required",
                    Code = ErrorCode.MissingField
                });

            if (string.IsNullOrEmpty(authResource.Password))
                Errors.Add(new Error
                {
                    Field = "Password",
                    Message = "Password required",
                    Code = ErrorCode.MissingField
                });

            return Errors;
        }
    }
}