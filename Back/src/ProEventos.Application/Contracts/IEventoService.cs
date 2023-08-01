using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.DTO;

namespace ProEventos.Application.Contracts
{
    public interface IEventoService
    {
        Task<EventoDTO> AddEventos(int userID, EventoDTO model);
        Task<EventoDTO> UpdateEvento(int userID, int eventoId, EventoDTO model);
        Task<bool> DeleteEvento(int userID, int eventoId);
        Task<EventoDTO[]> GetAllEventosByTemaAsync(int userID, string tema, bool includePalestrantes = false);
        Task<EventoDTO[]> GetAllEventosAsync(int userID, bool includePalestrantes = false);
        Task<EventoDTO> GetEventoByIdAsync(int userID, int eventoId, bool includePalestrantes = false);

    }
}