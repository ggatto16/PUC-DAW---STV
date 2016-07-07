using STV.Models;
using System.Data.Entity.ModelConfiguration;

namespace STV.Map
{
    public class UnidadeMap : EntityTypeConfiguration<Unidade>
    {
        public UnidadeMap()
        {
            ToTable("Unidade");
            HasKey(x => x.Idunidade);

            HasRequired(x => x.Curso).WithMany(x => x.Unidades)
                .Map(m => m.MapKey("Idcurso"));

        }
    }
}