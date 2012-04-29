using System;
using System.Collections.Generic;
using System.Linq;
using ATP.Web.Resources;
using Raven.Client;

namespace ATP.Web.Validators
{
    public class NewUserValidator : BaseValidator<User>
    {
        private IDocumentSession _documentSession;

        public  NewUserValidator(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public override List<Error> Validate(Resource resource)
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
            if (_documentSession.Query<Domain.Models.User>().FirstOrDefault<Domain.Models.User>(x => x.Email == user.Email) != null)
            {
                Errors.Add(
                    new Error
                    {
                        Field = "Email",
                        Message = "Email address already in use",
                        Code = ErrorCode.AlreadyExists,
                        Resource = user.Uri
                    });
            }

            return Errors;
        }
    }
}
