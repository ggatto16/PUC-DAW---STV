namespace STV.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Departamento")]
    public partial class Departamento
    {
        public int Iddepartamento { get; set; }

        [StringLength(50)]
        public string Descricao { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? Stamp { get; set; }

        public virtual ICollection<Curso> Cursos { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
