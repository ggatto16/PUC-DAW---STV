using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STV.Models
{
    public class NotaCurso
    {
        public int Idusuario { get; set; }

        public int Idcurso { get; set; }

        public int Pontos { get; set; }

        public string Comentario { get; set; }

        public virtual  Usuario Usuario { get; set; }

        public virtual Curso Curso { get; set; }
    }
}