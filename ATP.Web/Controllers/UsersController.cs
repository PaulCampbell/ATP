﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ATP.Domain;
using ATP.Web.Infrastructure;
using ATP.Web.Resources;
using ATP.Web.Validators;
using AutoMapper;
using Raven.Client;

namespace ATP.Web.Controllers
{
    public class UsersController : BaseController
    {
        private IAutomapper _automapper;
        private IAuthenticationService _authenticationService;
        private IValidationRunner _validationRunner;
        // GET /api/users
        public UsersController(IDocumentSession documentSession, IAutomapper automapper, 
            IAuthenticationService authenticationService,
            IValidationRunner validationRunner)
            : base(documentSession)
        {
            _automapper = automapper;
            _authenticationService = authenticationService;
            _validationRunner = validationRunner;
        }

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET /api/users/5
        public User Get(int id)
        {
            var user = DocumentSession.Load<Domain.Models.User>(id);
            if(user!=null)
            {
                return _automapper.Map<Domain.Models.User, User>(user); 
            }
            throw new HttpResponseException("User not found", HttpStatusCode.NotFound);
        }

        // POST /api/users
        public HttpResponseMessage Post(User user)
        {
            var domainUser = _automapper.Map<User, Domain.Models.User>(user);
            var passwordResult = _authenticationService.UpdatePassword(domainUser, user.Password);
            var responseObject = new UnprocessableEntity();
            switch(passwordResult)
            {
                case UpdatePasswordResult.unknown :
                case UpdatePasswordResult.reservedWord:
                case UpdatePasswordResult.notLongEnough:
                    responseObject.AddError(
                        new Error{Field = "Password", 
                            Message = "Invalid Password", 
                            Code = ErrorCode.Invalid,
                            Resource = user.Uri});
                    break;
            }

            responseObject.AddRange(ValidateUser(user));

            if (responseObject.Errors.Any())
            {
                return new HttpResponseMessage<UnprocessableEntity>(responseObject)
                           {
                               StatusCode = HttpStatusCode.BadRequest
                           };
            }

            DocumentSession.Store(domainUser);
            DocumentSession.SaveChanges();
            return new HttpResponseMessage<User>(user)
            {
                StatusCode = HttpStatusCode.Created
            };
        }

        // PUT /api/users/5
        public void Put(int id, string value)
        {
        }


        private List<Error> ValidateUser(User user)
        {
            var result = new List<Error>();
            if(string.IsNullOrEmpty(user.Email))
            {
                result.Add(
                    new Error { Field = "Email", Message = "Email address required", 
                        Code = ErrorCode.Missing, Resource = user.Uri});
            }
            return result;
        }
    }
}
