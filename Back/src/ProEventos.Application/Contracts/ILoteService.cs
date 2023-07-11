using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.DTO;

namespace ProEventos.Application.Contracts
{
    public interface ILoteService
    {
        Task<LoteDTO[]> SaveLote(int eventoId, LoteDTO[] model);
        Task<bool> DeleteLote(int eventoId, int loteId);
        Task<LoteDTO[]> GetLotesByEventosAsync(int eventoId);
        Task<LoteDTO> GetLoteByIdAsync(int eventoId, int loteId);

    }
}