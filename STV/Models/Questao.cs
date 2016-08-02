namespace STV.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    [Table("Questao")]
    public partial class Questao
    {
        public int Idquestao { get; set; }

        public int Idatividade { get; set; }

        [ForeignKey("Alternativa")]
        public int? IdalternativaCorreta { get; set; }

        public string Descricao { get; set; }

        public virtual Atividade Atividade { get; set; }

        public virtual Alternativa Alternativa { get; set; }

        public virtual ICollection<Alternativa> Alternativas { get; set; }

        [NotMapped]
        public int IdAlternativaSelecionada { get; set; }

        [NotMapped]
        public bool Respondida { get; set; }

        public virtual ICollection<Resposta> Respostas { get; set; }
    }
}