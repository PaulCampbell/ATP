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
                .ForMember(u => u.Password, opt => opt.Ignore())
                .ForMember(u => u.Uri, opt => opt.Ignore())
                .ForMember(u => u.Actions, opt => opt.Ignore());
                

            AutoMapper.Mapper.CreateMap<User, Domain.Models.User>()
               .ForMember(u => u.HashedPassword, opt => opt.Ignore())
               .ForMember(u => u.Added, opt => opt.Ignore());


            AutoMapper.Mapper.CreateMap<Domain.Models.List, List>()
               .ForMember(u => u.Uri, opt => opt.Ignore())
               .ForMember(u => u.Actions, opt => opt.Ignore())
               .ForMember(l => l.Places, opt => opt.Ignore());


            AutoMapper.Mapper.CreateMap<List, Domain.Models.List>()
               .ForMember(u => u.Added, opt => opt.Ignore())
               .ForMember(l=>l.Places, opt=>opt.Ignore());


            AutoMapper.Mapper.CreateMap<Domain.Models.Place, Place>()
              .ForMember(u => u.Uri, opt => opt.Ignore())
              .ForMember(u => u.Actions, opt => opt.Ignore());


            AutoMapper.Mapper.CreateMap<Place, Domain.Models.Place>()
               .ForMember(u => u.Added, opt => opt.Ignore());

        }
    }
}