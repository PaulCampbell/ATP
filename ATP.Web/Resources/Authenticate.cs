using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATP.Web.Resources
{
    public class Authenticate : Resource
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public Authenticate():base("/authenticate/")
        {
            
        }
    }
}