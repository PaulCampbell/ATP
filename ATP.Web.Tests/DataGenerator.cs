using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATP.Web.Resources;

namespace ATP.Web.Tests
{
    public static class DataGenerator
    {
        public static  User GenerateWebModelUser()
        {
            return new User
            {
                Email = "abc@d.org",
                FirstName = "Bill",
                LastName = "Grey",
                Password = "password",
                MobileNumber = "0777777777"
            };
        }

        public static Domain.Models.User GenerateDomainModelUser()
        {
            return new Domain.Models.User
            {
                Email = "abc@d.org",
                FirstName = "Bill",
                LastName = "Grey",
                MobileNumber = "0777777777"
            };
        }

    }
}
