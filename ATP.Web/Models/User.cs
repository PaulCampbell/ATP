using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATP.Web.Models
{
    public class User
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string PassWord { get; set; }
        public string Uri
        {
            get
            {
                return "/users/" + Id;
            }
        }
    }
}