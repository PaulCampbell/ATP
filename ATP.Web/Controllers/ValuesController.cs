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

            var user = DocumentSession.Load<User>(2);
            var list = new List();
            list.User = "/users/2";

           var p = new Place
            {
                Description =
                    "Kinda trendy place - multiple rooms, decent beer from Leeds brewary and guests",
                Latitude = 52.002324f,
                Longitude = -0.5734f,
                Name = "The Adelphi"

            };


            DocumentSession.Store(p);
            DocumentSession.SaveChanges();
            list.AddPlace(p);
            DocumentSession.Store(list);
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