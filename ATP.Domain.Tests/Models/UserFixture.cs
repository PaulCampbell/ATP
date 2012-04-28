using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ATP.Domain.Models;

namespace ATP.Domain.Tests
{
    public class UserFixture
    {
        [Test]
        public void user_inherits_from_entity()
        {
            var u = new User();
            Assert.IsTrue(u is Entity);
        }

    }
}
