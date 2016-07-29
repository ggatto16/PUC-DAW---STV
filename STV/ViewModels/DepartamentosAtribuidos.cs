using STV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STV.ViewModels
{
    public class DepartamentosAtribuidos
    {
        public int Iddepartamento { get; set; }

        public string Descricao { get; set; }

        public bool Atribuido { get; set; }
    }
}