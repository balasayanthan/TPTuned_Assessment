using AutoMapper;
using Chinook.ClientModels;

namespace Chinook.Profiles
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.Playlist, PlayListDto>().ReverseMap();
        }
    }
}
