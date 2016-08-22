using STV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STV.ViewModels
{
    public class AlternativaVM
    {
        public int Idalternativa { get; set; }

        public int Idquestao { get; set; }

        public string Descricao { get; set; }

        public virtual Questao Questao { get; set; }

        public bool IsCorreta { get; set; }
    }
}