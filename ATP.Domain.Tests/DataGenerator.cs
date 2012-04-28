using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATP.Domain.Tests
{
    public static class DataGenerator
    {
        
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
