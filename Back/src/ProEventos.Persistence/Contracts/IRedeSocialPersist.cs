using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface IRedeSocialPersist : IGeralPersist
    {
        Task<RedeSocial> GetRedeSocialEventoByIdAsync(int eventoId, int id);
        Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId);

        Task<RedeSocial> GetRedeSocialPalestranteByIdAsync(int palestranteId, int id);
        Task<RedeSocial[]> GetAllByPalestranteIdAsync(int eventoId);

    }
}