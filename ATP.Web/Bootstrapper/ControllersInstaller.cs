using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using ATP.Web.Controllers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace ATP.Web.Bootstrapper
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes
                .FromThisAssembly()
                .BasedOn<IHttpController>()
                .Configure(c => c.LifeStyle.Transient
                .Named(c.Implementation.FullName))
                );

            foreach (Type controller in typeof(UsersController).Assembly.GetTypes().Where(type => typeof(IHttpController).IsAssignableFrom(type)))
            {
                var name = controller.Name.Replace("Controller", "");

                container.Register(Component
                    .For(controller)
                    .Named(name)
                    .LifestylePerWebRequest());
            }
        }
    }
}