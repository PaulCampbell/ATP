using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using ATP.Domain;
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
        private IAuthenticationService _authenticationService;
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

            var user = new User()
            {
                Email = "test@decoratedworld.co.uk",
                FirstName = "Lola",
                LastName = "Dog",
                HashedPassword = "hashedpassword"
            };

            session.Store(user);
            session.SaveChanges();
            _userId = user.Id;

            _automapper = Substitute.For<IAutomapper>();
            _authenticationService = Substitute.For<IAuthenticationService>();
            _usersController = new UsersController(session, _automapper, _authenticationService);
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
            _usersController.Get(_userId);

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
        public void post_valid_user_persists_new_document()
        {
            var user = GenerateWebModelUser();
            _automapper.Map<Web.Models.User, User>(user).Returns(GenerateDomainModelUser());
            _authenticationService.UpdatePassword(Arg.Any<User>(), user.Password).ReturnsForAnyArgs(UpdatePasswordResult.successful);

            _usersController.Post(user);

            var storedUser = _usersController.DocumentSession.Query<User>().FirstOrDefault(x => x.Email == "abc@d.org");
            Assert.AreEqual(user.FirstName, storedUser.FirstName);
        }

         [Test]
        public void post_valid_returns_201()
        {
            
        }

        [Test]
        public void post_valid_calls_update_password()
        {
            var user = GenerateWebModelUser();
            _automapper.Map<Web.Models.User, User>(user).Returns(GenerateDomainModelUser());
            _usersController.Post(user);

            _authenticationService.Received().UpdatePassword(Arg.Any<User>(), user.Password);
        }

        [Test]
        public void post_invalid_password_returns_400_with_reason()
        {
            var user = GenerateWebModelUser();
            _automapper.Map<Web.Models.User, User>(user).Returns(GenerateDomainModelUser());
            _authenticationService.UpdatePassword(Arg.Any<User>(), user.Password).ReturnsForAnyArgs(UpdatePasswordResult.notLongEnough);

            var response = _usersController.Post(user);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Test]
        public void put_non_existent_user_returns404()
        {
            
        }

        private Web.Models.User GenerateWebModelUser()
        {
            return new Web.Models.User
            {
                Email = "abc@d.org",
                FirstName = "Bill",
                LastName = "Grey",
                Password = "password",
                MobileNumber = "0777777777"
            };
        }

        private Domain.Models.User GenerateDomainModelUser()
        {
            return new Domain.Models.User
            {
                Email = "abc@d.org",
                FirstName = "Bill",
                LastName = "Grey",
                MobileNumber = "0777777777"
            };
        }

    }
}
