using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STV.Models
{

    public class Arquivo
    {
        public int Idarquivo { get; set; }

        public string Nomearquivo { get; set; }

        public string ContentType { get; set; }

        public int Idmaterial { get; set; }

        public virtual Material Material { get; set; }

    }
}