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
        public void get_all_returns_pagable_sortable_list_of_lists()
        {
            _automapper.Map<List<ATP.Domain.Models.List>, List<Web.Resources.List>>(Arg.Any<List<ATP.Domain.Models.List>>())
                .Returns(new List<Web.Resources.List>());

            var result = _listController.Get();

            Assert.IsTrue(result.Content is ObjectContent<PagableSortableList<Web.Resources.List>>);
        }

        [Test]
        public void post_valid_place_to_valid_list_adds_place_to_list()
        {
            var listId = 1;
            var place = DataGenerator.GenerateResourcePlace();
            place.Name = "A New Place";
            _automapper.Map<Web.Resources.Place, Domain.Models.Place>(place).Returns(new Domain.Models.Place { Name = "A New Place"});
            _validationRunner.RunValidation(Arg.Any<NewPlaceValidator>(), Arg.Any<Web.Resources.Place>())
           .Returns(new List<Error>());

            var response = _listController.PlacesPost(listId, place);
            
            var list = Session.Load<Domain.Models.List>(listId);
            var listDocUri = "lists/" + listId;
            var listPlaces = Session.Query<Domain.Models.Place>()
                .Where(x => x.List == listDocUri).ToList<Domain.Models.Place>();
            var newPlace = listPlaces.FirstOrDefault(x => x.Name == "A New Place");

            Assert.IsNotNull(newPlace);
        }

        [Test]
        public void post_invalid_place_to_valid_list_return_400()
        {
            var listId = 1;
            var list = Session.Load<Domain.Models.List>(listId);
            var place = DataGenerator.GenerateResourcePlace();
            place.Name = string.Empty;
            _automapper.Map<Web.Resources.Place, Domain.Models.Place>(place).Returns(DataGenerator.GenerateDomainModelPlace());
            _validationRunner.RunValidation(Arg.Any<NewPlaceValidator>(), Arg.Any<Web.Resources.Place>())
                .Returns(new List<Error>
                            {
                                new Error {Code = ErrorCode.MissingField, Field = "Name"}
                            }
                         );
            var response = _listController.PlacesPost(listId, place);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public void post_valid_place_to_valid_list_returns_201()
        {
            var listId = 1;
            var place = DataGenerator.GenerateResourcePlace();
            _automapper.Map<Web.Resources.Place, Domain.Models.Place>(place).Returns(DataGenerator.GenerateDomainModelPlace());
            _validationRunner.RunValidation(Arg.Any<NewPlaceValidator>(), Arg.Any<Web.Resources.Place>())
                .Returns(new List<Error>());

            var response = _listController.PlacesPost(listId, place);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public void put_invalid_place_to_valid_list_correct_errors()
        {
            var listId = 1;
            var place = DataGenerator.GenerateResourcePlace();
            place.Name = String.Empty;
            _validationRunner.RunValidation(Arg.Any<IValidator>(), Arg.Any<Resource>()).Returns(new List<Error>() { new Error { Code = ErrorCode.MissingField, Field = "Email", Message = "Email missing" } });
            var result = _listController.PlacesPost(listId, place);

            Assert.IsTrue(result.Content is ObjectContent<UnprocessableEntity>);

        }

        [Test]
        public void post_place_tononexistent_place_returns_404()
        {
            var listId = 900;
            var place = DataGenerator.GenerateResourcePlace();
            var response = _listController.PlacesPost(listId, place);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

      
    }
}
