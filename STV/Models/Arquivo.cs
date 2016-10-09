using System.ComponentModel.DataAnnotations;

namespace STV.Models
{

    public class Arquivo
    {
        public int Idmaterial { get; set; }

        public byte[] Blob { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [StringLength(200, ErrorMessage = "Este campo suporta até 200 caracteres")]
        public string Nome { get; set; }

        public string ContentType { get; set; }

        public int? Tamanho { get; set; }

        public virtual Material Material { get; set; }



    }
}