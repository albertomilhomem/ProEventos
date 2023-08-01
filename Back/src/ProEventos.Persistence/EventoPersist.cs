using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence
{
    public class EventoPersist : IEventoPersist
    {
        private readonly ProEventosContext _context;
        public EventoPersist(ProEventosContext context)
        {
            this._context = context;
        }

        public async Task<Evento[]> GetAllEventosAsync(int userID, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            if (includePalestrantes)
            {
                query = query
                    .Include(e => e.PalestranteEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking().Where(e => e.UserId == userID).OrderBy(e => e.Id);

            return await query.AsNoTracking().ToArrayAsync();
        }
        public async Task<Evento[]> GetAllEventosByTemaAsync(int userID, string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e => e.Lotes).Include(e => e.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestranteEventos).ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking().OrderBy(e => e.Id).Where(e => e.Tema.ToLower().Contains(tema.ToLower()) && e.UserId == userID);

            return await query.ToArrayAsync();
        }
        public async Task<Evento> GetEventoByIdAsync(int userID, int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(e => e.Lotes).Include(e => e.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(e => e.PalestranteEventos).ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking().OrderBy(e => e.Id).Where(e => e.Id == eventoId && e.UserId == userID);

            return await query.FirstOrDefaultAsync();
        }
    }
}