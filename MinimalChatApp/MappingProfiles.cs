using AutoMapper;
using MinimalChatApp.Dtos;
using MinimalChatApp.Models;

namespace MinimalChatApp
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles() 
        {
            CreateMap<UserDto, User>();
        }
        
    }
}
