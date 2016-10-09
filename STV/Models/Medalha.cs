using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace STV.Models
{
    public enum Medalhas
    {
        Sortudo = 1,
        Nerd = 2,
        Genio = 3
    };


    [Table("Medalha")]
    public class Medalha
    {
        public int Idmedalha { get; set; }

        public string Descricao { get; set; }

        [JsonIgnore]
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}