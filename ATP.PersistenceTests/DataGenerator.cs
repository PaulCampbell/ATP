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
                ListName = "Leeds Pubs",
                Places = new List<Domain.Models.Place>
                                        {
                                            new Domain.Models.Place
                                                {
                                                    Description =
                                                        "Nice selection of guest ales, Live dodgy eighties rock bands. Perfect.",
                                                    Latitude = 52.002324f,
                                                    Longitude = -0.5734f,
                                                    Name = "The Duck and Drake"
                                                },
                                            new Domain.Models.Place
                                                {
                                                    Description =
                                                        "Kinda trendy place - multiple rooms, decent beer from Leeds brewary and guests",
                                                    Latitude = 52.002324f,
                                                    Longitude = -0.5734f,
                                                    Name = "The Adelphi"
                                                }
                                        },
                UserId = "users/1"
            };
        }

    }
}
