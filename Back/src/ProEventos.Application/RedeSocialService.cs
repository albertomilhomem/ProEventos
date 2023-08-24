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
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IRedeSocialPersist _redeSocialPersist;
        private readonly IMapper _mapper;

        public RedeSocialService(IRedeSocialPersist eventoPersist, IMapper mapper)
        {
            _redeSocialPersist = eventoPersist;
            _mapper = mapper;
        }

        public async Task AddRedeSocial(int id, RedeSocialDTO model, bool isEvento)
        {
            try
            {
                var redeSocial = _mapper.Map<RedeSocial>(model);
                if (isEvento)
                {
                    redeSocial.EventoId = id;
                    redeSocial.PalestranteId = null;
                }
                else
                {
                    redeSocial.EventoId = null;
                    redeSocial.PalestranteId = id;
                }

                _redeSocialPersist.Add<RedeSocial>(redeSocial);

                await _redeSocialPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<RedeSocialDTO[]> SaveByEvento(int eventoId, RedeSocialDTO[] models)
        {
            try
            {
                var redeSocials = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId);
                if (redeSocials == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(eventoId, model, true);

                    }

                    else
                    {
                        var redeSocial = redeSocials.FirstOrDefault(redeSocial => redeSocial.Id == model.Id);
                        model.EventoId = eventoId;

                        _mapper.Map(model, redeSocial);

                        _redeSocialPersist.Update<RedeSocial>(redeSocial);

                        await _redeSocialPersist.SaveChangesAsync();
                    }
                }

                var redeSocialRetorno = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId);

                return _mapper.Map<RedeSocialDTO[]>(redeSocialRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<RedeSocialDTO[]> SaveByPalestrante(int palestranteId, RedeSocialDTO[] models)
        {
            try
            {
                var redeSocials = await _redeSocialPersist.GetAllByPalestranteIdAsync(palestranteId);
                if (redeSocials == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(palestranteId, model, false);
                    }

                    else
                    {
                        var redeSocial = redeSocials.FirstOrDefault(redeSocial => redeSocial.Id == model.Id);
                        model.PalestranteId = palestranteId;

                        _mapper.Map(model, redeSocial);

                        _redeSocialPersist.Update<RedeSocial>(redeSocial);

                        await _redeSocialPersist.SaveChangesAsync();
                    }
                }

                var redeSocialRetorno = await _redeSocialPersist.GetAllByPalestranteIdAsync(palestranteId);

                return _mapper.Map<RedeSocialDTO[]>(redeSocialRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.GetRedeSocialEventoByIdAsync(eventoId, redeSocialId);
                if (redeSocial == null) throw new Exception("Rede Social de Evento não encontrada.");

                _redeSocialPersist.Delete<RedeSocial>(redeSocial);
                return await _redeSocialPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.GetRedeSocialPalestranteByIdAsync(palestranteId, redeSocialId);
                if (redeSocial == null) throw new Exception("Rede Social do Palestrante não encontrada.");

                _redeSocialPersist.Delete<RedeSocial>(redeSocial);
                return await _redeSocialPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDTO[]> GetAllByEventoIdAsync(int eventoId)
        {
            try
            {
                var redeSocials = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId);

                if (redeSocials == null) return null;

                var resultado = _mapper.Map<RedeSocialDTO[]>(redeSocials);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<RedeSocialDTO[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            try
            {
                var redeSocials = await _redeSocialPersist.GetAllByPalestranteIdAsync(palestranteId);

                if (redeSocials == null) return null;

                var resultado = _mapper.Map<RedeSocialDTO[]>(redeSocials);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDTO> GetRedeSocialEventoByIdAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.GetRedeSocialEventoByIdAsync(eventoId, redeSocialId);

                if (redeSocial == null) return null;

                var resultado = _mapper.Map<RedeSocialDTO>(redeSocial);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<RedeSocialDTO> GetRedeSocialPalestranteByIdAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.GetRedeSocialPalestranteByIdAsync(palestranteId, redeSocialId);

                if (redeSocial == null) return null;

                var resultado = _mapper.Map<RedeSocialDTO>(redeSocial);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}