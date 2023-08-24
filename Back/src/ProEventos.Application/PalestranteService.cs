using System;
using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Application.Contracts;
using ProEventos.Persistence.Contracts;
using ProEventos.Application.DTO;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ProEventos.Persistence.Models;

namespace ProEventos.Application
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestrantePersist _palestrantePersist;
        private readonly IMapper _mapper;

        public PalestranteService(IGeralPersist geralPersist, IPalestrantePersist palestrantePersist, IMapper mapper)
        {
            _palestrantePersist = palestrantePersist;
            _mapper = mapper;
        }

        public async Task<PalestranteDTO> AddPalestrantes(int userID, PalestranteAddDTO model)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userID;

                _palestrantePersist.Add<Palestrante>(palestrante);

                if (await _palestrantePersist.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestrantePersist.GetPalestranteByUserIdAsync(userID, false);
                    return _mapper.Map<PalestranteDTO>(palestranteRetorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDTO> UpdatePalestrante(int userID, PalestranteUpdateDTO model)
        {
            try
            {
                var palestrante = await _palestrantePersist.GetPalestranteByUserIdAsync(userID, false);
                if (palestrante == null) return null;

                model.Id = palestrante.Id;
                model.UserID = userID;
                _mapper.Map(model, palestrante);
                _palestrantePersist.Update<Palestrante>(palestrante);

                if (await _palestrantePersist.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestrantePersist.GetPalestranteByUserIdAsync(userID, false);
                    return _mapper.Map<PalestranteDTO>(palestranteRetorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PalestranteDTO>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            try
            {
                var palestrantes = await _palestrantePersist.GetAllPalestrantesAsync(pageParams, includeEventos);

                if (palestrantes == null) return null;

                var resultado = _mapper.Map<PageList<PalestranteDTO>>(palestrantes);

                resultado.CurrentPage = palestrantes.CurrentPage;
                resultado.TotalPages = palestrantes.TotalPages;
                resultado.PageSize = palestrantes.PageSize;
                resultado.TotalCount = palestrantes.TotalCount;

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDTO> GetPalestranteByUserIdAsync(int userID, bool includeEventos = false)
        {
            try
            {
                var palestrante = await _palestrantePersist.GetPalestranteByUserIdAsync(userID, includeEventos);

                if (palestrante == null) return null;

                var resultado = _mapper.Map<PalestranteDTO>(palestrante);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
