using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence.Contracts
{
    public interface IEventoPersist
    {
        Task<PageList<Evento>> GetAllEventosAsync(int userID, PageParams pageParams, bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int userID, int eventoId, bool includePalestrantes = false);
    }
}