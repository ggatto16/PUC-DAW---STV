using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace STV.Models
{
    [Table("Role")]
    public class Role
    {
        public int Idrole { get; set; }

        public string Nome { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}