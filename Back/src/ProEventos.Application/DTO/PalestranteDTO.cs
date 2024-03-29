using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.DTO
{
    public class PalestranteDTO
    {
        public int Id { get; set; }
        public string MiniCurriculo { get; set; }
        public int UserID { get; set; }
        public UserUpdateDTO User { get; set; }
        public IEnumerable<RedeSocialDTO> RedesSociais { get; set; }
        public IEnumerable<EventoDTO> Eventos { get; set; }

    }
}