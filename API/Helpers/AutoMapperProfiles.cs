using API.Dto;
using API.Entity;
using API.Interfaces;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Product, ProductDto>()
                    .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.User.Fullname))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.User.Street + ", " + src.User.City + ' ' + src.User.State))
                    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
           CreateMap<User, UserDto>();
                    //.ForMember(dest => dest.Token, opt => opt.MapFrom());

        }
    }
}