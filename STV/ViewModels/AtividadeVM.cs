using STV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STV.ViewModels
{
    public class AtividadeVM
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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        public DateTime DataAbertura { get; set; }

        [Display(Name = "Data Encerramento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        public DateTime DataEncerramento { get; set; }

        [Display(Name = "Data Abertura")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        public DateTime DataAberturaShow
        {
            get
            {
                return DataAbertura;
            }

        }

        [Display(Name = "Data Encerramento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        public DateTime DataEncerramentoShow
        {
            get
            {
                return DataEncerramento;
            }
        }

        public virtual Unidade Unidade { get; set; }

        public virtual ICollection<Questao> Questoes { get; set; }

        public virtual ICollection<Nota> Notas { get; set; }

        public Questao QuestaoToShow { get; set; }

        public decimal PorcentagemQuestao
        {
            get
            {
                if (Questoes != null && Questoes.Count > 0)
                    return (decimal)100 / Questoes.Count;
                else
                    return 0;
            }
        }

        public decimal Realizado { get; set; } = 0;

        public bool IsFinalizada { get; set; }

        public bool IsRevisao { get; set; }
    }
}