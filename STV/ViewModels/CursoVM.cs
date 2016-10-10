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

        [StringLength(80, ErrorMessage = "Este campo suporta at� 80 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo � obrigat�rio")]
        [Display(Name = "T�tulo")]
        public string Titulo { get; set; }

        [Display(Name = "Data In�cio")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inv�lido")]
        public DateTime? DataInicial { get; set; }

        public bool Encerrado { get; set; }

        public int IdusuarioInstrutor { get; set; }

        [StringLength(100, ErrorMessage = "Este campo suporta at� 100 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo � obrigat�rio")]
        public string Categoria { get; set; }

        [StringLength(150, ErrorMessage = "Este campo suporta at� 150 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo � obrigat�rio")]
        [Display(Name = "Palavras-chave")]
        public string Palavraschave { get; set; }

        [Display(Name = "Data de Cria��o")]
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
