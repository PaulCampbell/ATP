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
    public class UserFixture
    {
        [Test]
        public void user_inherits_from_entity()
        {
            var u = new User();
            Assert.IsTrue(u is Entity);
        }

        [Test]
        public void add_list_with_unique_name_for_this_user_addsAList()
        {
            var u = new User();
            var list = new List { Name = "My new list" };

        }
    }
}
