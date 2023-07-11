using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface ILotePersist
    {
        Task<Lote[]> GetLotesByEventosAsync(int eventoId);
        Task<Lote> GetLoteByIdAsync(int eventoId, int loteId);
    }
}