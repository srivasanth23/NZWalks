using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //CreateMap<UserDTO, UserDomain>()
            //    .ForMember(x => x.Name, opt => opt.MapFrom(x => x.FullName)).ReverseMap();


            CreateMap<Region, RegionDTO>().ReverseMap(); // Domain to DTO
            CreateMap<AddRegionRequestDTO, Region>().ReverseMap(); // RequestDTO to model
            CreateMap<UpdateRegionDTO, Region>().ReverseMap(); // RequestDTO to model
            CreateMap<AddrequestWalksDTO, Walk>().ReverseMap();
            CreateMap<Walk, WalkDTO>().ReverseMap();
            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();
            CreateMap<UpdateWalkRequestDTO, Walk>().ReverseMap();
        }
    }
}

//public class UserDTO
//{
//    public string FullName { get; set; }
//}

//public class UserDomain
//{
//    public string Name { get; set; }
//}