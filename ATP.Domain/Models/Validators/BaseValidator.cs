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
        private T _entity;

        protected BaseValidator(T entity)
        {
            _entity = entity;
        }

        public abstract void Validate();
    }
}
