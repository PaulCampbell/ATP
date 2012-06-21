using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ATP.Web.Bootstrapper;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Raven.Client.Document;
using Raven.Client;
using Raven.Client.Indexes;

namespace ATP.Web
{

    public class WebApiApplication : System.Web.HttpApplication
    {
       
 

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapHttpRoute(
                name: "ListPlacesPost",
                routeTemplate: "lists/{listId}/places/{id}",
                defaults: new { listId = 0, controller = "Lists", action = "PlacesPost" },
                 constraints: new { httpMethod = new HttpMethodConstraint("POST") }
            );
        }
            

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            InitialiseContainer();
            InitialiseControllerFactory();

            AutoMapperConfiguration.Init();
        }

        protected void InitialiseContainer()
        {
            if (_container == null)
            {
                _container = new WindsorContainer()
                    .Install(FromAssembly.InDirectory(new AssemblyFilter(HttpRuntime.BinDirectory, "ATP.*.dll")));
            }
        }

        protected void InitialiseControllerFactory()
        {
            var configuration = GlobalConfiguration.Configuration;
            configuration.ServiceResolver.SetService(
                typeof(IHttpControllerFactory),
                new WindsorControllerFactory(configuration, Container.Kernel));
        }

        static IWindsorContainer _container;
        public IWindsorContainer Container
        {
            get { return _container; }
        }
   
    }
}