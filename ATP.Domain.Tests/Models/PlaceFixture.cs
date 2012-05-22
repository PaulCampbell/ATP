using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATP.Domain.Models;
using NUnit.Framework;
using List = NUnit.Framework.List;

namespace ATP.Domain.Tests.Models
{
    [TestFixture]
    public class PlaceFixture
    {
        [Test]
        public void place_inherits_from_entity()
        {
            var l = new Place();
            Assert.IsTrue(l is Entity);
        }
    }
}
