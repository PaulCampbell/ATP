using Raven.Client;
using Raven.Client.Embedded;

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
                             DataDirectory = "RavenData"
                         };

            Store.Initialize();

            Session = Store.OpenSession();

            var user = DataGenerator.GenerateDomainModelUser();

            Session.Store(user);
            Session.SaveChanges();

            var list = DataGenerator.GenereateDomainModelList();

            Session.Store(list);
            Session.SaveChanges();
        }
    }
}
