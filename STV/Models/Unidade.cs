namespace STV.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Unidade")]
    public partial class Unidade
    {
        public int Idunidade { get; set; }

        public int Idcurso { get; set; }

        [StringLength(60)]
        [Required]
        public string Titulo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [Required]
        public DateTime DataAbertura { get; set; }

        public bool Encerrada { get; set; }

        public DateTime Stamp { get; set; }

        public virtual Curso Curso { get; set; }

        public virtual ICollection<Atividade> Atividades { get; set; }

        public virtual ICollection<Material> Materiais { get; set; }

    }

}