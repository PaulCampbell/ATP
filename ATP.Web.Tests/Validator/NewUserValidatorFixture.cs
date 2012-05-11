using System;
using System.Linq;
using ATP.PersistenceTests;
using ATP.Web.Validators;
using NUnit.Framework;

namespace ATP.Web.Tests.Validator
{
    [TestFixture]
    public class NewUserValidatorFixture : RavenSessionTest
    {
        [Test]
        public void validator_requires_user_resource()
        {
            var l = new Web.Resources.List();
            var sut = new NewUserValidator(Session);

            Assert.Throws<ArgumentException>(() => sut.Validate(l));
        }

        [Test]
        public void no_email_address_adds_error()
        {
            var user = DataGenerator.GenerateWebModelUser();
            user.Email = String.Empty;
            var sut = new NewUserValidator(Session);
            var errorList = sut.Validate(user);
            Assert.IsTrue(errorList.Any(x=>x.Field=="Email"));
            Assert.AreEqual(1, errorList.Count);
        }

        [Test]
        public void duplicate_email_address_adds_error()
        {
            var user = DataGenerator.GenerateWebModelUser();
            user.Email = "abc@d.org";
            var sut = new NewUserValidator(Session);
            var errorList = sut.Validate(user);
            Assert.IsTrue(errorList.Any(x => x.Field == "Email"));
            Assert.AreEqual(1, errorList.Count);
        }    
    }
}
