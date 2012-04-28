using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATP.Domain.Models;
using NSubstitute;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;

namespace ATP.Web.Tests
{
    public class RavenSessionTest
    {
        protected EmbeddableDocumentStore Store;
        protected IDocumentSession Session;

        public RavenSessionTest()
        {
            Store = new EmbeddableDocumentStore
                         {
                             RunInMemory = true,
                             DataDirectory = "RavenData"
                         };

            Store.Initialize();

            Session = Store.OpenSession();

            var user = new User
                           {
                               Email = "test@decoratedworld.co.uk",
                               FirstName = "Lola",
                               LastName = "Dog",
                               HashedPassword = "hashedpassword"
                           };

            Session.Store(user);
            Session.SaveChanges();
        }
    }
}
