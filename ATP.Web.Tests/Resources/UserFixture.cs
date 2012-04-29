using ATP.Web.Resources;
using NUnit.Framework;

namespace ATP.Web.Tests.Resources
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
