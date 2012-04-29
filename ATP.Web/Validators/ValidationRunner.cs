using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATP.Web.Resources;

namespace ATP.Web.Validators
{
    public interface  IValidationRunner
    {
        List<Error> RunValidation(IValidator validator, Resource resource);
    }

    public class ValidationRunner : IValidationRunner
    {
        
        public List<Error> RunValidation(IValidator validator, Resource resource)
        {
            return validator.Validate(resource);

        }
    }
}