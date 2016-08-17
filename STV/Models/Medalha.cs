using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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