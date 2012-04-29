using System;
using ATP.Web.Resources;

namespace ATP.Web.Validators
{
    public class NewUserValidator : BaseValidator<User>
    {
        public override void Validate(Resource resource)
        {
            var user = resource as User;
            if(user==null) throw new ArgumentException();

            if (string.IsNullOrEmpty(user.Email))
            {
                Errors.Add(
                    new Error
                    {
                        Field = "Email",
                        Message = "Email address required",
                        Code = ErrorCode.Missing,
                        Resource = user.Uri
                    });
            }
        }
    }
}
