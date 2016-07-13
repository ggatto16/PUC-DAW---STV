using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STV.Models
{

    public class Arquivo
    {
        public int Idmaterial { get; set; }

        public byte[] Blob { get; set; }

        public string Nome { get; set; }

        public string ContentType { get; set; }

        public virtual Material Material { get; set; }

    }
}