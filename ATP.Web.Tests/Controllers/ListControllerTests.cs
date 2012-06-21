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
    public class ListControllerTests : RavenSessionTest
    {
        private ListsController _listController;
        private IAutomapper _automapper;
        private IValidationRunner _validationRunner;
        private int _listId;

        [SetUp]
        public void OneTimeSetup()
        {
            _listId = Session.Query<Domain.Models.List>().FirstOrDefault().Id;
            _automapper = Substitute.For<IAutomapper>();
            _validationRunner = Substitute.For<IValidationRunner>();
            _listController = new ListsController(Session, _automapper, _validationRunner);
        }

        [Test]
        public void get_invalid_list_returns_404()
        {
            var result = _listController.Get(1001);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public void get_valid_list_returns_200()
        {
            var result = _listController.Get(_listId);

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void get_valid_list_maps_the_list_to_web_model()
        {
            const string listName = "Leeds Pubs";
            _listController.Get(_listId);

            _automapper.Received().Map<Domain.Models.List, Web.Resources.List>(Arg.Is<Domain.Models.List>(list => list.Name == listName));
        }

        [Test]
        public void get_valid_list_returns_model_of_type_WebModelsList()
        {
            _automapper.Map<Domain.Models.List, Web.Resources.List>(Arg.Any<Domain.Models.List>()).ReturnsForAnyArgs(new Web.Resources.List());
            var u = _listController.Get(_listId);

            Assert.IsTrue(u.Content is ObjectContent<Web.Resources.List>);
        }

        [Test]
        public void get_all_returns_pagable_sortable_list_of_users()
        {
            _automapper.Map<List<User>, List<Web.Resources.User>>(Arg.Any<List<User>>()).Returns(new List<Web.Resources.User>());

            var result = _listController.Get();

            Assert.IsTrue(result.Content is ObjectContent<PagableSortableList<Web.Resources.User>>);
        }

        [Test]
        public void post_valid_place_adds_place_to_list()
        {
            var place = DataGenerator.GenerateResourcePlace();
        }
      
    }
}
