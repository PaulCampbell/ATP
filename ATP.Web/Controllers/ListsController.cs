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
    public class ListsController : BaseController
    {
        private readonly IAutomapper _automapper;
        private readonly IValidationRunner _validationRunner;

        public ListsController(IDocumentSession documentSession, IAutomapper automapper,
            IValidationRunner validationRunner)
            : base(documentSession)
        {
            _automapper = automapper;
            _validationRunner = validationRunner;
           
        }

        public HttpResponseMessage Get(int id)
        {
            var list = DocumentSession.Load<Domain.Models.List>(id);
            if (list != null)
            {
                var l = _automapper.Map<Domain.Models.List, List>(list);
                return new HttpResponseMessage<List>(l) { StatusCode = HttpStatusCode.OK };
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

    }
}
