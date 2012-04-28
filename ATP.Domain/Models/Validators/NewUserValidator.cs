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
            
        }
    }
}
