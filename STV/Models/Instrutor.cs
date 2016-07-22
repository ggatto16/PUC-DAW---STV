using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STV.Models
{
    public class Instrutor : Usuario
    {
        public virtual ICollection<Curso> CursosInstrutor { get; set; }
    }
}