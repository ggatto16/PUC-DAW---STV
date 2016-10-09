using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace STV.Models
{
    [Table("Role")]
    public class Role
    {
        public int Idrole { get; set; }

        public string Nome { get; set; }

        [JsonIgnore]
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}