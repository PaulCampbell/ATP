using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using ATP.Domain;
using ATP.Web.Infrastructure;
using ATP.Web.Resources;
using ATP.Web.Validators;
using Raven.Client;

namespace ATP.Web.Controllers
{
    public class ListsController : PagableSortableResourceController<Domain.Models.List, Web.Resources.List>
    {
        private readonly IAutomapper _automapper;
        private readonly IValidationRunner _validationRunner;

        public ListsController(IDocumentSession documentSession, IAutomapper automapper,
            IValidationRunner validationRunner)
            : base(documentSession, automapper)
        {
            _automapper = automapper;
            _validationRunner = validationRunner;
           
        }
        
        public HttpResponseMessage PlacesPost(int listId, Resources.Place place)
        {
            var list = DocumentSession.Load<Domain.Models.List>(listId);
            var responseObject = new UnprocessableEntity();
            if (list == null)
                return new HttpResponseMessage<UnprocessableEntity>(responseObject)
                {
                    StatusCode = HttpStatusCode.NotFound
                };

            var domainPlace = _automapper.Map<Web.Resources.Place, Domain.Models.Place>(place);
            var validationErrors = _validationRunner.RunValidation(new NewPlaceValidator(), place);
            if (validationErrors.Any())
            {
                responseObject.AddRange(validationErrors);
                return new HttpResponseMessage<UnprocessableEntity>(responseObject)
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            domainPlace.List = "lists/" + list.Id;
            DocumentSession.Store(domainPlace);
            list.AddPlace(domainPlace);              
            DocumentSession.Store(list);
            DocumentSession.SaveChanges();
            return new HttpResponseMessage<Resources.Place>(place)
            {
                StatusCode = HttpStatusCode.Created
            };
        }

       

    }
}
