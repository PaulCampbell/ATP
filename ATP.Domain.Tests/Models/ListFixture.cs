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
        public void list_inherits_from_entity()
        {
            var l = new List();
            Assert.IsTrue(l is Entity);
        }

        [Test]
        public void can_add_new_place()
        {
            var p = new Place {Id = 1};
            var l = new List();
            l.AddPlace(p);

            Assert.AreEqual(1, l.Places.Count);
        }

        [Test]
        public void cannot_add_duplicate_places()
        {
            var p = new Place { Id = 1 };
            var l = new List();
            l.AddPlace(p);
            l.AddPlace(p);

            Assert.AreEqual(1, l.Places.Count);
        }
    }
   
}
