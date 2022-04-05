using AutoMapper;
using Fashe.Users.Core.Interfaces.Mapper;
using Fashe.Users.Core.Models;
using System;

namespace Fashe.Users.Application.Handlers.Users.Queries.GetUser
{
    public class GetUserVm : IMapWith<User>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, GetUserVm>()
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Email,
                    opt => opt.MapFrom(x => x.Email))
                .ForMember(x => x.Name,
                    opt => opt.MapFrom(x => x.Name));
        }
    }
}
