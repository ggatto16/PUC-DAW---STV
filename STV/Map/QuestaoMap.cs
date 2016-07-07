using STV.Models;
using System.Data.Entity.ModelConfiguration;

namespace STV.Map
{
    public class QuestaoMap : EntityTypeConfiguration<Questao>
    {
        public QuestaoMap()
        {
            ToTable("Questao");
            HasKey(x => x.Idquestao);

            HasRequired(x => x.Atividade).WithMany(x => x.Questoes)
                .Map(m => m.MapKey("Idatividade"));

            //HasRequired(x => x.Alternativa).WithRequiredPrincipal()
              //  .Map(m => m.MapKey("IdalternativaCorreta"));

        }
    }
}