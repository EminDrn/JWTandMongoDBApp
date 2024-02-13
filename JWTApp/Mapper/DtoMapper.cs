using AutoMapper;
using JWTApp.Models.DTOs;
using JWTApp.Models.Entities;

namespace JWTApp.Mapper
{
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<MovieDto, Movie>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<LoginDto, User>().ReverseMap();
            CreateMap<CommentDto, CommentMovie>().ReverseMap();


        }
    }
}
