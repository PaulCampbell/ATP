using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using ATP.Domain;
using ATP.Web.Resources;
using ATP.Web.Validators;
using Raven.Client;

namespace ATP.Web.Controllers
{
    public class AuthenticateController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IValidationRunner _validationRunner;

        public AuthenticateController(IDocumentSession documentSession,
            IAuthenticationService authenticationService,
            IValidationRunner validationRunner) : base(documentSession)
        {
            _authenticationService = authenticationService;
            _validationRunner = validationRunner;
        }


        public HttpResponseMessage Post(Authenticate authResource)
        {
            var responseObject = new UnprocessableEntity();
            var validationErrors = _validationRunner.RunValidation(new NewUserValidator(DocumentSession), authResource);
            if (validationErrors.Any())
            {
                responseObject.AddRange(validationErrors);
                return new HttpResponseMessage<UnprocessableEntity>(responseObject) { StatusCode = HttpStatusCode.BadRequest };
            }

            var loginResult = _authenticationService.Login(authResource.Email, authResource.Password);
            if(loginResult != LoginResult.successful)
            {
                responseObject.AddError(
                    new Error {Code = ErrorCode.Invalid, Message = "Login unsuccesful"}
                    );
                return new HttpResponseMessage<UnprocessableEntity>(responseObject) { StatusCode = HttpStatusCode.BadRequest };
            }

            return new HttpResponseMessage<String>("SomeToken") { StatusCode = HttpStatusCode.Created};
        }

    }
}
