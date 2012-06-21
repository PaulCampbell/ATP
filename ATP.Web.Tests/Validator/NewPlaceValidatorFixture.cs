using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ATP.PersistenceTests;
using ATP.Web.Validators;

namespace ATP.Web.Tests.Validator
{
    [TestFixture]
    public class NewPlaceValidatorFixture 
    {
        [Test]
        public void validator_requires_place_resource()
        {
            var l = new Web.Resources.List();
            var sut = new NewPlaceValidator();

            Assert.Throws<ArgumentException>(() => sut.Validate(l));
        }

        [Test]
        public void name_required()
        {
            var p = new Web.Resources.Place();
            var sut = new NewPlaceValidator();

            var errorList = sut.Validate(p);
            Assert.IsTrue(errorList.Any(x => x.Field == "Name"));
            Assert.AreEqual(1, errorList.Count);
        }
    }
}
