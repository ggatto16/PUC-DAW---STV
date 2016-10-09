namespace STV.Models
{
    public class Resposta
    {
        public int Idusuario { get; set; }

        public int Idquestao { get; set; }

        public int Idalternativa { get; set; }

        public virtual  Usuario Usuario { get; set; }

        public virtual Questao Questao { get; set; }

        public virtual Alternativa Alternativa { get; set; }

    }
}