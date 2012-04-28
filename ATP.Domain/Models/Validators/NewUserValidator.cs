using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATP.Domain.Models.Validators
{
    public class NewUserValidator : BaseValidator<User>
    {
        public NewUserValidator(User entity) : base(entity)
        {
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Entity.Email))
            {
                Errors.Add(
                    new ValidationError
                    {
                        Field = "Email",
                        Message = "Email address required",
                        Code = ValidationError.ErrorCode.Missing,
                        Resource = "/Users/" + Entity.Id
                    });
            }
        }
    }
}
