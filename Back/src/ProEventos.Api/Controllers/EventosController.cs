using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Api.Data;
using ProEventos.Api.Models;

namespace ProEventos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        public EventosController(DataContext context)
        {
            this.context = context;
        }
        private readonly DataContext context;

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return context.Eventos;
        }        
        [HttpGet("{id}")]
        public Evento Get(int id)
        {
            return context.Eventos.FirstOrDefault(evento => evento.EventoId == id);
        }
    }
}
