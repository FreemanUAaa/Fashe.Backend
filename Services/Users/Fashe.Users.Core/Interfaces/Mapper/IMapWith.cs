using AutoMapper;

namespace Fashe.Users.Core.Interfaces.Mapper
{
    public class IMapWith<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
