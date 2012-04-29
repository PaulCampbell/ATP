using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATP.Web.Resources;

namespace ATP.Web.Bootstrapper
{
    public class AutoMapperConfiguration
    {
        public static void Init()
        {
            AutoMapper.Mapper.CreateMap<Domain.Models.User, User>()
                .ForMember(u=>u.Password, opt=>opt.Ignore());

            AutoMapper.Mapper.CreateMap<User, Domain.Models.User>()
               .ForMember(u => u.HashedPassword, opt => opt.Ignore())
               .ForMember(u => u.Added, opt => opt.Ignore());

        }
    }
}