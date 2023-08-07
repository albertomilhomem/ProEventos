using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.DTO;
using ProEventos.Persistence.Models;

namespace ProEventos.Application.Contracts
{
    public interface IEventoService
    {
        Task<EventoDTO> AddEventos(int userID, EventoDTO model);
        Task<EventoDTO> UpdateEvento(int userID, int eventoId, EventoDTO model);
        Task<bool> DeleteEvento(int userID, int eventoId);
        Task<PageList<EventoDTO>> GetAllEventosAsync(int userID, PageParams pageParams, bool includePalestrantes = false);
        Task<EventoDTO> GetEventoByIdAsync(int userID, int eventoId, bool includePalestrantes = false);

    }
}