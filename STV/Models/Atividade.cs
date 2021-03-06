﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STV.Models
{

    [Table("Atividade")]
    public partial class Atividade
    {
        [Key]
        public int Idatividade { get; set; }

        public int Idunidade { get; set; }

        [Display(Name = "Descrição")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [StringLength(200, ErrorMessage = "Este campo suporta até 200 caracteres")]
        public string Descricao { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        public int Valor { get; set; }

        [Display(Name = "Data Abertura")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        public DateTime DataAbertura { get; set; }

        [Display(Name = "Data Encerramento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        public DateTime DataEncerramento { get; set; }

        public virtual Unidade Unidade { get; set; }

        public virtual ICollection<Questao> Questoes { get; set; }

        public virtual ICollection<Nota> Notas { get; set; }

    }
}