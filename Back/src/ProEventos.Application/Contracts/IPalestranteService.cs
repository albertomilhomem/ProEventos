using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.DTO;
using ProEventos.Persistence.Models;

namespace ProEventos.Application.Contracts
{
    public interface IPalestranteService
    {
        Task<PalestranteDTO> AddPalestrantes(int userID, PalestranteAddDTO model);
        Task<PalestranteDTO> UpdatePalestrante(int userID, PalestranteUpdateDTO model);
        Task<PageList<PalestranteDTO>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);
        Task<PalestranteDTO> GetPalestranteByUserIdAsync(int userID, bool includeEventos = false);

    }
}