using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATP.Web.Bootstrapper;
using AutoMapper;
using NUnit.Framework;

namespace ATP.Web.Tests.Bootstrapper
{
    [TestFixture]
    public class AutoMapperConfigerationFixture
    {
        [Test]
        public void test_automapper_mappings()
        {
            AutoMapperConfiguration.Init();
            Mapper.AssertConfigurationIsValid();
        }
    }
}
