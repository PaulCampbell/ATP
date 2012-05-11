using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ATP.Domain.Models;
using List = ATP.Domain.Models.List;

namespace ATP.Domain.Tests
{
    [TestFixture]
    public class ListFixture
    {
        [Test]
        public void user_inherits_from_entity()
        {
            var l = new List();
            Assert.IsTrue(l is Entity);
        }

    }
   
}
