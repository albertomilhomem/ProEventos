using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.DTO;

namespace ProEventos.Application.Contracts
{
    public interface ITokenService
    {
        Task<string> CreateToken(UserUpdateDTO userUpdateDTO);
    }
}