using System;
using System.Linq;
using NUnit.Framework;
using NSubstitute;
using ATP.Domain.Models;
using Raven.Client;
using System.Linq.Expressions;
using Raven.Client.Embedded;
using Raven.Client.Listeners;

namespace ATP.Domain.Tests
{
    [TestFixture]
    public class AuthenticationServiceFixture
    {
        private AuthenticationService _authService;
        private IPasswordHasher _passwordHasher;
        private EmbeddableDocumentStore _store;
        private IDocumentSession _session;

        [SetUp]
        public void OneTimeSetup()
        {
            _passwordHasher = Substitute.For<IPasswordHasher>();
            _passwordHasher.ComputeHash("somestring", new byte[4]).ReturnsForAnyArgs("hashedPassword");
            _store = new EmbeddableDocumentStore
            {
                RunInMemory = true,
                DataDirectory = "RavenData"
            };

            _store.Initialize();
     
            _session = _store.OpenSession();

            var user = new User
            {
                Email = "test@decoratedworld.co.uk",
                FirstName = "Lola",
                LastName = "Dog",
                HashedPassword = "hashedpassword"
            };

            _session.Store(user);
            _session.SaveChanges();
            _authService = new AuthenticationService(_session, _passwordHasher);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _session.Dispose();
            _store.Dispose();
        }

        [Test]
        public void password_minimum_five_charaters()
        {
            var newPassword = "four";
            var result = _authService.UpdatePassword(new User(), newPassword);

            Assert.AreEqual(UpdatePasswordResult.notLongEnough, result);
        }

        [Test]
        public void valid_password_can_be_updated()
        {
            var newPassword = "aNewValidPassword";
            var result = _authService.UpdatePassword(new User(), newPassword);

            Assert.AreEqual(UpdatePasswordResult.successful, result);
        }

        [Test]
        public void wrong_email_address_cannot_log_in()
        {
          
            _authService = new AuthenticationService(_session, _passwordHasher);
            var result = _authService.Login("invalidAddress", "SomePassword");

            Assert.AreEqual(LoginResult.unsuccessful, result);
        }

        [Test]
        public void wrong_password_cannot_log_in()
        {
            _passwordHasher.VerifyHash("wrongpass", "SomeHash").ReturnsForAnyArgs(false);

            var result = _authService.Login("test@decoratedworld.co.uk", "SomePassword");

            Assert.AreEqual(LoginResult.unsuccessful, result);
        }

        [Test]
        public void correct_password_and_email_can_log_in()
        {

            _passwordHasher.VerifyHash("goodpassword", "hashedpassword").ReturnsForAnyArgs(true);

            var result = _authService.Login("test@decoratedworld.co.uk", "hashedpassword");

            Assert.AreEqual(LoginResult.successful, result);
        }
    }

}
