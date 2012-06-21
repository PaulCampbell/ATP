using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using ATP.Web.Infrastructure;
using ATP.Web.Resources;
using Raven.Client;

namespace ATP.Web.Controllers
{
    public abstract class PagableSortableResourceController<TModel,TResource> : BaseController
    {
        private readonly IAutomapper _automapper;

        public PagableSortableResourceController(IDocumentSession documentSession, IAutomapper automapper)
            : base(documentSession)
        {
            _automapper = automapper;
        }

        public virtual HttpResponseMessage Get(int id)
        {
            var model = DocumentSession.Load<TModel>(id);
            if (model != null)
            {
                var resource = _automapper.Map<TModel, TResource>(model);
                return new HttpResponseMessage<TResource>(resource) { StatusCode = HttpStatusCode.OK };
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        public virtual HttpResponseMessage Get(int? resultsPerPage = 25, int? pageNumber = 1,
                                     string sortBy = "Id", string sortDirection = "Desc")
        {
            Raven.Client.Linq.RavenQueryStatistics stats;

            var models = DocumentSession.Query<TModel>().Statistics(out stats)
                .Skip(resultsPerPage.Value * (pageNumber.Value - 1))
                .Take(resultsPerPage.Value).ToList();

            var totalResult = stats.TotalResults;

            var resource = _automapper.Map<List<TModel>, List<TResource>>(models);

            var responseObject = new PagableSortableList<TResource>(totalResult, resultsPerPage.Value,
                pageNumber.Value, resource, sortBy, sortDirection);

            return new HttpResponseMessage<PagableSortableList<TResource>>(responseObject) { StatusCode = HttpStatusCode.OK };
        }


    }
}
