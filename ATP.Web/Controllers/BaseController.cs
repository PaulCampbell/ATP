using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using CuttingEdge.Conditions;
using Raven.Client;

namespace ATP.Web.Controllers
{
    public  class BaseController : ApiController  
    {  
          public readonly IDocumentSession DocumentSession;

          public BaseController(IDocumentSession documentSession)  
          {  
            DocumentSession = documentSession;  
          } 

        
    }
}