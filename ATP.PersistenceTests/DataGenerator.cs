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
            var l = new Domain.Models.List
            {
                Name = "Leeds Pubs",
                User = "users/1"
            };
            l.AddPlace(new Domain.Models.Place { Id = 1 });
            l.AddPlace(new Domain.Models.Place { Id = 2 });

            return l;
        }

        public static Place GenerateResourcePlace()
        {
            var p = new Web.Resources.Place
            {
                Latitude = 1.231234F,
                Longitude = 45.3333F,
                Name = "El Bareto",
                Description = "Place description"
            };
            return p;
        }

        public static Domain.Models.Place GenerateDomainModelPlace()
        {
            var p = new Domain.Models.Place
            {
                Latitude = 1.231234F,
                Longitude = 45.3333F,
                Name = "El Bareto",
                Description = "Place description"
            };
            return p;
        }

    }
}
