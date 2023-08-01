using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface IEventoPersist
    {
        Task<Evento[]> GetAllEventosByTemaAsync(int userID, string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(int userID, bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int userID, int eventoId, bool includePalestrantes = false);
    }
}