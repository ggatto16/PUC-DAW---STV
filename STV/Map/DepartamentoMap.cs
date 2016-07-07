using STV.Models;
using System.Data.Entity.ModelConfiguration;

namespace STV.Map
{
    public class DepartamentoMap : EntityTypeConfiguration<Departamento>
    {
        public DepartamentoMap()
        {
            ToTable("Departamento");
            HasKey(x => x.Iddepartamento);

        }
    }
}