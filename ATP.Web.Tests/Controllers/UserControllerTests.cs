using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using ATP.Domain.Models;
using ATP.Web.Controllers;
using ATP.Web.Infrastructure;
using NSubstitute;
using NUnit.Framework;
using Raven.Client.Embedded;

namespace ATP.Web.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private UsersController _usersController;
        private IAutomapper _automapper;
        private int _userId;

        [SetUp]
        public void OneTimeSetup()
        {
            var store = new EmbeddableDocumentStore
            {
                RunInMemory = true,
                DataDirectory = "RavenData"
            };

            store.Initialize();

            var session = store.OpenSession();

            var user = new ATP.Domain.Models.User()
            {
                Email = "test@decoratedworld.co.uk",
                FirstName = "Lola",
                LastName = "Dog",
                HashedPassword = "hashedpassword"
            };

            session.Store(user);
            session.SaveChanges();
            _userId = user.Id;

            _automapper = NSubstitute.Substitute.For<IAutomapper>();
            _usersController = new UsersController(session, _automapper);
        }

        [Test]
        public void get_invalid_user_returns_404()
        {
            Assert.Throws<HttpResponseException>(()=>_usersController.Get(1001));
        }

        [Test]
        public void get_valid_user_maps_the_user_to_web_model()
        {
            const string userEmail = "test@decoratedworld.co.uk";
            _automapper.Map<User,Web.Models.User>(Arg.Any<User>()).ReturnsForAnyArgs(new Web.Models.User { Email = userEmail});
            var u = _usersController.Get(_userId);

            _automapper.Received().Map<User, Web.Models.User>(Arg.Is<User>(user => user.Email == userEmail));
        }

        [Test]
        public void get_valid_user_returns_model_of_type_WebModelsUser()
        {
            _automapper.Map<User, Web.Models.User>(Arg.Any<User>()).ReturnsForAnyArgs(new Web.Models.User());
            var u = _usersController.Get(_userId);

            Assert.IsTrue(u is Web.Models.User);
        }

        [Test]
        public void post_valid_creates_new_user()
        {
            
        }

         [Test]
        public void post_valid_returns_201()
        {
            
        }

        [Test]
        public void put_non_existent_user_returns404()
        {
            
        }

    }
}
