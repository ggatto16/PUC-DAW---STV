using STV.Models;
using System.Data.Entity.ModelConfiguration;

namespace STV.Map
{
    public class AtividadeMap : EntityTypeConfiguration<Atividade>
    {
        public AtividadeMap()
        {
            ToTable("Atividade");
            HasKey(x => x.Idatividade);

            HasRequired(x => x.Unidade).WithMany(x => x.Atividades)
                .Map(m => m.MapKey("Idunidade"));

        }
    }
}