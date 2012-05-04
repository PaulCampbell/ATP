using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using ATP.Domain.Models;
using Microsoft.Isam.Esent.Interop;
using Raven.Client;

namespace ATP.Web.Controllers
{
    public class ValuesController : BaseController
    {

        public ValuesController(IDocumentSession documentSession) :
            base(documentSession)
        {
            
        }

        // GET /api/values
        public User Get()
        {

            var user = new User
            {
                Email = "test@decoratedworld.co.uk",
                FirstName = "Lola",
                LastName = "Dog",
                HashedPassword = "hashedpassword"
            };

            DocumentSession.Store(user);
            DocumentSession.SaveChanges();
            return user;
        }

        // GET /api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST /api/values
        public void Post(string value)
        {
        }

        // PUT /api/values/5
        public void Put(int id, string value)
        {
        }

        // DELETE /api/values/5
        public void Delete(int id)
        {
        }
    }
}