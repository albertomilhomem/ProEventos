using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProEventos.Application.DTO;

namespace ProEventos.Application.Contracts
{
    public interface IAccountService
    {
        Task<bool> UserExists(string username);
        Task<UserUpdateDTO> GetUserByUserNameAsync(string username);
        Task<SignInResult> CheckUserPasswordAsync(UserUpdateDTO userUpdateDTO, string password);
        Task<UserUpdateDTO> CreateAccountAsync(UserDTO userDTO);
        Task<UserUpdateDTO> UpdateAccount(UserUpdateDTO userUpdateDTO);
    }
}