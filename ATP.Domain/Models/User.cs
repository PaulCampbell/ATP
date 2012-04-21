using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATP.Domain.Models
{
    public class User : Entity
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public virtual string Email { get; set; }
        public virtual string MobileNumber { get; set; }

    }
}
