using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATP.Domain.Models;
using ATP.Domain.Models.Validators;
using ATP.Domain.Tests;
using NUnit.Framework;

namespace ATP.Domain.Tests.Models.Validator
{
    [TestFixture]
    public class NewUserValidatorFixture
    {
        private NewUserValidator _validator;

        [Test]
        public void email_address_required()
        {
            var user = DataGenerator.GenerateDomainModelUser();
            user.Email = String.Empty;
            var sut = new NewUserValidator(user);
            sut.Validate();
            Assert.IsFalse(sut.IsValid());
        }

     
    }
}
