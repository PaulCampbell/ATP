using System.Collections.Generic;
using System.Linq;
using ATP.Web.Resources;

namespace ATP.Web.Validators
{
    public interface IValidator
    {
        List<Error> Validate(Resource resource);
      
    }

    public abstract class BaseValidator<T> : IValidator
    {
        protected BaseValidator()
        {
            Errors = new List<Error>();
        }

        public abstract List<Error> Validate(Resource resource);

        public bool IsValid()
        {
            return !Errors.Any();
        }

        public List<Error> Errors { get; set; }
    }
}
