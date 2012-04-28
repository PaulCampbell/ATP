using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATP.Domain.Models;
using NUnit.Framework;
using Raven.Client.Embedded;

namespace ATP.Domain.Tests
{
    [TestFixture]
    public class Persistence
    {

        [Test]
        public void can_persist_and_query_documents()
        {
            using (var store = new EmbeddableDocumentStore
                {
                    RunInMemory = true,
                    DataDirectory = "RavenData"
                }
            )
            {

                store.Initialize();

                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Email = "test@test.co.uk",
                        FirstName = "Micky",
                        LastName = "Bubbles",
                    };

                    session.Store(user);
                    session.SaveChanges();
                    Assert.AreEqual(true, session.Query<User>().Where(x => x.Email == "test@test.co.uk").Any());

                }

            }
        }
    }
}
