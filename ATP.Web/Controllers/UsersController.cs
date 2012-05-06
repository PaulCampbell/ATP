using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ATP.Domain;
using ATP.Web.Infrastructure;
using ATP.Web.Resources;
using ATP.Web.Validators;
using Raven.Client;

namespace ATP.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IAutomapper _automapper;
        private readonly IAuthenticationService _authenticationService;
        private readonly IValidationRunner _validationRunner;
        // GET /api/users
        public UsersController(IDocumentSession documentSession, IAutomapper automapper, 
            IAuthenticationService authenticationService,
            IValidationRunner validationRunner)
            : base(documentSession)
        {
            _automapper = automapper;
            _authenticationService = authenticationService;
            _validationRunner = validationRunner;
           
        }

        public HttpResponseMessage Get(int? resultsPerPage = 25, int? pageNumber = 1, 
                                       string sortBy = "Id", string sortDirection = "Desc")
        {
            Raven.Client.Linq.RavenQueryStatistics stats;

            var users = DocumentSession.Query<Domain.Models.User>().Statistics(out stats)
                .Skip(resultsPerPage.Value*(pageNumber.Value - 1))
                .Take(resultsPerPage.Value).ToList();

            var totalResult = stats.TotalResults;

            var usersResource = _automapper.Map<List<Domain.Models.User>, List<User>>(users);

            var responseObject = new PagableSortableList<User>(totalResult, resultsPerPage.Value, 
                pageNumber.Value, usersResource, sortBy, sortDirection);

            return new HttpResponseMessage<PagableSortableList<User>>(responseObject) { StatusCode = HttpStatusCode.OK} ;
        }


        public HttpResponseMessage Get(int id)
        {
            var user = DocumentSession.Load<Domain.Models.User>(id);
            if(user!=null)
            {
                var u = _automapper.Map<Domain.Models.User, User>(user); 
                return new HttpResponseMessage<User>(u) { StatusCode = HttpStatusCode.OK};
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }


        public HttpResponseMessage Post(User user)
        {
            var domainUser = _automapper.Map<User, Domain.Models.User>(user);
            var passwordResult = _authenticationService.UpdatePassword(domainUser, user.Password);
            var responseObject = new UnprocessableEntity();
            switch(passwordResult)
            {
                case UpdatePasswordResult.unknown :
                case UpdatePasswordResult.reservedWord:
                case UpdatePasswordResult.notLongEnough:
                    responseObject.AddError(
                        new Error{Field = "Password", 
                            Message = "Invalid Password", 
                            Code = ErrorCode.Invalid,
                            Resource = user.Uri});
                    break;
            }
            var validationErrors = _validationRunner.RunValidation(new NewUserValidator(DocumentSession), user);
            if (validationErrors!=null)
                responseObject.AddRange(validationErrors);

            if (responseObject.Errors.Any())
            {
                return new HttpResponseMessage<UnprocessableEntity>(responseObject)
                           {
                               StatusCode = HttpStatusCode.BadRequest
                           };
            }

            DocumentSession.Store(domainUser);
            DocumentSession.SaveChanges();
            return new HttpResponseMessage<User>(user)
            {
                StatusCode = HttpStatusCode.Created
            };
        }


        public HttpResponseMessage Put(int id, User user)
        {
            if(user==null)
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest
                };

            var domainUser = DocumentSession.Load<Domain.Models.User>(id);
            var responseObject = new UnprocessableEntity();
            
            if (domainUser == null)
                return new HttpResponseMessage<UnprocessableEntity>(responseObject)
                           {
                               StatusCode = HttpStatusCode.NotFound
                           };

            var validationErrors = _validationRunner.RunValidation(new NewUserValidator(DocumentSession), user);
            
            if(validationErrors.Any())
            {
                responseObject.AddRange(validationErrors);
                return new HttpResponseMessage<UnprocessableEntity>(responseObject)
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            
            return  new HttpResponseMessage { StatusCode = HttpStatusCode.OK};
        } 
    }
}
