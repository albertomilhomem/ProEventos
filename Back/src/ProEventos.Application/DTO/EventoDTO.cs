using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.DTO
{
    public class EventoDTO
    {
        public int Id { get; set; }

        public string Local { get; set; }

        public string DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório."),
        //MinLength(3, ErrorMessage = "{0} deve ter no mínimo 3 caracteres."),
        //MaxLength(50, ErrorMessage = "{0} deve ter no máximo 50 caracteres.")
        StringLength(50, MinimumLength = 3, ErrorMessage = "Intervalo permitido de 3 a 50 caracteres")]
        public string Tema { get; set; }

        [Required]
        [Display(Name = "Quantidade de Pessoas")]
        [Range(1, 120000, ErrorMessage = "{0} não pode ser maior que 120000.")]
        public int QuantidadePessoas { get; set; }

        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = "Não é uma imagem válida. (GIF, JPEG, BMP, PNG)")]
        public string ImagemURL { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Phone(ErrorMessage = "O {0} está com número inválido.")]
        public string Telefone { get; set; }

        [Required]
        [Display(Name = "e-mail")]
        [EmailAddress(ErrorMessage = "É necessário ser um {e-mail} válido.")]
        public string Email { get; set; }
        public int UserID { get; set; }
        public UserDTO UserDTO { get; set; }
        public IEnumerable<LoteDTO> Lotes { get; set; }
        public IEnumerable<RedeSocialDTO> RedesSociais { get; set; }
        public IEnumerable<PalestranteDTO> Palestrantes { get; set; }

    }
}