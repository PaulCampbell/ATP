using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATP.Web.Models;
using NUnit.Framework;

namespace ATP.Web.Tests.Models
{
    [TestFixture]
    public class UserFixture
    {

        [Test]
        public void full_name_property_is_sensible()
        {
            var u = new User { FirstName = "Jimmy", LastName = "Nail" };
            Assert.AreEqual("Jimmy Nail", u.FullName);
        }
    }
}
