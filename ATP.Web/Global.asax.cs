using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Raven.Client.Document;
using Raven.Client;

namespace ATP.Web
{

    public class WebApiApplication : System.Web.HttpApplication
    {
        private const string RavenSessionKey = "RavenMVC.ATPSession";
        private static DocumentStore _documentStore;

        public WebApiApplication()
        {
            //Create a DocumentSession on BeginRequest  
            //create a document session for every unit of work
            BeginRequest += (sender, args) =>
                HttpContext.Current.Items[RavenSessionKey] = _documentStore.OpenSession();
            
            //Destroy the DocumentSession on EndRequest
            EndRequest += (o, eventArgs) =>
            {
            var disposable = HttpContext.Current.Items[RavenSessionKey] as IDisposable;
            if (disposable != null)
            disposable.Dispose();
            };
        }
 

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

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            _documentStore = new DocumentStore { Url = "http://localhost:8080/" };
            _documentStore.Initialize();

            AreaRegistration.RegisterAllAreas();

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.RegisterTemplateBundles();
        }

        public static IDocumentSession CurrentSession
        {
            get { return (IDocumentSession)HttpContext.Current.Items[RavenSessionKey]; }
        }
    }
}