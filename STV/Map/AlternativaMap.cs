using STV.Models;
using System.Data.Entity.ModelConfiguration;

namespace STV.Map
{
    public class AlternativaMap : EntityTypeConfiguration<Alternativa>
    {
        public AlternativaMap()
        {
            ToTable("Alternativa");
            HasKey(x => x.Idalternativa);

            HasRequired(x => x.Questao).WithMany(x => x.Alternativas)
                .Map(m => m.MapKey("Idquestao"));

        }
    }
}