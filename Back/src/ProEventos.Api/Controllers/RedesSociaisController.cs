using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contracts;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.DTO;
using Microsoft.AspNetCore.Authorization;
using ProEventos.Api.Extensions;

namespace ProEventos.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RedesSociaisController : ControllerBase
    {
        private readonly IRedeSocialService _redeSocialService;
        private readonly IEventoService _eventoService;
        private readonly IPalestranteService _palestranteService;
        public RedesSociaisController(IRedeSocialService redeSocialService, IEventoService eventoService, IPalestranteService palestranteService)
        {
            _palestranteService = palestranteService;
            _eventoService = eventoService;
            _redeSocialService = redeSocialService;
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetByEvento(int eventoId)
        {
            try
            {
                if (!await AutorEvento(eventoId)) return Unauthorized();

                var RedesSociais = await _redeSocialService.GetAllByEventoIdAsync(eventoId);

                if (RedesSociais == null) return NoContent();

                return Ok(RedesSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar RedesSociais. Erro: {ex.Message}");
            }
        }

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetByPalestrante()
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserID());
                if (palestrante == null) return Unauthorized();

                var RedesSociais = await _redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id);
                if (RedesSociais == null) return NoContent();

                return Ok(RedesSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar RedesSociais. Erro: {ex.Message}");
            }
        }

        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDTO[] models)
        {
            try
            {
                if (!await AutorEvento(eventoId)) return Unauthorized();

                var RedesSociais = await _redeSocialService.SaveByEvento(eventoId, models);

                if (RedesSociais == null) return NoContent();

                return Ok(RedesSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar RedesSociais. Erro: {ex.Message}");
            }
        }
        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(int palestranteId, RedeSocialDTO[] models)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserID());
                if (palestrante == null) return Unauthorized();

                var RedesSociais = await _redeSocialService.SaveByPalestrante(palestranteId, models);
                if (RedesSociais == null) return NoContent();

                return Ok(RedesSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar RedesSociais. Erro: {ex.Message}");
            }
        }
        [HttpDelete("evento/{eventoId}/{redeSocialId}")]

        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                if (!await AutorEvento(eventoId)) return Unauthorized();

                var redeSocial = await _redeSocialService.GetRedeSocialEventoByIdAsync(eventoId, redeSocialId);
                if (redeSocial == null) return NoContent();

                return await _redeSocialService.DeleteByEvento(eventoId, redeSocialId) ? Ok(new { message = "Deletado" }) : throw new Exception("Ocorreu um problema não específico ao tentar deletar Rede Social.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar o redeSocial. Erro: {ex.Message}");
            }

        }


        [HttpDelete("palestrante/{redeSocialId}")]

        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserID());
                if (palestrante == null) return Unauthorized();

                var redeSocial = await _redeSocialService.GetRedeSocialPalestranteByIdAsync(palestrante.Id, redeSocialId);
                if (redeSocial == null) return NoContent();

                return await _redeSocialService.DeleteByPalestrante(palestrante.Id, redeSocialId) ? Ok(new { message = "Deletado" }) : throw new Exception("Ocorreu um problema não específico ao tentar deletar Rede Social.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar o redeSocial. Erro: {ex.Message}");
            }

        }

        [NonAction]
        private async Task<bool> AutorEvento(int eventoId)
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserID(), eventoId, false);
            if (evento == null) return false;

            return true;
        }
    }
}
