namespace STV.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Curso")]
    public partial class Curso
    {
        public int Idcurso { get; set; }

        [StringLength(60)]
        public string Titulo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Dtinicial { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Dtfinal { get; set; }

        public int Idusuario { get; set; }

        [StringLength(30)]
        public string Categoria { get; set; }

        [StringLength(120)]
        public string Palavraschave { get; set; }

        public DateTime Stamp { get; set; }

        public virtual ICollection<Departamento> Departamentos { get; set; }

        public virtual Instrutor Instrutor { get; set; }

        public virtual ICollection<Unidade> Unidades { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
