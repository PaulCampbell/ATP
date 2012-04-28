﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ATP.Domain;
using ATP.Web.Infrastructure;
using ATP.Web.Models;
using AutoMapper;
using Raven.Client;

namespace ATP.Web.Controllers
{
    public class UsersController : BaseController
    {
        private IAutomapper _automapper;
        private IAuthenticationService _authenticationService;
        // GET /api/users
        public UsersController(IDocumentSession documentSession, IAutomapper automapper, 
            IAuthenticationService authenticationService) : base(documentSession)
        {
            _automapper = automapper;
            _authenticationService = authenticationService;
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
            var responseErrors = new List<KeyValuePair<string, string>>();
            switch(passwordResult)
            {
                case UpdatePasswordResult.unknown :
                case UpdatePasswordResult.reservedWord:
                case UpdatePasswordResult.notLongEnough:
                    responseErrors.Add(new KeyValuePair<string, string>("Password", "Invalid Password"));
                    break;
            }

            responseErrors.AddRange(ValidateUser(user));

            if (responseErrors.Any())
            {
                return new HttpResponseMessage<List<KeyValuePair<string, string>>>(responseErrors)
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


        private List<KeyValuePair<string, string>> ValidateUser(User user)
        {
            var result = new List<KeyValuePair<string, string>>();
            if(string.IsNullOrEmpty(user.Email))
            {
                result.Add(new KeyValuePair<string, string>("Email", "Email address required"));
            }
            return result;
        }
    }
}