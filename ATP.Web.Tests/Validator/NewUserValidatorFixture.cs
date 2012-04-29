using System;
using ATP.Web.Validators;
using NUnit.Framework;

namespace ATP.Web.Tests.Validator
{
    [TestFixture]
    public class NewUserValidatorFixture
    {
        private NewUserValidator _validator;

        [Test]
        public void validator_requires_user_resource()
        {
            var l = new Web.Resources.List();
            var sut = new NewUserValidator();

            Assert.Throws<ArgumentException>(() => sut.Validate(l));
        }

        [Test]
        public void email_address_required()
        {
            var user = DataGenerator.GenerateWebModelUser();
            user.Email = String.Empty;
            var sut = new NewUserValidator();
            sut.Validate(user);
            Assert.IsFalse(sut.IsValid());
        }

     
    }
}
