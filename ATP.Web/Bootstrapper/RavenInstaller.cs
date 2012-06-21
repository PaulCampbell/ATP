using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using ATP.Web.Infrastructure;

namespace ATP.Web.Bootstrapper
{
    public class RavenInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IDocumentStore>().Instance(CreateDocumentStore()).LifeStyle.Singleton,
                Component.For<IDocumentSession>().UsingFactoryMethod(GetDocumentSesssion).LifeStyle.PerWebRequest
                );
        }

        static IDocumentStore CreateDocumentStore()
        {
            var store = new DocumentStore
            {
                ConnectionStringName = "ATPConnection"
            };
            store.Initialize();
            IndexCreation.CreateIndexes(typeof(RavenIndexes).Assembly, store);
            return store;
        }

        static IDocumentSession GetDocumentSesssion(IKernel kernel)
        {
            var store = kernel.Resolve<IDocumentStore>();
            return store.OpenSession();
        }
    }
}