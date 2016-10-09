namespace STV.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Departamento")]
    public partial class Departamento
    {
        public int Iddepartamento { get; set; }

        [Display(Name = "Descri��o")]
        [StringLength(100, ErrorMessage = "Este campo suporta at� 100 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo � obrigat�rio")]
        public string Descricao { get; set; }

        [Display(Name = "Data de Cria��o")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Column(TypeName = "date")]
        public DateTime Stamp { get; set; }

        public virtual ICollection<Curso> Cursos { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
