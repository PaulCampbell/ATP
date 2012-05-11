using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATP.Domain.Models
{
    public class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string HashedPassword { get; set; }


    }
}
