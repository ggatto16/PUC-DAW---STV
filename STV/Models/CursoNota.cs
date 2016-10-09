using System.ComponentModel.DataAnnotations;

namespace STV.Models
{
    public class NotaCurso
    {
        public int Idusuario { get; set; }

        public int Idcurso { get; set; }

        public int Pontos { get; set; }

        [StringLength(500, ErrorMessage = "Este campo suporta até 500 caracteres")]
        public string Comentario { get; set; }

        public virtual  Usuario Usuario { get; set; }

        public virtual Curso Curso { get; set; }
    }
}