using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATP.Web.Tests
{
    public static class DataGenerator
    {
        public static  Web.Models.User GenerateWebModelUser()
        {
            return new Web.Models.User
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
