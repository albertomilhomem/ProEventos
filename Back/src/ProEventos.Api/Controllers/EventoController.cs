using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Api.Models;

namespace ProEventos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public IEnumerable<Evento> evento = new Evento[]{
            new Evento()
                {
                    EventoId = 1,
                    Tema = "Angular 11 e .NET 5",
                    Local = "Porto Velho",
                    Lote = "Primeiro Lote",
                    QuantidadePessoas = 250,
                    DataEvento = DateTime.Now.AddDays(2).ToString(),
                    ImagemURL = "foto.png"
                },
                new Evento()
                {
                    EventoId = 2,
                    Tema = "VueJS e Laravel",
                    Local = "São Paulo",
                    Lote = "Primeiro Lote",
                    QuantidadePessoas = 250,
                    DataEvento = DateTime.Now.AddDays(2).ToString(),
                    ImagemURL = "foto.png"
                }
                };
        public EventoController()
        {

        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return evento;
        }        
        [HttpGet("{id}")]
        public IEnumerable<Evento> Get(int id)
        {
            return evento.Where(evento => evento.EventoId == id);
        }
    }
}
