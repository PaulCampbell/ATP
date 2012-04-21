using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Core.Resource;
using System.Reflection;
using Castle.Core;
using System.Web.Routing;
using Castle.MicroKernel.Registration;

namespace ATP.Web
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        WindsorContainer container;
      
        public WindsorControllerFactory()
        {
            // Instantiate a container, taking configuration from web.config
            container = new WindsorContainer();

            // Also register all the controller types as transient
            var controllerTypes =
                from t in Assembly.GetExecutingAssembly().GetTypes()
                where typeof(IController).IsAssignableFrom(t)
                select t;
            foreach (Type t in controllerTypes)
                container.AddComponentLifeStyle 
                    (t.FullName, t, LifestyleType.Transient);
        }

         protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return (IController)container.Resolve(controllerType);
        }
    }
}