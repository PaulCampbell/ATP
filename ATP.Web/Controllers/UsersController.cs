using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ATP.Web.Infrastructure;
using ATP.Web.Models;
using AutoMapper;
using Raven.Client;

namespace ATP.Web.Controllers
{
    public class UsersController : BaseController
    {
        private IAutomapper _automapper;
        // GET /api/users
        public UsersController(IDocumentSession documentSession, IAutomapper automapper) : base(documentSession)
        {
            _automapper = automapper;
        }

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET /api/users/5
        public User Get(int id)
        {
            var user = DocumentSession.Load<ATP.Domain.Models.User>(id);
            if(user!=null)
            {
                return _automapper.Map<Domain.Models.User, User>(user); 
            }
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        // POST /api/users
        public void Post(User user)
        {
        }

        // PUT /api/users/5
        public void Put(int id, string value)
        {
        }

        // DELETE /api/users/5
        public void Delete(int id)
        {
        }
    }
}
