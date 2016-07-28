using STV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STV.ViewModels
{
    public class RolesAtribuidas
    {
        public int Idrole { get; set; }

        public string Nome { get; set; }

        public bool Atribuida { get; set; }
    }
}