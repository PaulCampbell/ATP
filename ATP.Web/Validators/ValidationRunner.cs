using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATP.Web.Resources;

namespace ATP.Web.Validators
{
    public interface  IValidationRunner
    {
        void RunValidation();
    }

    public class ValidationRunner : IValidationRunner
    {
        private IValidator _validator;
        private Resource _resource;

        public ValidationRunner(IValidator validator, Resource resource)
        {
            _validator = validator;
            _resource = resource;
        }
        public void RunValidation()
        {
            _validator.Validate(_resource);
        }
    }
}