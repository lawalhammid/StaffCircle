using api.DTOs;
using AutoMapper;
using Models.Entities;

namespace api.MappingModels
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUsersDTO, Users>();
            CreateMap<CreateComposedMessagesDTO, ComposedMessages>();
            CreateMap<ComposedMessagesDTO, ComposedMessages>();
            CreateMap<SendMessagesDTO, SendMessages>();
            CreateMap<SendMessages, SendMessagesListDTO>();
            
            //CreateMap<CreateComposedMessagesDTO, ComposedMessages>().ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}