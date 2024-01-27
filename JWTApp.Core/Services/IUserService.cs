using JWTApp.Core.DTOs;
using JWTApp.Core.Models;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTApp.Core.Services
{
    public interface IUserService
    {
        Task<Response<UserDto>> CreateUserAsync(CreateUserDTo createUserDTo);
        Task<Response<UserDto>> GetUserByNameAsync(string userName);
    }
}
