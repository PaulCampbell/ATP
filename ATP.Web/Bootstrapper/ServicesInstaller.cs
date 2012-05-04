
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATP.Domain;
using ATP.Web.Infrastructure;
using ATP.Web.Validators;
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
                Component.For<IAutomapper>().ImplementedBy<Automapper>(), 
                Component.For<IAuthenticationService>().ImplementedBy<AuthenticationService>(), 
                Component.For<IPasswordHasher>().ImplementedBy<PasswordHasher>(),
                Component.For<IValidationRunner>().ImplementedBy<ValidationRunner>()
            );
        }
    }
}