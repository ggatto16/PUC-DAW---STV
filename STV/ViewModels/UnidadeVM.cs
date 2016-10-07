using STV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace STV.ViewModels
{
    public class UnidadeVM
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
        public DateTime? DataAbertura { get; set; }

        public bool Status { get; set; }

        public DateTime Stamp { get; set; }

        public bool IsInstutor { get; set; }

        public virtual Curso Curso { get; set; }

        public virtual IEnumerable<AtividadeVM> AtividadesVM { get; set; }

        public virtual ICollection<Material> Materiais { get; set; }
    }
}