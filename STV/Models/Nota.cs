namespace STV.Models
{
    public class Nota
    {
        public int Idusuario { get; set; }

        public int Idatividade { get; set; }

        public int Pontos { get; set; }

        public virtual  Usuario Usuario { get; set; }

        public virtual Atividade Atividade { get; set; }
    }
}