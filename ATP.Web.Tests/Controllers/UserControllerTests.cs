using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using ATP.Domain;
using ATP.PersistenceTests;
using ATP.Web.Controllers;
using ATP.Web.Infrastructure;
using ATP.Web.Resources;
using ATP.Web.Validators;
using NSubstitute;
using NUnit.Framework;
using User = ATP.Domain.Models.User;

namespace ATP.Web.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests : RavenSessionTest
    {
        private UsersController _usersController;
        private IAutomapper _automapper;
        private IAuthenticationService _authenticationService;
        private IValidationRunner _validationRunner;
        private int _userId;

        [SetUp]
        public void OneTimeSetup()
        {
            _userId = Session.Query<User>().FirstOrDefault().Id;
            _automapper = Substitute.For<IAutomapper>();
            _authenticationService = Substitute.For<IAuthenticationService>();
            _validationRunner = Substitute.For<IValidationRunner>();
            _usersController = new UsersController(Session, _automapper, _authenticationService, _validationRunner);
        }

        [Test]
        public void get_invalid_user_returns_404()
        {
            var result = _usersController.Get(1001);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public void get_valid_user_returns_200()
        {
            _automapper.Map<User, Web.Resources.User>(Arg.Any<User>()).ReturnsForAnyArgs(new Web.Resources.User());
            var result = _usersController.Get(_userId);  

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void get_valid_user_maps_the_user_to_web_model()
        {
            const string userEmail = "abc@d.org";
            _automapper.Map<User, Web.Resources.User>(Arg.Any<User>()).ReturnsForAnyArgs(new Web.Resources.User());
            _usersController.Get(_userId);

            _automapper.Received().Map<User, Web.Resources.User>(Arg.Is<User>(user => user.Email == userEmail));
        }

        [Test]
        public void get_valid_user_returns_model_of_type_WebModelsUser()
        {
            _automapper.Map<User, Web.Resources.User>(Arg.Any<User>()).ReturnsForAnyArgs(new Web.Resources.User());
            _automapper.Map<List<Domain.Models.List>, List<Web.Resources.List>>(Arg.Any<List<Domain.Models.List>>()).ReturnsForAnyArgs(new List<Web.Resources.List>());

            var u = _usersController.Get(_userId);

            Assert.IsTrue(u.Content is ObjectContent<Web.Resources.User>);
        }

        [Test]
        public void get_valid_user_returns_pagable_list_of_list_resources()
        {
            _automapper.Map<User, Web.Resources.User>(Arg.Any<User>()).ReturnsForAnyArgs(new Web.Resources.User());
            _automapper.Map<List<Domain.Models.List>, List<Web.Resources.List>>(Arg.Any<List<Domain.Models.List>>()).ReturnsForAnyArgs(new List<Web.Resources.List>());

            var u = _usersController.Get(_userId);

            _automapper.Received().Map<List<Domain.Models.List>, List<Web.Resources.List>>(Arg.Any<List<Domain.Models.List>>());
        }
      

        [Test]
        public void get_all_returns_pagable_sortable_list_of_users()
        {
            _automapper.Map<List<User>, List<Web.Resources.User>>(Arg.Any<List<User>>()).Returns(new List<Web.Resources.User>());

            var result = _usersController.Get();

            Assert.IsTrue(result.Content is ObjectContent<PagableSortableList<Web.Resources.User>>);
        }

        [Test]
        public void post_valid_user_persists_new_document()
        {
            var user = DataGenerator.GenerateWebModelUser();
            _automapper.Map<Web.Resources.User, User>(user).Returns(DataGenerator.GenerateDomainModelUser());
            _authenticationService.UpdatePassword(Arg.Any<User>(), user.Password).ReturnsForAnyArgs(UpdatePasswordResult.successful);

            _usersController.Post(user);

            var storedUser = _usersController.DocumentSession.Query<User>().FirstOrDefault(x => x.Email == "abc@d.org");
            Assert.AreEqual(user.FirstName, storedUser.FirstName);
        }

        [Test]
        public void post_user_no_emailAddress_returns_400()
        {
            var user = DataGenerator.GenerateWebModelUser();
            user.Email = string.Empty;
            _authenticationService.UpdatePassword(Arg.Any<User>(), user.Password).ReturnsForAnyArgs(UpdatePasswordResult.successful);
            _validationRunner.RunValidation(Arg.Any<NewUserValidator>(), Arg.Any<Web.Resources.User>())
                .Returns(new List<Error>
                            {
                                new Error {Code = ErrorCode.MissingField, Field = "Email"}
                            }
                         );

            var response = _usersController.Post(user);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public void post_valid_returns_201()
        {
            var user = DataGenerator.GenerateWebModelUser();
            _automapper.Map<Web.Resources.User, User>(user).Returns(DataGenerator.GenerateDomainModelUser());
            _authenticationService.UpdatePassword(Arg.Any<User>(), user.Password).ReturnsForAnyArgs(UpdatePasswordResult.successful);

            var response = _usersController.Post(user);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public void post_valid_calls_update_password()
        {
            var user = DataGenerator.GenerateWebModelUser();
            _automapper.Map<Web.Resources.User, User>(user).Returns(DataGenerator.GenerateDomainModelUser());
            _usersController.Post(user);

            _authenticationService.Received().UpdatePassword(Arg.Any<User>(), user.Password);
        }

        [Test]
        public void post_invalid_returns_400()
        {
            var user = DataGenerator.GenerateWebModelUser();
            _automapper.Map<Web.Resources.User, User>(user).Returns(DataGenerator.GenerateDomainModelUser());
            _authenticationService.UpdatePassword(Arg.Any<User>(), user.Password).ReturnsForAnyArgs(UpdatePasswordResult.notLongEnough);

            var response = _usersController.Post(user);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }
        [Test]
        public void post_invalid_returns_UnprocessablEntity()
        {
            var user = DataGenerator.GenerateWebModelUser();
            _automapper.Map<Web.Resources.User, User>(user).Returns(DataGenerator.GenerateDomainModelUser());
            _authenticationService.UpdatePassword(Arg.Any<User>(), user.Password).ReturnsForAnyArgs(UpdatePasswordResult.notLongEnough);

            var response = _usersController.Post(user);
            Assert.IsTrue(response.Content is ObjectContent<UnprocessableEntity>);

        }

        [Test]
        public void put_non_existent_user_returns404()
        {
            var user = DataGenerator.GenerateWebModelUser();
            user.Id = 9000000;

            var result = _usersController.Put(9000000, user);

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode );
        }

        [Test]
        public void put_null_user_returns_400()
        {
            var result = _usersController.Put(1, null);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void put_invalid_user_returns_400()
        {
            var user = DataGenerator.GenerateWebModelUser();
            user.Email = String.Empty;
            _validationRunner.RunValidation(Arg.Any<IValidator>(), Arg.Any<Resource>()).Returns(new List<Error>() { new Error {Code = ErrorCode.MissingField, Field = "Email", Message = "Email missing"}});
            var result = _usersController.Put(1, user);

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void put_invalid_user_returns_correct_errors()
        {
            var user = DataGenerator.GenerateWebModelUser();
            user.Email = String.Empty;
            _validationRunner.RunValidation(Arg.Any<IValidator>(), Arg.Any<Resource>()).Returns(new List<Error>() { new Error { Code = ErrorCode.MissingField, Field = "Email", Message = "Email missing" } });
            var result = _usersController.Put(1, user);

            Assert.IsTrue(result.Content is ObjectContent<UnprocessableEntity>);
      
        }
        
    }
}
