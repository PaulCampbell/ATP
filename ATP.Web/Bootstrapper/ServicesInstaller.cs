
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATP.Web.Infrastructure;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace ATP.Web.Bootstrapper
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IAutomapper>().Instance(new Automapper()).LifeStyle.PerWebRequest
                );
        }
    }
}