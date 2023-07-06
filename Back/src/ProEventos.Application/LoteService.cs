using System;
using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Application.Contracts;
using ProEventos.Persistence.Contracts;
using ProEventos.Application.DTO;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ProEventos.Application
{
    public class LoteService : ILoteService
    {
        private readonly ILotePersist _lotePersist;
        private readonly IMapper _mapper;
        private readonly IGeralPersist _geralPersist;

        public LoteService(IGeralPersist geralPersist, ILotePersist eventoPersist, IMapper mapper)
        {
            _lotePersist = eventoPersist;
            _mapper = mapper;
            _geralPersist = geralPersist;
        }

        public async Task AddLote(int eventoId, LoteDTO model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;

                _geralPersist.Add<Lote>(lote);

                await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<LoteDTO[]> SaveLote(int eventoId, LoteDTO[] models)
        {
            try
            {
                var lotes = await _lotePersist.GetLotesByEventosAsync(eventoId);
                if (lotes == null) return null;

                foreach (var model in models)
                {
                    if (model.EventoId == 0)
                    {
                        await AddLote(eventoId, model);

                    }

                    else
                    {
                        var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
                        model.EventoId = eventoId;

                        _mapper.Map(model, lote);

                        _geralPersist.Update<Lote>(lote);

                        await _geralPersist.SaveChangesAsync();
                    }
                }

                var loteRetorno = await _lotePersist.GetLotesByEventosAsync(eventoId);

                return _mapper.Map<LoteDTO[]>(loteRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdAsync(eventoId, loteId);
                if (lote == null) throw new Exception("Lote não encontrado.");

                _geralPersist.Delete<Lote>(lote);
                return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDTO[]> GetLotesByEventosAsync(int idEvento)
        {
            try
            {
                var lotes = await _lotePersist.GetLotesByEventosAsync(idEvento);

                if (lotes == null) return null;

                var resultado = _mapper.Map<LoteDTO[]>(lotes);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDTO> GetLoteByIdAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdAsync(eventoId, loteId);

                if (lote == null) return null;

                var resultado = _mapper.Map<LoteDTO>(lote);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}