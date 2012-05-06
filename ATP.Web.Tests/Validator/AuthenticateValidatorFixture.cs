using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ATP.Web.Resources;
using ATP.Web.Validators;
using NUnit.Framework;

namespace ATP.Web.Tests.Validator
{
    [TestFixture]
    public class AuthenticateValidatorFixture
    {
        private Authenticate _authenticateResource;
        private AuthenticateValidator _sut;

        [SetUp]
         public void SetUp()
         {
           _authenticateResource = new Authenticate {ConfirmPassword = "password", Password = "password", Email = "username"};
            _sut = new AuthenticateValidator();
         }

        [Test]
        public void passwords_must_match()
        {
            _authenticateResource.ConfirmPassword = "password";
            _authenticateResource.Password = "badpassword";

            var result = _sut.Validate(_authenticateResource);
            Assert.IsTrue(result.Any(e=>e.Field == "ConfirmPassword"));
        }

        [Test]
        public void validator_requires_authenticate_resource()
        {
            var l = new Web.Resources.List();
    
            Assert.Throws<ArgumentException>(() => _sut.Validate(l));
        }

        [Test]
        public void email_required()
        {
            _authenticateResource.Email = String.Empty;
            var result = _sut.Validate(_authenticateResource);
            Assert.IsTrue( result.Any(e=>e.Field=="Username"));
        }

        [Test]
        public void password_required()
        {
            _authenticateResource.Password = String.Empty;
            var result = _sut.Validate(_authenticateResource);
            Assert.IsTrue(result.Any(e => e.Field == "Password"));
        }
    }
}
