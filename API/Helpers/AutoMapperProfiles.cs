using API.Dto;
using API.Entity;
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
           CreateMap<UpdateProductDto, Product>();
           CreateMap<Conversation, ConversationDto>()
                    .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender.Fullname))
                    .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => src.Receiver.Fullname));
            CreateMap<ConversationDto,Conversation>();
            CreateMap<Message, MessageDto>();
            CreateMap<MessageDto,Message>();
        }
    }
}