using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATP.Domain.Models.Validators
{
    public interface IValidator
    {
        void Validate();

    }

    public abstract class BaseValidator<T> : IValidator
    {
        protected T Entity;

        protected BaseValidator(T entity)
        {
            Entity = entity;
            Errors = new List<ValidationError>();
        }

        public abstract void Validate();

        public bool IsValid()
        {
            return !Errors.Any();
        }

        public List<ValidationError> Errors { get; set; }
    }
}
