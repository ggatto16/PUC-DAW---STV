namespace STV.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Alternativa
    {

        public int Idalternativa { get; set; }

        public int Idquestao { get; set; }

        public string Descricao { get; set; }

        public string Justificativa { get; set; }

        public virtual Questao Questao { get; set; }


    }
}