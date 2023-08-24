using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.DTO;

namespace ProEventos.Application.Contracts
{
    public interface IRedeSocialService
    {
        Task<RedeSocialDTO[]> SaveByEvento(int eventoId, RedeSocialDTO[] models);
        Task<bool> DeleteByEvento(int eventoId, int redeSocialId);
        Task<RedeSocialDTO[]> SaveByPalestrante(int palestranteId, RedeSocialDTO[] models);
        Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId);
        Task<RedeSocialDTO[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocialDTO[]> GetAllByPalestranteIdAsync(int palestranteId);
        Task<RedeSocialDTO> GetRedeSocialEventoByIdAsync(int eventoId, int redeSocialId);
        Task<RedeSocialDTO> GetRedeSocialPalestranteByIdAsync(int palestranteId, int redeSocialId);
    }
}