using STV.Models;
using System.Data.Entity.ModelConfiguration;

namespace STV.Map
{
    public class MaterialMap : EntityTypeConfiguration<Material>
    {
        public MaterialMap()
        {
            ToTable("Material");
            HasKey(x => x.Idmaterial);

            HasRequired(x => x.Unidade).WithMany(x => x.Materiais)
                .Map(m => m.MapKey("Idunidade"));

        }
    }
}