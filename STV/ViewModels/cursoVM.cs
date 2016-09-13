namespace STV.ViewModels
{
    using Newtonsoft.Json;
    using STV.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class DetalhesCurso
    {
        public int Idcurso { get; set; }

        public string Titulo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Dtinicial { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Dtfinal { get; set; }

        public int IdusuarioInstrutor { get; set; }

        public string Categoria { get; set; }

        public string Palavraschave { get; set; }

        public virtual ICollection<Departamento> Departamentos { get; set; }

        public virtual Usuario Instrutor { get; set; }

        public virtual ICollection<Unidade> Unidades { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }

        public virtual ICollection<NotaCurso> NotasCurso { get; set; }

        NotaCurso _NotaCursoAtual;

        public virtual NotaCurso NotaCursoAtual
        {
            get
            {
                return (_NotaCursoAtual == null) ? new NotaCurso() : _NotaCursoAtual;
            }
            set
            {
                _NotaCursoAtual = value;
            }
        }

        public bool IsInstutor { get; set; }

        public virtual ICollection<Departamento> departamentosQueJaContemInscritos { get; set; }
    }

}