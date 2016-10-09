using STV.Models;
using System.Collections.Generic;

namespace STV.ViewModels
{
    public class RelatorioUsuario
    {

        public int Idusuario { get; set; }

        public string Cpf { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public int Iddepartamento { get; set; }

        public virtual ICollection<Medalha> Medalhas { get; set; }

        public virtual Departamento Departamento { get; set; }

        public virtual ICollection<Curso> Cursos { get; set; }

        public virtual ICollection<Nota> Notas { get; set; }

        public virtual ICollection<Material> MateriaisConsultados { get; set; }

        public int MateriaisConsultadosPorUnidade { get; set; }

        public decimal PorcentagemMateriaisConsultados
        {
            get
            {
                if (MateriaisConsultados != null && MateriaisConsultados.Count > 0)
                    return 100 / MateriaisConsultados.Count;
                else
                    return 0;
            }
        }

    }
}