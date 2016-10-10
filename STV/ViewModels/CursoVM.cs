namespace STV.ViewModels
{
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class CursoVM
    {
        public int Idcurso { get; set; }

        [StringLength(80, ErrorMessage = "Este campo suporta até 80 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Display(Name = "Data Início")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime? DataInicial { get; set; }

        public bool Encerrado { get; set; }

        public int IdusuarioInstrutor { get; set; }

        [StringLength(100, ErrorMessage = "Este campo suporta até 100 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        public string Categoria { get; set; }

        [StringLength(150, ErrorMessage = "Este campo suporta até 150 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Palavras-chave")]
        public string Palavraschave { get; set; }

        [Display(Name = "Data de Criação")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Column(TypeName = "date")]
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
