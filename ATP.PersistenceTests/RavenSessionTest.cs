using Raven.Client;
using Raven.Client.Embedded;
using ATP.Web.Infrastructure;
using Raven.Client.Indexes;

namespace ATP.PersistenceTests
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
                             DataDirectory = "RavenData",
                         };

            Store.Initialize();
            IndexCreation.CreateIndexes(typeof(RavenIndexes).Assembly, Store);
            Session = Store.OpenSession();

            var user = DataGenerator.GenerateDomainModelUser();

            Session.Store(user);
            Session.SaveChanges();


            var list = DataGenerator.GenereateDomainModelList();

            Session.Store(list);
            Session.SaveChanges();

            var p1 = new Domain.Models.Place
            {
                Description =
                    "Nice selection of guest ales, Live dodgy eighties rock bands. Perfect.",
                Latitude = 52.002324f,
                Longitude = -0.5734f,
                Name = "The Duck and Drake",
                List = "lists/1"
            };
            var p2 = new Domain.Models.Place
            {
                Description =
                    "Kinda trendy place - multiple rooms, decent beer from Leeds brewary and guests",
                Latitude = 52.002324f,
                Longitude = -0.5734f,
                Name = "The Adelphi",
                List = "lists/1"
            };

            Session.Store(p1);
            Session.Store(p2);
            Session.SaveChanges();


        }
    }
}
