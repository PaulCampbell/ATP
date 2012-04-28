using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATP.Web.Bootstrapper
{
    public class AutoMapperConfiguration
    {
        public static void Init()
        {
            AutoMapper.Mapper.CreateMap<Domain.Models.User, Web.Models.User>();

        }
    }
}