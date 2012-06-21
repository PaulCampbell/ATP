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
    public class UsersController : PagableSortableResourceController<Domain.Models.User, Web.Resources.User>
    {
        private readonly IAutomapper _automapper;
        private readonly IAuthenticationService _authenticationService;
        private readonly IValidationRunner _validationRunner;

        private const int DefaultUserListResultsSize = 25;

        // GET /api/users
        public UsersController(IDocumentSession documentSession, IAutomapper automapper, 
            IAuthenticationService authenticationService,
            IValidationRunner validationRunner)
            : base(documentSession, automapper)
        {
            _automapper = automapper;
            _authenticationService = authenticationService;
            _validationRunner = validationRunner;
           
        }

        public override HttpResponseMessage Get(int id)
        {
            var model = DocumentSession.Load<Domain.Models.User>(id);
            if (model != null)
            {
                var resource = _automapper.Map<Domain.Models.User, User>(model);

                Raven.Client.Linq.RavenQueryStatistics stats;
                var userId = "/users/" + model.Id;

                var userListsFirstPage = DocumentSession.Query<Domain.Models.List>().Statistics(out stats)
                    .Where(x => x.User == userId).OrderByDescending(x => x.Added)
                    .Take(DefaultUserListResultsSize).ToList();

                var userListResources = _automapper.Map < List<Domain.Models.List>, List<List>>(userListsFirstPage);

                if (userListResources== null) userListResources = new List<List>();

                resource.Lists = new PagableSortableList<List>(stats.TotalResults, DefaultUserListResultsSize, 1, userListResources, "Added", "Desc");

                return new HttpResponseMessage<User>(resource) { StatusCode = HttpStatusCode.OK };
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
            
            return  new HttpResponseMessage<Resources.User>(user) { StatusCode = HttpStatusCode.OK};
        } 
    }
}
