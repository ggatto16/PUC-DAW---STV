namespace STV.ViewModels
{
    using STV.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class cursoVM
    {
        [Key]
        public int Idcurso { get; set; }

        [StringLength(60)]
        public string Titulo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Dtinicial { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Dtfinal { get; set; }

        public int? Idusuario { get; set; }

        [StringLength(30)]
        public string Categoria { get; set; }

        [StringLength(120)]
        public string Palavraschave { get; set; }

        public int? Iddepartamento { get; set; }

        public DateTime Stamp { get; set; }

        public virtual Departamento Departamento { get; set; }

        public virtual Usuario Usuario { get; set; }

        public ICollection<Unidade> Unidades { get; set; }

        public ICollection<Atividade> Atividades { get; set; }
    }

}