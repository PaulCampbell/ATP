using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATP.Web.Resources;
using NUnit.Framework;

namespace ATP.Web.Tests.Resources
{
    [TestFixture]
    public class PagableSortableListFixture
    {
       
        [Test]
        public void list_with_more_results_than_displayed_contains_next_link()
        {
            var sut = new PagableSortableList<User>(100, 1, 1, new List<User> { new User() }, "", "");

            Assert.IsTrue(sut.Actions.Any(a => a.Action == "Next"));
        }

        [Test]
        [Ignore("Sort out this querystring business")]
        public void list_with_more_results_than_displayed_contains_next_link_with_appropriate_querystring_values()
        {
            var sut = new PagableSortableList<User>(100, 1, 1, new List<User> {new User()}, "", "");

            var nextLink =  sut.Actions.First(a => a.Action == "Next");

            Assert.AreEqual("/user?PageNumber=2&ResultsPerPage=1", nextLink.Action);
        }

        [Test]
        public void list_on_page_2_contains_previous_link()
        {
            var sut = new PagableSortableList<User>(100, 5, 3, new List<User> { new User() }, "", "");

            Assert.IsTrue(sut.Actions.Any(a => a.Action == "Previous"));
        }
    }
}
