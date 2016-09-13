namespace STV.Models
{
    using Newtonsoft.Json;
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

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime? Dtinicial { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime? Dtfinal { get; set; }

        public int IdusuarioInstrutor { get; set; }

        [StringLength(30)]
        public string Categoria { get; set; }

        [StringLength(120)]
        public string Palavraschave { get; set; }

        public DateTime Stamp { get; set; }

        public virtual ICollection<Departamento> Departamentos { get; set; }

        public virtual Usuario Instrutor { get; set; }

        public virtual ICollection<Unidade> Unidades { get; set; }

        [JsonIgnore]
        public virtual ICollection<Usuario> Usuarios { get; set; }

        public virtual ICollection<NotaCurso> NotasCurso { get; set; }

        [NotMapped]
        public virtual ICollection<Departamento> departamentosQueJaContemInscritos { get; set; }

    }
}
