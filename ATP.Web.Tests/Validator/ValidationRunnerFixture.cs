using System;
using System.Collections.Generic;
using System.Linq;
using ATP.Web.Resources;
using ATP.Web.Validators;
using NSubstitute;
using NUnit.Framework;

namespace ATP.Web.Tests.Validator
{
    [TestFixture]
    public class ValidationRunnerFixture
    {
        [Test]
        public void calling_validate_runs_the_validator()
        {
            var user = new User();
            var validator = Substitute.For<IValidator>();
            var sut = new ValidationRunner(validator, user);
            sut.RunValidation();
            validator.Received().Validate(user);

        }
    }
}
