using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using ATP.Domain;
using ATP.Web.Controllers;
using ATP.Web.Resources;
using ATP.Web.Validators;
using NSubstitute;
using NUnit.Framework;

namespace ATP.Web.Tests.Controllers
{
    [TestFixture]
    public class AuthenticateControllerFixture : RavenSessionTest
    {
        private AuthenticateController _sut;
        private IAuthenticationService _authenticationService;
        private IValidationRunner _validationRunner;
      
        [SetUp]
        public void OneTimeSetup()
        {
             _authenticationService = Substitute.For<IAuthenticationService>();
             _validationRunner = Substitute.For<IValidationRunner>();
            _sut = new AuthenticateController(Session, _authenticationService, _validationRunner);
        }

        [Test]
        public void post_calls_validation_runner()
        {
            _validationRunner.RunValidation(Arg.Any<IValidator>(), Arg.Any<Resource>()).Returns(
                new List<Error>());

            _sut.Post(new Authenticate());
            _validationRunner.Received().RunValidation(Arg.Any<IValidator>(), Arg.Any<Resource>());
        }

        [Test]
        public void post_invalid_returns_UnprocessablEntity()
        {
            _validationRunner.RunValidation(Arg.Any<IValidator>(), Arg.Any<Resource>()).Returns(
                new List<Error>
                    {
                        new Error{
                            Code = ErrorCode.MissingField
                        , Field = "Username"}
                    });

            var result = _sut.Post(new Authenticate());

            Assert.IsTrue(result.Content is ObjectContent<UnprocessableEntity>);
        }

        [Test]
        public void post_invalid_returns_400()
        {
            _validationRunner.RunValidation(Arg.Any<IValidator>(), Arg.Any<Resource>()).Returns(
                new List<Error>
                    {
                        new Error{
                            Code = ErrorCode.MissingField
                        , Field = "Username"}
                    });

            var result = _sut.Post(new Authenticate());

            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test]
        public void post_valid_details_returns_201()
        {
            _authenticationService.Login(Arg.Any<string>(), Arg.Any<string>()).Returns(LoginResult.successful);
            _validationRunner.RunValidation(Arg.Any<IValidator>(), Arg.Any<Resource>()).Returns(new List<Error>());

            var result = _sut.Post(new Authenticate());

            Assert.IsTrue(result.StatusCode == HttpStatusCode.Created);
        }

        [Test]
        public void post_valid_details_returns_session_token()
        {
            _authenticationService.Login(Arg.Any<string>(), Arg.Any<string>()).Returns(LoginResult.successful);
            _validationRunner.RunValidation(Arg.Any<IValidator>(), Arg.Any<Resource>()).Returns(new List<Error>());
            var result = _sut.Post(new Authenticate());

            Assert.IsTrue(result.Content is ObjectContent<String>);
        }

        [Test]
        public void post_invalid_password_returns_400()
        {
            _authenticationService.Login(Arg.Any<string>(), Arg.Any<string>()).Returns(LoginResult.unsuccessful);
            _validationRunner.RunValidation(Arg.Any<IValidator>(), Arg.Any<Resource>()).Returns(new List<Error>());
            var result = _sut.Post(new Authenticate());

            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
        }

    }
}
