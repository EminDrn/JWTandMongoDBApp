using AutoMapper;
using JWTApp.Core.DTOs;
using JWTApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTApp.Service
{
    public class DtoMapper:Profile
    {
        public DtoMapper()
        {
            CreateMap<MovieDto, Movie>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<CreateUserDTo , User>().ReverseMap();
            CreateMap<LoginDto , User>().ReverseMap();

            
        }
    }
}
