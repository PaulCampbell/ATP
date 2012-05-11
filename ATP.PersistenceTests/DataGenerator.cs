using System.Collections.Generic;
using ATP.Web.Resources;

namespace ATP.PersistenceTests
{
    public static class DataGenerator
    {
        public static User GenerateWebModelUser()
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

        public static Domain.Models.List GenereateDomainModelList()
        {
          

            return new Domain.Models.List
            {
                Name = "Leeds Pubs",
                Places = new List<string> {"places/1", "places/2"},
                User = "users/1"
            };
        }

    }
}
