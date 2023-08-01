using System;
using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Application.Contracts;
using ProEventos.Persistence.Contracts;
using ProEventos.Application.DTO;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersist _eventoPersist;
        private readonly IMapper _mapper;
        private readonly IGeralPersist _geralPersist;

        public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist, IMapper mapper)
        {
            _eventoPersist = eventoPersist;
            _mapper = mapper;
            _geralPersist = geralPersist;
        }

        public async Task<EventoDTO> AddEventos(int userID, EventoDTO model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userID;

                _geralPersist.Add<Evento>(evento);

                if (await _geralPersist.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(userID, evento.Id, false);
                    return _mapper.Map<EventoDTO>(eventoRetorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO> UpdateEvento(int userID, int eventoId, EventoDTO model)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(userID, eventoId, false);
                if (evento == null) return null;

                model.Id = evento.Id;
                model.UserID = userID;
                _mapper.Map(model, evento);
                _geralPersist.Update<Evento>(evento);

                if (await _geralPersist.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(userID, evento.Id, false);
                    return _mapper.Map<EventoDTO>(eventoRetorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int userID, int eventoId)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(userID, eventoId, false);
                if (evento == null) throw new Exception("Evento não encontrado.");

                _geralPersist.Delete<Evento>(evento);
                return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO[]> GetAllEventosAsync(int userID, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(userID, includePalestrantes);

                if (eventos == null) return null;

                var resultado = _mapper.Map<EventoDTO[]>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO[]> GetAllEventosByTemaAsync(int userID, string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosByTemaAsync(userID, tema, includePalestrantes);

                if (eventos == null) return null;

                var resultado = _mapper.Map<EventoDTO[]>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO> GetEventoByIdAsync(int userID, int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(userID, eventoId, includePalestrantes);

                if (evento == null) return null;

                var resultado = _mapper.Map<EventoDTO>(evento);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
