namespace STV.ViewModels
{
    using Newtonsoft.Json;
    using STV.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DetalhesCurso
    {
        public int Idcurso { get; set; }

        [StringLength(80, ErrorMessage = "Este campo suporta até 80 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Display(Name = "Data Início")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime? DataInicial { get; set; }

        public bool Encerrado { get; set; }

        public int IdusuarioInstrutor { get; set; }

        [StringLength(100, ErrorMessage = "Este campo suporta até 100 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        public string Categoria { get; set; }

        [StringLength(150, ErrorMessage = "Este campo suporta até 150 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Palavras-chave")]
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

        public int NotaMaxima
        {
            get
            {
                if (Unidades != null)
                {
                    int nota = 0;
                    foreach (var unidade in Unidades)
                    {
                        if (unidade.Atividades != null)
                        {
                            foreach (var atividade in unidade.Atividades)
                            {
                                nota += atividade.Valor;
                            }
                        }
                    }
                    return nota;
                }
                else
                    return 0;
            }
        }

        public bool DisponibilizarCertificado { get; set; }

        public float MediaNota { get; set; }
    }

}